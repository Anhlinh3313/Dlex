using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Business.ViewModels;
using Core.Data.Abstract;
using Core.Entity.Entities;
using Core.Infrastructure.Helper;
using Core.Infrastructure.Helper.ExceptionHelper;
using Core.Infrastructure.Utils;
using Core.Infrastructure.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Core.Entity.Procedures;
using Core.Business.Core.Helpers;
using System.Linq.Expressions;
using LinqKit;
using Core.Data;
using Microsoft.EntityFrameworkCore;
using Core.Data.Core;
using Core.Business.ViewModels.General;
using Core.Business.ViewModels.Shipments;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public partial class RequestShipmentController : GeneralController<CreateUpdateRequestShipmentViewModel, RequestShipmentInfoViewModel, RequestShipment>
    {
        private readonly IGeneralService<CreateUpdateLadingScheduleViewModel, RequestLadingSchedule> _iRequestLadingScheduleService;
        private readonly IRequestShipmentService _iRequestShipmentService;
        private readonly IShipmentService _iShipmentService;
        private readonly IGeneralService<CreateUpdateLadingScheduleViewModel, LadingSchedule> _iLadingScheduleService;
        private readonly IGeneralService _iGeneralServiceRaw;
        private readonly IListGoodsService _iListGoodsService;
        private readonly IHubService _iHubService;
        private readonly IUserService _iuserService;
        private readonly CompanyInformation _icompanyInformation;
        private ApplicationContext _context;

        public RequestShipmentController(
            ApplicationContext context,
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IOptions<JwtIssuerOptions> jwtOptions,
            IOptions<CompanyInformation> companyInformation,
            IUnitOfWork unitOfWork,
            IGeneralService<CreateUpdateRequestShipmentViewModel, RequestShipmentInfoViewModel, RequestShipment> iGeneralService,
            IGeneralService iGeneralServiceRaw,
            IGeneralService<CreateUpdateLadingScheduleViewModel, RequestLadingSchedule> iRequestLadingScheduleService,
            IGeneralService<CreateUpdateLadingScheduleViewModel, LadingSchedule> iLadingScheduleService,
            IRequestShipmentService iRequestShipmentService,
            IShipmentService iShipmentService,
            IListGoodsService iListGoodsService,
            IUserService iuserService,
            IHubService iHubService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
            _context = context;
            _iRequestLadingScheduleService = iRequestLadingScheduleService;
            _iLadingScheduleService = iLadingScheduleService;
            _iRequestShipmentService = iRequestShipmentService;
            _iShipmentService = iShipmentService;
            _iGeneralServiceRaw = iGeneralServiceRaw;
            _iListGoodsService = iListGoodsService;
            _iHubService = iHubService;
            _iuserService = iuserService;
            _icompanyInformation = companyInformation.Value;
        }

        [HttpGet("GetByType")]
        public JsonResult GetByType(string type, int? pageSize = null, int? pageNumber = null, string cols = null)
        {
            return JsonUtil.Create(_iRequestShipmentService.GetByType(GetCurrentUser(), type, pageSize, pageNumber, cols));
        }

        [HttpPost("PostByTypeByProc")]
        public ResponseOfRequestShipmentViewModel PostByTypeByProc([FromBody]RequestShipmentFilterViewModel rf)
        {
            var user = GetCurrentUser();
            var listHub = _iuserService.GetListHubFromUserByProc(user);
            var listHubIds = String.Join(",", listHub);
            var data = _unitOfWork.Repository<Proc_GetListRequestShipmentByFilter>()
                      .ExecProcedure(Proc_GetListRequestShipmentByFilter.GetEntityProc(
                          user.Id,
                          listHubIds,
                          rf.Type,
                          rf.TypePickup,
                          rf.IsEnabled,
                          rf.ShipmentNumber,
                          rf.OrderDateFrom,
                          rf.OrderDateTo,
                          rf.SenderId,
                          rf.FromHubId,
                          rf.ToHubId,
                          rf.ShipmentStatusId,
                          rf.PickUserId,
                          rf.PageNumber,
                          rf.PageSize,
                          rf.IsSortDescending,
                          rf.PickupType
            ));
            var dataCount = data.Count();
            int totalCount = 0;
            var sumOfRequestShipment = new SumOfRequestShipment();
            if (dataCount == 0)
            {
                sumOfRequestShipment.SumCountShipmentAccept = 0;
                sumOfRequestShipment.SumTotalShipmentFilledUp = 0;
                sumOfRequestShipment.SumTotalShipmentNotFill = 0;
            }
            else
            {
                totalCount = data.First().TotalCount.Value;
                sumOfRequestShipment.SumCountShipmentAccept = data.Sum(rq => rq.CountShipmentAccept);
                sumOfRequestShipment.SumTotalShipmentFilledUp = data.Sum(rq => rq.TotalShipment);
                sumOfRequestShipment.SumTotalShipmentNotFill = data.Sum(rq => rq.TotalShipmentNotFill);

            }
            return ResponseOfRequestShipmentViewModel.CreateSuccess(data, null, totalCount, sumOfRequestShipment);
        }

        [HttpGet("GetWaitingForPickup")]
        public JsonResult GetWaitingForPickup()
        {
            var dataCount = _iRequestShipmentService.GetCountByType(GetCurrentUser(), ShipmentTypeHelper.WaitingToPickup);
            var model = new
            {
                dataCount = dataCount
            };
            return JsonUtil.Success(model);
        }

        //[HttpGet("GetByStatusCurrentEmp")]
        //public JsonResult GetByStatusCurrentEmp(int statusId, int? pageSize = null, int? pageNumber = null, string cols = null)
        //{
        //    var currentUserId = GetCurrentUserId();
        //    return JsonUtil.Create(_iGeneralService.FindBy(x => x.ShipmentStatusId == statusId && x.CurrentEmpId == currentUserId, pageSize, pageNumber, cols));
        //}

        [HttpGet("GetByStatusCurrentEmp")]
        public JsonResult GetByStatusCurrentEmp(int? statusId = null, string statusIds = null, string searchText = null, int? pageSize = 10, int? pageNumber = 1, string cols = null)
        {
            if (string.IsNullOrWhiteSpace(statusIds))
            {
                if (statusId.HasValue) statusIds = statusId.ToString();
            }
            else
            {
                if (statusId.HasValue) statusIds += ("," + statusId);
            }
            var currentUser = this.GetCurrentUser();
            var result = _unitOfWork.Repository<Proc_GetRequestShipmentCurrentEmp>()
                      .ExecProcedure(Proc_GetRequestShipmentCurrentEmp.GetEntityProc(currentUser.Id, statusIds, searchText, pageNumber, pageSize));
            return JsonUtil.Success(result);
        }

        public override async Task<JsonResult> Create([FromBody]CreateUpdateRequestShipmentViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return JsonUtil.Error(ModelState);
            }

            string empFireBaseToken = null;

            if (viewModel.PickUserId.HasValue)
            {
                var emp = _unitOfWork.RepositoryR<User>().GetSingle(viewModel.PickUserId.Value);
                if (emp != null)
                {
                    empFireBaseToken = emp.FireBaseToken;
                    viewModel.CurrentEmpId = emp.Id;
                    viewModel.ShipmentStatusId = StatusHelper.ShipmentStatusId.AssignEmployeePickup;
                }
            }
            //
            var fromHubRounting = _iShipmentService.GetInfoRouting(viewModel.IsTruckDelivery, viewModel.FromDistrictId, viewModel.FromWardId, viewModel.CalWeight);
            if (!Util.IsNull(fromHubRounting))
            {
                viewModel.FromHubId = fromHubRounting.HubId;
                viewModel.FromHubRoutingId = fromHubRounting.HubRoutingId;
            }
            var toHubRouting = _iShipmentService.GetInfoRouting(viewModel.IsTruckDelivery, viewModel.ToDistrictId, viewModel.ToWardId, viewModel.CalWeight);
            if (!Util.IsNull(toHubRouting))
            {
                viewModel.ToHubId = toHubRouting.HubId;
                viewModel.ToHubRoutingId = toHubRouting.HubRoutingId;
            }
            //
            var data = await _iGeneralServiceRaw.Create<RequestShipment, CreateUpdateRequestShipmentViewModel>(viewModel);

            if (data.IsSuccess)
            {
                var shipment = data.Data as RequestShipment;

                var lading = new CreateUpdateLadingScheduleViewModel(
                    shipment.Id,
                    shipment.FromHubId,
                    GetCurrentUserId(),
                    (int)shipment.ShipmentStatusId,
                    viewModel.CurrentLat,
                    viewModel.CurrentLng,
                    viewModel.Location,
                    viewModel.Note,
                    0
                );
                await _iRequestLadingScheduleService.Create(lading);

                if (string.IsNullOrWhiteSpace(shipment.ShipmentNumber))
                {
                    //shipment.ShipmentNumber = $"BCR{RandomUtil.GetCode(shipment.Id, 6)}";
                    var shipmentNumberBasic = _iRequestShipmentService.GetCodeByType(_icompanyInformation.TypeRequestCode, _icompanyInformation.PrefixRequestCode, shipment);
                    shipment.ShipmentNumber = $"{shipmentNumberBasic}";
                    data = await _iGeneralServiceRaw.Update<RequestShipment, ShipmentInfoViewModel, RequestShipment>(shipment);
                }

                if (shipment.ShipmentStatusId == StatusHelper.ShipmentStatusId.AssignEmployeePickup)
                {
                    if (empFireBaseToken == null)
                        empFireBaseToken = _unitOfWork.RepositoryR<User>().GetSingle((int)shipment.CurrentEmpId).FireBaseToken;
                    await FireBaseUtil.SendNotification(_icompanyInformation.FireBaseBrowserAPIKey, empFireBaseToken, string.Format(MessageHelper.REQUEST_TO_PICKUP, 1), 1);
                }
            }
            return JsonUtil.Create(data);
        }

        [HttpPost("PickupCompleteByCurrentEmp")]
        public async Task<JsonResult> PickupCompleteByCurrentEmp([FromBody]CreateUpdateRequestShipmentViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return JsonUtil.Error(ModelState);
            }

            if (!_unitOfWork.RepositoryR<RequestShipment>().Any(x160 => x160.Id == viewModel.Id && x160.ShipmentStatusId == StatusHelper.ShipmentStatusId.Picking))
            {
                return JsonUtil.Error("Trạng thái vận đơn không thể cập nhật lấy hàng thành công");
            }

            //if (string.IsNullOrWhiteSpace(viewModel.ShipmentNumber))
            //{
            //    return JsonUtil.Error("Mã vận đơn không được để trống");
            //}
            if (!Util.IsNull(viewModel.ShipmentNumber))
            {
                if (_unitOfWork.RepositoryR<Shipment>().Any(rqship => rqship.ShipmentNumber == viewModel.ShipmentNumber))
                {
                    return JsonUtil.Error("Mã vận đơn đã tồn tại!");
                }
            }

            viewModel.ShipmentStatusId = StatusHelper.ShipmentStatusId.PickupComplete;
            var currentUser = GetCurrentUser();
            var requestShipment = _unitOfWork.RepositoryR<RequestShipment>().GetSingle(viewModel.Id);
            if (requestShipment != null)
            {
                var requestShipmentNumber = requestShipment.ShipmentNumber;
                requestShipment = Mapper.Map(viewModel, requestShipment);
                requestShipment.ShipmentNumber = requestShipmentNumber;
                var data = await _iGeneralServiceRaw.Update<RequestShipment>(requestShipment);
                if (data.IsSuccess)
                {
                    //var requestShipment = data.Data as RequestShipment;
                    var requestLading = new CreateUpdateLadingScheduleViewModel(
                        requestShipment.Id,
                        requestShipment.FromHubId,
                        currentUser.Id,
                        StatusHelper.ShipmentStatusId.PickupComplete,
                        viewModel.CurrentLat,
                        viewModel.CurrentLng,
                        viewModel.Location,
                        viewModel.Note,
                        0
                    );
                    await _iRequestLadingScheduleService.Create(requestLading);
                    var shipmentC = new Shipment();
                    shipmentC = Mapper.Map(requestShipment, shipmentC);
                    shipmentC.ShipmentStatusId = StatusHelper.ShipmentStatusId.PickupComplete;
                    shipmentC.EndPickTime = DateTime.Now;
                    shipmentC.ShipmentNumber = null;
                    if (!Util.IsNull(viewModel.ShipmentNumber)) shipmentC.ShipmentNumber = viewModel.ShipmentNumber;
                    shipmentC.RequestShipmentId = requestShipment.Id;
                    shipmentC.CreatedHubId = viewModel.FromHubId;
                    shipmentC.CurrentHubId = currentUser.HubId;
                    shipmentC.CurrentEmpId = currentUser.Id;
                    shipmentC.EndPickTime = DateTime.Now;
                    shipmentC.PickUserId = currentUser.Id;
                    if (shipmentC.PaymentTypeId == PaymentTypeHelper.NGUOI_GUI_THANH_TOAN)
                    {
                        shipmentC.KeepingTotalPriceEmpId = currentUser.Id;
                        shipmentC.KeepingTotalPriceHubId = currentUser.HubId;
                    }
                    var shipmentResult = await _iGeneralServiceRaw.Create<Shipment>(shipmentC);
                    if (shipmentResult.IsSuccess)
                    {
                        var shipment = shipmentResult.Data as Shipment;
                        if (string.IsNullOrWhiteSpace(shipment.ShipmentNumber) || shipment.IsBox == true)
                        {
                            if (shipment.IsBox == true)
                            {
                                var result = _unitOfWork.Repository<Proc_GetBoxNumberAuto>()
                                    .ExecProcedureSingle(Proc_GetBoxNumberAuto.GetEntityProc(shipment.Id, shipment.ShipmentId));
                                shipment.ShipmentNumber = result.BoxNumer;
                            }
                            else
                            {
                                using (var context2 = new ApplicationContext())
                                {
                                    var resutlt = _unitOfWork.Repository<Proc_GetShipmentNumberAuto>()
                                    .ExecProcedureSingle(Proc_GetShipmentNumberAuto.GetEntityProc(shipment.Id, shipment.FromProvinceId));
                                    var shipmentNumberBasic = _iShipmentService.GetCodeByType(_icompanyInformation.TypeShipmentCode, _icompanyInformation.PrefixShipmentCode, shipment.Id, resutlt.CountNumber, shipment.FromProvinceId);
                                    if (string.IsNullOrWhiteSpace(shipmentNumberBasic))
                                    {
                                        return JsonUtil.Error("Tạo mã vận đơn không thành công!");
                                    }
                                    shipment.ShipmentNumber = shipmentNumberBasic;
                                    var _unitOfWork2 = new UnitOfWork(context2);
                                    _unitOfWork2.Repository<Proc_UpdateShipmentNumberAuto>()
                                 .ExecProcedureSingle(Proc_UpdateShipmentNumberAuto.GetEntityProc(shipment.Id, shipmentNumberBasic));
                                }
                            }
                        }
                        var lading = new CreateUpdateLadingScheduleViewModel(
                            shipment.Id,
                            shipment.FromHubId,
                            currentUser.Id,
                            StatusHelper.ShipmentStatusId.PickupComplete,
                            viewModel.CurrentLat,
                            viewModel.CurrentLng,
                            viewModel.Location,
                            viewModel.Note,
                            0
                        );
                        if ((viewModel.PriceDVGTs != null && viewModel.PriceDVGTs.Count() > 0))
                        {
                            foreach (var priceDVGT in viewModel.PriceDVGTs)
                            {
                                var ssDVGT = new ShipmentServiceDVGT();
                                ssDVGT.ShipmentId = shipment.Id;
                                ssDVGT.ServiceId = priceDVGT.ServiceId;
                                ssDVGT.IsAgree = priceDVGT.IsAgree;
                                ssDVGT.Price = priceDVGT.TotalPrice;
                                _unitOfWork.RepositoryCRUD<ShipmentServiceDVGT>().Insert(ssDVGT);
                            }
                            await _unitOfWork.CommitAsync();
                        }
                        if (!Util.IsNull(viewModel.Boxes) && viewModel.Boxes.Count() > 0)
                        {
                            foreach (var box in viewModel.Boxes)
                            {
                                var newBox = new Box();
                                newBox.ShipmentId = shipment.Id;
                                newBox.Weight = box.Weight;
                                newBox.CalWeight = box.CalWeight;
                                newBox.ExcWeight = box.ExcWeight;
                                newBox.Length = box.Length;
                                newBox.Width = box.Width;
                                newBox.Height = box.Height;
                                newBox.Content = box.Content;
                                _unitOfWork.RepositoryCRUD<Box>().Insert(newBox);
                            }
                            await _unitOfWork.CommitAsync();
                        }
                        await _iLadingScheduleService.Create(lading);
                        return JsonUtil.Success(shipment);
                    }
                    else
                    {
                        return JsonUtil.Success();
                    }
                }
                else
                {
                    return JsonUtil.Error("Mã vận đơn không tồn tại");
                }
            }
            else
            {
                return JsonUtil.Error("Mã vận đơn không tồn tại");
            }
        }

        [HttpPost("PickupCancel")]
        public async Task<JsonResult> PickupCancel([FromBody]CreateUpdateRequestShipmentViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return JsonUtil.Error(ModelState);
            }
            var requestShipment = _unitOfWork.RepositoryR<RequestShipment>().GetSingle(viewModel.Id);
            if (requestShipment == null)
            {
                return JsonUtil.Error("Không tìm thấy yêu cầu");
            }
            if (requestShipment.ShipmentStatusId == StatusHelper.ShipmentStatusId.PickupCancel)
            {
                return JsonUtil.Error("Yêu cầu đã hủy trước đó");
            }
            requestShipment = Mapper.Map(viewModel, requestShipment);
            requestShipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.PickupCancel;
            var currentUser = GetCurrentUser();
            requestShipment.CurrentHubId = currentUser.HubId;
            requestShipment.CurrentEmpId = currentUser.Id;
            var data = await _iGeneralServiceRaw.Update<RequestShipment>(requestShipment);
            if (data.IsSuccess)
            {
                //
                var requestLading = new CreateUpdateLadingScheduleViewModel(
                    requestShipment.Id,
                    currentUser.HubId,
                    currentUser.Id,
                    requestShipment.ShipmentStatusId,
                    0,
                    0,
                    null,
                    viewModel.Note,
                    0
                );
                await _iRequestLadingScheduleService.Create(requestLading);
                //
                var listShipments = _unitOfWork.RepositoryCRUD<Shipment>().FindBy(f => f.RequestShipmentId == requestShipment.Id);
                if (!Util.IsNull(listShipments))
                {
                    foreach (var ship in listShipments)
                    {
                        ship.ShipmentStatusId = requestShipment.ShipmentStatusId;
                        ship.CurrentHubId = currentUser.HubId;
                        ship.CurrentEmpId = currentUser.Id;
                        var ladingShip = new CreateUpdateLadingScheduleViewModel(
                            ship.Id,
                            currentUser.HubId,
                            currentUser.Id,
                            requestShipment.ShipmentStatusId,
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
                _unitOfWork.Commit();
                //
                return JsonUtil.Create(data);
            }
            else
            {
                return JsonUtil.Error("Hủy lấy hàng không thành công");
            }
        }

        [HttpPost("AcceptCompleteByWarehousing")]
        public async Task<JsonResult> AcceptCompleteByWarehousing([FromBody]CreateUpdateRequestShipmentViewModel assignRequest)
        {
            if (!ModelState.IsValid)
            {
                return JsonUtil.Error(ModelState);
            }
            if (string.IsNullOrWhiteSpace(assignRequest.ShipmentNumber))
            {
                return JsonUtil.Error("Mã yêu cầu không được để trống");
            }
            var requestShipment = _unitOfWork.RepositoryR<RequestShipment>().GetSingle(x287 => x287.ShipmentNumber == assignRequest.ShipmentNumber);
            if (requestShipment == null)
            {
                return JsonUtil.Error("Không tìm thấy yêu cầu");
            }
            if (!_unitOfWork.RepositoryR<RequestShipment>().Any(x292 => x292.Id == requestShipment.Id && x292.ShipmentStatusId == StatusHelper.ShipmentStatusId.PickupComplete))
            {
                return JsonUtil.Error("Trạng thái yêu cầu không thể cập nhật nhập kho");
            }
            requestShipment = Mapper.Map(assignRequest, requestShipment);
            requestShipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.ReadyToDelivery;
            var currentUser = GetCurrentUser();
            requestShipment.CurrentHubId = currentUser.HubId;
            requestShipment.CurrentEmpId = currentUser.Id;
            var data = await _iGeneralServiceRaw.Update<RequestShipment>(requestShipment);
            if (data.IsSuccess)
            {
                //var requestShipment = data.Data as RequestShipment;
                // thêm hành trình nhập kho lấy hàng
                var requestLadingPickup = new CreateUpdateLadingScheduleViewModel(
                    requestShipment.Id,
                    requestShipment.FromHubId,
                    currentUser.Id,
                    StatusHelper.ShipmentStatusId.StoreInWarehousePickup,
                    0,
                    0,
                    null,
                    "Nhập kho " + requestShipment.CountShipmentAccept + " Bill",
                    0
                );
                await _iRequestLadingScheduleService.Create(requestLadingPickup);
                //
                var requestLading = new CreateUpdateLadingScheduleViewModel(
                    requestShipment.Id,
                    requestShipment.FromHubId,
                    currentUser.Id,
                    StatusHelper.ShipmentStatusId.ReadyToDelivery,
                    0,
                    0,
                    null,
                    assignRequest.Note,
                    0
                );
                await _iRequestLadingScheduleService.Create(requestLading);
            }
            else
            {
                return JsonUtil.Error("Cập nhật nhập kho không thành công");
            }
            return JsonUtil.Success();
        }

        [HttpPost("AcceptCompleteByCurrentEmp")]
        public async Task<JsonResult> AcceptCompleteByCurrentEmp([FromBody]CreateUpdateRequestShipmentViewModel assignRequest)
        {
            if (string.IsNullOrWhiteSpace(assignRequest.ShipmentNumber))
            {
                return JsonUtil.Error("Mã yêu cầu không được để trống");
            }
            RequestShipment requestShipment = null;
            if (assignRequest.Id == 0)
            {
                CreateUpdateRequestShipmentViewModel model = Mapper.Map<CreateUpdateRequestShipmentViewModel>(assignRequest);
                model.ShipmentStatusId = StatusHelper.ShipmentStatusId.PickupComplete;
                var result = await _iGeneralServiceRaw.Create<RequestShipment, CreateUpdateRequestShipmentViewModel>(model);
                if (result.IsSuccess == true)
                {
                    requestShipment = result.Data as RequestShipment;
                }
                else
                {
                    return JsonUtil.Error(result.Message);
                }
            }
            else
            {
                requestShipment = _unitOfWork.RepositoryR<RequestShipment>().GetSingle(assignRequest.Id);
                if (!_unitOfWork.RepositoryR<RequestShipment>().Any(x364 => x364.Id == requestShipment.Id && x364.ShipmentStatusId == StatusHelper.ShipmentStatusId.Picking))
                {
                    return JsonUtil.Error("Trạng thái yêu cầu không thể cập nhật lấy hàng thành công");
                }
                requestShipment = Mapper.Map(assignRequest, requestShipment);
            }
            var currentUser = GetCurrentUser();
            requestShipment.CurrentHubId = currentUser.HubId;
            requestShipment.CurrentEmpId = currentUser.Id;
            requestShipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.PickupComplete;
            var data = await _iGeneralServiceRaw.Update<RequestShipment>(requestShipment);
            if (data.IsSuccess)
            {
                //var requestShipment = data.Data as RequestShipment;
                var requestLading = new CreateUpdateLadingScheduleViewModel(
                    requestShipment.Id,
                    requestShipment.FromHubId,
                    currentUser.Id,
                    StatusHelper.ShipmentStatusId.PickupComplete,
                    assignRequest.CurrentLat,
                    assignRequest.CurrentLng,
                    assignRequest.Location,
                    assignRequest.Note,
                    0
                );
                await _iRequestLadingScheduleService.Create(requestLading);
            }
            else
            {
                return JsonUtil.Error("Cập nhật lấy hàng lỗi");
            }
            return JsonUtil.Success();
        }

        [HttpGet("TrackingShort")]
        public JsonResult TrackingShort(string shipmentNumber, string cols)
        {
            var shipmentResult = _iGeneralServiceRaw
                .GetSingle<RequestShipment, RequestShipmentInfoViewModel>(reqShip => reqShip.ShipmentNumber == shipmentNumber, cols);
            //x => x.FromHub,
            //x => x.ToHub,
            //x => x.FromWard,
            //x => x.FromWard.District,
            //x => x.FromWard.District.Province,
            //x => x.ToWard,
            //x => x.ToWard.District,
            //x => x.ToWard.District.Province,
            //x => x.ShipmentStatus);

            if (shipmentResult.IsSuccess)
            {
                var shipment = shipmentResult.Data as RequestShipmentInfoViewModel;
                if (shipment == null) return JsonUtil.Error("Không tìm thấy đơn hàng");
                shipment.LadingSchedules = _unitOfWork.Repository<Proc_RequestLadingSchedule_Joined>().ExecProcedure(Proc_RequestLadingSchedule_Joined.GetEntityProc(shipment.Id));
            }
            return JsonUtil.Create(shipmentResult);
        }


        [HttpGet("Tracking")]
        public JsonResult Tracking(string shipmentNumber, string cols)
        {
            var data = _iGeneralServiceRaw.GetSingle<RequestShipment, RequestShipmentInfoViewModel>(x => x.ShipmentNumber == shipmentNumber, cols: cols);
            if (!data.IsSuccess) return JsonUtil.Create(data);

            var shipment = data.Data as RequestShipmentInfoViewModel;
            if (shipment == null) return JsonUtil.Error("Không tìm thấy đơn hàng");
            shipment.LadingSchedules = _unitOfWork.Repository<Proc_RequestLadingSchedule_Joined>().ExecProcedure(Proc_RequestLadingSchedule_Joined.GetEntityProc(shipment.Id));

            return JsonUtil.Create(data);
        }

        [HttpPost("UpdateStatusCurrentEmp")]
        public async Task<JsonResult> UpdateStatusCurrentEmp([FromBody]UpdateStatusViewModel viewModel)
        {
            var currentuser = GetCurrentUser();
            if (!ModelState.IsValid)
            {
                return JsonUtil.Error(ModelState);
            }
            /// check 
            
            if (viewModel.ShipmentStatusId == StatusHelper.ShipmentStatusId.Picking)
            {
                var result = _unitOfWork.Repository<Proc_CheckRequestShipmentPicking>()
                    .ExecProcedureSingle(Proc_CheckRequestShipmentPicking.GetEntityProc(viewModel.Id));
                if (result.IsSuccess == false) return JsonUtil.Error(result.Message);
            }
            if (viewModel.ShipmentStatusId == StatusHelper.ShipmentStatusId.PickupComplete)
            {
                var result = _unitOfWork.Repository<Proc_CheckRequestShipmentCompleted>()
                    .ExecProcedureSingle(Proc_CheckRequestShipmentCompleted.GetEntityProc(viewModel.Id));
                if (result.IsSuccess == false) return JsonUtil.Error(result.Message);
                viewModel.EndPickTime = DateTime.Now;
                viewModel.PickUserId = currentuser.Id;
            }
            else if (viewModel.ShipmentStatusId == StatusHelper.ShipmentStatusId.RejectPickup)
            {
                if (_icompanyInformation.Name == "be")
                {
                    viewModel.ShipmentStatusId = StatusHelper.ShipmentStatusId.PickupFail;
                }
            }
            viewModel.CurrentEmpId = currentuser.Id;
            viewModel.CurrentHubId = currentuser.HubId;
            var data = await _iGeneralService.Update<UpdateStatusViewModel>(viewModel);
            if (data.IsSuccess)
            {
                var shipment = data.Data as RequestShipmentInfoViewModel;
                var lading = new CreateUpdateLadingScheduleViewModel(
                    shipment.Id,
                    shipment.FromHubId,
                    GetCurrentUserId(),
                    viewModel.ShipmentStatusId,
                    viewModel.CurrentLat,
                    viewModel.CurrentLng,
                    viewModel.Location,
                    viewModel.Note,
                    0
                );
                await _iRequestLadingScheduleService.Create(lading);
                //

                //
                List<int> listStatusUpdateShipment = new List<int>();
                listStatusUpdateShipment.Add(StatusHelper.ShipmentStatusId.Picking);
                listStatusUpdateShipment.Add(StatusHelper.ShipmentStatusId.PickupFail);
                listStatusUpdateShipment.Add(StatusHelper.ShipmentStatusId.PickupCancel);
                listStatusUpdateShipment.Add(StatusHelper.ShipmentStatusId.CallSuccess);
                listStatusUpdateShipment.Add(StatusHelper.ShipmentStatusId.CallFailedOne);
                listStatusUpdateShipment.Add(StatusHelper.ShipmentStatusId.CallFailedTwo);
                listStatusUpdateShipment.Add(StatusHelper.ShipmentStatusId.CallFailedTwo);
                if (shipment.ShipmentStatusId != StatusHelper.ShipmentStatusId.PickupComplete)
                {
                    int[] listStatusBlock = { StatusHelper.ShipmentStatusId.DeliveryComplete, StatusHelper.ShipmentStatusId.ReturnComplete };
                    var listShipments = _unitOfWork.RepositoryCRUD<Shipment>().FindBy(f => f.RequestShipmentId == viewModel.Id && !listStatusBlock.Contains(f.ShipmentStatusId));
                    if (!Util.IsNull(listShipments))
                    {
                        foreach (var ship in listShipments)
                        {
                            ship.ShipmentStatusId = viewModel.ShipmentStatusId;
                            ship.CurrentHubId = currentuser.HubId;
                            ship.CurrentEmpId = currentuser.Id;
                            var ladingShip = new CreateUpdateLadingScheduleViewModel(
                                ship.Id,
                                currentuser.HubId,
                                currentuser.Id,
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
                    _unitOfWork.Commit();
                }
            }
            return JsonUtil.Create(data);
        }

        //
        [HttpPost("PickupFails")]
        public async Task<JsonResult> PickupFails([FromBody] PickupFailsViewModel viewModel)
        {
            var currentuser = GetCurrentUser();
            if (!ModelState.IsValid)
            {
                return JsonUtil.Error(ModelState);
            }
            string ids = "";
            if(viewModel.Ids != null && viewModel.Ids.Count() > 0)
            {
                ids = string.Join(',', viewModel.Ids);
            }
            //viewModel.CurrentEmpId = currentuser.Id;
            //viewModel.CurrentHubId = currentuser.HubId;

            foreach(var requestShipmentId in viewModel.Ids)
            {
                var requestShipments = _unitOfWork.RepositoryR<RequestShipment>().GetSingle(f => f.Id == requestShipmentId);
                if(requestShipments == null)
                {
                    return JsonUtil.Error("Không tìm thấy yêu cầu");
                }
                //requestShipments = Mapper.Map(viewModel, requestShipments);
                requestShipments.Note = viewModel.Note;
                requestShipments.ReasonId = viewModel.ReasonId;
                requestShipments.CurrentEmpId = currentuser.Id;
                requestShipments.CurrentHubId = currentuser.HubId;
                requestShipments.IsEnabled = viewModel.IsEnabled;
                requestShipments.ShipmentStatusId = viewModel.ShipmentStatusId;
                var data = await _iGeneralService.Update<RequestShipment>(requestShipments);
                if (data.IsSuccess)
                {
                    //

                    //
                    List<int> listStatusUpdateShipment = new List<int>();
                    listStatusUpdateShipment.Add(StatusHelper.ShipmentStatusId.Picking);
                    listStatusUpdateShipment.Add(StatusHelper.ShipmentStatusId.PickupFail);
                    listStatusUpdateShipment.Add(StatusHelper.ShipmentStatusId.PickupCancel);
                    listStatusUpdateShipment.Add(StatusHelper.ShipmentStatusId.CallSuccess);
                    listStatusUpdateShipment.Add(StatusHelper.ShipmentStatusId.CallFailedOne);
                    listStatusUpdateShipment.Add(StatusHelper.ShipmentStatusId.CallFailedTwo);
                    listStatusUpdateShipment.Add(StatusHelper.ShipmentStatusId.CallFailedTwo);
                    if (data.Data.ShipmentStatusId != StatusHelper.ShipmentStatusId.PickupComplete)
                    {
                        int[] listStatusBlock = { StatusHelper.ShipmentStatusId.DeliveryComplete, StatusHelper.ShipmentStatusId.ReturnComplete };
                        var listShipments =  _unitOfWork.RepositoryCRUD<Shipment>().FindBy(f => f.RequestShipmentId == requestShipmentId && !listStatusBlock.Contains(f.ShipmentStatusId));
                        if (!Util.IsNull(listShipments))
                        {
                            foreach (var ship in listShipments)
                            {
                                ship.ShipmentStatusId = viewModel.ShipmentStatusId;
                                ship.CurrentHubId = currentuser.HubId;
                                ship.CurrentEmpId = currentuser.Id;
                                ship.RequestShipmentId = null;
                                //await _iGeneralServiceRaw.Update<CreateUpdateShipmentViewModel,Shipment>(ship);
                                var ladingShip = new CreateUpdateLadingScheduleViewModel(
                                    ship.Id,
                                    currentuser.HubId,
                                    currentuser.Id,
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
                        _unitOfWork.Commit();
                    }
                }
                //return JsonUtil.Create(data);
            }
            return JsonUtil.Success("Đã cập nhật trạng thái lấy hàng không thành công");
        }

        [HttpPost("UpdateStatus")]
        public async Task<JsonResult> UpdateStatus([FromBody]UpdateStatusViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return JsonUtil.Error(ModelState);
            }

            var data = await _iGeneralService.Update<UpdateStatusViewModel>(viewModel);

            if (data.IsSuccess)
            {
                var shipment = data.Data as RequestShipmentInfoViewModel;
                var lading = new CreateUpdateLadingScheduleViewModel(
                    shipment.Id,
                    shipment.FromHubId,
                    GetCurrentUserId(),
                    viewModel.ShipmentStatusId,
                    viewModel.CurrentLat,
                    viewModel.CurrentLng,
                    viewModel.Location,
                    viewModel.Note,
                    0
                );
                await _iRequestLadingScheduleService.Create(lading);

                var empFireBaseToken = _unitOfWork.RepositoryR<User>().GetSingle((int)shipment.CurrentEmpId).FireBaseToken;
                await FireBaseUtil.SendNotification(_icompanyInformation.FireBaseBrowserAPIKey, empFireBaseToken, string.Format(MessageHelper.REQUEST_CANCEL, shipment.ShipmentNumber), 1);
            }

            return JsonUtil.Create(data);
        }

        [HttpPost("GetByStatusIds")]
        public JsonResult GetByStatusIds([FromBody]GetByIdsViewModel viewModel)
        {
            return JsonUtil.Success(_iGeneralService.FindBy(x => viewModel.Ids.Contains(x.ShipmentStatusId), cols: viewModel.Cols));
        }

        [HttpGet("GetListRequestShipmentKeeping")]
        public JsonResult GetListRequestShipmentKeeping(int? userId, bool? recovery, DateTime? dateFrom, DateTime? dateTo, string[] cols = null, int? pageSize = null, int? pageNumber = null)
        {
            Expression<Func<RequestShipment, bool>> predicate = x509 => x509.Id > 0;
            int[] statusIds =
            {
                StatusHelper.ShipmentStatusId.PickupComplete,
            };
            predicate = predicate.And(x => statusIds.Contains(x.ShipmentStatusId) && x.RequestShipmentId.HasValue);
            var currentUser = GetCurrentUser();
            if (!Util.IsNull(userId))
            {
                predicate = predicate.And(x => x.CurrentEmpId == userId);
            }
            else
            {
                predicate = predicate.And(s2043 => s2043.CurrentHubId == currentUser.HubId);
            }
            if (!Util.IsNull(dateFrom))
            {
                predicate = predicate.And(x => x.OrderDate >= dateFrom);
            }
            if (!Util.IsNull(dateTo))
            {
                predicate = predicate.And(x => x.OrderDate <= dateTo);
            }
            return JsonUtil.Success(_iGeneralService.FindBy(predicate, pageSize, pageNumber, cols));
        }

        [HttpPost("GetByListCode")]
        public JsonResult GetByListCode([FromBody]List<string> list)
        {
            return JsonUtil.Success(_iGeneralService.FindBy(f => list.Contains(f.ShipmentNumber)));
        }

        [HttpPost("CreateListRequestShipmentAndShipment")]
        public async Task<JsonResult> CreateListRequestShipmentAndShipment([FromBody]CreateListRequestShipmentAndShipmentViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return JsonUtil.Error(ModelState);
            }
            var currentUser = GetCurrentUser();
            var requestShipmentId = viewModel.RequestShipmentId;
            var parentRequestShipment = _unitOfWork.RepositoryR<RequestShipment>().GetSingle(requestShipmentId);
            var cloneMapParentRequestShipment = Mapper.Map<CreateUpdateRequestShipmentViewModel>(parentRequestShipment);
            if (Util.IsNull(parentRequestShipment))
            {
                return JsonUtil.Error("Mã yêu cầu không tồn tại");
            }
            parentRequestShipment.TotalChildRequestShipment = 0;
            parentRequestShipment.TotalChildShipment = 0;
            if (viewModel.ListRequestShipmentAndShipment == null || viewModel.ListRequestShipmentAndShipment.Count() == 0)
            {
                return JsonUtil.Error("Đã có lỗi xảy ra!");
            }

            // check duplicate ShipmentNumber
            var listRequestItemShipments = viewModel.ListRequestShipmentAndShipment.Where(x => x.RequestShipment != null).Select(x => x.RequestShipment).ToList();
            var requestItemShipmentNumbers = listRequestItemShipments.Select(x => x.ShipmentNumber).ToList();
            foreach (var requestShipmentNumberItem in requestItemShipmentNumbers)
            {
                if (!string.IsNullOrWhiteSpace(requestShipmentNumberItem))
                {
                    var checkDuplicateRequestShipmentNumber = _unitOfWork.RepositoryR<RequestShipment>().GetSingle(f568 => f568.ShipmentNumber == requestShipmentNumberItem.Trim());
                    if (!Util.IsNull(checkDuplicateRequestShipmentNumber))
                    {
                        return JsonUtil.Error($"Đã tồn tại mã yêu cầu {requestShipmentNumberItem}!");
                    }
                }
            }

            var shipmentShipmentNumbers = viewModel.ListRequestShipmentAndShipment.Select(x => x.ShipmentNumbers).SelectMany(x => x).ToList();
            foreach (var shipmentShipmentNumberItem in shipmentShipmentNumbers)
            {
                if (!string.IsNullOrWhiteSpace(shipmentShipmentNumberItem))
                {
                    var checkDuplicateShipmentNumber = _unitOfWork.RepositoryR<Shipment>().GetSingle(checlDLC => checlDLC.ShipmentNumber == shipmentShipmentNumberItem.Trim());
                    if (!Util.IsNull(checkDuplicateShipmentNumber))
                    {
                        return JsonUtil.Error($"Đã tồn tại mã vận đơn {shipmentShipmentNumberItem}!");
                    }
                }
            }

            // create multi child RequestShipment and shipments
            foreach (var item in viewModel.ListRequestShipmentAndShipment)
            {
                var requestItem = item.RequestShipment;
                var cusDepartmentIdItem = item.CusDepartmentId;
                var shipmentNumbers = item.ShipmentNumbers;
                var mapParentRequestShipment = new CreateUpdateRequestShipmentViewModel();
                mapParentRequestShipment = Mapper.Map<CreateUpdateRequestShipmentViewModel>(parentRequestShipment);
                mapParentRequestShipment.Id = 0;
                mapParentRequestShipment.ShipmentNumber = null;
                if (cusDepartmentIdItem != null && cusDepartmentIdItem > 0)
                {
                    // set Sender Infomation from Cusdepartment
                    var cusDepartment = _unitOfWork.RepositoryR<CusDepartment>().GetSingle(cusDepartmentIdItem.Value);
                    if (cusDepartment == null)
                    {
                        return JsonUtil.Error("Không tồn tại phòng ban!");
                    }

                    mapParentRequestShipment.CusDepartmentId = cusDepartmentIdItem;
                    if (!String.IsNullOrWhiteSpace(cusDepartment.RepresentativeName))
                    {
                        mapParentRequestShipment.SenderName = cusDepartment.RepresentativeName;
                    }
                    if (!String.IsNullOrWhiteSpace(cusDepartment.PhoneNumber))
                    {
                        mapParentRequestShipment.SenderPhone = cusDepartment.PhoneNumber;
                    }
                    if (!String.IsNullOrWhiteSpace(cusDepartment.AddressNote))
                    {
                        mapParentRequestShipment.AddressNoteFrom = cusDepartment.AddressNote;
                    }

                    if (!String.IsNullOrWhiteSpace(cusDepartment.Address) && cusDepartment.ProvinceId > 0 && cusDepartment.DistrictId > 0 && cusDepartment.WardId > 0)
                    {
                        mapParentRequestShipment.PickingAddress = cusDepartment.Address;
                        mapParentRequestShipment.FromProvinceId = cusDepartment.ProvinceId.Value;
                        mapParentRequestShipment.FromDistrictId = cusDepartment.DistrictId.Value;
                        mapParentRequestShipment.FromWardId = cusDepartment.WardId.Value;
                        if (cusDepartment.Lat != null)
                        {
                            mapParentRequestShipment.LatFrom = cusDepartment.Lat.Value;
                            mapParentRequestShipment.CurrentLat = cusDepartment.Lat.Value;
                        }
                        if (cusDepartment.Lng != null)
                        {
                            mapParentRequestShipment.LngFrom = cusDepartment.Lng.Value;
                            mapParentRequestShipment.CurrentLng = cusDepartment.Lng.Value;
                        }
                    }
                }
                // create ItemRequest
                if (!Util.IsNull(requestItem))
                {
                    mapParentRequestShipment.ShipmentNumber = requestItem.ShipmentNumber;
                    mapParentRequestShipment.RequestShipmentId = requestShipmentId;
                    mapParentRequestShipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.PickupComplete;
                    mapParentRequestShipment.Weight = (requestItem.Weight > 0) ? requestItem.Weight : 0;
                    mapParentRequestShipment.CalWeight = (requestItem.CalWeight > 0) ? requestItem.CalWeight : 0;
                    mapParentRequestShipment.TotalBox = (requestItem.TotalBox > 0) ? requestItem.TotalBox : 0;
                    mapParentRequestShipment.TotalChildShipment = (requestItem.TotalChildShipment > 0) ? requestItem.TotalChildShipment : null;
                    mapParentRequestShipment.CountShipment = mapParentRequestShipment.TotalChildShipment.Value;
                    if (!string.IsNullOrWhiteSpace(requestItem.Note))
                    {
                        mapParentRequestShipment.Note = requestItem.Note;
                    }
                    var dataItem = await _iGeneralServiceRaw.Create<RequestShipment, CreateUpdateRequestShipmentViewModel>(mapParentRequestShipment);
                    if (dataItem.IsSuccess)
                    {
                        parentRequestShipment.TotalChildRequestShipment++;
                        var itemRequestShipment = dataItem.Data as RequestShipment;

                        var lading = new CreateUpdateLadingScheduleViewModel(
                            itemRequestShipment.Id,
                            mapParentRequestShipment.FromHubId,
                            currentUser.Id,
                            (int)StatusHelper.ShipmentStatusId.PickupComplete,
                            mapParentRequestShipment.CurrentLat,
                            mapParentRequestShipment.CurrentLng,
                            mapParentRequestShipment.Location,
                            mapParentRequestShipment.Note,
                            0
                        );
                        await _iRequestLadingScheduleService.Create(lading);

                        if (string.IsNullOrWhiteSpace(itemRequestShipment.ShipmentNumber))
                        {
                            var shipmentNumberBasic = _iRequestShipmentService.GetCodeByType(_icompanyInformation.TypeRequestCode, _icompanyInformation.PrefixRequestCode, itemRequestShipment);
                            itemRequestShipment.ShipmentNumber = $"{shipmentNumberBasic}";
                            _unitOfWork.RepositoryCRUD<RequestShipment>().Update(itemRequestShipment);
                            await _unitOfWork.CommitAsync();
                        }
                    }
                }
                // create shipments
                if (shipmentNumbers.Count() > 0)
                {
                    foreach (var shipmentNumber in shipmentNumbers)
                    {
                        var mapRequestItem = Mapper.Map<RequestShipment>(mapParentRequestShipment);
                        //var shipment = Mapper.Map<Shipment>(mapRequestItem);
                        var shipment = new Shipment();
                        shipment.OrderDate = DateTime.Now;
                        if (!String.IsNullOrWhiteSpace(shipmentNumber))
                        {
                            shipment.ShipmentNumber = shipmentNumber.Trim();
                        }
                        shipment.SenderId = mapParentRequestShipment.SenderId;
                        shipment.SenderName = mapParentRequestShipment.SenderName;
                        shipment.SenderPhone = mapParentRequestShipment.SenderPhone;
                        shipment.PickingAddress = mapParentRequestShipment.PickingAddress;
                        shipment.FromProvinceId = mapParentRequestShipment.FromProvinceId;
                        shipment.FromDistrictId = mapParentRequestShipment.FromDistrictId;
                        shipment.FromWardId = mapParentRequestShipment.FromWardId;
                        shipment.FromHubId = mapParentRequestShipment.FromHubId.Value;
                        shipment.PickUserId = mapParentRequestShipment.PickUserId;

                        shipment.RequestShipmentId = requestShipmentId;
                        shipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.PickupComplete;
                        shipment.CreatedHubId = currentUser.HubId;
                        shipment.CurrentHubId = currentUser.HubId;
                        shipment.CurrentEmpId = currentUser.Id;
                        //if (mapParentRequestShipment.ServiceDVGTIds != null && mapParentRequestShipment.ServiceDVGTIds.Count() > 0)
                        //{
                        //    shipment.ServiceDVGTIds = string.Join(",", mapParentRequestShipment.ServiceDVGTIds);
                        //}
                        //if (shipment.PaymentTypeId == PaymentTypeHelper.NGUOI_GUI_THANH_TOAN)
                        //{
                        //    shipment.KeepingTotalPriceEmpId = currentUser.Id;
                        //    shipment.KeepingTotalPriceHubId = currentUser.HubId;
                        //}
                        _context.Database.ExecuteSqlCommand("ALTER TABLE Post_Shipment NOCHECK CONSTRAINT ALL");
                        var ship = await _iGeneralServiceRaw.Create<Shipment>(shipment);
                        if (ship.IsSuccess)
                        {
                            _context.Database.ExecuteSqlCommand("ALTER TABLE Post_Shipment CHECK CONSTRAINT ALL");
                            parentRequestShipment.TotalChildShipment++;
                            var itemShipment = ship.Data as Shipment;

                            var lading = new CreateUpdateLadingScheduleViewModel(
                                itemShipment.Id,
                                shipment.FromHubId,
                                GetCurrentUserId(),
                                (int)StatusHelper.ShipmentStatusId.PickupComplete,
                                cloneMapParentRequestShipment.CurrentLat,
                                cloneMapParentRequestShipment.CurrentLng,
                                cloneMapParentRequestShipment.Location,
                                cloneMapParentRequestShipment.Note,
                                0
                            );
                            await _iLadingScheduleService.Create(lading);

                            if (string.IsNullOrWhiteSpace(itemShipment.ShipmentNumber))
                            {
                                var shipmentNumberBasic = _iShipmentService.GetCodeByType(1, _icompanyInformation.PrefixShipmentCode, itemShipment.Id, itemShipment.Id, itemShipment.FromProvinceId);
                                itemShipment.ShipmentNumber = $"{shipmentNumberBasic}";
                                _unitOfWork.RepositoryCRUD<Shipment>().Update(itemShipment);
                                await _unitOfWork.CommitAsync();
                            }
                        }
                    }
                }
            }
            // update parentRequestShipment
            parentRequestShipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.PickupComplete;
            var dataParentRequestShipment = await _iGeneralServiceRaw.Update<RequestShipment>(parentRequestShipment);
            if (dataParentRequestShipment.IsSuccess)
            {
                var parentRequestLading = Mapper.Map<CreateUpdateRequestShipmentViewModel>(parentRequestShipment);
                var requestLading = new CreateUpdateLadingScheduleViewModel(
                    parentRequestShipment.Id,
                    parentRequestShipment.FromHubId,
                    currentUser.Id,
                    StatusHelper.ShipmentStatusId.PickupComplete,
                    cloneMapParentRequestShipment.CurrentLat,
                    cloneMapParentRequestShipment.CurrentLng,
                    cloneMapParentRequestShipment.Location,
                    cloneMapParentRequestShipment.Note,
                    0
                );
                await _iRequestLadingScheduleService.Create(requestLading);
            }
            else
            {
                return JsonUtil.Error("Đã có lỗi xảy ra!");
            }
            return JsonUtil.Success();
        }

        [HttpGet("GetByParentRequestShipmentId")]
        public async Task<JsonResult> GetByParentRequestShipmentId(int requestShipmentId, string cols = null)
        {
            var parentRequestShipment = await _unitOfWork.RepositoryR<RequestShipment>().GetSingleAsync(rq => rq.Id == requestShipmentId);
            if (Util.IsNull(parentRequestShipment))
            {
                return JsonUtil.Error("Mã yêu cầu không tồn tại!");
            }

            //var childRequestShipments = _unitOfWork.RepositoryR<RequestShipment>().FindBy(rq => rq.RequestShipmentId == requestShipmentId);
            var childShipments = _unitOfWork.RepositoryR<Shipment>().FindBy(rq => rq.RequestShipmentId == requestShipmentId).ToList();
            var listAllChildShipments = new List<Shipment>();

            string[] includeProperties = new string[0];

            // if get cols and check exist receiver information
            if (!string.IsNullOrWhiteSpace(cols))
            {
                includeProperties = cols.Split(',');
                var cloneListIncludeProps = includeProperties.ToList();
                var itemChildShipmentIds = childShipments.Select(x => x.Id).ToList();
                var listChildShipmentNullReceiverIds = new List<int>();
                var listChildShipmentNullReceiver = new List<Shipment>();
                var listChildShipmentNotNullReceiverIds = new List<int>();
                var listChildShipmentNotNullReceiver = new List<Shipment>();
                if (childShipments.Count() > 0)
                {
                    foreach (var itemChild in childShipments)
                    {
                        if (itemChild.ToHubId == 0)
                        {
                            cloneListIncludeProps.Remove("ToHub");
                        }
                        if (itemChild.ToHubRoutingId == 0)
                        {
                            cloneListIncludeProps.Remove("ToHubRouting");
                        }
                        if (itemChild.ToProvinceId == 0)
                        {
                            cloneListIncludeProps.Remove("ToWard.District.Province");
                        }
                        if (itemChild.ToDistrictId == 0)
                        {
                            cloneListIncludeProps.Remove("ToWard.District");
                        }
                        if (itemChild.ToWardId == 0)
                        {
                            cloneListIncludeProps.Remove("ToWard");
                        }

                        if (cloneListIncludeProps.Count() < includeProperties.Count())
                        {
                            listChildShipmentNullReceiverIds.Add(itemChild.Id);
                        }
                    }

                    if (listChildShipmentNullReceiverIds.Count() > 0)
                    {
                        listChildShipmentNotNullReceiverIds = itemChildShipmentIds.AsEnumerable().Except(listChildShipmentNullReceiverIds.AsEnumerable()).ToList();
                        listChildShipmentNullReceiver = _unitOfWork.RepositoryR<Shipment>().GetAll(cloneListIncludeProps.ToArray()).Where(sm383 => listChildShipmentNullReceiverIds.Contains(sm383.Id)).ToList();
                        listChildShipmentNotNullReceiver = _unitOfWork.RepositoryR<Shipment>().GetAll(includeProperties).Where(sm839 => listChildShipmentNotNullReceiverIds.Contains(sm839.Id)).ToList();
                        listAllChildShipments = listChildShipmentNotNullReceiver.Concat(listChildShipmentNullReceiver).ToList();
                    }
                    else
                    {
                        listAllChildShipments = _unitOfWork.RepositoryR<Shipment>().GetAll(includeProperties).Where(sm844 => itemChildShipmentIds.Contains(sm844.Id)).ToList();
                    }
                }
            }
            else
            {
                listAllChildShipments = childShipments;
            }

            if (listAllChildShipments.Count() > 0)
            {
                var result = new ParentRequestShipmentTrackingViewModel();
                result.RequestShipmentId = requestShipmentId;
                result.ChildRequestShipments = new List<ChildRequestShipmentTrackingViewModel>();
                var childRequestShipmentTracking = new ChildRequestShipmentTrackingViewModel();
                childRequestShipmentTracking.Shipments = listAllChildShipments;
                result.ChildRequestShipments.Add(childRequestShipmentTracking);
                return JsonUtil.Success(result);
            }
            else
            {
                //return JsonUtil.Error("Không tìm thấy thông tin phòng ban!");
                return JsonUtil.Success();
            }
        }

        [HttpPost("PickupComplete")]
        public async Task<JsonResult> PickupComplete([FromBody] CompletePickupViewModel viewModel)
        {
            var res = _unitOfWork.Repository<Proc_CheckCompleteRequestShipment>()
                   .ExecProcedureSingle(Proc_CheckCompleteRequestShipment.GetEntityProc(viewModel.RequestShipmentId));
            if (res.Id == 0)
            {
                return JsonUtil.Error("Vui lòng hoàn tất tất cả các đơn hàng");
            }
            viewModel.ShipmentStatusId = StatusHelper.ShipmentStatusId.PickupComplete;
            var currentUser = GetCurrentUser();
            var requestShipment = _unitOfWork.RepositoryR<RequestShipment>().GetSingle(viewModel.RequestShipmentId);
            if (requestShipment != null)
            {
                var requestShipmentNumber = requestShipment.ShipmentNumber;
                requestShipment.ShipmentNumber = requestShipmentNumber;
                requestShipment.ShipmentStatusId = viewModel.ShipmentStatusId;
                requestShipment.Id = viewModel.RequestShipmentId;
                var data = await _iGeneralServiceRaw.Update<RequestShipment>(requestShipment);
                if (data.IsSuccess)
                {
                    //var requestShipment = data.Data as RequestShipment;
                    var requestLading = new CreateUpdateLadingScheduleViewModel(
                        requestShipment.Id,
                        requestShipment.FromHubId,
                        currentUser.Id,
                        StatusHelper.ShipmentStatusId.PickupComplete,
                        viewModel.CurrentLat,
                        viewModel.CurrentLng,
                        "",
                        "",
                        0
                    );
                    await _iRequestLadingScheduleService.Create(requestLading);
                }
                else
                {
                    return JsonUtil.Error("Mã vận đơn không tồn tại");
                }
                return JsonUtil.Success(null, "Hoàn tất yêu cầu thành công");
            }
            else
            {
                return JsonUtil.Error("Mã vận đơn không tồn tại");
            }

        }

        [HttpPost("PickupCompletes")]
        public async Task<JsonResult> PickupCompletes([FromBody] CompletePickupsViewModel viewModel)
        {
            string requestShipmentIds = "";
            if (viewModel.RequestShipmentIds != null && viewModel.RequestShipmentIds.Count() > 0)
            {
                requestShipmentIds = string.Join(",", viewModel.RequestShipmentIds);
            }
            else
            {
                return JsonUtil.Error("Không tìm thấy yêu cầu");
            }
            var res = _unitOfWork.Repository<Proc_CheckCompleteRequestShipments>()
                   .ExecProcedureSingle(Proc_CheckCompleteRequestShipments.GetEntityProc(requestShipmentIds));
            if (res.Id == 0)
            {
                return JsonUtil.Error("Vui lòng hoàn tất tất cả các đơn hàng!");
            }
            viewModel.ShipmentStatusId = StatusHelper.ShipmentStatusId.PickupComplete;

            foreach (var requestShipmentId in viewModel.RequestShipmentIds)
            {
                var currentUser = GetCurrentUser();
                var requestShipments = _unitOfWork.RepositoryR<RequestShipment>().GetSingle(requestShipmentId);
                if (requestShipments != null)
                {
                    if(requestShipments.ShipmentStatusId != 2)
                    {
                        return JsonUtil.Error("Yêu cầu không hợp lệ!");
                    }
                    var requestShipmentNumbers = requestShipments.ShipmentNumber;
                    requestShipments.ShipmentNumber = requestShipmentNumbers;
                    requestShipments.ShipmentStatusId = viewModel.ShipmentStatusId;
                    requestShipments.Id = requestShipmentId;
                    var data = await _iGeneralServiceRaw.Update<RequestShipment>(requestShipments);
                    if (data.IsSuccess)
                    {
                        //var requestShipment = data.Data as RequestShipment;
                        var requestLading = new CreateUpdateLadingScheduleViewModel(
                            requestShipments.Id,
                            requestShipments.FromHubId,
                            currentUser.Id,
                            StatusHelper.ShipmentStatusId.PickupComplete,
                            viewModel.CurrentLat,
                            viewModel.CurrentLng,
                            "",
                            "",
                            0
                        );
                        await _iRequestLadingScheduleService.Create(requestLading);
                    }
                    else
                    {
                        return JsonUtil.Error("Mã vận đơn không tồn tại");
                    }
                    
                }
            }
            return JsonUtil.Success(null, "Hoàn tất yêu cầu thành công");

        }

        [HttpPost("GetListShipmentByRequestShipmentId")]
        public JsonResult GetListShipmentByRequestShipmentId([FromBody] GetListShipmentByRequestShipmentIdViewModel viewModel)
        {
            var currentUser = this.GetCurrentUser();
            var result = _unitOfWork.Repository<Proc_GetListShipmentByRequestShipmentId>()
                     .ExecProcedure(Proc_GetListShipmentByRequestShipmentId.GetEntityProc(currentUser.Id, viewModel.SearchText, viewModel.PageNumber, viewModel.PageSize));
            return JsonUtil.Success(result);
        }

        [HttpGet("GetByStatusCurrentEmpMobile")]
        public JsonResult GetByStatusCurrentEmpMobile(int? statusId = null, string statusIds = null, string searchText = null, int? pageSize = 10, int? pageNumber = 1, string cols = null)
        {
            if (string.IsNullOrWhiteSpace(statusIds))
            {
                if (statusId.HasValue) statusIds = statusId.ToString();
            }
            else
            {
                if (statusId.HasValue) statusIds += ("," + statusId);
            }
            var currentUser = this.GetCurrentUser();
            var result = _unitOfWork.Repository<Proc_GetRequestShipmentCurrentEmpMobile>()
                      .ExecProcedure(Proc_GetRequestShipmentCurrentEmpMobile.GetEntityProc(currentUser.Id, statusIds, searchText, pageNumber, pageSize));
            return JsonUtil.Success(result);
        }

    }
}
