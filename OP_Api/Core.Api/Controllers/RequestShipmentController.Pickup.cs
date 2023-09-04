using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Business.Core.Helpers;
using Core.Business.ViewModels;
using Core.Entity.Entities;
using Core.Entity.Procedures;
using Core.Infrastructure.Helper;
using Core.Infrastructure.Helper.ExceptionHelper;
using Core.Infrastructure.Utils;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Api.Controllers
{
    public partial class RequestShipmentController
    {
        [HttpGet("GetPickupHistory")]
        public JsonResult GetPickupHistory(DateTime fromDate, DateTime toDate)
        {
            var currentUser = GetCurrentUser();
            return JsonUtil.Create(_iRequestShipmentService.GetLadingHistory(currentUser, ShipmentTypeHelper.Pickup, fromDate, toDate));
        }

        [HttpPost("AssignPickupList")]
        public async Task<JsonResult> AssignPickupList([FromBody]ListRequestShipmentUpdateStatusViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return JsonUtil.Error(ModelState);
            }

            try
            {
                int[] statusIds = { StatusHelper.ShipmentStatusId.NewRequest, StatusHelper.ShipmentStatusId.ReadyToPick,
                    StatusHelper.ShipmentStatusId.PickupFail, StatusHelper.ShipmentStatusId.AssignEmployeePickup,
                    StatusHelper.ShipmentStatusId.RejectPickup, StatusHelper.ShipmentStatusId.Idle};
                DateTime currentDate = DateTime.Now;
                var user = GetCurrentUser();
                string message = "";
                var listLading = new List<CreateUpdateLadingScheduleViewModel>();
                int pickupId = viewModel.EmpId;
                var shipments = _unitOfWork.RepositoryR<RequestShipment>().FindBy(r => viewModel.ShipmentIds.Contains(r.Id), r => r.ShipmentStatus);

                foreach (var item in shipments)
                {
                    if (!statusIds.Contains(item.ShipmentStatusId))
                    {
                        message += string.Format("Vận đơn {0} đã cập nhật trạng thái {1} trước đó!",
                            item.ShipmentNumber,
                            item.ShipmentStatus.Name);
                        continue;
                    }
                }

                if (!Util.IsNull(message))
                {
                    return JsonUtil.Error(message);
                }
                foreach (var item in shipments)
                {
                    //Add To Shipping Order
                    item.ShipmentStatusId = (int)StatusHelper.ShipmentStatusId.AssignEmployeePickup;
                    item.AssignPickTime = DateTime.Now;
                    item.PickUserId = viewModel.EmpId;
                    item.CurrentEmpId = viewModel.EmpId;
                    item.CurrentHubId = user.HubId;
                    //if (!item.FirstPickupTime.HasValue) item.FirstPickupTime = currentDate;
                    _unitOfWork.RepositoryCRUD<RequestShipment>().Update(item);
                    if (viewModel != null)
                    {
                        listLading.Add(new CreateUpdateLadingScheduleViewModel(
                            item.Id,
                            item.FromHubId,
                            GetCurrentUserId(),
                            item.ShipmentStatusId,
                            viewModel.CurrentLat,
                            viewModel.CurrentLng,
                            viewModel.Location,
                            viewModel.Note,
                            0
                        ));
                    }
                    //
                    var listShipments = _unitOfWork.RepositoryCRUD<Shipment>().FindBy(f => f.RequestShipmentId == item.Id);
                    if (!Util.IsNull(listShipments))
                    {
                        foreach (var ship in listShipments)
                        {
                            ship.ShipmentStatusId = viewModel.ShipmentStatusId;
                            ship.CurrentHubId = user.HubId;
                            ship.CurrentEmpId = viewModel.EmpId;
                            var ladingShip = new CreateUpdateLadingScheduleViewModel(
                                ship.Id,
                                user.HubId,
                                viewModel.EmpId,
                                viewModel.ShipmentStatusId,
                                viewModel.CurrentLat,
                                viewModel.CurrentLng,
                                viewModel.Location,
                                viewModel.Note,
                                0,
                                viewModel.ReasonId
                            );
                            await _iLadingScheduleService.Create(ladingShip);
                        }
                    }
                    //
                }
                _unitOfWork.Commit();
                if (listLading.Count > 0)
                    await _iRequestLadingScheduleService.Create(listLading);

                var empFireBaseToken = _unitOfWork.RepositoryR<User>().GetSingle((int)viewModel.EmpId).FireBaseToken;
                int count = viewModel.ShipmentIds.Count;
                await FireBaseUtil.SendNotification(_icompanyInformation.FireBaseBrowserAPIKey, empFireBaseToken, string.Format(MessageHelper.REQUEST_TO_PICKUP, count), count);

            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }

            return JsonUtil.Success();
        }

        [HttpPost("AssignUpdatePickupList")]
        public async Task<JsonResult> AssignUpdatePickupList([FromBody]ListRequestShipmentUpdateStatusViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return JsonUtil.Error(ModelState);
            }

            if ((viewModel.ShipmentStatusId == StatusHelper.ShipmentStatusId.PickupFail ||
                 viewModel.ShipmentStatusId == StatusHelper.ShipmentStatusId.PickupCancel ||
                 viewModel.ShipmentStatusId == StatusHelper.ShipmentStatusId.PickupLostPackage) && string.IsNullOrEmpty(viewModel.Note))
            {
                return JsonUtil.Error("Vui lòng nhập ghi chú!");
            }

            var currentUser = GetCurrentUser();
            int currentUserId = GetCurrentUserId();
            ListGoods listGoods = null;

            try
            {
                int statusIdNextStep = 0;
                var shipments = new List<Shipment>();
                var ladingSchedules = new List<CreateUpdateLadingScheduleViewModel>();
                int[] statusIds =
                {
                    StatusHelper.ShipmentStatusId.ReadyToPick,
                    StatusHelper.ShipmentStatusId.PickupComplete,
                    StatusHelper.ShipmentStatusId.Picking,
                    StatusHelper.ShipmentStatusId.AssignEmployeePickup
                };
                DateTime currentDate = DateTime.Now;
                var user = GetCurrentUser();
                string message = "";
                var requestLadingSchedules = new List<CreateUpdateLadingScheduleViewModel>();
                int pickupId = viewModel.EmpId;
                var requestShipments = _unitOfWork.RepositoryR<RequestShipment>().FindBy(r138 => viewModel.ShipmentIds.Contains(r138.Id), r => r.ShipmentStatus);

                if (viewModel.ShipmentStatusId == StatusHelper.ShipmentStatusId.StoreInWarehousePickup)
                {
                    //Create BK
                    listGoods = new ListGoods(
                        ListGoodsTypeHelper.BK_NKLH,
                        currentUser.HubId.Value
                    );

                    _unitOfWork.RepositoryCRUD<ListGoods>().Insert(listGoods);
                    await _unitOfWork.CommitAsync();
                }

                foreach (var item in requestShipments)
                {
                    if (!statusIds.Contains(item.ShipmentStatusId))
                    {
                        message += string.Format("Vận đơn {0} đã cập nhật trạng thái {1} trước đó!",
                            item.ShipmentNumber,
                            item.ShipmentStatus.Name);
                        continue;
                    }

                    if (item.FromHubId != item.ToHubId)
                        statusIdNextStep = StatusHelper.ShipmentStatusId.WaitingToTransfer;
                    else
                        statusIdNextStep = StatusHelper.ShipmentStatusId.ReadyToDelivery;

                    //Add To Shipping Order
                    item.ShipmentStatusId = viewModel.ShipmentStatusId;
                    item.CurrentEmpId = null;
                    item.CurrentHubId = user.HubId;
                    item.Note += (!string.IsNullOrEmpty(viewModel.Note)) ? $", {viewModel.Note}" : "";
                    //item.PickupNote += (!string.IsNullOrEmpty(viewModel.Note)) ? $", {viewModel.Note}" : "";

                    switch (item.ShipmentStatusId)
                    {
                        case StatusHelper.ShipmentStatusId.PickupCancel:
                            {
                                item.EndPickTime = currentDate;
                                break;
                            }
                        case StatusHelper.ShipmentStatusId.PickupLostPackage:
                            {
                                item.EndPickTime = currentDate;
                                break;
                            }
                        case StatusHelper.ShipmentStatusId.StoreInWarehousePickup:
                            {
                                var shipment = _unitOfWork.RepositoryR<Shipment>().GetSingle(rqship => rqship.RequestShipmentId == item.Id);
                                if (shipment != null)
                                {
                                    //Set complete request shipment
                                    item.IsCompleted = true;
                                    if (item.PaymentTypeId == PaymentTypeHelper.NGUOI_GUI_THANH_TOAN)
                                    {
                                        shipment.KeepingTotalPriceEmpId = user.Id;
                                        shipment.KeepingTotalPriceHubId = user.HubId;
                                    }
                                    //
                                    shipment.ShipmentStatusId = statusIdNextStep;
                                    ladingSchedules.Add(new CreateUpdateLadingScheduleViewModel(
                                        shipment.Id,
                                        shipment.FromHubId,
                                        currentUserId,
                                        StatusHelper.ShipmentStatusId.StoreInWarehousePickup,
                                        viewModel.CurrentLat,
                                        viewModel.CurrentLng,
                                        viewModel.Location,
                                        viewModel.Note,
                                        0
                                    ));
                                    //
                                    shipment.ShipmentStatusId = statusIdNextStep;
                                    ladingSchedules.Add(new CreateUpdateLadingScheduleViewModel(
                                        shipment.Id,
                                        shipment.FromHubId,
                                        currentUserId,
                                        statusIdNextStep,
                                        0,
                                        0,
                                        "",
                                        viewModel.Note,
                                        0
                                    ));

                                    _unitOfWork.RepositoryCRUD<Shipment>().Update(shipment);

                                    //Add link Shipment BK
                                    var shipmentListGoods = new ShipmentListGoods(
                                        shipment.Id,
                                        listGoods.Id
                                    );
                                    _unitOfWork.RepositoryCRUD<ShipmentListGoods>().Insert(shipmentListGoods);
                                }
                                break;
                            }
                        default:
                            {
                                return JsonUtil.Error("Trạng thái cập nhật không hợp lệ");
                            }
                    }

                    _unitOfWork.RepositoryCRUD<RequestShipment>().Update(item);
                }

                if (string.IsNullOrEmpty(message))
                {
                    _unitOfWork.Commit();
                    await _iLadingScheduleService.Create(ladingSchedules);

                    foreach (var item in requestShipments)
                    {
                        if (viewModel != null)
                        {
                            //Lading Request Shipment
                            requestLadingSchedules.Add(new CreateUpdateLadingScheduleViewModel(
                                item.Id,
                                item.FromHubId,
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

                    if (requestLadingSchedules.Count > 0)
                        await _iRequestLadingScheduleService.Create(requestLadingSchedules);
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
    }
}
