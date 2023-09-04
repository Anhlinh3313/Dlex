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
using Core.Infrastructure.Extensions;
using Core.Entity.Procedures;
using Core.Business.ViewModels.General;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Api.Controllers
{
    public partial class ShipmentController
    {
        [HttpPost("UnAssign")]
        public JsonResult UnAssign([FromBody]UnAssignShipmentListGoodsViewModel viewModel)
        {
            try
            {
                var shipment = _unitOfWork.RepositoryCRUD<Shipment>().GetSingle(viewModel.ShipmentId);
                if (Util.IsNull(shipment)) return JsonUtil.Error("Không tìm thấy thông tin vận đơn!");
                var listGoods = _unitOfWork.RepositoryR<ListGoods>().GetSingle(viewModel.ListGoodsId);
                if (Util.IsNull(listGoods)) return JsonUtil.Error("Không tìm thấy thông tin bảng kê!");
                List<int> listAllowInAssigns = new List<int>();
                listAllowInAssigns.Add(StatusHelper.ShipmentStatusId.AssignEmployeeTransfer);
                listAllowInAssigns.Add(StatusHelper.ShipmentStatusId.AssignEmployeeTransferReturn);
                listAllowInAssigns.Add(StatusHelper.ShipmentStatusId.AssignEmployeeDelivery);
                listAllowInAssigns.Add(StatusHelper.ShipmentStatusId.AssignEmployeeReturn);
                listAllowInAssigns.Add(StatusHelper.ShipmentStatusId.Delivering);
                listAllowInAssigns.Add(StatusHelper.ShipmentStatusId.Returning);
                listAllowInAssigns.Add(StatusHelper.ShipmentStatusId.Transferring);
                listAllowInAssigns.Add(StatusHelper.ShipmentStatusId.TransferReturning);
                if (!listAllowInAssigns.Contains(shipment.ShipmentStatusId))
                {
                    return JsonUtil.Error("Trạng thái vận đơn không cho phép gỡ!");
                }
                if (shipment.IsReturn == false)
                {
                    if (GetCurrentUser().HubId == shipment.ToHubId)
                    {
                        shipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.ReadyToDelivery;
                    }
                    else
                    {
                        shipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.WaitingToTransfer;
                    }
                }
                else
                {
                    if (GetCurrentUser().HubId == shipment.FromHubId)
                    {
                        shipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.ReadyToDelivery;
                    }
                    else
                    {
                        shipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.WaitingToTransfer;
                    }
                }
                var currentUser = GetCurrentUser();
                shipment.CurrentHubId = currentUser.HubId;
                shipment.CurrentEmpId = null;
                var ladingWarehouseExistShip = new LadingSchedule(
                                    shipment.Id,
                                    currentUser.HubId,
                                    null,
                                    currentUser.Id,
                                    shipment.ShipmentStatusId,
                                    0,
                                    0,
                                    "",
                                    "GỠ VĐ",
                                    0
                                );
                _unitOfWork.RepositoryCRUD<LadingSchedule>().Insert(ladingWarehouseExistShip);
                _unitOfWork.RepositoryCRUD<ShipmentListGoods>().DeleteWhere(f => f.ListGoodsId == viewModel.ListGoodsId && f.ShipmentId == viewModel.ShipmentId);
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                return JsonUtil.Error("Gỡ vận đơn không thành công, vui lòng kiểm tra lại!");
            }
            return JsonUtil.Success("Gỡ vận đơn thành công!");
        }

        [HttpPost("GetByListGoodsId")]
        public JsonResult GetByListGoodsId([FromBody]GetByIdViewModel viewModel)
        {
            if (viewModel.PageSize == 0) viewModel.PageSize = null;
            if (viewModel.PageNumber == 0) viewModel.PageNumber = null;
            var shipmentIds = _unitOfWork.RepositoryR<ShipmentListGoods>().FindBy(x => x.ListGoodsId == viewModel.Id).Select(x => x.ShipmentId).ToArray();
            return JsonUtil.Create(_iGeneralService.FindBy(x => shipmentIds.Contains(x.Id)
            && (!viewModel.ShipmentStatusId.HasValue || x.ShipmentStatusId == viewModel.ShipmentStatusId)
            && (viewModel.IsHideInPackage == false || (viewModel.IsHideInPackage == true && !x.PackageId.HasValue)),
            viewModel.PageSize, viewModel.PageNumber, cols: viewModel.Cols));
        }

        [HttpPost("GetByListGoodsIds")]
        public JsonResult GetByListGoodsIds([FromBody]GetByIdsViewModel viewModel)
        {
            if (viewModel.PageSize == 0) viewModel.PageSize = null;
            if (viewModel.PageNumber == 0) viewModel.PageNumber = null;
            var shipmentIds = _unitOfWork.RepositoryR<ShipmentListGoods>().FindBy(x => viewModel.Ids.Contains(x.ListGoodsId)).Select(x => x.ShipmentId).ToArray();
            return JsonUtil.Create(_iGeneralService.FindBy(x => shipmentIds.Contains(x.Id)
            && (!viewModel.ShipmentStatusId.HasValue || x.ShipmentStatusId == viewModel.ShipmentStatusId)
            && (viewModel.IsHideInPackage == false || (viewModel.IsHideInPackage == true && !x.PackageId.HasValue)),
            viewModel.PageSize, viewModel.PageNumber, cols: viewModel.Cols));
        }

        [HttpPost("GetByListListGoodsId")]
        public JsonResult GetByListListGoodsId([FromBody]GetByIdsViewModel viewModel)
        {
            var listGoodsIds = String.Join(",", viewModel.Ids);
            var data = _unitOfWork.Repository<Proc_GetReportShipmentsDeliveryByListGoodsIds>()
                      .ExecProcedure(Proc_GetReportShipmentsDeliveryByListGoodsIds.GetEntityProc(listGoodsIds));
            if (data.Count() == 0) return JsonUtil.Error("Không tìm thấy đơn hàng");
            return JsonUtil.Success(data);
        }
        //
        [HttpPost("GetByListGoodsIdProcs")]
        public JsonResult GetByListGoodsIdProcs([FromBody]GetByIdsViewModel viewModel)
        {
            //
            var listGoodsIds = viewModel.Ids.Count() > 0 ? String.Join(",", viewModel.Ids) : "";
            var data = _unitOfWork.Repository<Proc_GetReportShipmentsDeliveryByListGoodsIdsProc>()
                      .ExecProcedure(Proc_GetReportShipmentsDeliveryByListGoodsIdsProc.GetEntityProc(listGoodsIds, viewModel.ShipmentStatusId,
                      viewModel.PageNumber, viewModel.PageSize));
            if (data.Count() == 0) return JsonUtil.Error("Không tìm thấy đơn hàng");
            return JsonUtil.Success(data);
        }
        //
        [HttpGet("GetDeliveryAndHubRouting")]
        public JsonResult GetDeliveryAndHubRouting(int listGoodsId)
        {
            var data = _unitOfWork.Repository<Proc_GetDeliveryAndHubRouting>()
                      .ExecProcedure(Proc_GetDeliveryAndHubRouting.GetEntityProc(listGoodsId));
            if (data.Count() == 0) return JsonUtil.Error(string.Format("Không tìm thấy thông tin theo: '{0}'", listGoodsId));
            return JsonUtil.Success(data);
        }

        [HttpPost("GetByListGoodsCode")]
        public async Task<JsonResult> GetByListGoodsNumber([FromBody]GetByListGoodsCodeViewModel viewModel)
        {
            var listGoods = await _unitOfWork.RepositoryR<ListGoods>().GetSingleAsync(x => x.Code == viewModel.Code);

            if (listGoods == null)
            {
                return JsonUtil.Error("Không tìm thấy mã bảng kê");
            }

            if (viewModel.StatusIds != null)
                return JsonUtil.Create(_iGeneralService.FindBy(x => x.ListGoodsId == listGoods.Id && viewModel.StatusIds.Contains(x.ShipmentStatusId), cols: viewModel.Cols));
            else
                return JsonUtil.Create(_iGeneralService.FindBy(x => x.ListGoodsId == listGoods.Id, cols: viewModel.Cols));
        }
    }
}
