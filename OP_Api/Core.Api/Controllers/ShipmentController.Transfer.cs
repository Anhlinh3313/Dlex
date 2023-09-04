using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Business.ViewModels;
using Core.Infrastructure.Extensions;
using Core.Entity.Entities;
using Core.Infrastructure.Helper;
using Core.Infrastructure.Helper.ExceptionHelper;
using Core.Infrastructure.Utils;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Core.Business.Core.Helpers;
using Core.Business.ViewModels.Shipments;
using Core.Data;
using Core.Data.Core;
using Core.Entity.Procedures;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Api.Controllers
{
    public partial class ShipmentController
    {
        [HttpGet("GetTransferHistory")]
        public JsonResult GetTransferHistory(DateTime fromDate, DateTime toDate)
        {
            return JsonUtil.Create(_iShipmentService.GetLadingHistory(GetCurrentUser(), ShipmentTypeHelper.Transfer, fromDate, toDate));
        }

        [HttpPost("AssignTransferList")]
        public async Task<JsonResult> AssignTransferList([FromBody]ListShipmentUpdateStatusViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return JsonUtil.Error(ModelState);
            }
            var currentUser = GetCurrentUser();
            ListGoods listGoods = null;
            var data = new ListGoodsInfoViewModel();
            var user = _unitOfWork.RepositoryR<User>().GetSingle(u => u.Id == viewModel.EmpId);

            try
            {
                int[] statusIsdTransfer = {
                    StatusHelper.ShipmentStatusId.WaitingToTransfer,
                    StatusHelper.ShipmentStatusId.StoreInWarehouseTransfer
                };
                //
                int[] statusIsdRetrunTransfer = {
                    StatusHelper.ShipmentStatusId.StoreInWarehouseReturnTransfer
                };
                DateTime currentDate = DateTime.Now;
                string message = "";
                int pickupId = viewModel.EmpId;
                listGoods = _unitOfWork.RepositoryR<ListGoods>().GetSingle(f => f.Id == viewModel.ListGoodsId);
                if (Util.IsNull(listGoods))
                {
                    //Create BK
                    listGoods = new ListGoods(
                        ListGoodsTypeHelper.BK_CTTT,
                        currentUser.HubId.Value,
                        ListGoodsStatusHelper.READY_TO_TRANSFER,
                        viewModel.TPLId,
                        viewModel.FromHubId,
                        viewModel.ToHubId
                    );
                }
                else
                {
                    if (listGoods.ListGoodsStatusId != ListGoodsStatusHelper.READY_TO_TRANSFER)
                    {
                        return JsonUtil.Error("Trạng thái bảng kê không cho phép chỉnh sửa.");
                    }
                }
                //
                listGoods.EmpId = viewModel.EmpId;
                listGoods.TransportTypeId = viewModel.TransportTypeId;
                listGoods.StartExpectedTime = viewModel.StartExpectedTime;
                listGoods.StartTime = viewModel.StartTime;
                listGoods.EndExpectedTime = viewModel.EndExpectedTime;
                listGoods.EndTime = viewModel.EndTime;
                listGoods.RealWeight = viewModel.RealWeight;
                listGoods.TruckNumber = viewModel.TruckNumber;
                listGoods.MAWB = viewModel.MAWB;
                listGoods.Note = viewModel.Note;
                listGoods.SealNumber = viewModel.SealNumber;
                //
                if (listGoods.Id == 0)
                {
                    _unitOfWork.RepositoryCRUD<ListGoods>().Insert(listGoods);
                }
                else
                {
                    _unitOfWork.RepositoryCRUD<ListGoods>().Update(listGoods);
                }
                await _unitOfWork.CommitAsync();
                data = await _iListGoodsService.UpdateCode(listGoods);
                //

                if (viewModel.ListGoodsId == 0)
                {
                    var listLading = new List<CreateUpdateLadingScheduleViewModel>();
                    var shipments = _unitOfWork.RepositoryR<Shipment>().FindBy(shipTrans => viewModel.ShipmentIds.Contains(shipTrans.Id), shipTrans => shipTrans.ShipmentStatus);
                    foreach (var item in shipments)
                    {
                        if (!statusIsdTransfer.Contains(item.ShipmentStatusId) && !statusIsdRetrunTransfer.Contains(item.ShipmentStatusId) && viewModel.IsTransferAllHub != true)
                        {
                            message += string.Format("Vận đơn {0} đã cập nhật trạng thái {1} trước đó!",
                                item.ShipmentNumber,
                                item.ShipmentStatus.Name);
                            continue;
                        }
                        //Add To Shipping Order
                        if (statusIsdTransfer.Contains(item.ShipmentStatusId) || item.ShipmentStatusId == StatusHelper.ShipmentStatusId.ReadyToDelivery)
                        {
                            item.ShipmentStatusId = (int)StatusHelper.ShipmentStatusId.AssignEmployeeTransfer;
                        }
                        else
                        {
                            item.ShipmentStatusId = (int)StatusHelper.ShipmentStatusId.AssignEmployeeTransferReturn;
                        }

                        item.ListGoodsId = listGoods.Id;
                        item.ReceiveHubId = viewModel.ReceiveHubId;
                        item.CurrentEmpId = user.Id;
                        item.CurrentHubId = currentUser.HubId;
                        _unitOfWork.RepositoryCRUD<Shipment>().Update(item);

                        //Add link Shipment BK
                        var shipmentListGoods = new ShipmentListGoods(
                            item.Id,
                            listGoods.Id
                        );
                        _unitOfWork.RepositoryCRUD<ShipmentListGoods>().Insert(shipmentListGoods);
                    }
                    if (string.IsNullOrEmpty(message))
                    {
                        _unitOfWork.Commit();

                        if (viewModel != null)
                        {
                            if (!String.IsNullOrWhiteSpace(viewModel.Note))
                            {
                                viewModel.Note = data.Code + "-" + user.UserName + ", " + viewModel.Note;
                            }
                            else
                            {
                                viewModel.Note = data.Code + "-" + user.UserName;
                            }
                        }
                        foreach (var item in shipments)
                        {
                            if (viewModel != null)
                            {
                                listLading.Add(new CreateUpdateLadingScheduleViewModel(
                                    item.Id,
                                    currentUser.HubId,
                                    currentUser.Id,
                                    item.ShipmentStatusId,
                                    viewModel.CurrentLat,
                                    viewModel.CurrentLng,
                                    viewModel.Location,
                                    viewModel.Note,
                                    0
                                ));
                            }
                        }

                        if (listLading.Count > 0)
                            await _iLadingScheduleService.Create(listLading);
                        var emp = _unitOfWork.RepositoryR<User>().GetSingle(x => x.Id == viewModel.EmpId);
                        if (emp != null && !string.IsNullOrWhiteSpace(emp.FireBaseToken))
                        {
                            var count = viewModel.ShipmentIds.Count;
                            await FireBaseUtil.SendNotification(_icompanyInformation.FireBaseBrowserAPIKey, emp.FireBaseToken, string.Format(MessageHelper.REQUEST_TO_DELIVERY, count), count);
                        }
                    }
                    else
                    {
                        return JsonUtil.Error(message);
                    }
                }
                else//update
                {
                    //Add thêm vận đơn
                    if (!Util.IsNull(viewModel.AddShipmentIds))
                    {
                        var AddShipment = _unitOfWork.RepositoryR<Shipment>().FindBy(shipTrans => viewModel.AddShipmentIds.Contains(shipTrans.Id), shipTrans => shipTrans.ShipmentStatus);
                        foreach (var item in AddShipment)
                        {
                            if (!statusIsdTransfer.Contains(item.ShipmentStatusId) && !statusIsdRetrunTransfer.Contains(item.ShipmentStatusId) && viewModel.IsTransferAllHub != true)
                            {
                                message += string.Format("Vận đơn {0} đã cập nhật trạng thái {1} trước đó!",
                                    item.ShipmentNumber,
                                    item.ShipmentStatus.Name);
                                continue;
                            }
                            //Add To Shipping Order
                            if (statusIsdTransfer.Contains(item.ShipmentStatusId) || item.ShipmentStatusId == StatusHelper.ShipmentStatusId.ReadyToDelivery)
                            {
                                item.ShipmentStatusId = (int)StatusHelper.ShipmentStatusId.AssignEmployeeTransfer;
                            }
                            else
                            {
                                item.ShipmentStatusId = (int)StatusHelper.ShipmentStatusId.AssignEmployeeTransferReturn;
                            }

                            item.ListGoodsId = listGoods.Id;
                            item.ReceiveHubId = viewModel.ReceiveHubId;
                            item.CurrentEmpId = user.Id;
                            item.CurrentHubId = currentUser.HubId;
                            _unitOfWork.RepositoryCRUD<Shipment>().Update(item);

                            //Add link Shipment BK
                            var shipmentListGoods = new ShipmentListGoods(
                                item.Id,
                                listGoods.Id
                            );
                            _unitOfWork.RepositoryCRUD<ShipmentListGoods>().Insert(shipmentListGoods);
                        }
                        if (string.IsNullOrEmpty(message))
                        {
                            var listLading = new List<CreateUpdateLadingScheduleViewModel>();
                            _unitOfWork.Commit();

                            var note = "";
                            if (viewModel != null)
                            {
                                if (!String.IsNullOrWhiteSpace(viewModel.Note))
                                {
                                    note = data.Code + "-" + user.UserName + ", " + viewModel.Note;
                                }
                                else
                                {
                                    note = data.Code + "-" + user.UserName;
                                }
                            }
                            foreach (var item in AddShipment)
                            {
                                if (viewModel != null)
                                {
                                    listLading.Add(new CreateUpdateLadingScheduleViewModel(
                                        item.Id,
                                        currentUser.HubId,
                                        currentUser.Id,
                                        item.ShipmentStatusId,
                                        viewModel.CurrentLat,
                                        viewModel.CurrentLng,
                                        viewModel.Location,
                                        note,
                                        0
                                    ));
                                }
                            }

                            if (listLading.Count > 0)
                                await _iLadingScheduleService.Create(listLading);
                        }
                        else
                        {
                            return JsonUtil.Error(message);
                        }
                    }
                    //UnShipment
                    if (!Util.IsNull(viewModel.UnShipmentIds))
                    {
                        int[] statusIsdAssignTransfer = {
                            StatusHelper.ShipmentStatusId.AssignEmployeeTransfer,
                            StatusHelper.ShipmentStatusId.AssignEmployeeTransferReturn
                        };
                        var UnShipment = _unitOfWork.RepositoryR<Shipment>().FindBy(shipTrans => viewModel.UnShipmentIds.Contains(shipTrans.Id), shipTrans => shipTrans.ShipmentStatus);
                        foreach (var item in UnShipment)
                        {
                            if (!statusIsdAssignTransfer.Contains(item.ShipmentStatusId))
                            {
                                message += string.Format("Vận đơn {0} đã cập nhật trạng thái {1} trước đó!",
                                    item.ShipmentNumber,
                                    item.ShipmentStatus.Name);
                                continue;
                            }
                            if (item.ShipmentStatusId == StatusHelper.ShipmentStatusId.AssignEmployeeTransfer)
                            {
                                item.ShipmentStatusId = (int)StatusHelper.ShipmentStatusId.WaitingToTransfer;
                            }
                            item.CurrentEmpId = user.Id;
                            item.CurrentHubId = currentUser.HubId;
                            item.ListGoodsId = null;
                            item.ReceiveHubId = null;
                            _unitOfWork.RepositoryCRUD<Shipment>().Update(item);
                            _unitOfWork.RepositoryCRUD<ShipmentListGoods>().DeleteWhere(f => f.ListGoodsId == listGoods.Id && f.ShipmentId == item.Id);
                        }
                        if (string.IsNullOrEmpty(message))
                        {
                            _unitOfWork.Commit();
                            var listLading = new List<CreateUpdateLadingScheduleViewModel>();

                            foreach (var item in UnShipment)
                            {
                                if (viewModel != null)
                                {
                                    listLading.Add(new CreateUpdateLadingScheduleViewModel(
                                        item.Id,
                                        currentUser.HubId,
                                        currentUser.Id,
                                        item.ShipmentStatusId,
                                        viewModel.CurrentLat,
                                        viewModel.CurrentLng,
                                        viewModel.Location,
                                        viewModel.Note,
                                        0
                                    ));
                                }
                            }
                            if (listLading.Count > 0)
                                await _iLadingScheduleService.Create(listLading);
                        }
                        else
                        {
                            return JsonUtil.Error(message);
                        }
                        //
                        var shipmentInListGood = _unitOfWork.RepositoryR<Shipment>().FindBy(shipTrans => viewModel.ShipmentIds.Contains(shipTrans.Id) && !viewModel.UnShipmentIds.Contains(shipTrans.Id), shipTrans => shipTrans.ShipmentStatus);
                        foreach (var item in shipmentInListGood)
                        {
                            item.CurrentEmpId = user.Id;
                            item.CurrentHubId = currentUser.HubId;
                            _unitOfWork.RepositoryCRUD<Shipment>().Update(item);
                        }
                        //
                        var emp = _unitOfWork.RepositoryR<User>().GetSingle(x => x.Id == viewModel.EmpId);

                        if (emp != null && !string.IsNullOrWhiteSpace(emp.FireBaseToken))
                        {
                            var count = viewModel.AddShipmentIds.Count + shipmentInListGood.Count();
                            await FireBaseUtil.SendNotification(_icompanyInformation.FireBaseBrowserAPIKey, emp.FireBaseToken, string.Format(MessageHelper.REQUEST_TO_TRANSIT, count), count);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }

            return JsonUtil.Success(data);
        }

        [HttpPost("AssignUpdateTransferList")]
        public async Task<JsonResult> AssignUpdateTransferList([FromBody]ListShipmentUpdateStatusViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return JsonUtil.Error(ModelState);
            }
            if ((viewModel.ShipmentStatusId == StatusHelper.ShipmentStatusId.TransferLostPackage) && string.IsNullOrEmpty(viewModel.Note))
            {
                return JsonUtil.Error("Vui lòng nhập ghi chú!");
            }

            var currentUser = GetCurrentUser();
            ListGoods listGoods = null;

            try
            {
                int[] statusIds = {
                    StatusHelper.ShipmentStatusId.Transferring,
                    StatusHelper.ShipmentStatusId.TransferReturning,
                    StatusHelper.ShipmentStatusId.AssignEmployeeTransfer,
                    StatusHelper.ShipmentStatusId.AssignEmployeeTransferReturn
                };
                string message = "";
                List<int> listGoodIds = new List<int>();
                var ladingSchedules = new List<CreateUpdateLadingScheduleViewModel>();
                int pickupId = viewModel.EmpId;

                // check create or update listGoods
                listGoods = _unitOfWork.RepositoryR<ListGoods>().GetSingle(lg => lg.Id == viewModel.ListGoodsId);
                var isCreate = true;

                if (!Util.IsNull(listGoods))
                {
                    isCreate = false;
                }

                var shipments = _unitOfWork.RepositoryR<Shipment>().FindBy(shipTrans => viewModel.ShipmentIds.Contains(shipTrans.Id), shipTrans => shipTrans.ShipmentStatus);

                if (viewModel.ShipmentStatusId == StatusHelper.ShipmentStatusId.StoreInWarehouseTransfer)
                {
                    //Create BK
                    if (isCreate)
                    {
                        listGoods = new ListGoods(
                            ListGoodsTypeHelper.BK_NKTT,
                            currentUser.HubId.Value,
                            null,
                            null,
                            currentUser.HubId.Value,
                            null,
                            currentUser.Id
                        );
                        if (viewModel.ScheduleErrorShipmentIds.Count() > 0)
                        {
                            listGoods.Note = viewModel.Note + ", Vận đơn lỗi: " + string.Join(' ', viewModel.ScheduleErrorShipmentIds);
                        }
                        else
                        {
                            listGoods.Note = viewModel.Note;
                        }
                        _unitOfWork.RepositoryCRUD<ListGoods>().Insert(listGoods);
                    }
                    else
                    {
                        string note;
                        if (viewModel.ScheduleErrorShipmentIds.Count() > 0)
                        {
                            var index = listGoods.Note.IndexOf(", Vận đơn lỗi:");
                            if (index != -1)
                            {
                                if (index == 0)
                                {
                                    note = (string.IsNullOrWhiteSpace(viewModel.Note) ? ", " + viewModel.Note + " , " : ", ") + "Vận đơn lỗi: " + string.Join(' ', viewModel.ScheduleErrorShipmentIds);
                                }
                                else
                                {
                                    var notErrorString = listGoods.Note.Substring(0, index - 1);
                                    note = notErrorString + (string.IsNullOrWhiteSpace(viewModel.Note) ? ", " + viewModel.Note + " , " : ", ") + "Vận đơn lỗi: " + string.Join(' ', viewModel.ScheduleErrorShipmentIds);
                                }
                            }
                            else
                            {
                                note = listGoods.Note + (string.IsNullOrWhiteSpace(viewModel.Note) ? ", " + viewModel.Note : ", ") + "Vận đơn lỗi: " + string.Join(' ', viewModel.ScheduleErrorShipmentIds);
                            }
                        }
                        else
                        {
                            note = listGoods.Note + viewModel.Note;
                        }
                        // Xử lý nhanh
                        if (viewModel.ScheduleErrorShipmentIds.Count() > 0)
                        {
                            foreach (var shipmentNumber in viewModel.ScheduleErrorShipmentIds)
                            {
                                // tạo bill mới
                                if (_icompanyInformation.Name == "vietstar")
                                {
                                    // check đúng định dạng mã vận đơn mới
                                    bool isIsValidShipmentNumberToWarehouse = true;
                                    if (_icompanyInformation.Name == "vietstar")
                                    {
                                        isIsValidShipmentNumberToWarehouse = ShipmentNumberFormatUtil.IsValidShipmentNumberToWarehouse(shipmentNumber);
                                    }
                                    //mã vận đơn mới hợp lệ
                                    if (isIsValidShipmentNumberToWarehouse == true)
                                    {
                                        var newShipment = new CreateUpdateShipmentViewModel();
                                        newShipment.ShipmentNumber = shipmentNumber;
                                        newShipment.FromHubId = currentUser.HubId;
                                        newShipment.CurrentHubId = currentUser.HubId;
                                        var currentHub = _unitOfWork.RepositoryR<Hub>().GetSingle(currentUser.HubId.Value, x => x.District);
                                        newShipment.FromProvinceId = currentHub.District.ProvinceId;
                                        newShipment.FromDistrictId = currentUser.Hub.DistrictId;
                                        newShipment.FromWardId = currentUser.Hub.WardId;
                                        newShipment.PickUserId = currentUser.Id;
                                        newShipment.OrderDate = DateTime.Now;
                                        newShipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.WaitingToTransfer;
                                        var dataNew = await _iGeneralServiceRaw.Create<Shipment, CreateUpdateShipmentViewModel>(newShipment);
                                        if (dataNew.IsSuccess)
                                        {
                                            var shipmentCreated = dataNew.Data as Shipment;

                                            // thêm hành trình loại nhập kho
                                            var ladingWarehouseNewShip = new LadingSchedule(
                                                 shipmentCreated.Id,
                                                 currentUser.HubId,
                                                 null,
                                                 currentUser.Id,
                                                 WarehousingUtil.GetStatusWarehousing(0),
                                                 newShipment.CurrentLat,
                                                 newShipment.CurrentLng,
                                                 newShipment.Location,
                                                 "SCER",
                                                 0
                                             );
                                            _unitOfWork.RepositoryCRUD<LadingSchedule>().Insert(ladingWarehouseNewShip);

                                            var ladingNewShipment = new CreateUpdateLadingScheduleViewModel(
                                                shipmentCreated.Id,
                                                shipmentCreated.FromHubId,
                                                currentUser.Id,
                                                shipmentCreated.ShipmentStatusId,
                                                newShipment.CurrentLat,
                                                newShipment.CurrentLng,
                                                newShipment.Location,
                                                viewModel.Note,
                                                0
                                            );
                                            await _iLadingScheduleService.Create(ladingNewShipment);
                                        }
                                    }
                                }
                            }
                        }
                        //
                        listGoods.Note = note;
                        _unitOfWork.RepositoryCRUD<ListGoods>().Update(listGoods);
                    }
                    await _unitOfWork.CommitAsync();
                }


                // nếu cập nhật BK => lấy ra vận đơn đã nằm trong bảng ShipmentListGoods khi cập nhật BK
                var oldShipments = new List<int>();
                if (!isCreate)
                {
                    // lấy ra vận đơn đã nhận trung chuyển
                    oldShipments = _unitOfWork.RepositoryR<ShipmentListGoods>().FindBy(x => x.ListGoodsId == listGoods.Id).Select(x => x.ShipmentId).ToList();

                    if (oldShipments.Count() > 0)
                    {
                        // chỉ cập nhật vận đơn mới scan khi đã tồn tại bảng kê
                        listGoodIds = oldShipments;
                        shipments = shipments.Where(x => !oldShipments.Contains(x.Id));
                    }
                }

                if (shipments.Count() > 0)
                {
                    foreach (var item in shipments)
                    {
                        if (!statusIds.Contains(item.ShipmentStatusId))
                        {
                            message += string.Format("Vận đơn {0} đã cập nhật trạng thái {1} trước đó!",
                                item.ShipmentNumber,
                                item.ShipmentStatus.Name);
                            continue;
                        }
                        if (!listGoodIds.Contains(item.ListGoodsId.Value))
                        {
                            listGoodIds.Add(item.ListGoodsId.Value);
                        }
                        if (item.ShipmentStatusId == StatusHelper.ShipmentStatusId.Transferring
                            || item.ShipmentStatusId == StatusHelper.ShipmentStatusId.TransferReturning
                            || item.ShipmentStatusId == StatusHelper.ShipmentStatusId.AssignEmployeeTransfer
                            || item.ShipmentStatusId == StatusHelper.ShipmentStatusId.AssignEmployeeTransferReturn
                            )
                        {
                            if (item.ReceiveHubId == item.ToHubId)
                            {
                                if (item.ShipmentStatusId == StatusHelper.ShipmentStatusId.Transferring
                                    || item.ShipmentStatusId == StatusHelper.ShipmentStatusId.AssignEmployeeTransfer)
                                {
                                    item.ShipmentStatusId = StatusHelper.ShipmentStatusId.ReadyToDelivery;
                                }
                            }
                            else
                            {
                                if (item.ShipmentStatusId == StatusHelper.ShipmentStatusId.Transferring
                                    || item.ShipmentStatusId == StatusHelper.ShipmentStatusId.AssignEmployeeTransfer)
                                {
                                    item.ShipmentStatusId = StatusHelper.ShipmentStatusId.StoreInWarehouseTransfer;
                                }
                                else if (item.ShipmentStatusId == StatusHelper.ShipmentStatusId.TransferReturning
                                    || item.ShipmentStatusId == StatusHelper.ShipmentStatusId.AssignEmployeeTransferReturn)
                                {
                                    item.ShipmentStatusId = StatusHelper.ShipmentStatusId.StoreInWarehouseReturnTransfer;
                                }
                            }
                        }
                        else if (item.ShipmentStatusId == StatusHelper.ShipmentStatusId.TransferLostPackage)
                        {
                            if (item.ShipmentStatusId == StatusHelper.ShipmentStatusId.Transferring)
                            {
                                item.ShipmentStatusId = StatusHelper.ShipmentStatusId.TransferLostPackage;
                            }
                            else if (item.ShipmentStatusId == StatusHelper.ShipmentStatusId.TransferReturning)
                            {
                                item.ShipmentStatusId = StatusHelper.ShipmentStatusId.TransferReturnLostPackage;
                            }
                        }

                        //Add To Shipping Order
                        item.CurrentEmpId = null;
                        item.CurrentHubId = currentUser.HubId;
                        item.Note += (!string.IsNullOrEmpty(viewModel.Note)) ? $", {viewModel.Note}" : "";

                        switch (item.ShipmentStatusId)
                        {
                            case StatusHelper.ShipmentStatusId.TransferLostPackage:
                                {
                                    break;
                                }
                            case StatusHelper.ShipmentStatusId.ReadyToDelivery:
                                {
                                    ladingSchedules.Add(new CreateUpdateLadingScheduleViewModel(
                                        item.Id,
                                        currentUser.HubId,
                                        currentUser.Id,
                                        StatusHelper.ShipmentStatusId.StoreInWarehouseTransfer,
                                        viewModel.CurrentLat,
                                        viewModel.CurrentLng,
                                        viewModel.Location,
                                        "",
                                        0
                                     ));

                                    //Add link Shipment BK
                                    var shipmentListGoods = new ShipmentListGoods(
                                        item.Id,
                                        listGoods.Id
                                    );
                                    _unitOfWork.RepositoryCRUD<ShipmentListGoods>().Insert(shipmentListGoods);
                                    break;
                                }
                            case StatusHelper.ShipmentStatusId.TransferReturnLostPackage:
                                {
                                    break;
                                }
                            case StatusHelper.ShipmentStatusId.StoreInWarehouseTransfer:
                            case StatusHelper.ShipmentStatusId.StoreInWarehouseReturnTransfer:
                                {
                                    //Add link Shipment BK
                                    var shipmentListGoods = new ShipmentListGoods(
                                        item.Id,
                                        listGoods.Id
                                    );
                                    _unitOfWork.RepositoryCRUD<ShipmentListGoods>().Insert(shipmentListGoods);
                                    break;
                                }
                            default:
                                {
                                    return JsonUtil.Error("Trạng thái cập nhật không hợp lệ");
                                }
                        }
                        _unitOfWork.RepositoryCRUD<Shipment>().Update(item);
                    }
                }

                var NotInShipments = _unitOfWork.RepositoryR<Shipment>().FindBy(shipTrans => viewModel.NotInShipmentIds.Contains(shipTrans.Id), shipTrans => shipTrans.ShipmentStatus);
                int[] statusIdsComplete = {
                    StatusHelper.ShipmentStatusId.DeliveryComplete,
                    StatusHelper.ShipmentStatusId.ReturnComplete
                    };
                DateTime currentDate = DateTime.Now;

                if (oldShipments.Count() > 0)
                {
                    // nếu cập nhật BK => chỉ cập nhật vận đơn quy trình lỗi mới scan khi đã tồn tại bảng kê
                    NotInShipments = NotInShipments.Where(x => !oldShipments.Contains(x.Id));
                }

                if (NotInShipments.Count() > 0)
                {
                    foreach (var item in NotInShipments)
                    {
                        if (statusIdsComplete.Contains(item.ShipmentStatusId))
                        {
                            message += string.Format("Vận đơn {0} đã cập nhật trạng thái {1} trước đó!",
                                item.ShipmentNumber,
                                item.ShipmentStatus.Name);
                            continue;
                        }
                        if (!Util.IsNull(item.ListGoodsId))
                        {
                            if (!listGoodIds.Contains(item.ListGoodsId.Value))
                            {
                                listGoodIds.Add(item.ListGoodsId.Value);
                            }
                        }
                        //Add To Shipping Order
                        item.ShipmentStatusId = (int)StatusHelper.ShipmentStatusId.StoreInWarehouseTransfer;

                        item.ListGoodsId = listGoods.Id;
                        item.ReceiveHubId = viewModel.ReceiveHubId;
                        item.CurrentEmpId = viewModel.EmpId;
                        item.CurrentHubId = currentUser.HubId;
                        // processError
                        _unitOfWork.RepositoryCRUD<Shipment>().Update(item);
                        //Add link Shipment BK
                        var shipmentListGoods = new ShipmentListGoods(
                            item.Id,
                            listGoods.Id
                        );
                        _unitOfWork.RepositoryCRUD<ShipmentListGoods>().Insert(shipmentListGoods);
                    }
                }

                if (string.IsNullOrEmpty(message))
                {
                    _unitOfWork.Commit();

                    if (shipments.Count() > 0)
                    {
                        foreach (var item in shipments)
                        {
                            if (viewModel != null)
                            {
                                //Lading Request Shipment
                                ladingSchedules.Add(new CreateUpdateLadingScheduleViewModel(
                                    item.Id,
                                    currentUser.HubId,
                                    currentUser.Id,
                                    item.ShipmentStatusId,
                                    viewModel.CurrentLat,
                                    viewModel.CurrentLng,
                                    viewModel.Location,
                                    "",
                                    0
                                ));
                            }
                        }
                    }

                    if (NotInShipments.Count() > 0)
                    {
                        foreach (var item in NotInShipments)
                        {
                            if (viewModel != null)
                            {
                                //Lading Request Shipment
                                ladingSchedules.Add(new CreateUpdateLadingScheduleViewModel(
                                    item.Id,
                                    currentUser.HubId,
                                    currentUser.Id,
                                    item.ShipmentStatusId,
                                    viewModel.CurrentLat,
                                    viewModel.CurrentLng,
                                    viewModel.Location,
                                    viewModel.Note + " quy trình lỗi",
                                    0
                                ));
                            }
                        }
                    }

                    if (ladingSchedules.Count > 0)
                        await _iLadingScheduleService.Create(ladingSchedules);
                    //

                    await _unitOfWork.CommitAsync();
                    if (listGoods != null)
                    {
                        listGoods.TotalReceived = viewModel.ShipmentIds.Count();
                        listGoods.TotalReceivedOther = viewModel.NotInShipmentIds.Count();
                        listGoods.TotalReceivedError = viewModel.ScheduleErrorShipmentIds.Count();
                        listGoods.TotalNotReceive = 0;
                        var listGoodsSendTo = _unitOfWork.RepositoryR<ListGoods>()
                            .FindBy(f => f.ListGoodsTypeId == ListGoodsTypeHelper.BK_CTTT && listGoodIds.Contains(f.Id));
                        foreach (var item in listGoodsSendTo)
                        {
                            item.TotalNotReceive = _unitOfWork.RepositoryR<Shipment>().FindBy(shipTrans => shipTrans.ListGoodsId == item.Id && statusIds.Contains(shipTrans.ShipmentStatusId) && shipTrans.IsEnabled).Count();
                            var totalShipment = _unitOfWork.RepositoryR<ShipmentListGoods>().Count(x => x.ListGoodsId == item.Id);
                            item.TotalReceived = totalShipment - item.TotalNotReceive;
                            if (item.TotalNotReceive == 0)
                            {
                                item.ListGoodsStatusId = ListGoodsStatusHelper.TRANSFER_COMPLETE;
                            }
                            _unitOfWork.RepositoryCRUD<ListGoods>().Update(item);
                        }
                        _unitOfWork.RepositoryCRUD<ListGoods>().Update(listGoods);
                        return JsonUtil.Success(await _iListGoodsService.UpdateCode(listGoods));
                    }
                    else
                        return JsonUtil.Success("Nhập kho thành công!");

                }
                else
                {
                    return JsonUtil.Error(message);
                }
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.InnerException.ToString());
            }

        }

        [HttpPost("IssueTransfer")]
        public async Task<JsonResult> IssueTransfer([FromBody]ReceiveShipmentViewModel viewModel)
        {
            if (!viewModel.ListGoodsId.HasValue)
            {
                return JsonUtil.Error("Mã xuất kho trống, vui lòng tạo mã xuất kho trước.");
            }
            if (viewModel.IsScan && !viewModel.IsAccept)
            {
                var res = _unitOfWork.Repository<Proc_CheckByShipmentNumbers>()
                 .ExecProcedureSingle(Proc_CheckByShipmentNumbers.GetEntityProc(viewModel.ShipmentNumber));
                if (res == null) return JsonUtil.Error("Không tìm thấy đơn hàng");
                else if (res.TotalBox > 1)
                {
                    return JsonUtil.Success(res, "Đơn hàng hơn 1 kiện");
                }
            }
            bool isReturn = false;
            bool isScheduleValid = false;
            var currentUser = GetCurrentUser();
            // Hàng chờ trung chuyển
            List<int> listStatusReadyTransfer = new List<int>();
            listStatusReadyTransfer.Add(StatusHelper.ShipmentStatusId.WaitingToTransfer);
            listStatusReadyTransfer.Add(StatusHelper.ShipmentStatusId.StoreInWarehouseReturnTransfer);
            listStatusReadyTransfer.Add(StatusHelper.ShipmentStatusId.StoreInWarehouseTransfer);
            //hàng hoàn tát
            List<int> listStatusComplete = new List<int>();
            listStatusComplete.Add(StatusHelper.ShipmentStatusId.DeliveryComplete);
            listStatusComplete.Add(StatusHelper.ShipmentStatusId.ReturnComplete);
            //hàng chờ giao
            List<int> listStatusReadyDelivery = new List<int>();
            listStatusReadyDelivery.Add(StatusHelper.ShipmentStatusId.ReadyToDelivery);
            listStatusReadyDelivery.Add(StatusHelper.ShipmentStatusId.DeliveryContinue);
            listStatusReadyDelivery.Add(StatusHelper.ShipmentStatusId.StoreInWarehouseDelivery);
            listStatusReadyDelivery.Add(StatusHelper.ShipmentStatusId.StoreInWarehousePickup);
            listStatusReadyDelivery.Add(StatusHelper.ShipmentStatusId.StoreInWarehouseReturn);
            using (var contextProc = new ApplicationContext())
            {
                try
                {
                    var unitOfWorkProc = new UnitOfWork(contextProc);
                    var checkShipments = unitOfWorkProc.Repository<Proc_CheckShipmentNumber>()
                        .ExecProcedure(Proc_CheckShipmentNumber.GetEntityProc(viewModel.ShipmentNumber)).ToList();
                    Shipment shipment = null;
                    if (checkShipments.Count() > 0)
                    {
                        var shipmentF = checkShipments.First();
                        isReturn = shipmentF.IsReturn;
                        if (shipmentF.IsShipment == 0)
                        {
                            return JsonUtil.Error("Bill tổng, không được phép xuất kho.");
                        }else if (shipmentF.CurrentHubId != currentUser.HubId)
                        {
                            return JsonUtil.Error("Vận đơn đang được BC/Kho khác xử lý, không được phép xuất kho.");
                        }
                        if (shipmentF.PackageId.HasValue && viewModel.IsPackage == false)
                        {
                            return JsonUtil.Error(string.Format("Vận đơn {0} đã đóng gói, vui lòng thao tác gói.", viewModel.ShipmentNumber));
                        }
                        else if (listStatusComplete.Contains(shipmentF.ShipmentStatusId))
                        {
                            return JsonUtil.Error("Vận đơn đã hoàn tất, không được phép xuất kho.");
                        }
                        else if (shipmentF.ToHubId.HasValue && viewModel.IsCheckSchedule)
                        {
                            if (shipmentF.ToHubId != viewModel.ToHubId) return JsonUtil.Error("Xuất kho không đúng Bưu Cục cần đến.", "IsAllow");
                        }
                        else if (shipmentF.ShipmentStatusId == StatusHelper.ShipmentStatusId.AssignEmployeeTransfer)
                        {
                            if (viewModel.ListGoodsId.HasValue && shipmentF.ListGoodsId == viewModel.ListGoodsId)
                            {
                                return JsonUtil.Error("Vẫn đơn đã có trong mã xuất kho, không được phép xuất kho.");
                            }
                        }
                        else if (listStatusReadyTransfer.Contains(shipmentF.ShipmentStatusId) && shipmentF.CurrentHubId == currentUser.HubId)
                        {
                            isScheduleValid = true;
                        }
                        else if (listStatusReadyDelivery.Contains(shipmentF.ShipmentStatusId) && shipmentF.CurrentHubId == currentUser.HubId)
                        {
                            isScheduleValid = true;
                        }
                        else
                        {
                            return JsonUtil.Error("Tình trạng vận đơn không được phép xuất kho.");
                        }
                        shipment = _unitOfWork.RepositoryR<Shipment>().GetSingle(shipmentF.Id);
                    }
                    else
                    {
                        if (_icompanyInformation.Name == "vietstar" || _icompanyInformation.Name == "dlexs")
                        {
                            // check đúng định dạng mã vận đơn mới
                            bool isIsValidShipmentNumberToWarehouse = true;
                            if (_icompanyInformation.Name == "vietstar")
                            {
                                isIsValidShipmentNumberToWarehouse = ShipmentNumberFormatUtil.IsValidShipmentNumberToWarehouse(viewModel.ShipmentNumber);
                                if (isIsValidShipmentNumberToWarehouse == false)
                                {
                                    return JsonUtil.Error(string.Format("Mã vận đơn '{0}' không hợp lệ", viewModel.ShipmentNumber));
                                }
                            }
                            //mã vận đơn mới hợp lệ
                            if (isIsValidShipmentNumberToWarehouse == true)
                            {
                                var res = await _iShipmentService.CreateShipmentNoneInfo(currentUser, viewModel.ShipmentNumber);
                                if (res == null) return JsonUtil.Error("Tạo vận đơn mới không thành công", viewModel.ShipmentNumber);
                                shipment = res;
                            }
                        }
                        else
                        {
                            return JsonUtil.Error("Mã vận đơn {0} không tìm thấy. ", viewModel.ShipmentNumber);
                        }
                    }
                    if (shipment == null) return JsonUtil.Error(string.Format("Xuất không không thành công vận đơn '{0}'. ", viewModel.ShipmentNumber));

                    // insert listGoofsShipment
                    // if (isReturn == true) shipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.AssignEmployeeTransferReturn;
                    else shipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.AssignEmployeeTransfer;
                    shipment.InOutDate = DateTime.Now;
                    //update shipment status
                    var lading = new CreateUpdateLadingScheduleViewModel(
                                            shipment.Id,
                                            currentUser.HubId,
                                            viewModel.ToHubId,
                                            currentUser.Id,
                                            shipment.ShipmentStatusId,
                                            0,
                                            0,
                                            "",
                                            (isScheduleValid == true ? "" : "SCD-ER ") + viewModel.Note,
                                            0
                                        );
                    await _iLadingScheduleService.Create(lading);
                    shipment.ListGoodsId = viewModel.ListGoodsId;
                    shipment.InOutDate = DateTime.Now;
                    _unitOfWork.RepositoryCRUD<Shipment>().Update(shipment);
                    //
                    var shipmentListGoods = new ShipmentListGoods(
                        shipment.Id,
                        viewModel.ListGoodsId.Value
                    );
                    _unitOfWork.RepositoryCRUD<ShipmentListGoods>().Insert(shipmentListGoods);
                    await _unitOfWork.CommitAsync();

                    //var resUpdate = _unitOfWork.Repository<Proc_UpdateRealExportSAP>()
                    //          .ExecProcedureSingle(Proc_UpdateRealExportSAP.GetEntityProc(shipment.Id,7, null));


                    return JsonUtil.Success(shipment);
                }
                catch (Exception ex)
                {
                    return JsonUtil.Error("Xảy ra lỗi khi xuất kho vận đơn {0}. ", viewModel.ShipmentNumber);
                }
            }
        }

        [HttpPost("ReceiveShipment")]
        public async Task<JsonResult> ReceiveShipment([FromBody]ReceiveShipmentViewModel viewModel)
        {
            var currentUser = GetCurrentUser();
            var shipment = _unitOfWork.RepositoryR<Shipment>().GetSingle(shipTrans => shipTrans.ShipmentNumber == viewModel.ShipmentNumber.Trim());

            if (shipment == null)
            {
                return JsonUtil.Error("Không tìm thấy vận đơn!");
            }

            shipment.CurrentEmpId = currentUser.Id;
            _unitOfWork.RepositoryCRUD<Shipment>().Update(shipment);
            await _unitOfWork.CommitAsync();

            var lading = new CreateUpdateLadingScheduleViewModel(
                    shipment.Id,
                    currentUser.HubId,
                    (int)currentUser.Id,
                    shipment.ShipmentStatusId,
                    viewModel.CurrentLat,
                    viewModel.CurrentLng,
                    viewModel.Location,
                    "",
                    0
                );
            await _iLadingScheduleService.Create(lading);

            return JsonUtil.Success(Mapper.Map<ShipmentInfoViewModel>(shipment));
        }

        [HttpPost("ReceiveTransitShipment")]
        public async Task<JsonResult> ReceiveTransitShipment([FromBody]ReceiveShipmentViewModel viewModel)
        {
            var currentUser = GetCurrentUser();
            int[] statusTransfers =
            {
                //StatusHelper.ShipmentStatusId.WaitingToTransfer,
                StatusHelper.ShipmentStatusId.AssignEmployeeTransfer,
            };
            int[] statusTransferReturns =
            {
                //StatusHelper.ShipmentStatusId.ReadyToTransferReturn,
                StatusHelper.ShipmentStatusId.AssignEmployeeTransferReturn,
            };

            var listGoods = _unitOfWork.RepositoryCRUD<ListGoods>().GetSingle(x => x.Code == viewModel.ShipmentNumber && x.ListGoodsStatusId == ListGoodsStatusHelper.READY_TO_TRANSFER);

            if (listGoods != null)
            {
                listGoods.TotalShipment = _unitOfWork.RepositoryR<Shipment>().Count(shipTrans => shipTrans.ListGoodsId == listGoods.Id && StatusHelper.GetReadyToTransitId().Contains(shipTrans.ShipmentStatusId));

                if (viewModel.IsUpdateBK)
                {
                    var listShipments = _unitOfWork.RepositoryR<Shipment>().FindBy(shipTrans => shipTrans.ListGoodsId == listGoods.Id && StatusHelper.GetReadyToTransitId().Contains(shipTrans.ShipmentStatusId));

                    foreach (var shipment in listShipments)
                    {
                        if (shipment == null)
                        {
                            return JsonUtil.Error("Không tìm thấy vận đơn!");
                        }

                        if (!statusTransfers.Contains(shipment.ShipmentStatusId) && !statusTransferReturns.Contains(shipment.ShipmentStatusId))
                        {
                            return JsonUtil.Error("Vận đơn không đúng tình trạng!");
                        }

                        shipment.CurrentEmpId = currentUser.Id;
                        shipment.CurrentHubId = currentUser.HubId;
                        if (statusTransfers.Contains(shipment.ShipmentStatusId))
                        {
                            shipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.Transferring;
                        }

                        if (statusTransferReturns.Contains(shipment.ShipmentStatusId))
                        {
                            shipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.TransferReturning;
                        }
                        _unitOfWork.RepositoryCRUD<Shipment>().Update(shipment);
                        await _unitOfWork.CommitAsync();

                        var lading = new CreateUpdateLadingScheduleViewModel(
                                shipment.Id,
                                currentUser.HubId,
                                (int)currentUser.Id,
                                shipment.ShipmentStatusId,
                                viewModel.CurrentLat,
                                viewModel.CurrentLng,
                                viewModel.Location,
                                "",
                                0
                            );
                        await _iLadingScheduleService.Create(lading);
                        //
                        int[] statusWaitingTranfer =
                        {
                            StatusHelper.ShipmentStatusId.AssignEmployeeTransfer,
                            StatusHelper.ShipmentStatusId.AssignEmployeeTransferReturn
                        };
                        if (_unitOfWork.RepositoryR<Shipment>().Count(shipTrans => shipTrans.ListGoodsId == listGoods.Id && statusWaitingTranfer.Contains(shipTrans.ShipmentStatusId)) == 0)
                        {
                            listGoods.ListGoodsStatusId = ListGoodsStatusHelper.TRANSFERRING;
                            _unitOfWork.RepositoryCRUD<ListGoods>().Update(listGoods);
                        }
                    }
                    return JsonUtil.Success(Mapper.Map<ShipmentInfoViewModel>(listShipments));
                }
                else
                {
                    return JsonUtil.Success(listGoods);
                }
            }
            #region MyRegion
            else
            {
                var shipment = _unitOfWork.RepositoryR<Shipment>().GetSingle(shipTrans => shipTrans.ShipmentNumber == viewModel.ShipmentNumber.Trim());

                if (shipment == null)
                {
                    return JsonUtil.Error("Không tìm thấy vận đơn!");
                }

                if (!statusTransfers.Contains(shipment.ShipmentStatusId) && !statusTransferReturns.Contains(shipment.ShipmentStatusId))
                {
                    return JsonUtil.Error("Vận đơn không đúng tình trạng!");
                }

                shipment.CurrentEmpId = currentUser.Id;
                shipment.CurrentHubId = currentUser.HubId;
                if (statusTransfers.Contains(shipment.ShipmentStatusId))
                {
                    shipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.Transferring;
                }

                if (statusTransferReturns.Contains(shipment.ShipmentStatusId))
                {
                    shipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.TransferReturning;
                }
                _unitOfWork.RepositoryCRUD<Shipment>().Update(shipment);
                await _unitOfWork.CommitAsync();

                var lading = new CreateUpdateLadingScheduleViewModel(
                        shipment.Id,
                        currentUser.HubId,
                        (int)currentUser.Id,
                        shipment.ShipmentStatusId,
                        viewModel.CurrentLat,
                        viewModel.CurrentLng,
                        viewModel.Location,
                        "",
                        0
                    );
                await _iLadingScheduleService.Create(lading);

                return JsonUtil.Success(Mapper.Map<ShipmentInfoViewModel>(shipment));
            }
            #endregion
        }

        [HttpPost("ReceiveDeliveryReturnShipment")]
        public async Task<JsonResult> ReceiveDeliveryReturnShipment([FromBody]ReceiveShipmentViewModel viewModel)
        {
            var currentUser = GetCurrentUser();

            int[] statusReturns =
            {
                StatusHelper.ShipmentStatusId.AssignEmployeeReturn,
                StatusHelper.ShipmentStatusId.StoreInWarehouseReturn,
            };

            var listGoods = _unitOfWork.RepositoryR<ListGoods>().GetSingle(x => x.Code == viewModel.ShipmentNumber && x.ListGoodsStatusId == ListGoodsStatusHelper.READY_TO_TRANSFER);

            if (listGoods != null)
            {
                listGoods.TotalShipment = _unitOfWork.RepositoryR<Shipment>().Count(shipTrans => shipTrans.ListGoodsId == listGoods.Id && StatusHelper.GetReadyToTransitId().Contains(shipTrans.ShipmentStatusId));

                if (viewModel.IsUpdateBK)
                {
                    var listShipments = _unitOfWork.RepositoryR<Shipment>().FindBy(shipTrans => shipTrans.ListGoodsId == listGoods.Id && StatusHelper.GetReadyToTransitId().Contains(shipTrans.ShipmentStatusId));

                    foreach (var shipment in listShipments)
                    {
                        if (shipment == null)
                        {
                            return JsonUtil.Error("Không tìm thấy vận đơn!");
                        }

                        if (StatusHelper.ShipmentStatusId.DeliveryComplete == shipment.ShipmentStatusId)
                        {
                            return JsonUtil.Error("Vận đơn đã được giao thành công!");
                        }

                        shipment.CurrentEmpId = currentUser.Id;
                        shipment.CurrentHubId = currentUser.HubId;
                        if (statusReturns.Contains(shipment.ShipmentStatusId))
                        {
                            shipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.Returning;
                        }
                        else
                        {
                            shipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.Delivering;
                        }
                        _unitOfWork.RepositoryCRUD<Shipment>().Update(shipment);
                        await _unitOfWork.CommitAsync();

                        var lading = new CreateUpdateLadingScheduleViewModel(
                                shipment.Id,
                                currentUser.HubId,
                                (int)currentUser.Id,
                                shipment.ShipmentStatusId,
                                viewModel.CurrentLat,
                                viewModel.CurrentLng,
                                viewModel.Location,
                                "",
                                0
                            );
                        await _iLadingScheduleService.Create(lading);
                    }
                    return JsonUtil.Success(Mapper.Map<ShipmentInfoViewModel>(listShipments));
                }
                else
                {
                    return JsonUtil.Success(listGoods);
                }
            }
            #region List Goods is null
            else
            {
                var shipment = _unitOfWork.RepositoryCRUD<Shipment>().GetSingle(shipTrans => shipTrans.ShipmentNumber == viewModel.ShipmentNumber.Trim());

                if (shipment == null)
                {
                    return JsonUtil.Error("Không tìm thấy vận đơn!");
                }

                if (StatusHelper.ShipmentStatusId.DeliveryComplete == shipment.ShipmentStatusId)
                {
                    return JsonUtil.Error("Vận đơn đã được giao thành công!");
                }

                shipment.CurrentEmpId = currentUser.Id;
                shipment.CurrentHubId = currentUser.HubId;
                if (statusReturns.Contains(shipment.ShipmentStatusId))
                {
                    shipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.Returning;
                }
                else
                {
                    shipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.Delivering;
                }
                _unitOfWork.RepositoryCRUD<Shipment>().Update(shipment);
                await _unitOfWork.CommitAsync();

                var lading = new CreateUpdateLadingScheduleViewModel(
                        shipment.Id,
                        currentUser.HubId,
                        (int)currentUser.Id,
                        shipment.ShipmentStatusId,
                        viewModel.CurrentLat,
                        viewModel.CurrentLng,
                        viewModel.Location,
                        "",
                        0
                    );
                await _iLadingScheduleService.Create(lading);

                return JsonUtil.Success(Mapper.Map<ShipmentInfoViewModel>(shipment));
            }
            #endregion
        }
    }
}
