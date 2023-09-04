using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Business.ViewModels;
using Core.Data;
using Core.Data.Abstract;
using Core.Data.Core;
using Core.Entity.Entities;
using Core.Entity.Procedures;
using Core.Infrastructure.Helper;
using Core.Infrastructure.Utils;
using LinqKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class ListGoodsController : GeneralController<ListGoodsCreateUpdateViewModel, ListGoodsViewModel, ListGoods>
    {
        private readonly IListGoodsService _iListGoodsService;
        private readonly IGeneralService _iGeneralServiceRaw;
        private readonly CompanyInformation _icompanyInformation;
        private ApplicationContextRRP _contextRRP;
        public ListGoodsController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IOptions<JwtIssuerOptions> jwtOptions,
            ApplicationContextRRP contextRRP,
            IUnitOfWork unitOfWork,
            IListGoodsService iListGoodsService,
            IGeneralService iGeneralServiceRaw,
            IOptions<CompanyInformation> companyInformation,
            IGeneralService<ListGoodsCreateUpdateViewModel, ListGoodsViewModel, ListGoods> iGeneralService
        ) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
            _contextRRP = contextRRP;
            _iListGoodsService = iListGoodsService;
            _iGeneralServiceRaw = iGeneralServiceRaw;
            _icompanyInformation = companyInformation.Value;
        }

        public override async Task<JsonResult> Create([FromBody] ListGoodsCreateUpdateViewModel viewModel)
        {
            var currentUser = GetCurrentUser();
            if (!viewModel.FromHubId.HasValue) viewModel.FromHubId = currentUser.HubId;
            viewModel.CreatedByHub = currentUser.HubId.Value;
            var res = await _iGeneralServiceRaw.Create<ListGoods, ListGoodsCreateUpdateViewModel>(viewModel);
            if (res.IsSuccess)
            {
                var listGoods = res.Data as ListGoods;
                if (string.IsNullOrWhiteSpace(listGoods.Code))
                {
                    await _iListGoodsService.UpdateCode(listGoods);
                }
                return JsonUtil.Success(listGoods);
            }
            else
            {
                return JsonUtil.Error("Tạo BK KTC.");
            }
        }

        [HttpPost("UpdateInfo")]
        public async Task<JsonResult> UpdateInfo([FromBody]ListGoodsCreateUpdateViewModel viewModel)
        {
            return JsonUtil.Create(await _iGeneralService.Update(viewModel));
        }

        public async override Task<JsonResult> Update([FromBody]ListGoodsCreateUpdateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return JsonUtil.Error(ModelState);
            }
            var listGoodsShipments = _unitOfWork.RepositoryR<ShipmentListGoods>().FindBy(x => x.ListGoodsId == viewModel.Id).ToList();
            var listGoodsShipmentIds = listGoodsShipments.Select(x => x.ShipmentId);
            var notIncludesListGoodsShipmentIds = listGoodsShipmentIds.Where(x => !viewModel.ShipmentIds.Contains(x));
            var shipments = _unitOfWork.RepositoryR<Shipment>().FindBy(lg => notIncludesListGoodsShipmentIds.Contains(lg.Id));

            foreach (var shipmentId in viewModel.ShipmentIds)
            {
                if (!listGoodsShipmentIds.Contains(shipmentId))
                {
                    var shipment = _unitOfWork.RepositoryR<Shipment>().GetSingle(shipmentId);
                    if (shipment != null && !shipment.ListGoodsId.HasValue)
                    {
                        var slg = new ShipmentListGoods(shipmentId, viewModel.Id);
                        _unitOfWork.RepositoryCRUD<ShipmentListGoods>().Insert(slg);
                        shipment.ListGoodsId = viewModel.Id;
                        _unitOfWork.RepositoryCRUD<Shipment>().Update(shipment);
                    }
                }
            }

            foreach (var item in shipments)
            {
                item.ListGoodsId = null;
                _unitOfWork.RepositoryCRUD<Shipment>().Update(item);
            }

            _unitOfWork.RepositoryCRUD<ShipmentListGoods>().DeleteWhere(x => x.ListGoodsId == viewModel.Id && notIncludesListGoodsShipmentIds.Contains(x.ShipmentId));
            await _unitOfWork.CommitAsync();

            return JsonUtil.Create(await _iGeneralService.Update(viewModel));
        }

        [HttpPost("CreateListGoods")]
        public async Task<JsonResult> CreateListGoods([FromBody]CreateListGoodsViewModel viewModel)
        {
            //Create BK
            var currentUser = this.GetCurrentUser();
            var listGoods = new ListGoods(
                viewModel.TypeWarehousing,
                currentUser.HubId.Value,
                null,
                null,
                currentUser.HubId.Value,
                null,
                currentUser.Id
            );
            if (!String.IsNullOrWhiteSpace(viewModel.Note))
            {
                listGoods.Note = viewModel.Note;
            }
            _unitOfWork.RepositoryCRUD<ListGoods>().Insert(listGoods);
            await _unitOfWork.CommitAsync();
            if (listGoods.Id > 0)
            {
                if (string.IsNullOrWhiteSpace(listGoods.Code))
                {
                    await _iListGoodsService.UpdateCode(listGoods);
                }
                return JsonUtil.Success(listGoods);
            }
            else
            {
                return JsonUtil.Error("Tạo BK KTC.");
            }

        }

        [HttpPost("BlockListGoods")]
        public async Task<JsonResult> BlockListGoods([FromBody]ListGoodsCreateUpdateViewModel viewModel)
        {
            var result = await _iGeneralService.Update(viewModel);
            if (result.IsSuccess == true)
            {
                var listGoods = _unitOfWork.Repository<Proc_BlockListGoods>()
                    .ExecProcedureSingle(Proc_BlockListGoods.GetEntityProc(viewModel.Id, viewModel.EmpId));
                if (listGoods.Result > 0)
                {
                    var emp = _unitOfWork.RepositoryR<User>().GetSingle(x => x.Id == viewModel.EmpId);
                    if (emp != null)
                    {
                        var info = _unitOfWork.Repository<Proc_CheckInfoInListGoods>()
                            .ExecProcedure(Proc_CheckInfoInListGoods.GetEntityProc(viewModel.Id)).FirstOrDefault();
                        if (info != null)
                        {
                            if (info.CountShipment > 0)
                            {
                                if (viewModel.ListGoodsTypeId == 3)//Phát hàng
                                {
                                    await FireBaseUtil.SendNotification(_icompanyInformation.FireBaseBrowserAPIKey, emp.FireBaseToken, string.Format("Có '{0}' vận đơn được phân cho bạn ĐI PHÁT", info.CountShipment), info.CountShipment);
                                }
                                else if (viewModel.ListGoodsTypeId == 8)//trung chuyển
                                {
                                    await FireBaseUtil.SendNotification(_icompanyInformation.FireBaseBrowserAPIKey, emp.FireBaseToken, string.Format("Có {0} vận đươn được phân cho bạn ĐI TRUNG CHUYỂN", info.CountShipment), info.CountShipment);
                                }
                            }
                        }
                    }
                    return JsonUtil.Success("Chốt bảng kê thành công.");
                }
                else
                {
                    return JsonUtil.Error("Chốt bảng kê không thành công!");
                }
            }
            else
            {
                return JsonUtil.Create(result);
            }
        }

        [HttpGet("GetInfoInListGoods")]
        public JsonResult GetInfoInListGoods(int id)
        {
            var listGoods = _unitOfWork.Repository<Proc_CheckInfoInListGoods>()
               .ExecProcedure(Proc_CheckInfoInListGoods.GetEntityProc(id)).FirstOrDefault();
            return JsonUtil.Success(listGoods);
        }

        [HttpGet("GetListGoodsReceive")]
        public JsonResult GetListGoodsReceive(int? fromHubId)
        {
            var currentHubId = GetCurrentUser().HubId;
            var unitOfWordRRP = new UnitOfWorkRRP(_contextRRP);
            var listGoods = unitOfWordRRP.Repository<Proc_GetListGoodsReceive>()
               .ExecProcedure(Proc_GetListGoodsReceive.GetEntityProc(currentHubId, fromHubId));
            return JsonUtil.Success(listGoods);
        }

        [HttpGet("Block")]
        public async Task<JsonResult> Block(int id)
        {
            var listGoods = _unitOfWork.RepositoryR<ListGoods>().GetSingle(x => x.Id == id);
            if (listGoods == null)
                return JsonUtil.Error("Bảng kê không tồn tại!");
            else if (!listGoods.IsBlock)
            {
                listGoods.IsBlock = true;
                _unitOfWork.RepositoryCRUD<ListGoods>().Update(listGoods);
                //Conmmit
                await _unitOfWork.CommitAsync();
            }

            return JsonUtil.Success(listGoods);
        }

        [HttpGet("UnBlock")]
        public async Task<JsonResult> UnBlock(int id)
        {
            var listGoods = _unitOfWork.RepositoryR<ListGoods>().GetSingle(x => x.Id == id);

            if (listGoods == null)
                return JsonUtil.Error("Bảng kê không tồn tại!");
            else if (listGoods.ListGoodsStatusId == ListGoodsStatusHelper.TRANSFER_COMPLETE)
            {
                return JsonUtil.Error($"Không thể mở khoá bảng kê {listGoods.Code} ở trạng thái hoàn tất!");
            }
            else if (listGoods.IsBlock)
            {
                listGoods.IsBlock = false;
                _unitOfWork.RepositoryCRUD<ListGoods>().Update(listGoods);
                //Conmmit
                await _unitOfWork.CommitAsync();
            }

            return JsonUtil.Success(listGoods);
        }

        [HttpPost("Cancel")]
        public async Task<JsonResult> Cancel([FromBody]ListGoodsCancelViewModel viewModel)
        {
            var listGoods = _unitOfWork.RepositoryR<ListGoods>().GetSingle(x => x.Id == viewModel.Id);

            if (listGoods == null)
                return JsonUtil.Error("Bảng kê không tồn tại!");
            else if (listGoods.ListGoodsStatusId == ListGoodsStatusHelper.TRANSFER_COMPLETE)
            {
                return JsonUtil.Error($"Không thể huỷ bảng kê {listGoods.Code} ở trạng thái hoàn tất!");
            }
            else if (listGoods.IsBlock)
            {
                return JsonUtil.Error($"Không thể huỷ bảng kê {listGoods.Code} đã bị khoá!");
            }

            listGoods.CancelTime = DateTime.Now;
            listGoods.CancelNote = viewModel.Note;
            listGoods.ListGoodsStatusId = ListGoodsStatusHelper.TRANSFER_CANCEL;
            _unitOfWork.RepositoryCRUD<ListGoods>().Update(listGoods);

            var listShipment = _unitOfWork.RepositoryR<Shipment>().FindBy(lg => lg.ListGoodsId == viewModel.Id);

            foreach (var shipment in listShipment)
            {
                shipment.ListGoodsId = null;
                _unitOfWork.RepositoryCRUD<Shipment>().Update(shipment);
            }

            //Conmmit
            await _unitOfWork.CommitAsync();

            return JsonUtil.Success(listGoods);
        }

        [HttpPost("GetByType")]
        public JsonResult GetByType([FromBody]GetByTypeViewModel viewModel)
        {
            return JsonUtil.Create(_iGeneralService.FindBy(x => viewModel.Ids.Contains(x.ListGoodsTypeId), cols: viewModel.Cols));
        }

        [HttpGet("GetListGoodsByHubId")]
        public JsonResult GetListGoodsByHubId(int? hubId = null, int? typeId = null)
        {
            var data = _unitOfWork.Repository<Proc_GetListGoodsSendToHub>()
                      .ExecProcedure(Proc_GetListGoodsSendToHub.GetEntityProc(hubId, typeId));
            return JsonUtil.Success(data);
        }

        [HttpGet("GetListGoodsTransferNew")]
        public JsonResult GetListGoodsTransferNew(int toHubId)
        {
            var currentUser = GetCurrentUser();
            return JsonUtil.Create(_iGeneralService.
                FindBy(x202 => x202.ToHubId == toHubId && x202.FromHubId == currentUser.HubId
                && x202.ListGoodsTypeId == ListGoodsTypeHelper.BK_CTTT && x202.ListGoodsStatusId == ListGoodsStatusHelper.READY_TO_TRANSFER
                && x202.IsBlock != true, 20, 1, cols: "ToHub"));
        }

        [HttpGet("GetListGoodsDeliveryNew")]
        public JsonResult GetListGoodsDeliveryNew(int empId)
        {
            var currentUser = GetCurrentUser();
            return JsonUtil.Create(_iGeneralService.
                FindBy(deliverNew => deliverNew.EmpId == empId && deliverNew.FromHubId == currentUser.HubId
                && (deliverNew.ListGoodsTypeId == ListGoodsTypeHelper.BK_CTGH || deliverNew.ListGoodsTypeId == ListGoodsTypeHelper.BK_CTTH)
                && deliverNew.IsBlock != true, 20, 1, cols: "User,Emp,ToHub,FromHub"));
        }

        [HttpPost("AddShipments")]
        public async Task<JsonResult> AddShipments([FromBody]RemoveShipmentViewModel viewModel)
        {
            var listGoods = _unitOfWork.RepositoryR<ListGoods>().GetSingle(x => x.Id == viewModel.Id);

            if (listGoods == null)
                return JsonUtil.Error("Bảng kê không tồn tại!");
            else if (listGoods.ListGoodsStatusId == ListGoodsStatusHelper.TRANSFER_COMPLETE)
            {
                return JsonUtil.Error($"Không thể chỉnh sửa bảng kê {listGoods.Code} ở trạng thái hoàn tất!");
            }
            else if (listGoods.IsBlock)
            {
                return JsonUtil.Error($"Không thể  chỉnh sửa bảng kê {listGoods.Code} đã bị khoá!");
            }
            //Luu Modified User, Modified Time
            _unitOfWork.RepositoryCRUD<ListGoods>().Update(listGoods);

            //Tim list shipment va xoa khoi bang ke history
            var listShipmentListGoodsIds = _unitOfWork.RepositoryR<ShipmentListGoods>().FindBy(x => x.ListGoodsId == listGoods.Id).Select(x => x.ShipmentId).ToArray();

            //Tim list shipment
            var listShipment = _unitOfWork.RepositoryR<Shipment>().FindBy(lg => viewModel.ShipmentIds.Contains(lg.Id));
            foreach (var shipment in listShipment)
            {
                shipment.ListGoodsId = null;
                _unitOfWork.RepositoryCRUD<Shipment>().Update(shipment);

                if (!listShipmentListGoodsIds.Contains(shipment.Id))
                {
                    var shipmentListGoods = new ShipmentListGoods(shipment.Id, listGoods.Id);
                    _unitOfWork.RepositoryCRUD<ShipmentListGoods>().Insert(shipmentListGoods);
                }
            }

            //Conmmit
            await _unitOfWork.CommitAsync();

            return JsonUtil.Success(listGoods);
        }

        [HttpPost("RemoveShipments")]
        public async Task<JsonResult> RemoveShipments([FromBody]RemoveShipmentViewModel viewModel)
        {
            var listGoods = _unitOfWork.RepositoryR<ListGoods>().GetSingle(x => x.Id == viewModel.Id);

            if (listGoods == null)
                return JsonUtil.Error("Bảng kê không tồn tại!");
            else if (listGoods.ListGoodsStatusId == ListGoodsStatusHelper.TRANSFER_COMPLETE)
            {
                return JsonUtil.Error($"Không thể chỉnh sửa bảng kê {listGoods.Code} ở trạng thái hoàn tất!");
            }
            else if (listGoods.IsBlock)
            {
                return JsonUtil.Error($"Không thể  chỉnh sửa bảng kê {listGoods.Code} đã bị khoá!");
            }
            //Luu Modified User, Modified Time
            _unitOfWork.RepositoryCRUD<ListGoods>().Update(listGoods);

            //Tim list shipment va xoa khoi bang ke
            var listShipment = _unitOfWork.RepositoryR<Shipment>().FindBy(lg => viewModel.ShipmentIds.Contains(lg.Id));
            foreach (var shipment in listShipment)
            {
                shipment.ListGoodsId = null;
                _unitOfWork.RepositoryCRUD<Shipment>().Update(shipment);
            }

            //Tim list shipment va xoa khoi bang ke history
            _unitOfWork.RepositoryCRUD<ShipmentListGoods>().DeleteWhere(x => viewModel.ShipmentIds.Contains(x.Id) && x.ListGoodsId == listGoods.Id);

            //Conmmit
            await _unitOfWork.CommitAsync();

            return JsonUtil.Success(listGoods);
        }

        [HttpGet("GetListGoods")]
        public JsonResult GetListGoods(int? typeId = null, int? createByHubId = null, int? fromHubId = null, int? toHubId = null,
            int? userId = null, int? statusId = null, int? transportTypeId = null, int? tplId = null, DateTime? dateFrom = null, DateTime? dateTo = null, string listGoodsCode = null)
        {
            if (!string.IsNullOrWhiteSpace(listGoodsCode))
            {
                if (listGoodsCode.ToLower() == "null" || listGoodsCode.ToLower() == "undefined") listGoodsCode = null;
            }
            var data = _unitOfWork.Repository<Proc_ReportListGoods>()
                      .ExecProcedure(Proc_ReportListGoods.GetEntityProc(typeId, createByHubId, fromHubId, toHubId,
                      userId, statusId, transportTypeId, tplId, dateFrom, dateTo, listGoodsCode));
            return JsonUtil.Success(data);
        }

        [HttpGet("GetListGoodsDetail")]
        public JsonResult GetListGoodsDetail(int? typeId = null, int? createByHubId = null, int? fromHubId = null, int? toHubId = null,
            int? userId = null, int? statusId = null, int? transportTypeId = null, int? tplId = null, DateTime? dateFrom = null, DateTime? dateTo = null,
            string listGoodsCode = null, int? pageNumber = null, int? pageSize = null, string listIds = null)
        {
            if (!string.IsNullOrWhiteSpace(listGoodsCode))
            {
                if (listGoodsCode.ToLower() == "null" || listGoodsCode.ToLower() == "undefined") listGoodsCode = null;
            }
            var data = _unitOfWork.Repository<Proc_ReportListGoodsDetail>()
                      .ExecProcedure(Proc_ReportListGoodsDetail.GetEntityProc(typeId, createByHubId, fromHubId, toHubId,
                      userId, statusId, transportTypeId, tplId, dateFrom, dateTo, listGoodsCode, pageNumber, pageSize, listIds));
            return JsonUtil.Success(data);
        }

        [HttpPost("GetListGoodsByStatusIdsAndFromHubId")]
        public JsonResult GetListGoodsByStatusIdsAndFromHubId([FromBody]ListGoodsStatusIdsFilterModel viewModel)
        {
            var listGoods = _unitOfWork.RepositoryR<ListGoods>().FindBy(f => viewModel.statusIds.Contains(f.ListGoodsStatusId) && f.FromHubId == viewModel.fromHubId);
            var listGoodsSelected = listGoods.Select(s => new SimpleSelectModel
            {
                Id = s.Id,
                Code = s.Code
            }
            );
            return JsonUtil.Success(listGoodsSelected);
        }

        [HttpGet("GetReportBroadcastListGoodsByAppAndMobil")]
        public JsonResult GetReportBroadcastListGoodsByAppAndMobil(int? hubId = null, int? empId = null, DateTime? dateFrom = null, DateTime? dateTo = null, string shipmentNumber = null)
        {
            var data = _unitOfWork.Repository<Proc_GetReportBroadcastListGoodsByAppAndMobil>()
                        .ExecProcedure(Proc_GetReportBroadcastListGoodsByAppAndMobil.GetEntityProc(hubId, empId, dateFrom, dateTo, shipmentNumber));
            return JsonUtil.Success(data);
        }

        [HttpPost("PostByType")]
        public JsonResult PostByType([FromBody] ListGoodsFilterViewModel filterViewModel = null)
        {
            return JsonUtil.Create(_iListGoodsService.PostByType(filterViewModel.type, filterViewModel.pageSize, filterViewModel.pageNumber, filterViewModel.cols, filterViewModel));
        }

        [HttpPost("GetListGoodsByTruckScheduleId")]
        public JsonResult GetListGoodsByTruckScheduleId([FromBody]GetByIdViewModel viewModel)
        {
            var shipmentIds = _unitOfWork.RepositoryR<ListGoods>().FindBy(x => x.TruckScheduleId == viewModel.Id).ToArray();
            return JsonUtil.Success(_iGeneralService.FindBy(x => shipmentIds.Contains(x), cols: viewModel.Cols));
        }
    }
}
