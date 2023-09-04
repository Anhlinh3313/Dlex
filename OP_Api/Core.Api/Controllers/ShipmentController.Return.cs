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
        [HttpGet("GetReturnHistory")]
        public JsonResult GetReturnHistory(DateTime fromDate, DateTime toDate)
        {
            return JsonUtil.Create(_iShipmentService.GetLadingHistory(GetCurrentUser(), ShipmentTypeHelper.Return, fromDate, toDate));
        }

        [HttpPost("AssignReturnList")]
        public async Task<JsonResult> AssignReturnList([FromBody]ListShipmentUpdateStatusViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return JsonUtil.Error(ModelState);
            }

            var currentUser = GetCurrentUser();
            ListGoods listGoods = null;

            try
            {
                int[] statusIds = {  StatusHelper.ShipmentStatusId.StoreInWarehouseReturn, StatusHelper.ShipmentStatusId.AssignEmployeeReturn };
                DateTime currentDate = DateTime.Now;
                var user = GetCurrentUser();
                string message = "";
                var listLading = new List<CreateUpdateLadingScheduleViewModel>();
                int pickupId = viewModel.EmpId;
                var shipments = _unitOfWork.RepositoryR<Shipment>().FindBy(shipReturn => viewModel.ShipmentIds.Contains(shipReturn.Id), shipReturn => shipReturn.ShipmentStatus);
                //Create BK
                listGoods = new ListGoods(
                    ListGoodsTypeHelper.BK_CTTH,
                    currentUser.HubId.Value,
                    null,
                    null,
                    currentUser.HubId,
                    null,
                    viewModel.EmpId
                );
                _unitOfWork.RepositoryCRUD<ListGoods>().Insert(listGoods);
                await _unitOfWork.CommitAsync();

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
                    item.ShipmentStatusId = (int)StatusHelper.ShipmentStatusId.AssignEmployeeReturn;
                    item.ReturnUserId = viewModel.EmpId;
                    item.CurrentEmpId = viewModel.EmpId;
                    item.CurrentHubId = user.HubId;
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

            return JsonUtil.Success(await _iListGoodsService.UpdateCode(listGoods));
        }

        [HttpPost("AssignUpdateReturnList")]
        public async Task<JsonResult> AssignUpdateReturnList([FromBody]ListShipmentUpdateStatusViewModel viewModel)
        {
            if (viewModel.ShipmentStatusId == StatusHelper.ShipmentStatusId.ReturnLostPackage && string.IsNullOrEmpty(viewModel.Note))
            {
                return JsonUtil.Error("Vui lòng nhập ghi chú!");
            }
            var currentUser = GetCurrentUser();
            ListGoods listGoods = null;
            try
            {
                int c = GetCurrentUserId();
                int[] statusIds = { StatusHelper.ShipmentStatusId.AssignEmployeeReturn, StatusHelper.ShipmentStatusId.Returning, StatusHelper.ShipmentStatusId.AcceptReturn };
                DateTime currentDate = DateTime.Now;
                var user = GetCurrentUser();
                string message = "";
                var ladingSchedules = new List<CreateUpdateLadingScheduleViewModel>();
                var shipments = _unitOfWork.RepositoryR<Shipment>().FindBy(shipReturn => viewModel.ShipmentIds.Contains(shipReturn.Id), shipReturn => shipReturn.ShipmentStatus);
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
                    item.CurrentEmpId = null;
                    item.CurrentHubId = user.HubId;
                    switch (item.ShipmentStatusId)
                    {
                        case StatusHelper.ShipmentStatusId.ReturnLostPackage:
                            {
                                item.EndReturnTime = currentDate;
                                break;
                            }
                        case StatusHelper.ShipmentStatusId.ReturnComplete:
                            {
                                item.EndReturnTime = currentDate;
                                break;
                            }
                        case StatusHelper.ShipmentStatusId.ReturnFail:
                            {
                                break;
                            }
                        case StatusHelper.ShipmentStatusId.AcceptReturn:
                            {
                                item.IsReturn = true;
                                if(_icompanyInformation.Name=="dlex"|| _icompanyInformation.Name == "gsdp")
                                {
                                    item.COD = 0;
                                    if(Util.IsNull(item.Note))item.Note = string.Format("COD hoàn: {0} ",item.Note);
                                    else item.Note += string.Format(", COD hoàn: {0} ", item.Note);
                                }
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

                    if (ladingSchedules.Count > 0)
                        await _iLadingScheduleService.Create(ladingSchedules);
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

        [HttpPost("CancelReturn")]
        public async Task<JsonResult> CancelReturn([FromBody]BasicViewModel viewModel)
        {
            var user = GetCurrentUser();
            var shipment = _unitOfWork.RepositoryR<Shipment>().GetSingle(viewModel.Id);
            if (shipment.ShipmentStatusId != StatusHelper.ShipmentStatusId.DeliveryComplete && shipment.ShipmentStatusId != StatusHelper.ShipmentStatusId.ReturnComplete && shipment.IsReturn)
            {
                shipment.IsReturn = false;
                shipment.CurrentEmpId = user.Id;
                shipment.CurrentHubId = user.HubId;
                if (shipment.ToHubId == shipment.CurrentHubId)
                {
                    shipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.ReadyToDelivery;
                }
                else
                {
                    shipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.WaitingToTransfer;
                }

                var lading = new CreateUpdateLadingScheduleViewModel(
                    shipment.Id,
                    shipment.FromHubId,
                    shipment.CurrentEmpId.Value,
                    shipment.ShipmentStatusId,
                    0,
                    0,
                    "",
                    "",
                    0
                );
                await _iLadingScheduleService.Create(lading);
                return JsonUtil.Success(await _iGeneralServiceRaw.Update<Shipment>(shipment));
            } else
            {
                return JsonUtil.Error("Đã có lỗi xảy ra");
            }
        }
    }
}
