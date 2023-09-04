using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Business.Core.Helpers;
using Core.Business.ViewModels;
using Core.Business.ViewModels.Shipments;
using Core.Data;
using Core.Data.Core;
using Core.Entity.Entities;
using Core.Entity.Procedures;
using Core.Infrastructure.Helper;
using Core.Infrastructure.Helper.ExceptionHelper;
using Core.Infrastructure.Utils;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Api.Controllers
{
    public partial class ShipmentController
    {
        [HttpGet("GetDeliveryHistory")]
        public JsonResult GetDeliveryHistory(DateTime fromDate, DateTime toDate)
        {
            return JsonUtil.Create(_iShipmentService.GetLadingHistory(GetCurrentUser(), ShipmentTypeHelper.Delivery, fromDate, toDate));
        }

        [HttpPost("CancelDeliveryComplete")]
        public async Task<JsonResult> CancelDeliveryComplete([FromBody]SimpleViewModel viewModel)
        {
            var shipment = await _unitOfWork.RepositoryR<Shipment>().GetSingleAsync(viewModel.Id);
            if (Util.IsNull(shipment))
            {
                return JsonUtil.Error("Không tìm thấy thông tin vận đơn");
            }
            else if (shipment.ShipmentStatusId != StatusHelper.ShipmentStatusId.DeliveryComplete && shipment.ShipmentStatusId != StatusHelper.ShipmentStatusId.ReturnComplete)
            {
                return JsonUtil.Error("Vận đơn chưa trả/phát thành công");
            }
            else if (shipment.COD > 0 && shipment.KeepingCODHubId.HasValue)
            {
                return JsonUtil.Error("Không thể hủy, đã được tạo bảng kê trả cước cho kế toán");
            }
            else if (shipment.PaymentTypeId == PaymentTypeHelper.NGUOI_NHAN_THANH_TOAN && shipment.KeepingTotalPriceHubId.HasValue)
            {
                return JsonUtil.Error("Không thể hủy, đã được tạo bảng kê trả COD cho kế toán");
            }
            _unitOfWork.RepositoryCRUD<LadingSchedule>().DeleteWhere(f => f.ShipmentId == shipment.Id && f.ShipmentStatusId == shipment.ShipmentStatusId);
            var backStatus = await _unitOfWork.RepositoryR<LadingSchedule>().FindByAsync(f => f.ShipmentId == shipment.Id && f.ShipmentStatusId != shipment.ShipmentStatusId).OrderByDescending(o => o.Id).FirstOrDefault();
            if (Util.IsNull(backStatus))
            {
                if (shipment.IsReturn == true)
                {
                    shipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.Returning;
                }
                else
                {
                    shipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.Delivering;
                }
            }
            else
            {
                shipment.ShipmentStatusId = backStatus.ShipmentStatusId;
            }
            shipment.RealRecipientName = null;
            shipment.KeepingCODEmpId = null;
            shipment.EndDeliveryTime = null;
            shipment.DeliverUserId = null;
            shipment.EndReturnTime = null;
            shipment.ReturnUserId = null;
            if (shipment.PaymentTypeId == PaymentTypeHelper.NGUOI_NHAN_THANH_TOAN)
                shipment.KeepingTotalPriceEmpId = null;
            await _unitOfWork.CommitAsync();
            return JsonUtil.Success(shipment, "Đã hủy hành trình hàng thành công");
        }

        [HttpPost("AssignDeliveryList")]
        public async Task<JsonResult> AssignDeliveryList([FromBody]ListShipmentUpdateStatusViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return JsonUtil.Error(ModelState);
            }
            var currentUser = GetCurrentUser();
            ListGoods listGoods = null;
            var data = new ListGoodsInfoViewModel();
            var userName = _unitOfWork.RepositoryR<User>().GetSingle(u => u.Id == viewModel.EmpId).FullName;

            try
            {
                List<int> statusIds = new List<int>();
                statusIds.Add(StatusHelper.ShipmentStatusId.ReadyToDelivery);
                statusIds.Add(StatusHelper.ShipmentStatusId.StoreInWarehouseDelivery);
                statusIds.Add(StatusHelper.ShipmentStatusId.AssignEmployeeDelivery);
                if (viewModel.DeliveryOther == true)
                {
                    statusIds.Add(StatusHelper.ShipmentStatusId.StoreInWarehouseTransfer);
                    statusIds.Add(StatusHelper.ShipmentStatusId.StoreInWarehouseReturnTransfer);
                }
                DateTime currentDate = DateTime.Now;
                string message = "";
                int pickupId = viewModel.EmpId;

                // check create or update listGoods
                listGoods = _unitOfWork.RepositoryR<ListGoods>().GetSingle(lg => lg.Id == viewModel.ListGoodsId);
                var isCreate = true;
                if (Util.IsNull(listGoods))
                {
                    //Create BK
                    listGoods = new ListGoods(
                        ListGoodsTypeHelper.BK_CTGH,
                        currentUser.HubId.Value,
                        viewModel.ShipmentStatusId,
                        null,
                        currentUser.HubId.Value,
                        null,
                        pickupId
                    );
                }
                else
                {
                    // check update listGoods
                    isCreate = false;
                    if (listGoods.ListGoodsStatusId != StatusHelper.ShipmentStatusId.AssignEmployeeDelivery)
                    {
                        return JsonUtil.Error("Trạng thái bảng kê không cho phép chỉnh sửa.");
                    }

                    // check item Shipment has statusId = 11 or 12
                    var shipments = _unitOfWork.RepositoryR<Shipment>().FindBy(shipDeliv => viewModel.ShipmentIds.Contains(shipDeliv.Id), shipDeliv => shipDeliv.ShipmentStatus);
                    var isAllowUpdateListGoods = true;
                    var deliveringOrDeliveryComplete = new List<string>();
                    foreach (var item in shipments)
                    {
                        if (item.ShipmentStatusId == StatusHelper.ShipmentStatusId.Delivering || item.ShipmentStatusId == StatusHelper.ShipmentStatusId.DeliveryComplete)
                        {
                            deliveringOrDeliveryComplete.Add(item.ShipmentNumber);
                            isAllowUpdateListGoods = false;
                        }
                    }
                    if (deliveringOrDeliveryComplete != null && deliveringOrDeliveryComplete.Count() > 0)
                    {
                        message += string.Format("Vận đơn {0} đã được nhân viên xác nhận giao hàng trước đó!",
                        string.Join(",", deliveringOrDeliveryComplete));
                    }
                    if (!isAllowUpdateListGoods)
                    {
                        return JsonUtil.Error("Cập nhật Bảng kê sai, vui lòng thử lại!");
                    }

                }

                listGoods.RealWeight = viewModel.RealWeight;
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

                if (isCreate)
                {
                    var listLading = new List<CreateUpdateLadingScheduleViewModel>();
                    var shipments = _unitOfWork.RepositoryR<Shipment>().FindBy(shipDeliv => viewModel.ShipmentIds.Contains(shipDeliv.Id), shipDeliv => shipDeliv.ShipmentStatus);
                    foreach (var item in shipments)
                    {
                        if (!statusIds.Contains(item.ShipmentStatusId))
                        {
                            message += string.Format("Vận đơn {0} đã cập nhật trạng thái {1} trước đó!",
                                item.ShipmentNumber,
                                item.ShipmentStatus.Name);
                            continue;
                        }
                        //Add To Shipping Order
                        item.ShipmentStatusId = (int)StatusHelper.ShipmentStatusId.AssignEmployeeDelivery;
                        item.DeliverUserId = viewModel.EmpId;

                        item.CurrentEmpId = viewModel.EmpId;
                        item.CurrentHubId = currentUser.HubId;
                        item.ListGoodsId = listGoods.Id;
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
                        await _unitOfWork.CommitAsync();

                        if (viewModel != null)
                        {
                            if (!String.IsNullOrWhiteSpace(viewModel.Note))
                            {
                                viewModel.Note = data.Code + "-" + userName + ", " + viewModel.Note;
                            }
                            else
                            {
                                viewModel.Note = data.Code + "-" + userName;
                            }
                        }
                        foreach (var item in shipments)
                        {
                            if (viewModel != null)
                            {
                                listLading.Add(new CreateUpdateLadingScheduleViewModel(
                                    item.Id,
                                    item.ToHubId,
                                    GetCurrentUserId(),
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

                        //SendNotification
                        if (emp != null && !string.IsNullOrWhiteSpace(emp.FireBaseToken))
                        {
                            var count = viewModel.ShipmentIds.Count;
                            await FireBaseUtil.SendNotification(_icompanyInformation.FireBaseBrowserAPIKey, emp.FireBaseToken, string.Format(MessageHelper.REQUEST_TO_DELIVERY, count), count);
                        }
                    }
                    else
                    {
                        _unitOfWork.RepositoryCRUD<ListGoods>().Delete(listGoods.Id);
                        await _unitOfWork.CommitAsync();
                        return JsonUtil.Error(message);
                    }
                }
                else //update
                {
                    //Add thêm vận đơn
                    if (viewModel.AddShipmentIds.Count() > 0)
                    {
                        var listLading = new List<CreateUpdateLadingScheduleViewModel>();
                        var AddShipment = _unitOfWork.RepositoryR<Shipment>().FindBy(shipDeliv => viewModel.AddShipmentIds.Contains(shipDeliv.Id), shipDeliv => shipDeliv.ShipmentStatus);
                        foreach (var item in AddShipment)
                        {
                            if (!statusIds.Contains(item.ShipmentStatusId))
                            {
                                message += string.Format("Vận đơn {0} đã cập nhật trạng thái {1} trước đó!",
                                    item.ShipmentNumber,
                                    item.ShipmentStatus.Name);
                                continue;
                            }
                            //Add To Shipping Order
                            item.ShipmentStatusId = (int)StatusHelper.ShipmentStatusId.AssignEmployeeDelivery;
                            item.DeliverUserId = viewModel.EmpId;

                            item.CurrentEmpId = viewModel.EmpId;
                            item.CurrentHubId = currentUser.HubId;
                            item.ListGoodsId = listGoods.Id;
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
                            await _unitOfWork.CommitAsync();

                            _unitOfWork.Commit();
                            var note = "";
                            if (viewModel != null)
                            {
                                if (!String.IsNullOrWhiteSpace(viewModel.Note))
                                {
                                    note = data.Code + "-" + userName + ", " + viewModel.Note;
                                }
                                else
                                {
                                    note = data.Code + "-" + userName;
                                }
                            }
                            foreach (var item in AddShipment)
                            {
                                if (viewModel != null)
                                {
                                    listLading.Add(new CreateUpdateLadingScheduleViewModel(
                                        item.Id,
                                        item.ToHubId,
                                        GetCurrentUserId(),
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
                    if (viewModel.UnShipmentIds.Count() > 0)
                    {
                        var listLading = new List<CreateUpdateLadingScheduleViewModel>();
                        var UnShipment = _unitOfWork.RepositoryR<Shipment>().FindBy(shipDeliv => viewModel.UnShipmentIds.Contains(shipDeliv.Id), shipDeliv => shipDeliv.ShipmentStatus);
                        foreach (var item in UnShipment)
                        {
                            if (item.ShipmentStatusId != StatusHelper.ShipmentStatusId.AssignEmployeeDelivery)
                            {
                                message += string.Format("Vận đơn {0} đã cập nhật trạng thái {1} trước đó!",
                                    item.ShipmentNumber,
                                    item.ShipmentStatus.Name);
                                continue;
                            }
                            else
                            {
                                if (item.FromHubId != item.ToHubId)
                                {
                                    item.ShipmentStatusId = (int)StatusHelper.ShipmentStatusId.WaitingToTransfer;
                                }
                                else
                                {
                                    item.ShipmentStatusId = (int)StatusHelper.ShipmentStatusId.ReadyToDelivery;
                                }
                            }
                            item.CurrentEmpId = currentUser.Id;
                            item.CurrentHubId = currentUser.HubId;
                            item.DeliverUserId = null;
                            item.ListGoodsId = null;
                            _unitOfWork.RepositoryCRUD<Shipment>().Update(item);
                            _unitOfWork.RepositoryCRUD<ShipmentListGoods>().DeleteWhere(f => f.ListGoodsId == listGoods.Id && f.ShipmentId == item.Id);
                        }
                        if (string.IsNullOrEmpty(message))
                        {
                            _unitOfWork.Commit();
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
                    }
                    //ShipmentInListGood
                    var countShipmentInListGood = 0;
                    if (viewModel.UnShipmentIds.Count() > 0)
                    {
                        var shipmentInListGood = _unitOfWork.RepositoryR<Shipment>().FindBy(shipDeliv => viewModel.ShipmentIds.Contains(shipDeliv.Id) && !viewModel.UnShipmentIds.Contains(shipDeliv.Id), shipDeliv => shipDeliv.ShipmentStatus);
                        countShipmentInListGood = shipmentInListGood.Count();
                        foreach (var item in shipmentInListGood)
                        {
                            item.CurrentEmpId = currentUser.Id;
                            item.CurrentHubId = currentUser.HubId;
                            _unitOfWork.RepositoryCRUD<Shipment>().Update(item);
                        }
                    }
                    //SendNotification
                    var emp = _unitOfWork.RepositoryR<User>().GetSingle(x => x.Id == viewModel.EmpId);

                    if (emp != null && !string.IsNullOrWhiteSpace(emp.FireBaseToken))
                    {
                        var count = viewModel.AddShipmentIds.Count + countShipmentInListGood;
                        await FireBaseUtil.SendNotification(_icompanyInformation.FireBaseBrowserAPIKey, emp.FireBaseToken, string.Format(MessageHelper.REQUEST_TO_DELIVERY, count), count);
                    }
                }
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }

            return JsonUtil.Success(data);
        }

        [HttpPost("AssignUpdateDeliveryList")]
        public async Task<JsonResult> AssignUpdateDeliveryList([FromBody]ListShipmentUpdateStatusViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return JsonUtil.Error(ModelState);
            }

            var currentUser = GetCurrentUser();
            ListGoods listGoods = null;
            if (viewModel.ShipmentStatusId == StatusHelper.ShipmentStatusId.AcceptReturn && string.IsNullOrEmpty(viewModel.Note))
            {
                return JsonUtil.Error("Vui lòng nhập ghi chú!");
            }

            try
            {
                int currentUserId = GetCurrentUserId();
                int[] statusIds =
                {
                    StatusHelper.ShipmentStatusId.ReadyToDelivery,
                    StatusHelper.ShipmentStatusId.WaitingToTransfer,
                    StatusHelper.ShipmentStatusId.StoreInWarehouseDelivery,
                    StatusHelper.ShipmentStatusId.Delivering,
                    StatusHelper.ShipmentStatusId.DeliveryFail,
                    StatusHelper.ShipmentStatusId.AssignEmployeeDelivery,
                    StatusHelper.ShipmentStatusId.AssignEmployeeReturn,
                    StatusHelper.ShipmentStatusId.ReadyToReturn,
                    StatusHelper.ShipmentStatusId.AcceptReturn,
                    StatusHelper.ShipmentStatusId.Returning
                };
                DateTime currentDate = DateTime.Now;
                var user = GetCurrentUser();
                string message = "";
                var ladingSchedules = new List<CreateUpdateLadingScheduleViewModel>();
                int deliveryId = viewModel.EmpId;
                var shipments = _unitOfWork.RepositoryR<Shipment>().FindBy(shipDeliv => viewModel.ShipmentIds.Contains(shipDeliv.Id), shipDeliv => shipDeliv.ShipmentStatus);

                if (viewModel.ShipmentStatusId == StatusHelper.ShipmentStatusId.StoreInWarehouseDelivery)
                {
                    //Create BK
                    listGoods = new ListGoods(
                        ListGoodsTypeHelper.BK_NKGH,
                        currentUser.HubId.Value
                    );
                    _unitOfWork.RepositoryCRUD<ListGoods>().Insert(listGoods);
                    await _unitOfWork.CommitAsync();
                }

                foreach (var item in shipments)
                {
                    if (!statusIds.Contains(item.ShipmentStatusId))
                    {
                        message += string.Format("Vận đơn {0} đã cập nhật trạng thái {1} trước đó!",
                            item.ShipmentNumber,
                            item.ShipmentStatus.Name);
                        continue;
                    }

                    //Add To Shipping Order
                    item.ShipmentStatusId = viewModel.ShipmentStatusId;
                    item.CurrentEmpId = currentUserId;
                    item.CurrentHubId = user.HubId;
                    item.Note = (!string.IsNullOrEmpty(viewModel.Note)) ? $", {viewModel.Note}" : "";
                    item.DeliveryNote = (!string.IsNullOrEmpty(viewModel.Note)) ? $", {viewModel.Note}" : "";
                    item.CusNote = viewModel.CusNote;
                    switch (item.ShipmentStatusId)
                    {
                        case StatusHelper.ShipmentStatusId.DeliveryContinue:
                            {
                                var statusIdNextStep = 0;
                                if (item.IsReturn == false)
                                {
                                    if (GetCurrentUser().HubId == item.ToHubId)
                                    {
                                        statusIdNextStep = StatusHelper.ShipmentStatusId.ReadyToDelivery;
                                    }
                                    else
                                    {
                                        statusIdNextStep = StatusHelper.ShipmentStatusId.WaitingToTransfer;
                                    }
                                }
                                else
                                {
                                    if (GetCurrentUser().HubId == item.FromHubId)
                                    {
                                        statusIdNextStep = StatusHelper.ShipmentStatusId.ReadyToDelivery;
                                    }
                                    else
                                    {
                                        statusIdNextStep = StatusHelper.ShipmentStatusId.WaitingToTransfer;
                                    }
                                }
                                item.StartReturnTime = null;
                                item.ShipmentStatusId = statusIdNextStep;
                                item.IsReturn = false;
                                //
                                if (item.ListCustomerPaymentTotalPriceId.HasValue)
                                {
                                    item.PriceReturn = 0;
                                    var total = item.DefaultPrice + item.FuelPrice + item.VATPrice + item.OtherPrice
                                        + item.TotalDVGT + item.RemoteAreasPrice + item.PriceReturn;
                                    //if (viewModel.dis > 0) total = total - (total * viewModel.DisCount / 100);
                                    if (total.HasValue) item.TotalPrice = Math.Round(total.Value, 0);
                                }
                                //
                                break;
                            }
                        case StatusHelper.ShipmentStatusId.DeliveryLostPackage:
                        case StatusHelper.ShipmentStatusId.DeliveryComplete:
                            {
                                if (item.IsReturn == true)
                                {
                                    if (viewModel.EndDeliveryTime.HasValue)
                                    {
                                        item.EndReturnTime = viewModel.EndDeliveryTime;
                                        currentDate = viewModel.EndDeliveryTime.Value;
                                    }
                                    else
                                    {
                                        item.EndReturnTime = currentDate;
                                    }
                                    item.ShipmentStatusId = StatusHelper.ShipmentStatusId.ReturnComplete;
                                    item.ReturnUserId = deliveryId;
                                }
                                else
                                {
                                    if (viewModel.EndDeliveryTime.HasValue)
                                    {
                                        item.EndDeliveryTime = viewModel.EndDeliveryTime;
                                        currentDate = viewModel.EndDeliveryTime.Value;
                                    }
                                    else
                                    {
                                        item.EndDeliveryTime = currentDate;
                                    }
                                    if (item.PaymentTargetCOD.HasValue) item.PaymentTargetCODConversion = 24 - currentDate.Hour + item.PaymentTargetCOD;
                                    if (item.PaymentTypeId == PaymentTypeHelper.NGUOI_NHAN_THANH_TOAN)
                                    {
                                        item.KeepingTotalPriceEmpId = deliveryId;
                                        var emp = _unitOfWork.RepositoryR<User>().GetSingle(u => u.Id == deliveryId);
                                        item.KeepingTotalPriceHubId = emp.HubId;
                                    }
                                    if (item.COD > 0)
                                        item.KeepingCODEmpId = deliveryId;
                                    //var resUpdate = _unitOfWork.Repository<Proc_UpdateRealExportSAP>()
                                    //    .ExecProcedureSingle(Proc_UpdateRealExportSAP.GetEntityProc(item.Id, 3, item.EndDeliveryTime));
                                }
                                item.RealRecipientName = viewModel.RealRecipientName;

                                break;
                            }
                        case StatusHelper.ShipmentStatusId.StoreInWarehouseDelivery:
                            {
                                //Add link Shipment BK
                                var shipmentListGoods = new ShipmentListGoods(
                                    item.Id,
                                    listGoods.Id
                                );
                                _unitOfWork.RepositoryCRUD<ShipmentListGoods>().Insert(shipmentListGoods);
                                break;
                            }
                        case StatusHelper.ShipmentStatusId.DeliveryFail:
                            {
                                item.NumDeliver++;
                                if (item.NumDeliver == 1) item.FirstDeliveryTime = DateTime.Now;
                                break;
                            }
                        case StatusHelper.ShipmentStatusId.AcceptReturn:
                            {
                                item.IsReturn = true;
                                if (_icompanyInformation.Name == "gsdp" || _icompanyInformation.Name == "gsdp-staging")
                                {
                                    item.COD = 0;
                                    if (Util.IsNull(item.Note)) item.Note = string.Format("COD hoàn: {0} ", item.Note);
                                    else item.Note += string.Format(", COD hoàn: {0} ", item.Note);
                                    //
                                    if (item.ListCustomerPaymentTotalPriceId.HasValue)
                                    {
                                        item.PriceReturn = (item.DefaultPrice * 0.5);
                                        var total = item.DefaultPrice + item.FuelPrice + item.VATPrice + item.OtherPrice
                                            + item.TotalDVGT + item.RemoteAreasPrice + item.PriceReturn;
                                        if (total.HasValue) item.TotalPrice = Math.Round(total.Value, 0);
                                    }
                                }
                                else
                                {
                                    if (item.PaymentTypeId == PaymentTypeHelper.NGUOI_NHAN_THANH_TOAN)
                                    {
                                        item.PaymentTypeId = PaymentTypeHelper.THANH_TOAN_CUOI_THANG;
                                    }
                                    await _iShipmentService.ReCalculatePrice(item.Id);
                                }
                                var statusIdNextStep = 0;
                                if (item.IsReturn == false)
                                {
                                    if (GetCurrentUser().HubId == item.ToHubId)
                                    {
                                        statusIdNextStep = StatusHelper.ShipmentStatusId.ReadyToDelivery;
                                    }
                                    else
                                    {
                                        statusIdNextStep = StatusHelper.ShipmentStatusId.WaitingToTransfer;
                                    }
                                }
                                else
                                {
                                    if (GetCurrentUser().HubId == item.FromHubId)
                                    {
                                        statusIdNextStep = StatusHelper.ShipmentStatusId.ReadyToReturn;
                                    }
                                    else
                                    {
                                        statusIdNextStep = StatusHelper.ShipmentStatusId.WaitingToTransfer;
                                    }
                                }
                                item.ShipmentStatusId = statusIdNextStep;
                                break;
                            }
                        default:
                            {
                                return JsonUtil.Error("Trạng thái cập nhật không hợp lệ");
                            }
                    }
                    _unitOfWork.RepositoryCRUD<Shipment>().Update(item);
                }

                if (string.IsNullOrEmpty(message))
                {
                    _unitOfWork.Commit();
                    foreach (var item in shipments)
                    {
                        if (viewModel != null)
                        {
                            //Lading Request Shipment
                            ladingSchedules.Add(new CreateUpdateLadingScheduleViewModel(
                                item.Id,
                                user.HubId,
                                user.Id,
                                item.ShipmentStatusId,
                                viewModel.CurrentLat,
                                viewModel.CurrentLng,
                                viewModel.Location,
                                viewModel.Note,
                                0, null, currentDate, currentDate
                            ));
                        }
                    }

                    if (ladingSchedules.Count > 0)
                        await _iLadingScheduleService.CreateMDF(ladingSchedules);
                }
                else
                {
                    return JsonUtil.Error(message);
                }
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }

            if (listGoods != null)
                return JsonUtil.Success(await _iListGoodsService.UpdateCode(listGoods));
            else
                return JsonUtil.Success();
        }

        [HttpPost("IssueDelivery")]
        public async Task<JsonResult> IssueDelivery([FromBody]ReceiveShipmentViewModel viewModel)
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
            List<int> listStatusReadyDelivery = new List<int>();
            listStatusReadyDelivery.Add(StatusHelper.ShipmentStatusId.ReadyToDelivery);
            listStatusReadyDelivery.Add(StatusHelper.ShipmentStatusId.ReadyToReturn);
            listStatusReadyDelivery.Add(StatusHelper.ShipmentStatusId.DeliveryContinue);
            listStatusReadyDelivery.Add(StatusHelper.ShipmentStatusId.StoreInWarehouseDelivery);
            listStatusReadyDelivery.Add(StatusHelper.ShipmentStatusId.StoreInWarehousePickup);
            listStatusReadyDelivery.Add(StatusHelper.ShipmentStatusId.StoreInWarehouseReturn);
            //
            List<int> listStatusComplete = new List<int>();
            listStatusComplete.Add(StatusHelper.ShipmentStatusId.DeliveryComplete);
            listStatusComplete.Add(StatusHelper.ShipmentStatusId.ReturnComplete);
            //
            List<int> listStatusReadyTransfer = new List<int>();
            listStatusReadyTransfer.Add(StatusHelper.ShipmentStatusId.WaitingToTransfer);
            listStatusReadyTransfer.Add(StatusHelper.ShipmentStatusId.StoreInWarehouseReturnTransfer);
            listStatusReadyTransfer.Add(StatusHelper.ShipmentStatusId.StoreInWarehouseTransfer);
            //
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
                        }
                        else if (shipmentF.CurrentHubId != currentUser.HubId)
                        {
                            return JsonUtil.Error("Vận đơn đang được BC/Kho khác xử lý, không được phép xuất kho.");
                        }
                        else if (shipmentF.PackageId.HasValue && viewModel.IsPackage == false)
                        {
                            return JsonUtil.Error(string.Format("Vận đơn {0} đã đóng gói, vui lòng thao tác gói.", viewModel.ShipmentNumber));
                        }
                        else if (listStatusComplete.Contains(shipmentF.ShipmentStatusId))
                        {
                            return JsonUtil.Error("Vận đơn đã hoàn tất, không được phép xuất kho.");
                        }
                        else if (shipmentF.ShipmentStatusId == StatusHelper.ShipmentStatusId.AssignEmployeeDelivery)
                        {
                            if (viewModel.ListGoodsId.HasValue && shipmentF.ListGoodsId == viewModel.ListGoodsId)
                            {
                                return JsonUtil.Error("Vẫn đơn đã có trong mã xuất kho, không được phép xuất kho.");
                            }
                        }
                        else if (listStatusReadyDelivery.Contains(shipmentF.ShipmentStatusId) && shipmentF.CurrentHubId == currentUser.HubId)
                        {
                            isScheduleValid = true;
                        }
                        else if (listStatusReadyTransfer.Contains(shipmentF.ShipmentStatusId) && shipmentF.CurrentHubId == currentUser.HubId)
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
                    if (isReturn == true) shipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.AssignEmployeeReturn;
                    else shipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.AssignEmployeeDelivery;
                    shipment.CurrentEmpId = null;
                    shipment.InOutDate = DateTime.Now;
                    //update shipment status
                    var lading = new CreateUpdateLadingScheduleViewModel(
                                            shipment.Id,
                                            currentUser.HubId,
                                            currentUser.Id,
                                            shipment.ShipmentStatusId,
                                            0,
                                            0,
                                            "",
                                            (isScheduleValid == true ? "" : "SCD-ER ") + viewModel.Note,
                                            0,
                                            null,
                                            null,
                                            null,
                                            viewModel.ToUserId
                                        );
                    await _iLadingScheduleService.Create(lading);
                    shipment.ListGoodsId = viewModel.ListGoodsId;
                    _unitOfWork.RepositoryCRUD<Shipment>().Update(shipment);
                    //
                    var shipmentListGoods = new ShipmentListGoods(
                        shipment.Id,
                        viewModel.ListGoodsId.Value
                    );
                    _unitOfWork.RepositoryCRUD<ShipmentListGoods>().Insert(shipmentListGoods);
                    await _unitOfWork.CommitAsync();

                    //var resUpdate = _unitOfWork.Repository<Proc_UpdateRealExportSAP>()
                    //           .ExecProcedureSingle(Proc_UpdateRealExportSAP.GetEntityProc(shipment.Id, 8, null));

                    return JsonUtil.Success(shipment);
                }
                catch (Exception ex)
                {
                    return JsonUtil.Error("Xảy ra lỗi khi xuất kho vận đơn {0}. ", viewModel.ShipmentNumber);
                }
            }
        }
    }
}
