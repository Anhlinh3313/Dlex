using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Business.ViewModels;
using Core.Business.ViewModels.Shipments;
using Core.Entity.Entities;
using Core.Infrastructure.Helper;
using Core.Infrastructure.Helper.ExceptionHelper;
using Core.Infrastructure.Utils;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Api.Controllers
{
    public partial class ShipmentController
    {
        #region HubConfirmMoneyFromRider
        [HttpGet("GetHubConfirmMoneyFromRiderHistory")]
        public JsonResult GetHubConfirmMoneyFromRiderHistory(DateTime fromDate, DateTime toDate)
        {
            return JsonUtil.Create(_iShipmentService.GetLadingHistory(GetCurrentUser(), ShipmentTypeHelper.HubConfirmMoneyFromRider, fromDate, toDate));
        }

        [HttpPost("HubConfirmMoneyFromRider")]
        public async Task<JsonResult> HubConfirmMoneyFromRider([FromBody]ListShipmentUpdateStatusViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return JsonUtil.Error(ModelState);
            }

            try
            {
                int[] statusIds = { StatusHelper.ShipmentStatusId.DeliveryComplete };
                DateTime currentDate = DateTime.Now;
                var user = GetCurrentUser();
                string message = "";
                var listLading = new List<CreateUpdateLadingScheduleViewModel>();
                var shipments = _unitOfWork.RepositoryR<Shipment>().FindBy(shipCod => viewModel.ShipmentIds.Contains(shipCod.Id), shipCod => shipCod.ShipmentStatus);

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
                    item.DMTransferCODToHubTime = currentDate;
                    item.ShipmentStatusId = StatusHelper.ShipmentStatusId.HubReceivedCOD;
                    item.CurrentEmpId = null;
                    item.CurrentHubId = user.HubId;

                    if (item.PaymentTypeId == PaymentTypeHelper.NGUOI_GUI_THANH_TOAN)
                    {
                        item.KeepingTotalPriceEmpId = user.Id;
                        item.KeepingTotalPriceHubId = user.HubId;

                    }
                    else if (item.PaymentTypeId == PaymentTypeHelper.NGUOI_NHAN_THANH_TOAN)
                    {
                        item.KeepingTotalPriceEmpId = user.Id;
                        item.KeepingTotalPriceHubId = user.HubId;
                        item.KeepingCODEmpId = user.Id;
                        item.KeepingCODHubId = user.HubId;
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
                            listLading.Add(new CreateUpdateLadingScheduleViewModel(
                                item.Id,
                                item.FromHubId,
                                user.Id,
                                StatusHelper.ShipmentStatusId.HubReceivedCOD,
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
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }

            return JsonUtil.Success();
        }
        #endregion

        #region AccountantConfirmMoneyFromHub
        [HttpGet("GetAccountantConfirmMoneyFromHubHistory")]
        public JsonResult GetAccountantConfirmMoneyFromHubHistory(DateTime fromDate, DateTime toDate)
        {
            return JsonUtil.Create(_iShipmentService.GetLadingHistory(GetCurrentUser(), ShipmentTypeHelper.AccountantConfirmMoneyFromHub, fromDate, toDate));
        }

        [HttpPost("AccountantConfirmMoneyFromHub")]
        public async Task<JsonResult> AccountantConfirmMoneyFromHub([FromBody]ListShipmentUpdateStatusViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return JsonUtil.Error(ModelState);
            }

            try
            {
                int[] statusIds = { StatusHelper.ShipmentStatusId.HubReceivedCOD };
                DateTime currentDate = DateTime.Now;
                var user = GetCurrentUser();
                string message = "";
                var listLading = new List<CreateUpdateLadingScheduleViewModel>();
                var shipments = _unitOfWork.RepositoryR<Shipment>().FindBy(shipCod => viewModel.ShipmentIds.Contains(shipCod.Id), shipCod => shipCod.ShipmentStatus);

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
                    item.DMTransferCODToHubTime = currentDate;
                    item.ShipmentStatusId = StatusHelper.ShipmentStatusId.AccountantReceivedCOD;
                    item.CurrentEmpId = null;
                    item.CurrentHubId = user.HubId;
                    item.KeepingCODEmpId = user.Id;
                    item.KeepingCODHubId = user.HubId;
                    item.KeepingTotalPriceEmpId = user.Id;
                    item.KeepingTotalPriceHubId = user.HubId;

                    _unitOfWork.RepositoryCRUD<Shipment>().Update(item);
                }

                if (string.IsNullOrEmpty(message))
                {
                    _unitOfWork.Commit();

                    foreach (var item in shipments)
                    {
                        if (viewModel != null)
                        {
                            listLading.Add(new CreateUpdateLadingScheduleViewModel(
                                item.Id,
                                item.FromHubId,
                                user.Id,
                                StatusHelper.ShipmentStatusId.HubReceivedCOD,
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
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }

            return JsonUtil.Success();
        }
        #endregion

        #region AccountantConfirmMoneyFromHub
        [HttpGet("GetTreasurerConfirmMoneyFromAccountantHistory")]
        public JsonResult GetTreasurerConfirmMoneyFromAccountantHubHistory(DateTime fromDate, DateTime toDate)
        {
            return JsonUtil.Create(_iShipmentService.GetLadingHistory(GetCurrentUser(), ShipmentTypeHelper.TreasurerConfirmMoneyFromAccountant, fromDate, toDate));
        }

        [HttpPost("TreasurerConfirmMoneyFromAccountant")]
        public async Task<JsonResult> TreasurerConfirmMoneyFromAccountant([FromBody]ListShipmentUpdateStatusViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return JsonUtil.Error(ModelState);
            }

            try
            {
                int[] statusIds = { StatusHelper.ShipmentStatusId.HubReceivedCOD };
                DateTime currentDate = DateTime.Now;
                var user = GetCurrentUser();
                string message = "";
                var listLading = new List<CreateUpdateLadingScheduleViewModel>();
                var shipments = _unitOfWork.RepositoryR<Shipment>().FindBy(shipCod => viewModel.ShipmentIds.Contains(shipCod.Id), shipCod => shipCod.ShipmentStatus);

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
                    item.DMTransferCODToHubTime = currentDate;
                    item.ShipmentStatusId = StatusHelper.ShipmentStatusId.TreasurertReceivedCOD;
                    item.CurrentEmpId = null;
                    item.CurrentHubId = user.HubId;
                    item.KeepingCODEmpId = user.Id;
                    item.KeepingCODHubId = user.HubId;
                    item.KeepingTotalPriceEmpId = user.Id;
                    item.KeepingTotalPriceHubId = user.HubId;

                    _unitOfWork.RepositoryCRUD<Shipment>().Update(item);
                }

                if (string.IsNullOrEmpty(message))
                {
                    _unitOfWork.Commit();

                    foreach (var item in shipments)
                    {
                        if (viewModel != null)
                        {
                            listLading.Add(new CreateUpdateLadingScheduleViewModel(
                                item.Id,
                                item.FromHubId,
                                user.Id,
                                StatusHelper.ShipmentStatusId.HubReceivedCOD,
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
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }

            return JsonUtil.Success();
        }
        #endregion
    }
}
