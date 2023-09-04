using AutoMapper;
using Core.Api.Infrastruture;
using Core.Business.Core.Utils;
using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Business.ViewModels;
using Core.Business.ViewModels.Shipments;
using Core.Data;
using Core.Data.Abstract;
using Core.Data.Core;
using Core.Entity.Entities;
using Core.Entity.Procedures;
using Core.Infrastructure.Helper;
using Core.Infrastructure.Utils;
using Core.Infrastructure.ViewModels;
using LinqKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MoreLinq;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using OfficeOpenXml.Drawing;
using Core.Api.Library;
using Core.Business.ViewModels.General;
using Core.Entity.Procedures.Shipment;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public partial class ShipmentController : GeneralController<CreateUpdateShipmentViewModel, ShipmentInfoViewModel, Shipment>
    {
        private readonly IGeneralService<CreateUpdateLadingScheduleViewModel, LadingSchedule> _iLadingScheduleService;
        private readonly IShipmentService _iShipmentService;
        private readonly IGeneralService _iGeneralServiceRaw;
        private readonly IListGoodsService _iListGoodsService;
        private readonly IUserService _iuserService;
        private readonly IKPIShipmentDetailService _iKPIShipmentDetailService;
        private readonly CompanyInformation _icompanyInformation;
        private ApplicationContext _context;
        private ApplicationContextRRP _contextRRP;

        public ShipmentController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            ApplicationContext context,
            ApplicationContextRRP contextRRP,
            IOptions<AppSettings> optionsAccessor,
            IOptions<JwtIssuerOptions> jwtOptions,
            IOptions<CompanyInformation> companyInformation,
            IUnitOfWork unitOfWork,
            IGeneralService<CreateUpdateShipmentViewModel, ShipmentInfoViewModel, Shipment> iGeneralService,
            IGeneralService<CreateUpdateLadingScheduleViewModel, LadingSchedule> iLadingScheduleService,
            IShipmentService iShipmentService,
            IGeneralService iGeneralServiceRaw,
            IListGoodsService iListGoodsService,
            IKPIShipmentDetailService iKPIShipmentDetailService,
            IUserService iuserService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
            _context = context;
            _contextRRP = contextRRP;
            _iLadingScheduleService = iLadingScheduleService;
            _iShipmentService = iShipmentService;
            _iGeneralServiceRaw = iGeneralServiceRaw;
            _iListGoodsService = iListGoodsService;
            _iuserService = iuserService;
            _icompanyInformation = companyInformation.Value;
            _iKPIShipmentDetailService = iKPIShipmentDetailService;
        }

        //[HttpGet("TestBAO")]
        //public JsonResult TestBAO()
        //{
        //    var data = _unitOfWork.RepositoryR<Shipment>().GetAll();
        //    return JsonUtil.Success(data);
        //}

        public override JsonResult GetAll(int? pageSize = null, int? pageNumber = null, string cols = null)
        {
            return JsonUtil.Error("Died");
            //return base.GetAll(pageSize, pageNumber, cols);
        }


        [HttpGet("GetAllHistory")]
        public JsonResult GetAllHistory(DateTime fromDate, DateTime toDate)
        {
            return JsonUtil.Create(_iShipmentService.GetLadingHistory(GetCurrentUser(), "", fromDate, toDate));
        }

        [HttpGet("GetByType")]
        public JsonResult GetByType(string type, int? pageSize = null, int? pageNumber = null, string cols = null)
        {
            return JsonUtil.Create(_iShipmentService.GetByType(GetCurrentUser(), type, pageSize, pageNumber, cols, null));
        }

        [HttpPost("PostByType")]
        public JsonResult PostByType([FromBody] ShipmentFilterViewModel filterViewModel = null)
        {
            return JsonUtil.Create(_iShipmentService.GetByType(GetCurrentUser(), filterViewModel.type, filterViewModel.pageSize, filterViewModel.pageNumber, filterViewModel.cols, filterViewModel));
        }

        [HttpPost("GetByListId")]
        public JsonResult GetByListId([FromBody] ShipmentListPrintViewModel viewModel)
        {
            Expression<Func<Shipment, bool>> predicate = x => viewModel.ShipmentIds.Contains(x.Id);
            return JsonUtil.Create(_iGeneralService.FindBy(predicate, null, null, viewModel.cols));
        }

        [HttpGet("GetByTPL")]
        public JsonResult GetByTPL(DateTime? fromDate = null, DateTime? toDate = null, int? pageSize = null, int? pageNumber = null, string cols = null)
        {
            Expression<Func<Shipment, bool>> predicate = x => x.TPLId.HasValue == true && x.TPLCreatedWhen.HasValue == true;
            if (fromDate.HasValue)
            {
                predicate = predicate.And(f => f.TPLCreatedWhen >= fromDate);
            }
            if (toDate.HasValue)
            {
                predicate = predicate.And(f => f.TPLCreatedWhen <= toDate);
            }
            return JsonUtil.Create(_iGeneralService.FindBy(predicate, pageSize, pageNumber, cols));
        }

        [HttpGet("GetByStatusEmpId")]
        public JsonResult GetByStatusEmpId(int empId, int statusId, DateTime? fromDate = null, DateTime? toDate = null, int? pageSize = null, int? pageNumber = null, string cols = null)
        {
            Expression<Func<Shipment, bool>> predicate = x => x.ShipmentStatusId == statusId;

            switch (statusId)
            {
                case StatusHelper.ShipmentStatusId.Picking:
                    {
                        Expression<Func<RequestShipment, bool>> predicateRequest = x => x.ShipmentStatusId == statusId;
                        predicateRequest = predicateRequest.And(x => x.CurrentEmpId == empId);
                        return JsonUtil.Create(_iGeneralServiceRaw.FindBy<RequestShipment, RequestShipmentInfoViewModel>(predicateRequest, pageSize, pageNumber, cols));
                    }
                case StatusHelper.ShipmentStatusId.Transferring:
                case StatusHelper.ShipmentStatusId.TransferReturning:
                case StatusHelper.ShipmentStatusId.Delivering:
                case StatusHelper.ShipmentStatusId.Returning:
                    {
                        predicate = predicate.And(x => x.CurrentEmpId == empId);
                        return JsonUtil.Create(_iGeneralService.FindBy(predicate, pageSize, pageNumber, cols));
                    }
                case StatusHelper.ShipmentStatusId.PickupComplete:
                    {
                        Expression<Func<RequestShipment, bool>> predicateRequest = x => x.ShipmentStatusId == statusId;

                        if (fromDate.HasValue && toDate.HasValue)
                        {
                            predicateRequest = predicateRequest.And(x => fromDate <= x.StartPickTime && x.StartPickTime <= toDate);
                        }
                        else if (fromDate.HasValue)
                        {
                            predicateRequest = predicateRequest.And(x => fromDate <= x.StartPickTime);
                        }
                        else if (toDate.HasValue)
                        {
                            predicateRequest = predicateRequest.And(x => x.StartPickTime <= toDate);
                        }

                        predicateRequest = predicateRequest.And(x => x.PickUserId == empId);
                        return JsonUtil.Create(_iGeneralServiceRaw.FindBy<RequestShipment, RequestShipmentInfoViewModel>(predicateRequest, pageSize, pageNumber, cols));
                    }
                case StatusHelper.ShipmentStatusId.DeliveryComplete:
                    {
                        if (fromDate.HasValue && toDate.HasValue)
                        {
                            predicate = predicate.And(x => fromDate <= x.EndDeliveryTime && x.EndDeliveryTime <= toDate);
                        }
                        else if (fromDate.HasValue)
                        {
                            predicate = predicate.And(x => fromDate <= x.EndDeliveryTime);
                        }
                        else if (toDate.HasValue)
                        {
                            predicate = predicate.And(x => x.EndDeliveryTime <= toDate);
                        }

                        predicate = predicate.And(x => x.DeliverUserId == empId);
                        return JsonUtil.Create(_iGeneralService.FindBy(predicate, pageSize, pageNumber, cols));
                    }
                case StatusHelper.ShipmentStatusId.ReturnComplete:
                    {
                        if (fromDate.HasValue && toDate.HasValue)
                        {
                            predicate = predicate.And(x => fromDate <= x.EndReturnTime && x.EndReturnTime <= toDate);
                        }
                        else if (fromDate.HasValue)
                        {
                            predicate = predicate.And(x => fromDate <= x.EndReturnTime);
                        }
                        else if (toDate.HasValue)
                        {
                            predicate = predicate.And(x => x.EndReturnTime <= toDate);
                        }

                        predicate = predicate.And(x => x.ReturnUserId == empId);
                        return JsonUtil.Create(_iGeneralService.FindBy(predicate, pageSize, pageNumber, cols));
                    }
            }
            return JsonUtil.Success(new ShipmentInfoViewModel[0]);
        }

        [HttpGet("GetShipmentReportCurrentEmp")]
        public JsonResult GetShipmentReportCurrentEmp()
        {
            var currentUserId = GetCurrentUserId();
            var unitOfWordRRP = new UnitOfWorkRRP(_contextRRP);
            var data = unitOfWordRRP.Repository<Proc_ReportEmployeeOne>()
                .ExecProcedureSingle(Proc_ReportEmployeeOne.GetEntityProc(currentUserId));
            return JsonUtil.Success(data);
        }

        [HttpGet("GetShipmentReportByEmpId")]
        public async Task<JsonResult> GetShipmentReportByEmpId(int empId, DateTime? fromDate = null, DateTime? toDate = null, int? pageSize = null, int? pageNumber = null, string cols = null)
        {
            var reportEmp = new ShipmentReportEmpViewModel();
            reportEmp.Picking = _unitOfWork.RepositoryR<RequestShipment>().Count(x239 => x239.ShipmentStatusId == StatusHelper.ShipmentStatusId.Picking && x239.CurrentEmpId == empId);
            reportEmp.Transferring = await _unitOfWork.RepositoryR<Shipment>().CountAsync(shipment => shipment.ShipmentStatusId == StatusHelper.ShipmentStatusId.Transferring && shipment.CurrentEmpId == empId);
            reportEmp.TransferReturning = await _unitOfWork.RepositoryR<Shipment>().CountAsync(shipment => shipment.ShipmentStatusId == StatusHelper.ShipmentStatusId.TransferReturning && shipment.CurrentEmpId == empId);
            reportEmp.Delivering = await _unitOfWork.RepositoryR<Shipment>().CountAsync(shipment => shipment.ShipmentStatusId == StatusHelper.ShipmentStatusId.Delivering && shipment.CurrentEmpId == empId);
            reportEmp.Returning = await _unitOfWork.RepositoryR<Shipment>().CountAsync(shipment => shipment.ShipmentStatusId == StatusHelper.ShipmentStatusId.Returning && shipment.CurrentEmpId == empId);
            reportEmp.WaitingToTransfer = await _unitOfWork.RepositoryR<Shipment>().CountAsync(shipment => shipment.ShipmentStatusId == StatusHelper.ShipmentStatusId.WaitingToTransfer && shipment.CurrentEmpId == empId);
            reportEmp.AssignEmployeePickup = await _unitOfWork.RepositoryR<RequestShipment>().CountAsync(x245 => x245.ShipmentStatusId == StatusHelper.ShipmentStatusId.AssignEmployeePickup && x245.CurrentEmpId == empId);

            #region Pick
            if (fromDate.HasValue && toDate.HasValue)
            {
                reportEmp.PickupComplete = await _unitOfWork.RepositoryR<RequestShipment>().CountAsync(
                    x251 => fromDate <= x251.StartPickTime && x251.StartPickTime <= toDate &&
                    x251.ShipmentStatusId == StatusHelper.ShipmentStatusId.PickupComplete &&
                    x251.PickUserId == empId);
            }
            else if (fromDate.HasValue)
            {
                reportEmp.PickupComplete = await _unitOfWork.RepositoryR<RequestShipment>().CountAsync(
                    x258 => fromDate <= x258.StartPickTime &&
                    x258.ShipmentStatusId == StatusHelper.ShipmentStatusId.PickupComplete &&
                    x258.PickUserId == empId);
            }
            else if (toDate.HasValue)
            {
                reportEmp.PickupComplete = await _unitOfWork.RepositoryR<RequestShipment>().CountAsync(
                    x265 => x265.StartPickTime <= toDate &&
                    x265.ShipmentStatusId == StatusHelper.ShipmentStatusId.PickupComplete &&
                    x265.PickUserId == empId);
            }
            else
            {
                reportEmp.PickupComplete = await _unitOfWork.RepositoryR<RequestShipment>().CountAsync(
                    x272 => x272.ShipmentStatusId == StatusHelper.ShipmentStatusId.PickupComplete &&
                    x272.PickUserId == empId);
            }
            #endregion

            #region Delivery
            if (fromDate.HasValue && toDate.HasValue)
            {
                reportEmp.DeliveryComplete = await _unitOfWork.RepositoryR<Shipment>().CountAsync(
                    shipment => fromDate <= shipment.EndDeliveryTime && shipment.EndDeliveryTime <= toDate &&
                    shipment.ShipmentStatusId == StatusHelper.ShipmentStatusId.DeliveryComplete &&
                    shipment.DeliverUserId == empId);
            }
            else if (fromDate.HasValue)
            {
                reportEmp.DeliveryComplete = await _unitOfWork.RepositoryR<Shipment>().CountAsync(
                    shipment => fromDate <= shipment.EndDeliveryTime &&
                    shipment.ShipmentStatusId == StatusHelper.ShipmentStatusId.DeliveryComplete &&
                    shipment.DeliverUserId == empId);
            }
            else if (toDate.HasValue)
            {
                reportEmp.DeliveryComplete = await _unitOfWork.RepositoryR<Shipment>().CountAsync(
                    shipment => shipment.EndDeliveryTime <= toDate &&
                    shipment.ShipmentStatusId == StatusHelper.ShipmentStatusId.DeliveryComplete &&
                    shipment.DeliverUserId == empId);
            }
            else
            {
                reportEmp.DeliveryComplete = await _unitOfWork.RepositoryR<Shipment>().CountAsync(
                    shipment => shipment.ShipmentStatusId == StatusHelper.ShipmentStatusId.DeliveryComplete &&
                    shipment.DeliverUserId == empId);
            }
            #endregion

            #region Return
            if (fromDate.HasValue && toDate.HasValue)
            {
                reportEmp.ReturnComplete = await _unitOfWork.RepositoryR<Shipment>().CountAsync(
                    shipment => fromDate <= shipment.EndReturnTime && shipment.EndReturnTime <= toDate &&
                    shipment.ShipmentStatusId == StatusHelper.ShipmentStatusId.ReturnComplete &&
                    shipment.ReturnUserId == empId);
            }
            else if (fromDate.HasValue)
            {
                reportEmp.ReturnComplete = await _unitOfWork.RepositoryR<Shipment>().CountAsync(
                    shipment => fromDate <= shipment.EndReturnTime &&
                    shipment.ShipmentStatusId == StatusHelper.ShipmentStatusId.ReturnComplete &&
                    shipment.ReturnUserId == empId);
            }
            else if (toDate.HasValue)
            {
                reportEmp.ReturnComplete = await _unitOfWork.RepositoryR<Shipment>().CountAsync(
                    shipment => shipment.EndReturnTime <= toDate &&
                    shipment.ShipmentStatusId == StatusHelper.ShipmentStatusId.ReturnComplete &&
                    shipment.ReturnUserId == empId);
            }
            else
            {
                reportEmp.ReturnComplete = await _unitOfWork.RepositoryR<Shipment>().CountAsync(
                    shipment => shipment.ShipmentStatusId == StatusHelper.ShipmentStatusId.ReturnComplete &&
                    shipment.ReturnUserId == empId);
            }
            #endregion

            return JsonUtil.Success(reportEmp);
        }

        public override async Task<JsonResult> Create([FromBody] CreateUpdateShipmentViewModel viewModel)
        {
            var checkNumber = _unitOfWork.Repository<Proc_GetShipmentByShipmentNumber>()
                .ExecProcedure(Proc_GetShipmentByShipmentNumber.GetEntityProc(viewModel.ShipmentNumber)).ToList();
            if (checkNumber.Count() > 0)
            {
                return JsonUtil.Error("Mã vận đơn đã được sử dụng!");
            }
            if (!viewModel.ShipmentStatusId.HasValue)
            {
                return JsonUtil.Error("Trạng thái vận đơn chưa có. IT!");
            }
            if (viewModel.IsBox == true) viewModel.ShipmentNumber = null;
            //PUSH VSE
            viewModel.IsPushVSE = false;
            viewModel.CountPushVSE = 0;
            //
            var currrentUser = GetCurrentUser();
            if (_icompanyInformation.Name == "gsdp" || _icompanyInformation.Name == "gsdp-staging" || _icompanyInformation.Name == "dlex" || _icompanyInformation.Name == "flashship_vn")
            {
                viewModel.ShipmentStatusId = StatusHelper.ShipmentStatusId.NewRequest;
            }
            if (viewModel.ShipmentStatusId != StatusHelper.ShipmentStatusId.NewRequest)
            {
                if (!viewModel.EndPickTime.HasValue) viewModel.EndPickTime = DateTime.Now;
            }
            if (viewModel.ToProvinceId.HasValue) viewModel.InputUserId = currrentUser.Id;
            if (viewModel.COD > 0 && !viewModel.PaymentCODTypeId.HasValue)
            {
                if (viewModel.PaymentTypeId == PaymentTypeHelper.NGUOI_NHAN_THANH_TOAN) viewModel.PaymentCODTypeId = PaymentCODTypeHelper.NGUOI_NHAN;
                else viewModel.PaymentCODTypeId = PaymentCODTypeHelper.NGUOI_GUI;
            }
            //if (viewModel.TypeId.HasValue)
            //{
            //    if (viewModel.TypeId == CreateShipmentType.Return) viewModel.IsReturn = true;
            //}
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
            if (viewModel.OrderDate == DateTime.MinValue) viewModel.OrderDate = DateTime.Now;
            if (viewModel.ShipmentStatusId == StatusHelper.ShipmentStatusId.PickupComplete)
            {
                viewModel.CurrentHubId = currrentUser.HubId;
            }
            var data = await _iGeneralServiceRaw.Create<Shipment, CreateUpdateShipmentViewModel>(viewModel);
            if (data.IsSuccess)
            {
                var shipment = data.Data as Shipment;
                var lading = new CreateUpdateLadingScheduleViewModel(
                    shipment.Id,
                    shipment.FromHubId,
                    currrentUser.Id,
                    viewModel.ShipmentStatusId.Value,
                    viewModel.CurrentLat,
                    viewModel.CurrentLng,
                    viewModel.Location,
                    viewModel.Note,
                    0,
                    viewModel.ReasonId
                );
                await _iLadingScheduleService.Create(lading);
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
                if ((viewModel.priceDVGTs != null && viewModel.priceDVGTs.Count() > 0))
                {
                    foreach (var priceDVGT in viewModel.priceDVGTs)
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
                        newBox.ExcWeight = box.ExcWeight;
                        newBox.CalWeight = box.CalWeight;
                        newBox.Length = box.Length;
                        newBox.Width = box.Width;
                        newBox.Height = box.Height;
                        newBox.Content = box.Content;
                        _unitOfWork.RepositoryCRUD<Box>().Insert(newBox);
                    }
                    await _unitOfWork.CommitAsync();
                }
                if (shipment.ShipmentId.HasValue)
                {
                    var isPaymentChild = false;
                    if (shipment.PriceReturn > 0) isPaymentChild = true;
                    _unitOfWork.Repository<Proc_AcceptCreateShipmentChild>()
                                .ExecProcedureSingle(Proc_AcceptCreateShipmentChild.GetEntityProc(shipment.ShipmentId.Value, isPaymentChild));
                }
                //if (_icompanyInformation.Name == "vietstar")
                //{
                //    VSEApi _VSEApi = new VSEApi();
                //    string message = string.Format("VIETSTAR EXPRESS DA NHAN DON HANG CUA BAN MA VAN DON {0}", shipment.ShipmentNumber);
                //    _VSEApi.SendSMSNormal(shipment.SenderPhone, message);
                //}
            }
            return JsonUtil.Create(data);
        }

        [HttpPost("PushVSE")]
        public JsonResult PushVSE([FromBody] List<CreateUpdateShipmentViewModel> shipments)
        {
            foreach (var itemShipment in shipments)
            {
                var data = _unitOfWork.RepositoryR<Shipment>().FindBy(f463 => f463.ShipmentNumber == itemShipment.ShipmentNumber);
                foreach (var shipment in data)
                {
                    //Create ebill VSE
                    shipment.CountPushVSE = 0;
                    shipment.IsPushVSE = false;
                    _unitOfWork.RepositoryCRUD<Shipment>().Update(shipment);
                }
            }
            _unitOfWork.Commit();
            return JsonUtil.Success("SUCCESS");
        }

        [HttpPost("PushVSEImg")]
        public JsonResult PushVSEImg([FromBody] List<CreateUpdateShipmentViewModel> shipments)
        {
            foreach (var itemShipment in shipments)
            {
                var shipment = _unitOfWork.RepositoryR<Shipment>().GetSingle(f463 => f463.ShipmentNumber == itemShipment.ShipmentNumber);
                if (!Util.IsNull(shipment))
                {
                    //push bill img VSE
                    using (var context = new ApplicationContext())
                    {
                        var _unitOfWork = new UnitOfWork(context);
                        try
                        {
                            string UserVSE = "";
                            if (shipment.PickUserId.HasValue)
                            {
                                var user = _unitOfWork.RepositoryR<User>().GetSingle(shipment.PickUserId.Value);
                                if (Util.IsNull(user.VSEOracleCode))
                                {
                                    UserVSE = user.Code;
                                }
                                else
                                {
                                    UserVSE = user.VSEOracleCode;
                                }
                            }
                            else if (shipment.CreatedBy.HasValue)
                            {
                                var user = _unitOfWork.RepositoryR<User>().GetSingle(shipment.CreatedBy.Value);
                                if (Util.IsNull(user.VSEOracleCode))
                                {
                                    UserVSE = user.Code;
                                }
                                else
                                {
                                    UserVSE = user.VSEOracleCode;
                                }
                            }
                            //
                            VSEApi vSEApi = new VSEApi();
                            bool checkResult = false;
                            string resultPushIMG = "--0--";
                            checkResult = true;
                            //
                            if (!Util.IsNull(shipment.PickupImagePath))
                            {
                                var imgInfo = FileUtil.GetFile(shipment.PickupImagePath);
                                string[] fileNames = shipment.PickupImagePath.Split('/');
                                if (Util.IsNull(imgInfo.FileName))
                                {
                                    imgInfo.FileName = fileNames[fileNames.Count() - 1];
                                }
                                var taskPushIMG = vSEApi.PushImagePickup("0", shipment.ShipmentNumber, "0", imgInfo.FileName,
                                     UserVSE, "", "", imgInfo.FileBase64String + "");
                                try
                                {
                                    resultPushIMG = taskPushIMG.Result + "";
                                }
                                catch (Exception ex)
                                {
                                    resultPushIMG = ex.Message;
                                }
                            }
                            else
                            {
                                resultPushIMG = "--not image--";
                            }
                            //
                            var resultVSE = "{" + shipment.ShipmentNumber + "} - push-img-pickup: {" + resultPushIMG + "}";
                            using (var context2 = new ApplicationContext())
                            {
                                var _unitOfWork2 = new UnitOfWork(context2);
                                _unitOfWork2.Repository<Proc_PushImgPickupVSE>()
                                .ExecProcedureSingle(Proc_PushImgPickupVSE.GetEntityProc(shipment.Id, checkResult, "", resultVSE));
                            }
                            _unitOfWork.Repository<Proc_TaskScheduler>()
                            .ExecProcedureSingle(Proc_TaskScheduler.GetEntityProc("PUSHIMG-PICKUP Run Task Push VSE Oracle auto", 0, "success"));

                        }
                        catch (Exception ex)
                        {
                            _unitOfWork.Repository<Proc_TaskScheduler>()
                             .ExecProcedureSingle(Proc_TaskScheduler.GetEntityProc("PUSHIMG-PICKUP Run Task Push VSE Oracle auto", 0, "Error: " + ex.Message));
                        }
                    }
                }
            }
            _unitOfWork.Commit();
            return JsonUtil.Success("SUCCESS");
        }

        [HttpPost("UploadExcel")]
        public async Task<JsonResult> UploadExcel([FromBody] List<CreateUpdateShipmentViewModel> viewModels)
        {
            if (Util.IsNull(viewModels))
            {
                return JsonUtil.Error("Danh sách upload excel trống");
            }
            int updateSuccess = 0;
            int createSuccess = 0;
            var currentUser = GetCurrentUser();

            List<Shipment> listNewShipment = new List<Shipment>();
            foreach (var viewModel in viewModels)
            {
                var shipmentCheck = await _unitOfWork.RepositoryR<Shipment>().GetSingleAsync(viewModel.Id);
                if (Util.IsNull(shipmentCheck))
                {
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
                    if (viewModel.COD > 0 && !viewModel.PaymentCODTypeId.HasValue) viewModel.PaymentCODTypeId = PaymentCODTypeHelper.NGUOI_GUI;
                    //create
                    viewModel.CountPushVSE = 0;
                    viewModel.IsPushVSE = false;
                    if (_icompanyInformation.Name == "gsdp")
                    {
                        viewModel.ShipmentStatusId = StatusHelper.ShipmentStatusId.NewRequest;
                    }
                    if (viewModel.ShipmentStatusId != StatusHelper.ShipmentStatusId.NewRequest)
                    {
                        if (!viewModel.EndPickTime.HasValue) viewModel.EndPickTime = DateTime.Now;
                    }
                    var data = await _iGeneralServiceRaw.Create<Shipment, CreateUpdateShipmentViewModel>(viewModel);
                    if (data.IsSuccess)
                    {
                        createSuccess++;
                        var shipment = data.Data as Shipment;
                        var lading = new CreateUpdateLadingScheduleViewModel(
                            shipment.Id,
                            shipment.FromHubId,
                            GetCurrentUserId(),
                            viewModel.ShipmentStatusId.Value,
                            viewModel.CurrentLat,
                            viewModel.CurrentLng,
                            viewModel.Location,
                            viewModel.Note,
                            0,
                            viewModel.ReasonId
                        );
                        await _iLadingScheduleService.Create(lading);

                        if (string.IsNullOrWhiteSpace(shipment.ShipmentNumber))
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

                        if (viewModel.ServiceDVGTIds != null && viewModel.ServiceDVGTIds.Count() > 0)
                        {
                            foreach (var sDVGTId in viewModel.ServiceDVGTIds)
                            {
                                var ssDVGT = new ShipmentServiceDVGT();
                                ssDVGT.ShipmentId = shipment.Id;
                                ssDVGT.ServiceId = sDVGTId;
                                _unitOfWork.RepositoryCRUD<ShipmentServiceDVGT>().Insert(ssDVGT);
                            }
                        }
                        if ((viewModel.ServiceDVGTIds != null && viewModel.ServiceDVGTIds.Count() > 0) || (viewModel.Boxes != null && viewModel.Boxes.Count() > 0))
                        {
                            await _unitOfWork.CommitAsync();
                        }
                        //
                        listNewShipment.Add(shipment);
                    }
                }
                else
                {
                    //if (shipmentCheck.ShipmentStatusId != viewModel.ShipmentStatusId)
                    //{
                    //    //return JsonUtil.Error("Trạng thái vận đơn cập nhật không hợp lệ");
                    //}
                    //else
                    //{
                    //    var data = await _iGeneralServiceRaw.Update<Shipment, CreateUpdateShipmentViewModel>(viewModel);
                    //    if (data.IsSuccess)
                    //    {
                    //        updateSuccess++;
                    //        var shipment = data.Data as Shipment;
                    //        //Log change data
                    //        if (viewModel.ServiceDVGTIds != null && viewModel.ServiceDVGTIds.Count() > 0)
                    //        {
                    //            _unitOfWork.RepositoryCRUD<ShipmentServiceDVGT>().DeleteWhere(x => x.ShipmentId == shipment.Id);

                    //            foreach (var sDVGTId in viewModel.ServiceDVGTIds)
                    //            {
                    //                var ssDVGT = new ShipmentServiceDVGT();
                    //                ssDVGT.ShipmentId = shipment.Id;
                    //                ssDVGT.ServiceId = sDVGTId;
                    //                _unitOfWork.RepositoryCRUD<ShipmentServiceDVGT>().Insert(ssDVGT);
                    //            }
                    //            await _unitOfWork.RepositoryCRUD<ShipmentServiceDVGT>().CommitAsync();
                    //        }
                    //    }
                    //}
                }
            }
            if (createSuccess > 0)
            {
                // ghi log phiên upload excel
                var UploadExcelHistory = new UploadExcelHistory();
                UploadExcelHistory.UserId = currentUser.Id;
                UploadExcelHistory.TotalCreated = createSuccess;
                _unitOfWork.RepositoryCRUD<UploadExcelHistory>().Insert(UploadExcelHistory);
                await _unitOfWork.RepositoryCRUD<UploadExcelHistory>().CommitAsync();

                foreach (var shipment in listNewShipment)
                {
                    shipment.UploadExcelHistoryId = UploadExcelHistory.Id;
                    await _iGeneralServiceRaw.Update<Shipment, ShipmentInfoViewModel, Shipment>(shipment);
                }
            }

            return JsonUtil.Success($"Tạo mới {createSuccess}, Cập nhật {updateSuccess} vận đơn / tổng {viewModels.Count()} vận đơn");
        }

        //
        [HttpPost("UploadExcelWithTableValued")]
        public JsonResult UploadExcelWithTableValued([FromBody] List<CreateUpdateShipmentViewModel> viewModels)
        {
            UploadExcelShipmentByProc sqlUserList = new UploadExcelShipmentByProc();
            sqlUserList.AddRange(viewModels);
            var data = _unitOfWork.Repository<Proc_UploadExcelWithTableValued>()
            .ExecProcedure(Proc_UploadExcelWithTableValued.GetEntityProc(sqlUserList));
            return JsonUtil.Success(data);
        }

        public override async Task<JsonResult> Update([FromBody] CreateUpdateShipmentViewModel viewModel)
        {
            var dataShipments = _unitOfWork.Repository<Proc_GetShipmentByShipmentNumber>()
                .ExecProcedure(Proc_GetShipmentByShipmentNumber.GetEntityProc(viewModel.ShipmentNumber)).ToList();
            if (dataShipments.Count() == 0)
            {
                return JsonUtil.Error("Không tìm dữ liệu cập nhật!");
            }
            var checkNumber = dataShipments.FirstOrDefault(f => f.Id != viewModel.Id);
            if (!Util.IsNull(checkNumber))
            {
                return JsonUtil.Error("Mã vận đơn đã được sử dụng!");
            }
            var shipmentCheck = _unitOfWork.RepositoryR<Shipment>().GetSingle(viewModel.Id);
            if (shipmentCheck.ShipmentStatusId != viewModel.ShipmentStatusId)
            {
                return JsonUtil.Error("Trạng thái vận đơn cập nhật không hợp lệ");
            }
            if (shipmentCheck.ListCustomerPaymentTotalPriceId.HasValue)
            {
                viewModel.DefaultPrice = shipmentCheck.DefaultPrice;
                viewModel.PriceCOD = shipmentCheck.PriceCOD;
                viewModel.PriceReturn = shipmentCheck.PriceReturn;
                viewModel.RemoteAreasPrice = shipmentCheck.RemoteAreasPrice;
                viewModel.OtherPrice = shipmentCheck.OtherPrice;
                viewModel.TotalDVGT = shipmentCheck.TotalDVGT;
                viewModel.VATPrice = shipmentCheck.VATPrice;
                viewModel.TotalPrice = shipmentCheck.TotalPrice;
            }
            var currentUserId = GetCurrentUserId();
            if (!shipmentCheck.ToProvinceId.HasValue && viewModel.ToProvinceId.HasValue) viewModel.InputUserId = currentUserId;

            var isChangeShipmentStatus = false;

            if (viewModel.ShipmentStatusId != shipmentCheck.ShipmentStatusId)
            {
                isChangeShipmentStatus = true;
            }
            //
            //if (_icompanyInformation.Name == "vietstar" && viewModel.SenderId != 4)
            //{
            //    var result = PushUtil.PushUpdateBillVSE(viewModel);
            //    if (result == false) return JsonUtil.Error("Cập nhật thông tin vận đơn bên Oracle không thành công.");
            //}
            //
            var fromHubRounting = _iShipmentService.GetInfoRouting(viewModel.IsTruckDelivery, viewModel.FromDistrictId, viewModel.FromWardId, viewModel.CalWeight);
            if (!Util.IsNull(fromHubRounting))
            {
                viewModel.FromHubId = fromHubRounting.HubId;
                viewModel.FromHubRoutingId = fromHubRounting.HubRoutingId;
            }
            if (!viewModel.ToWardId.HasValue && !viewModel.ToDistrictId.HasValue)
            {
                viewModel.ToHubId = shipmentCheck.ToHubId;
                viewModel.ToHubRoutingId = shipmentCheck.ToHubRoutingId;
            }
            else
            {
                var toHubRouting = _iShipmentService.GetInfoRouting(viewModel.IsTruckDelivery, viewModel.ToDistrictId, viewModel.ToWardId, viewModel.CalWeight);
                if (!Util.IsNull(toHubRouting))
                {
                    viewModel.ToHubId = toHubRouting.HubId;
                    viewModel.ToHubRoutingId = toHubRouting.HubRoutingId;
                }
            }
            //
            //if (viewModel.FromHubId != viewModel.ToHubId)
            //{
            //    if (viewModel.ShipmentStatusId == StatusHelper.ShipmentStatusId.ReadyToDelivery)
            //    {
            //        viewModel.ShipmentStatusId = StatusHelper.ShipmentStatusId.WaitingToTransfer;
            //    }
            //}
            //else
            //{
            //    if (viewModel.ShipmentStatusId == StatusHelper.ShipmentStatusId.WaitingToTransfer)
            //    {
            //        viewModel.ShipmentStatusId = StatusHelper.ShipmentStatusId.ReadyToDelivery;
            //    }
            //}
            //
            var data = await _iGeneralServiceRaw.Update<Shipment, CreateUpdateShipmentViewModel>(viewModel);

            if (data.IsSuccess)
            {
                var shipment = data.Data as Shipment;
                //change request
                List<int> listChangeRequest = new List<int>();
                listChangeRequest.Add(StatusHelper.ShipmentStatusId.NewRequest);
                listChangeRequest.Add(StatusHelper.ShipmentStatusId.ReadyToPick);
                listChangeRequest.Add(StatusHelper.ShipmentStatusId.RejectPickup);
                listChangeRequest.Add(StatusHelper.ShipmentStatusId.Picking);
                listChangeRequest.Add(StatusHelper.ShipmentStatusId.PickupFail);
                listChangeRequest.Add(StatusHelper.ShipmentStatusId.AssignEmployeePickup);
                //
                if (listChangeRequest.Contains(shipment.ShipmentStatusId))
                {
                    _unitOfWork.Repository<Proc_UpdateFastBooking>().ExecProcedureSingle(
                        Proc_UpdateFastBooking.GetEntityProc(shipment.Id));
                }
                //Log change data
                if (viewModel.ServiceDVGTIds != null && viewModel.ServiceDVGTIds.Count() > 0)
                {
                    _unitOfWork.RepositoryCRUD<ShipmentServiceDVGT>().DeleteEmptyWhere(x => x.ShipmentId == shipment.Id);
                    foreach (var sDVGTId in viewModel.ServiceDVGTIds)
                    {
                        var ssDVGT = new ShipmentServiceDVGT();
                        ssDVGT.ShipmentId = shipment.Id;
                        ssDVGT.ServiceId = sDVGTId;
                        var priceDVGT = viewModel.priceDVGTs.FirstOrDefault(f => f.ServiceId == ssDVGT.ServiceId);
                        if (!Util.IsNull(priceDVGT))
                        {
                            ssDVGT.IsAgree = priceDVGT.IsAgree;
                            ssDVGT.Price = priceDVGT.TotalPrice;
                        }
                        _unitOfWork.RepositoryCRUD<ShipmentServiceDVGT>().Insert(ssDVGT);
                    }
                    await _unitOfWork.RepositoryCRUD<ShipmentServiceDVGT>().CommitAsync();
                }
                //
                if (!Util.IsNull(viewModel.Boxes) && viewModel.Boxes.Count() > 0)
                {
                    _unitOfWork.RepositoryCRUD<Box>().DeleteWhere(x => x.ShipmentId == shipment.Id && !viewModel.Boxes.Select(s => s.Id).Contains(x.Id));
                    //
                    foreach (var box in viewModel.Boxes.Where(f => f.Id == 0))
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
                    //
                    foreach (var box in viewModel.Boxes.Where(f => f.Id > 0))
                    {
                        var newBox = _unitOfWork.RepositoryCRUD<Box>().GetSingle(box.Id);
                        newBox.ShipmentId = shipment.Id;
                        newBox.Weight = box.Weight;
                        newBox.CalWeight = box.CalWeight;
                        newBox.ExcWeight = box.ExcWeight;
                        newBox.Length = box.Length;
                        newBox.Width = box.Width;
                        newBox.Height = box.Height;
                        newBox.Content = box.Content;
                    }
                    await _unitOfWork.CommitAsync();
                }
                //
                if (shipment.IsBox && shipment.ShipmentId.HasValue) await _iShipmentService.ReCalculatePrice(shipment.ShipmentId.Value);
                // create lading update status
                if (isChangeShipmentStatus)
                {
                    var lading2 = new CreateUpdateLadingScheduleViewModel(
                        shipment.Id,
                        shipment.FromHubId,
                        currentUserId,
                        shipment.ShipmentStatusId,
                        viewModel.CurrentLat,
                        viewModel.CurrentLng,
                        viewModel.Location,
                        shipment.Note,
                        0,
                            viewModel.ReasonId
                    );
                    await _iLadingScheduleService.Create(lading2);
                }
            }
            return JsonUtil.Create(data);
        }

        [HttpPost("ChangeShipmentNumber")]
        public JsonResult ChangeShipmentNumber([FromBody] List<CreateUpdateShipmentViewModel> shipments)
        {
            //List<string> SUCCESS = new List<string>();
            //foreach (var itemShipment in shipments)
            //{
            //    var shipment = _unitOfWork.RepositoryCRUD<Shipment>().GetSingle(itemShipment.Id);
            //    if (!Util.IsNull(shipment))
            //        if (Util.IsNull(shipment.ShipmentNumber))
            //        {
            //            shipment.ShipmentNumber = _iShipmentService.GetCodeByType(_icompanyInformation.TypeShipmentCode, _icompanyInformation.PrefixShipmentCode, shipment);
            //            if (shipment.ShipmentId.HasValue && shipment.TypeId == ShipmentTypeHelper.CreateShipmentType.Support)
            //            {
            //                shipment.ShipmentNumber = $"{shipment.ShipmentNumber}SP";
            //            }
            //            SUCCESS.Add(shipment.ShipmentNumber);
            //        }
            //}
            //await _unitOfWork.CommitAsync();
            //return JsonUtil.Success(SUCCESS, "SUCCESS");
            return JsonUtil.Success("Api locked BY BAOLQ");
        }

        [AllowAnonymous]
        [HttpGet("TrackingShort")]
        public JsonResult TrackingShort(string shipmentNumber, string cols)
        {
            var shipmentResult = _iGeneralServiceRaw.GetSingle<Shipment, ShipmentInfoViewModel>(shipInfo => shipInfo.ShipmentNumber == shipmentNumber || shipInfo.SOENTRY == shipmentNumber, cols);
            if (shipmentResult.IsSuccess)
            {
                var shipment = shipmentResult.Data as ShipmentInfoViewModel;
                if (shipment == null) return JsonUtil.Error("Không tìm thấy đơn hàng");
                //PROC
                shipment.LadingSchedules = _unitOfWork.Repository<Proc_LadingSchedule_Joined>().ExecProcedure(Proc_LadingSchedule_Joined.GetEntityProc(shipment.Id));
            }
            return JsonUtil.Create(shipmentResult);
        }

        [HttpGet("Tracking")]
        public JsonResult Tracking(string shipmentNumber, string SOEntry, string cols)
        {
            var data = _iGeneralServiceRaw.GetSingle<Shipment, ShipmentInfoViewModel>(x => x.ShipmentNumber == shipmentNumber || x.SOENTRY == shipmentNumber, cols: cols);
            if (!data.IsSuccess) return JsonUtil.Create(data);
            var shipment = data.Data as ShipmentInfoViewModel;
            if (shipment == null) return JsonUtil.Error("Không tìm thấy đơn hàng");
            shipment.LadingSchedules = _unitOfWork.Repository<Proc_LadingSchedule_Joined>().ExecProcedure(Proc_LadingSchedule_Joined.GetEntityProc(shipment.Id));
            return JsonUtil.Create(data);
        }

        [HttpGet("GetOnlyShipment")]
        public JsonResult GetOnlyShipment(string shipmentNumber, string cols)
        {
            var data = _iGeneralServiceRaw.GetSingle<Shipment, ShipmentInfoViewModel>(x => x.ShipmentNumber == shipmentNumber, cols: cols);
            return JsonUtil.Create(data);
        }

        [HttpGet("CheckExistTrackingNumber")]
        public JsonResult CheckExistTrackingNumber(string shipmentNumber)
        {
            var data = _unitOfWork.Repository<Proc_GetShipmentByShipmentNumber>()
                .ExecProcedure(Proc_GetShipmentByShipmentNumber.GetEntityProc(shipmentNumber)).ToList();
            if (data.Count() > 0)
            {
                return JsonUtil.Success(data, "Đã sử dụng", 1);
            }
            else
            {
                return JsonUtil.Error("Chưa sửa dụng");
            }
        }

        [HttpGet("CheckExistShipmentNumber")]
        public JsonResult CheckExistShipmentNumber(string shipmentNumber)
        {
            var dataShipments = _unitOfWork.Repository<Proc_GetShipmentByShipmentNumber>()
                .ExecProcedure(Proc_GetShipmentByShipmentNumber.GetEntityProc(shipmentNumber)).ToList();
            if (dataShipments.Count() == 0)
            {
                return JsonUtil.Success("Mã hợp lệ");
            }
            else
            {
                return JsonUtil.Error("Đã được sử dụng");
            }
        }

        [HttpPost("UpdateStatusList")]
        public async Task<JsonResult> UpdateStatusList([FromBody] List<UpdateStatusViewModel> listViewModel)
        {
            var data = await _iGeneralService.Update<UpdateStatusViewModel>(listViewModel);
            var listLading = new List<CreateUpdateLadingScheduleViewModel>();
            var listInfo = new List<ShipmentInfoViewModel>(data.Data);

            if (data.IsSuccess)
            {
                foreach (var item in listInfo)
                {
                    var viewModel = listViewModel.SingleOrDefault(x => x.Id == item.Id);

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
                            0,
                            viewModel.ReasonId
                        ));
                    }
                }
            }

            if (listLading.Count > 0)
            {
                await _iLadingScheduleService.Update(listLading);
            }

            return JsonUtil.Create(data);
        }

        [HttpPost("AcceptCreditTransfer")]
        public async Task<JsonResult> AcceptCreditTransfer([FromBody] BasicViewModel viewModel)
        {
            var shipment = await _unitOfWork.RepositoryR<Shipment>().GetSingleAsync(viewModel.Id);
            if (Util.IsNull(shipment))
            {
                return JsonUtil.Error("Không tìm thấy thông tin vận đơn");
            }
            if (shipment.ShipmentStatusId != StatusHelper.ShipmentStatusId.DeliveryComplete)
            {
                return JsonUtil.Error("Chỉ được phép xác nhận chuyển khoản đối với vận đơn đã giao thành công");
            }
            if (shipment.IsCreditTransfer == true)
            {
                return JsonUtil.Error("Vận đơn đã được xác nhận chuyển khoản trước đó");
            }
            shipment.IsCreditTransfer = true;
            _unitOfWork.RepositoryCRUD<Shipment>().Update(shipment);
            await _unitOfWork.CommitAsync();
            return JsonUtil.Success(shipment);
        }

        [HttpPost("Cancel")]
        public async Task<JsonResult> Cancel([FromBody] UpdateStatusViewModel viewModel)
        {
            var shipment = await _unitOfWork.RepositoryR<Shipment>().GetSingleAsync(viewModel.Id);
            if (Util.IsNull(shipment))
            {
                return JsonUtil.Error("Không tìm thấy thông tin vận đơn");
            }
            if (Util.IsNull(viewModel.Note))
            {
                return JsonUtil.Error("Vui lòng nhập lý do hủy");
            }
            int[] statusComplete =
            {
                StatusHelper.ShipmentStatusId.DeliveryComplete,
                StatusHelper.ShipmentStatusId.ReturnComplete
            };
            if (statusComplete.Contains(shipment.ShipmentStatusId))
            {
                return JsonUtil.Error("Vận đơn đã hoàn tất, không được phép hủy");
            }
            if (shipment.ShipmentStatusId == StatusHelper.ShipmentStatusId.Cancel || !shipment.IsEnabled)
            {
                return JsonUtil.Error("Vận đơn đã hủy trước đó");
            }
            shipment.ReasonDelete = viewModel.Note;
            shipment.IsPushCustomer = true;
            shipment.ModifiedWhen = DateTime.Now;
            var data = await _iGeneralService.Update<Shipment>(shipment);
            await this.Delete(new BasicViewModel() { Id = shipment.Id });
            if (shipment.IsBox && shipment.ShipmentId.HasValue) await _iShipmentService.ReCalculatePrice(shipment.ShipmentId.Value);
            return JsonUtil.Create(data);
        }

        [HttpPost("ReCalculateShipment")]
        public async Task<JsonResult> ReCalculateShipment([FromBody] UpdateStatusViewModel viewModel)
        {
            var result = await _iShipmentService.ReCalculatePrice(viewModel.Id);
            if (Util.IsNull(result)) return JsonUtil.Error("Lỗi cập nhật cước.");
            else return JsonUtil.Success(result);
        }

        [HttpPost("UpdateStatusCurrentEmp")]
        public async Task<JsonResult> UpdateStatusCurrentEmp([FromBody] UpdateStatusViewModel viewModel)
        {
            viewModel.IsEnabled = true;
            var currentUser = GetCurrentUser();
            if (Util.IsNull(viewModel.CalWeight)) viewModel.CalWeight = 0;
            var checkShipments = _unitOfWork.Repository<Proc_GetShipmentByShipmentNumber>()
               .ExecProcedure(Proc_GetShipmentByShipmentNumber.GetEntityProc(viewModel.ShipmentNumber)).ToList();
            if (checkShipments.Count() > 0)
            {
                var checkNumber = checkShipments.Find(f => f.Id != viewModel.Id);
                if (!Util.IsNull(checkNumber))
                {
                    return JsonUtil.Error("Mã vận đơn đã được sử dụng!");
                }
            }
            //if(viewModel.ShipmentStatusId == 3)
            //{
            //    var checkStatusShipment = _unitOfWork
            //}
            var shipmentC = checkShipments.Find(f => f.Id == viewModel.Id);
            if (shipmentC.PackageId.HasValue && viewModel.IsPackage == false)
            {
                return JsonUtil.Error(string.Format("Vận đơn {0} đã đóng gói, vui lòng thao tác gói.", viewModel.ShipmentNumber));
            }
            if (viewModel.ShipmentStatusId == StatusHelper.ShipmentStatusId.DeliveryComplete || viewModel.ShipmentStatusId == StatusHelper.ShipmentStatusId.ReturnComplete)
            {
                if (string.IsNullOrWhiteSpace(viewModel.RealRecipientName)) return JsonUtil.Error(string.Format("Vui lòng nhập tên người nhận."));
                if (_icompanyInformation.Name == "vietstar") //  || _icompanyInformation.Name == "gsdp"
                {
                    var resCheckExistImageDelivery = _unitOfWork.Repository<Proc_CheckExistImageDelivery>()
               .ExecProcedureSingle(Proc_CheckExistImageDelivery.GetEntityProc(shipmentC.Id));
                    if (resCheckExistImageDelivery.IsSuccess == false) return JsonUtil.Error(string.Format("Upload ảnh ký nhận lỗi, vui lòng chụp lại.", viewModel.ShipmentNumber));
                }
            }
            if (shipmentC.CurrentEmpId.HasValue && shipmentC.CurrentEmpId != currentUser.Id && viewModel.ShipmentStatusId != StatusHelper.ShipmentStatusId.WaitingHandling)
            {
                return JsonUtil.Error(string.Format("Vận đơn '{0}' không thuộc quyền kiểm soát của bạn.", viewModel.ShipmentNumber));

            }
            if (!Util.IsNull(shipmentC) && !Util.IsNull(shipmentC.TPLNumber) && Util.IsNull(viewModel.TPLNumber))
            {
                viewModel.TPLNumber = shipmentC.TPLNumber;
            }
            List<int> statusPush = new List<int>();
            statusPush.Add(StatusHelper.ShipmentStatusId.DeliveryComplete);
            statusPush.Add(StatusHelper.ShipmentStatusId.ReturnComplete);
            statusPush.Add(StatusHelper.ShipmentStatusId.DeliveryFail);
            statusPush.Add(StatusHelper.ShipmentStatusId.ReturnComplete);
            statusPush.Add(StatusHelper.ShipmentStatusId.Cancel);
            if (statusPush.Contains(viewModel.ShipmentStatusId))
            {
                viewModel.IsPushCustomer = true;
            }
            if (viewModel.ShipmentStatusId == StatusHelper.ShipmentStatusId.DeliveryComplete && shipmentC.IsReturn == true)
            {
                viewModel.ShipmentStatusId = StatusHelper.ShipmentStatusId.ReturnComplete;
                if (!viewModel.EndReturnTime.HasValue && viewModel.EndDeliveryTime.HasValue) viewModel.EndReturnTime = viewModel.EndDeliveryTime;
            }
            else if (viewModel.ShipmentStatusId == StatusHelper.ShipmentStatusId.Delivering)
            {
                if (shipmentC.IsReturn == true)
                {
                    viewModel.ShipmentStatusId = StatusHelper.ShipmentStatusId.Returning;
                }
            }
            else if (viewModel.ShipmentStatusId == StatusHelper.ShipmentStatusId.DeliveryFail && shipmentC.IsReturn == true)
            {
                viewModel.ShipmentStatusId = StatusHelper.ShipmentStatusId.ReturnFail;
            }
            else if (viewModel.ShipmentStatusId == StatusHelper.ShipmentStatusId.PickupComplete)
            {
                if (!viewModel.EndPickTime.HasValue) viewModel.EndPickTime = DateTime.Now;
            }
            else if (viewModel.ShipmentStatusId == StatusHelper.ShipmentStatusId.WaitingHandling)
            {
                if (_icompanyInformation.Name == "gsdp")
                {
                    viewModel.EndPickTime = null;
                }
            }
            else if (viewModel.ShipmentStatusId == StatusHelper.ShipmentStatusId.ReadyToDelivery || viewModel.ShipmentStatusId == StatusHelper.ShipmentStatusId.WaitingToTransfer)
            {
                if (_icompanyInformation.Name == "gsdp")
                {
                    if (!viewModel.EndPickTime.HasValue) viewModel.EndPickTime = DateTime.Now;
                }
            }
            viewModel.CurrentHubId = currentUser.HubId;
            viewModel.CurrentEmpId = currentUser.Id;
            var data = await _iGeneralServiceRaw.Update<Shipment, UpdateStatusViewModel>(viewModel);
            if (data.IsSuccess)
            {
                var shipment = data.Data as Shipment;
                var lading = new CreateUpdateLadingScheduleViewModel(
                    shipment.Id,
                    currentUser.HubId,
                    currentUser.Id,
                    viewModel.ShipmentStatusId,
                    viewModel.CurrentLat,
                    viewModel.CurrentLng,
                    viewModel.Location,
                    viewModel.Note,
                    0,
                    viewModel.ReasonId
                );
                await _iLadingScheduleService.Create(lading);
                //
                if (shipment.ShipmentStatusId == StatusHelper.ShipmentStatusId.PickupComplete)
                {
                    //Push lading schedule bill to VSE

                    shipment.IsPushVSE = false;
                    shipment.CountPushVSE = 0;
                    shipment.EndPickTime = DateTime.Now;
                    shipment.PickUserId = currentUser.Id;
                    _unitOfWork.RepositoryCRUD<Shipment>().Update(shipment);
                    _unitOfWork.Commit();
                    if (_icompanyInformation.Name == "be" || _icompanyInformation.Name == "dlex")
                    {
                        if (shipment.ServiceId == 2)
                        {
                            shipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.Delivering;
                            _unitOfWork.RepositoryCRUD<Shipment>().Update(shipment);
                            _unitOfWork.Commit();
                            var deliveryLading = new CreateUpdateLadingScheduleViewModel(
                           shipment.Id,
                           currentUser.HubId,
                           currentUser.Id,
                           shipment.ShipmentStatusId,
                           viewModel.CurrentLat,
                           viewModel.CurrentLng,
                           viewModel.Location,
                           viewModel.Note,
                           0,
                           null);
                            await _iLadingScheduleService.Create(deliveryLading);
                        }
                    }
                }
                else if (shipment.ShipmentStatusId == StatusHelper.ShipmentStatusId.DeliveryFail)
                {
                    if (_icompanyInformation.Name == "gsdp")
                    {
                        viewModel.ShipmentStatusId = StatusHelper.ShipmentStatusId.WarehouseEmp;
                        viewModel.CurrentHubId = currentUser.HubId;
                        var result = await _iGeneralServiceRaw.Update<Shipment, UpdateStatusViewModel>(viewModel);
                        if (result.IsSuccess)
                        {
                            var shipment2 = result.Data as Shipment;
                            var lading2 = new CreateUpdateLadingScheduleViewModel(
                                shipment.Id,
                                currentUser.HubId,
                                currentUser.Id,
                                viewModel.ShipmentStatusId,
                                viewModel.CurrentLat,
                                viewModel.CurrentLng,
                                viewModel.Location,
                                null,
                                0,
                                null
                            );
                            await _iLadingScheduleService.Create(lading2);
                        }
                    }
                    if (_icompanyInformation.Name == "be" || _icompanyInformation.Name == "dlex")
                    {
                        if (shipment.ServiceId == 2)
                        {
                            shipment.IsReturn = true;
                            shipment.PriceReturn = Math.Round((shipment.DefaultPrice * 0.5 + shipment.VATPrice * 0.5), 0);
                            var total = shipment.DefaultPrice + shipment.FuelPrice + shipment.VATPrice + shipment.OtherPrice
                                + shipment.TotalDVGT + shipment.RemoteAreasPrice + shipment.PriceReturn;
                            //if (viewModel.dis > 0) total = total - (total * viewModel.DisCount / 100);
                            if (total.HasValue) shipment.TotalPrice = Math.Round(total.Value, 0);
                            if (shipment.IsPickupWH == false)
                            {
                                shipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.Returning;
                                _unitOfWork.RepositoryCRUD<Shipment>().Update(shipment);
                                _unitOfWork.Commit();
                                var deliveryLading = new CreateUpdateLadingScheduleViewModel(
                               shipment.Id,
                               currentUser.HubId,
                               currentUser.Id,
                               shipment.ShipmentStatusId,
                               viewModel.CurrentLat,
                               viewModel.CurrentLng,
                               viewModel.Location,
                               viewModel.Note,
                               0,
                               null);
                                await _iLadingScheduleService.Create(deliveryLading);
                            }
                            else
                            {

                            }
                            _unitOfWork.Commit();
                        }
                    }
                }
                else if (viewModel.ShipmentStatusId == StatusHelper.ShipmentStatusId.DeliveryComplete)
                {
                    //
                    if (_icompanyInformation.Name == "be" || _icompanyInformation.Name == "dlex")
                    {
                        if (shipment.TotalPrice == 0 && shipment.SenderId.HasValue && shipment.ServiceId.HasValue)
                        {
                            GoogleDistnaceModel distanceModel = new GoogleDistnaceModel();
                            distanceModel.AddressFrom = shipment.PickingAddress;
                            distanceModel.LatFrom = shipment.LatFrom;
                            distanceModel.LngFrom = shipment.LngFrom;
                            distanceModel.AddressTo = shipment.ShippingAddress;
                            distanceModel.LatTo = viewModel.CurrentLat;
                            distanceModel.LngTo = viewModel.CurrentLng;
                            var distance = GoogleUtil.GetDistancs(distanceModel, _icompanyInformation.ApiKey);
                            if (distance > 0)
                            {
                                shipment.Distance = distance;
                                ShipmentCalculateViewModel priceModel = new ShipmentCalculateViewModel();
                                priceModel.IsAgreementPrice = shipment.IsAgreementPrice;
                                priceModel.DefaultPrice = shipment.DefaultPrice;
                                priceModel.PriceReturn = shipment.PriceReturn;
                                priceModel.FromProvinceId = shipment.FromProvinceId;
                                priceModel.FromDistrictId = shipment.FromDistrictId;
                                priceModel.Weight = shipment.Weight;
                                if (shipment.CalWeight > shipment.Weight) priceModel.Weight = shipment.CalWeight.Value;
                                if (shipment.ToDistrictId.HasValue) priceModel.ToDistrictId = shipment.ToDistrictId.Value;
                                priceModel.ToWardId = shipment.ToWardId;
                                priceModel.SenderId = shipment.SenderId.Value;
                                priceModel.Distance = shipment.Distance;
                                priceModel.DisCount = shipment.DisCount;
                                priceModel.ServiceId = shipment.ServiceId.Value;
                                priceModel.StructureId = shipment.StructureId;
                                var result = PriceUtil.Calculate(priceModel, _icompanyInformation.Name, true);
                                if (result.IsSuccess == true)
                                {
                                    var calculateData = result.Data as PriceViewModel;
                                    if (calculateData.TotalPrice > 0)
                                    {
                                        shipment.DefaultPrice = calculateData.DefaultPrice;
                                        shipment.DefaultPriceS = calculateData.DefaultPriceS;
                                        shipment.DefaultPriceP = calculateData.DefaultPriceP;
                                        shipment.TotalDVGT = calculateData.TotalDVGT;
                                        shipment.FuelPrice = calculateData.FuelPrice;
                                        shipment.OtherPrice = calculateData.OtherPrice;
                                        shipment.VATPrice = calculateData.VATPrice;
                                        shipment.TotalPriceSYS = calculateData.TotalPrice;
                                        shipment.TotalPrice = calculateData.TotalPrice;
                                        _unitOfWork.RepositoryCRUD<Shipment>().Update(shipment);
                                        _unitOfWork.Commit();
                                    }
                                }
                            }
                        }
                    }
                    if (_icompanyInformation.Name == "vietstar")
                    {
                        //VSEApi _VSEApi = new VSEApi();
                        //string message = string.Format("VIETSTAR EXPRESS THONG BAO DON HANG {0} CUA BAN DA DUOC PHAT THANH CONG", shipment.ShipmentNumber);
                        //_VSEApi.SendSMSNormal(shipment.SenderPhone, message);
                    }
                    if (shipment.PaymentTargetCOD != null)
                    {
                        var results = _unitOfWork.Repository<Proc_GetPaymentTargetCODConversion>().ExecProcedureSingle(Proc_GetPaymentTargetCODConversion.GetEntityProc(shipment.Id));
                        shipment.PaymentTargetCODConversion = results.PaymentTargetCODConversion;
                        _unitOfWork.RepositoryCRUD<Shipment>().Update(shipment);
                        _unitOfWork.Commit();
                    }

                    //var resUpdate = _unitOfWork.Repository<Proc_UpdateRealExportSAP>()
                    //                    .ExecProcedureSingle(Proc_UpdateRealExportSAP.GetEntityProc(shipment.Id, 3, null));
                }
            }
            //
            return JsonUtil.Create(data);
        }

        [HttpPost("UpdateStatusShipment")]
        public async Task<JsonResult> UpdateStatusShipment([FromBody] UpdateStatusViewModel viewModel)
        {
            //if (!ModelState.IsValid)
            //{
            //    return JsonUtil.Error(ModelState);
            //}
            var user = GetCurrentUser();
            if (!Util.IsNull(viewModel.UserCode))
            {
                var gUser = _unitOfWork.RepositoryR<User>().GetSingle(f => f.Code == viewModel.UserCode);
                if (!Util.IsNull(gUser)) user = gUser;
            }
            var shipment = _unitOfWork.RepositoryR<Shipment>().FindBy(f999 => f999.ShipmentNumber == viewModel.ShipmentNumber).FirstOrDefault();
            if (Util.IsNull(shipment))
            {
                return JsonUtil.Error("Mã vận đơn không chính xác!");
            }
            if (shipment.ShipmentStatusId == StatusHelper.ShipmentStatusId.DeliveryComplete)
            {
                return JsonUtil.Error("Vận đơn đã phát thành công!");
            }
            if (!Util.IsNull(viewModel.RealRecipientName) && !Util.IsNull(viewModel.appointmentTime))
            {
                shipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.DeliveryComplete;
                shipment.RealRecipientName = viewModel.RealRecipientName;
                shipment.DeliveryDate = viewModel.appointmentTime;
                shipment.DeliverUserId = user.Id;
                shipment.IsPushCustomer = true;
            }
            else if (!Util.IsNull(viewModel.ShipmentStatusId))
            {
                shipment.IsPushCustomer = true;
                shipment.ShipmentStatusId = viewModel.ShipmentStatusId;
            }
            else
            {
                shipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.TransferTo3PL;
            }
            _unitOfWork.RepositoryCRUD<Shipment>().Update(shipment);
            var lading = new CreateUpdateLadingScheduleViewModel(
                shipment.Id,
                null,
                user.Id,
                shipment.ShipmentStatusId,
                viewModel.CurrentLat,
                viewModel.CurrentLng,
                viewModel.Location,
                viewModel.Note + " AT",
                0,
                            viewModel.ReasonId
            );
            if (!Util.IsNull(viewModel.appointmentTime))
            {
                lading.CreatedWhen = viewModel.appointmentTime;
                lading.ModifiedWhen = viewModel.appointmentTime;
                lading.Note = viewModel.Note + " AT-M";
            }
            else
            {
                var currentDate = DateTime.Now;
                lading.CreatedWhen = currentDate;
                lading.ModifiedWhen = currentDate;
                lading.Note = viewModel.Note + " AT-D";
            }
            await _iLadingScheduleService.CreateMDF(lading);
            return JsonUtil.Success("SUCCESS");
            //
        }

        [HttpPost("TransferLostPackage")]
        public async Task<JsonResult> TransferLostPackage([FromBody] Shipment viewModel)
        {
            if (!ModelState.IsValid)
            {
                return JsonUtil.Error(ModelState);
            }
            var ship = _unitOfWork.RepositoryR<Shipment>().GetSingle(viewModel.Id);
            ship.ShipmentStatusId = viewModel.ShipmentStatusId;
            var data = await _iGeneralServiceRaw.Update<Shipment>(ship);
            if (data.IsSuccess)
            {
                var shipment = data.Data as Shipment;
                var lading = new CreateUpdateLadingScheduleViewModel(
                    shipment.Id,
                    GetCurrentUser().HubId,
                    GetCurrentUserId(),
                    viewModel.ShipmentStatusId,
                    0,
                    0,
                    "",
                    viewModel.Note,
                    0
                );
                await _iLadingScheduleService.Create(lading);
            }

            return JsonUtil.Create(data);
        }


        [HttpGet("CheckExistTPLNumber")]
        public async Task<JsonResult> CheckExistTPLNumber(string tplNumber)
        {
            if (string.IsNullOrEmpty(tplNumber))
            {
                return JsonUtil.Error("Mã vận đơn đối tác trống!");
            }
            else
            {
                var shipment = await _unitOfWork.RepositoryCRUD<Shipment>()
                    .FindByAsync(f => f.TPLNumber == tplNumber).FirstOrDefault();
                if (shipment == null)
                {
                    return JsonUtil.Success("Mã vận đơn đối tác chưa sử dụng!");
                }
                else
                {
                    return JsonUtil.Error("Mã vận đơn đã được sử dụng!");
                }
            }
        }

        [HttpPost("AssignShipmentToTPL")]
        public async Task<JsonResult> AssignShipmentToTPL([FromBody] List<ShipmentAssignToTPLViewModel> listShipmentAssign)
        {
            if (!ModelState.IsValid)
            {
                return JsonUtil.Error(ModelState);
            }
            string messages = "";
            int[] statusIds = { StatusHelper.ShipmentStatusId.ReadyToDelivery, StatusHelper.ShipmentStatusId.WaitingToTransfer };
            foreach (var shipmentAssign in listShipmentAssign)
            {
                try
                {
                    var shipment = await _unitOfWork.RepositoryR<Shipment>().GetSingleAsync(shipmentAssign.ShipmentId);
                    if (shipment != null)
                    {
                        if (shipment.TPLCreatedWhen.HasValue)
                        {
                            shipment.TPLId = shipmentAssign.TPLId;
                            shipment.TPLNumber = shipmentAssign.TPLNumber;
                            shipment.TPLPrice = shipmentAssign.TPLPrice;
                        }
                        else
                        {
                            if (!statusIds.Contains(shipment.ShipmentStatusId))
                            {
                                messages += string.Format("Mã vận đơn {0} đã cập nhật cho đối tác. ", shipment.ShipmentNumber);
                            }
                            else
                            {
                                shipment.TPLId = shipmentAssign.TPLId;
                                shipment.TPLNumber = shipmentAssign.TPLNumber;
                                shipment.TPLPrice = shipmentAssign.TPLPrice;
                                //shipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.TransferTo3PL;
                                if (shipmentAssign.TPLCreatedWhen.HasValue)
                                {
                                    shipment.TPLCreatedWhen = shipmentAssign.TPLCreatedWhen;
                                }
                                else
                                {
                                    shipment.TPLCreatedWhen = DateTime.Now;
                                }
                            }
                        }
                    }
                    else
                    {
                        messages += string.Format("Mã vận đơn {0} không tìm thấy. ", shipmentAssign.ShipmentNumber);
                    }
                }
                catch (Exception ex)
                {
                    messages += string.Format("Mã vận đơn {0} lỗi:{1}. ", shipmentAssign.ShipmentNumber, ex.Message);
                }
            }
            if (string.IsNullOrEmpty(messages))
            {
                _unitOfWork.Commit();
                return JsonUtil.Success();
            }
            else
            {
                _unitOfWork.Dispose();
                return JsonUtil.Error(messages);
            }
        }

        [HttpPost("UpdateLadingSchedule")]
        public async Task<JsonResult> UpdateLadingSchedule([FromBody] List<UpdateLadingScheduleViewModel> listUpdateLadingSchedule)
        {
            if (listUpdateLadingSchedule == null)
            {
                return JsonUtil.Error("Danh sách cập nhật trống!");
            }
            if (listUpdateLadingSchedule.Count() == 0)
            {
                return JsonUtil.Error("Danh sách cập nhật 0 có vận đơn!");
            }
            int[] statusId = {StatusHelper.ShipmentStatusId.DeliveryComplete, StatusHelper.ShipmentStatusId.ReturnComplete,
            StatusHelper.ShipmentStatusId.PickupCancel, StatusHelper.ShipmentStatusId.LotteCancel,StatusHelper.ShipmentStatusId.RejectPickup};
            int[] statusIdComplete = { StatusHelper.ShipmentStatusId.DeliveryComplete, StatusHelper.ShipmentStatusId.ReturnComplete };
            string messages = "";
            List<int> statusPush = new List<int>();
            statusPush.Add(StatusHelper.ShipmentStatusId.DeliveryComplete);
            statusPush.Add(StatusHelper.ShipmentStatusId.ReturnComplete);
            statusPush.Add(StatusHelper.ShipmentStatusId.Cancel);
            foreach (var item in listUpdateLadingSchedule)
            {
                if (string.IsNullOrEmpty(item.TPLNumber))
                {
                    messages += string.Format("Mã vận đơn không thể trống. ");
                }
                else
                {
                    var shipment = await _unitOfWork.RepositoryR<Shipment>()
                        .FindByAsync(f => f.TPLNumber == item.TPLNumber && f.TPLId.HasValue == true).FirstOrDefault();
                    if (shipment == null)
                    {
                        messages += string.Format("Vận đơn {0} không tìm thấy. ", item.TPLNumber);
                    }
                    else
                    {
                        if (statusId.Contains(shipment.ShipmentStatusId))
                        {
                            messages += string.Format("Vận đơn {0} trạng thái không cho cập nhật. ", shipment.TPLNumber);
                        }
                        else
                        {
                            var shipmentStatus = await _unitOfWork.RepositoryR<ShipmentStatus>().FindByAsync(f => f.Code == item.StatusCode).FirstOrDefault();
                            if (shipmentStatus == null)
                            {
                                messages += string.Format("Vận đơn {0} trạng thái {1} không đúng. ", item.TPLNumber, item.StatusCode);
                            }
                            else
                            {
                                shipment.ShipmentStatusId = shipmentStatus.Id;
                                if (statusPush.Contains(shipment.ShipmentStatusId))
                                {
                                    shipment.IsPushCustomer = true;
                                    shipment.ModifiedWhen = DateTime.Now;
                                }
                                if (statusIdComplete.Contains(shipment.ShipmentStatusId))
                                {
                                    if (item.UpdatedWhen.HasValue)
                                    {
                                        shipment.DeliveryDate = item.UpdatedWhen;
                                    }
                                    else
                                    {
                                        shipment.DeliveryDate = DateTime.Now;
                                    }
                                    shipment.DeliveryNote = item.Note;
                                    shipment.RealRecipientName = item.SignalBy;
                                }
                                LadingSchedule ladingSchedule = new LadingSchedule();
                                ladingSchedule.CreatedBy = ladingSchedule.ModifiedBy = ladingSchedule.UserId = GetCurrentUserId();
                                if (item.UpdatedWhen.HasValue)
                                {
                                    ladingSchedule.CreatedWhen = ladingSchedule.ModifiedWhen = item.UpdatedWhen;
                                }
                                else
                                {
                                    ladingSchedule.CreatedWhen = ladingSchedule.ModifiedWhen = DateTime.Now;
                                }
                                ladingSchedule.HubId = GetCurrentUser().HubId;
                                ladingSchedule.IsEnabled = true;
                                ladingSchedule.Location = item.Location;
                                ladingSchedule.Note = item.Note;
                                ladingSchedule.ShipmentId = shipment.Id;
                                ladingSchedule.ShipmentStatusId = shipment.ShipmentStatusId;
                                _unitOfWork.RepositoryCRUD<LadingSchedule>().Insert(ladingSchedule);
                            }
                        }
                    }
                }
            }
            if (string.IsNullOrEmpty(messages))
            {
                _unitOfWork.RepositoryCRUD<Shipment>().Commit();
                _unitOfWork.RepositoryCRUD<LadingSchedule>().Commit();
                return JsonUtil.Success("Cập nhật thành công.");
            }
            else
            {
                _unitOfWork.RepositoryCRUD<Shipment>().Dispose();
                _unitOfWork.RepositoryCRUD<LadingSchedule>().Dispose();
                return JsonUtil.Error(messages);
            }
        }

        [HttpGet("DetectAddressTo")]
        public JsonResult DetectAddressTo(string shipmentNumber)
        {
            return JsonUtil.Success(_unitOfWork.Repository<Proc_DetectAddressTo>().ExecProcedureSingle(Proc_DetectAddressTo.GetEntityProc(shipmentNumber)));
        }

        [HttpPost("GetByStatusIds")]
        public JsonResult GetByStatusIds([FromBody] GetByIdsViewModel viewModel)
        {
            return JsonUtil.Success(_iGeneralService.FindBy(x => viewModel.Ids.Contains(x.ShipmentStatusId), cols: viewModel.Cols));
        }

        [HttpGet("GetByShopCode")]
        public JsonResult GetByShopCode(string shopCode, int? customerId, string cols)
        {
            return JsonUtil.Success(_iGeneralService.FindBy(x => (x.SenderId == customerId || customerId == null) && x.ShopCode == shopCode, null, null, cols));
        }

        [HttpGet("GetByShopCodeFirst")]
        public JsonResult GetByShopCodeFirst(string shopCode, int? customerId, string cols)
        {
            List<int> statusIds = new List<int>();
            statusIds.Add(StatusHelper.ShipmentStatusId.NewRequest);
            statusIds.Add(StatusHelper.ShipmentStatusId.PickupComplete);
            statusIds.Add(StatusHelper.ShipmentStatusId.Picking);
            statusIds.Add(StatusHelper.ShipmentStatusId.AssignEmployeePickup);
            var data = _unitOfWork.RepositoryR<Shipment>().FindBy(x => (x.SenderId == customerId || customerId == null) && statusIds.Contains(x.ShipmentStatusId) && x.ShopCode == shopCode).OrderByDescending(o => o.Id);
            return JsonUtil.Success(data);
        }

        [HttpPost("AssignShipmentWarehousing")]
        public async Task<JsonResult> AssignShipmentWarehousing([FromBody] AssignShipmentWarehousingViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return JsonUtil.Error(ModelState);
            }
            var currentUser = GetCurrentUser();
            string messages = "";
            int[] shipmentDelivered = {
                StatusHelper.ShipmentStatusId.DeliveryComplete,
                StatusHelper.ShipmentStatusId.ReturnComplete
            };
            int[] shipmentInWarehousing = {
                StatusHelper.ShipmentStatusId.ReadyToDelivery,
                StatusHelper.ShipmentStatusId.WaitingToTransfer
            };
            // thêm check trạng thái nhập kho lấy hàng
            int[] checkStatusWarehousePickup = {
                StatusHelper.ShipmentStatusId.PickupComplete,
                StatusHelper.ShipmentStatusId.NewRequest,
                StatusHelper.ShipmentStatusId.Picking,
            };
            //
            var shipment = await _unitOfWork.RepositoryR<Shipment>().GetSingleAsync(ship => ship.ShipmentNumber == viewModel.ShipmentNumber);
            var newShipmentUpdate = new Shipment();
            var isCreateShip = false; // check quét vận đơn đã tồn tại hay chưa tồn tại
            LadingSchedule ladingWarehouseNewShip = null; // hành trình nhập kho của vận đơn chưa tồn tại
            LadingSchedule ladingWarehouseExistShip = null; // hành trình nhập kho của vận đơn đã tồn tại
            try
            {
                if (shipment != null)
                {
                    var currentHubId = 0;
                    if (shipment.CurrentHubId.HasValue) currentHubId = shipment.CurrentHubId.Value;
                    //var listHub = _iuserService.GetListHubFromUser(GetCurrentUser());
                    if (currentUser.HubId == currentHubId)
                    {
                        if (shipmentInWarehousing.Contains(shipment.ShipmentStatusId))
                        {
                            messages += string.Format("Mã vận đơn {0} đã nhập kho trước đó, không được nhập kho lại.", viewModel.ShipmentNumber);
                        }
                        else
                        {
                            if (shipmentDelivered.Contains(shipment.ShipmentStatusId))
                            {
                                if (shipment.ShipmentStatusId == StatusHelper.ShipmentStatusId.DeliveryComplete)
                                {
                                    if (shipment.IsRecoveryDeliveryComplete != true)
                                    {
                                        shipment.IsRecoveryDeliveryComplete = true;
                                    }
                                    else
                                    {
                                        messages += string.Format("Mã vận đơn {0} đã thu hồi báo phát trước đó. ", viewModel.ShipmentNumber);
                                    }
                                }
                                else
                                {
                                    messages += string.Format("Mã vận đơn {0} đã hoàn tất. ", viewModel.ShipmentNumber);
                                }
                            }
                            else
                            {
                                // thêm hành trình loại nhập kho, check trạng thái nhập kho lấy hàng
                                if (viewModel.TypeWarehousing == ListGoodsTypeHelper.BK_NKLH && checkStatusWarehousePickup.Contains(shipment.ShipmentStatusId)
                                )
                                {
                                    ladingWarehouseExistShip = new LadingSchedule(
                                        shipment.Id,
                                        currentUser.HubId,
                                        null,
                                        currentUser.Id,
                                        WarehousingUtil.GetStatusWarehousing(viewModel.TypeWarehousing),
                                        0,
                                        0,
                                        "",
                                        null,
                                        0
                                    );
                                    _unitOfWork.RepositoryCRUD<LadingSchedule>().Insert(ladingWarehouseExistShip);
                                }

                                if (currentUser.HubId == shipment.ToHubId)
                                {
                                    shipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.ReadyToDelivery;
                                }
                                else
                                {
                                    shipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.WaitingToTransfer;
                                }
                                shipment.InOutDate = DateTime.Now;
                                shipment.CurrentHubId = currentUser.HubId;
                                //
                                var lading = new CreateUpdateLadingScheduleViewModel(
                                    shipment.Id,
                                    currentUser.HubId,
                                    currentUser.Id,
                                    shipment.ShipmentStatusId,
                                    0,
                                    0,
                                    "",
                                    viewModel.Note,
                                    0
                                );
                                await _iLadingScheduleService.Create(lading);
                            }
                        }
                    }
                    else
                    {
                        if (shipmentDelivered.Contains(shipment.ShipmentStatusId))
                        {
                            messages += string.Format("Mã vận đơn {0} đã hoàn tất. ", viewModel.ShipmentNumber);
                        }
                        else
                        {
                            // thêm hành trình loại nhập kho, check trạng thái nhập kho lấy hàng
                            if (viewModel.TypeWarehousing == ListGoodsTypeHelper.BK_NKLH && checkStatusWarehousePickup.Contains(shipment.ShipmentStatusId))
                            {
                                ladingWarehouseExistShip = new LadingSchedule(
                                    shipment.Id,
                                    currentUser.HubId,
                                    null,
                                    currentUser.Id,
                                    WarehousingUtil.GetStatusWarehousing(viewModel.TypeWarehousing),
                                    0,
                                    0,
                                    "",
                                    null,
                                    0
                                );
                                _unitOfWork.RepositoryCRUD<LadingSchedule>().Insert(ladingWarehouseExistShip);
                            }
                            //
                            if (GetCurrentUser().HubId == shipment.ToHubId)
                            {
                                shipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.ReadyToDelivery;
                            }
                            else
                            {
                                shipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.WaitingToTransfer;
                            }
                            shipment.InOutDate = DateTime.Now;
                            shipment.CurrentHubId = currentUser.HubId;
                            //
                            var lading = new CreateUpdateLadingScheduleViewModel(
                                shipment.Id,
                                currentUser.HubId,
                                currentUser.Id,
                                shipment.ShipmentStatusId,
                                0,
                                0,
                                "",
                                "SCD-ER",
                                0
                            );
                            await _iLadingScheduleService.Create(lading);
                        }
                    }
                    //
                    if (viewModel.IsCheck == true)
                    {
                        if (viewModel.Weight <= 0)
                            messages += string.Format("Vận đơn {0} trọng lượng phải lớn hơn 0", viewModel.ShipmentNumber);
                        if (viewModel.TotalBox < 0)
                            messages += string.Format("Vận đơn {0} số kiện chưa đúng", viewModel.ShipmentNumber);
                        //
                        shipment.OrderDate = DateTime.Now;
                        shipment.TotalBox = viewModel.TotalBox;
                        shipment.Weight = viewModel.Weight;
                        shipment.CalWeight = viewModel.CalWeight;
                        shipment.Doc = viewModel.Doc;
                        ShipmentCalculateViewModel sh = new ShipmentCalculateViewModel();
                        sh.COD = shipment.COD;
                        sh.DefaultPrice = shipment.DefaultPrice;
                        sh.FromDistrictId = shipment.FromDistrictId.Value;
                        sh.FromWardId = shipment.FromWardId.Value;
                        sh.Insured = shipment.Insured;
                        sh.IsAgreementPrice = shipment.IsAgreementPrice;
                        sh.OtherPrice = shipment.OtherPrice;
                        sh.SenderId = shipment.SenderId.Value;
                        sh.Distance = shipment.Distance;
                        if (!shipment.TotalItem.HasValue) shipment.TotalItem = 0;
                        var dataDVGT = _unitOfWork.RepositoryR<ShipmentServiceDVGT>().FindBy(f => f.ShipmentId == shipment.Id);
                        if (dataDVGT.Count() > 0)
                        {
                            sh.ServiceDVGTIds = dataDVGT.Where(w => w.ServiceId.HasValue).Select(s => s.ServiceId.Value).ToList();
                        }
                        sh.Weight = shipment.Weight;
                        if (!Util.IsNull(shipment.CalWeight))
                        {
                            if (sh.Weight < shipment.CalWeight)
                                sh.Weight = shipment.CalWeight.Value;
                        }
                        sh.ServiceId = shipment.ServiceId.Value;
                        sh.StructureId = shipment.StructureId;
                        sh.ToDistrictId = shipment.ToDistrictId.Value;
                        if (shipment.ToWardId.HasValue) sh.ToWardId = shipment.ToWardId.Value;
                        sh.TotalItem = shipment.TotalItem.Value;
                        ResponseViewModel result = PriceUtil.Calculate(sh, _icompanyInformation.Name, true);
                        if (result.IsSuccess == true)
                        {
                            PriceViewModel price = result.Data as PriceViewModel;
                            shipment.DefaultPrice = price.DefaultPrice;
                            shipment.TotalDVGT = price.TotalDVGT;
                            shipment.RemoteAreasPrice = price.RemoteAreasPrice;
                            shipment.FuelPrice = price.FuelPrice;
                            shipment.OtherPrice = price.OtherPrice;
                            shipment.VATPrice = price.VATPrice;
                            shipment.TotalPrice = price.TotalPrice;
                            shipment.PriceCOD = price.PriceCOD;
                            shipment.PriceReturn = price.PriceReturn;
                            shipment.TotalPriceSYS = price.TotalPriceSYS;
                        }
                        //Create ebill VSE
                        shipment.CountPushVSE = 0;
                        shipment.IsPushVSE = false;
                        shipment.IsPushCustomer = true;
                        _unitOfWork.RepositoryCRUD<Shipment>().Update(shipment);
                        _unitOfWork.Commit();
                        if (shipment.IsBox && shipment.ShipmentId.HasValue) await _iShipmentService.ReCalculatePrice(shipment.ShipmentId.Value);
                        if (!Util.IsNull(viewModel.Boxes) && viewModel.Boxes.Count() > 0)
                        {
                            _unitOfWork.RepositoryCRUD<Box>().DeleteWhere(x => x.ShipmentId == shipment.Id && !viewModel.Boxes.Select(s => s.Id).Contains(x.Id));
                            //
                            foreach (var box in viewModel.Boxes.Where(f => f.Id == 0))
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
                            //
                            foreach (var box in viewModel.Boxes.Where(f => f.Id > 0))
                            {
                                var newBox = _unitOfWork.RepositoryCRUD<Box>().GetSingle(box.Id);
                                newBox.ShipmentId = shipment.Id;
                                newBox.Weight = box.Weight;
                                newBox.CalWeight = box.CalWeight;
                                newBox.ExcWeight = box.ExcWeight;
                                newBox.Length = box.Length;
                                newBox.Width = box.Width;
                                newBox.Height = box.Height;
                                newBox.Content = box.Content;
                            }
                            await _unitOfWork.CommitAsync();
                        }
                    }
                    //

                    if (viewModel.IsPushCustomer)
                    {

                    }
                }
                else
                {
                    //messages += string.Format("Mã vận đơn {0} không tìm thấy. ", viewModel.ShipmentNumber);
                    // kiểm tra xem có phải quét mã yêu cầu hay không
                    var requestShipment = await _unitOfWork.RepositoryR<RequestShipment>().GetSingleAsync(req => req.ShipmentNumber == viewModel.ShipmentNumber);
                    if (requestShipment != null)
                    {
                        messages += string.Format("ScanRequestShipment");
                    }
                    else
                    {
                        // tạo bill mới
                        if (_icompanyInformation.Name == "vietstar")
                        {
                            // check đúng định dạng mã vận đơn mới
                            bool isIsValidShipmentNumberToWarehouse = true;
                            isIsValidShipmentNumberToWarehouse = ShipmentNumberFormatUtil.IsValidShipmentNumberToWarehouse(viewModel.ShipmentNumber);
                            if (isIsValidShipmentNumberToWarehouse != true)
                            {
                                messages += string.Format("Mã vận đơn {0} không hợp lệ", viewModel.ShipmentNumber);
                            }
                            //mã vận đơn mới hợp lệ
                            if (isIsValidShipmentNumberToWarehouse == true)
                            {
                                isCreateShip = true;
                                var newShipment = new CreateUpdateShipmentViewModel();
                                newShipment.ShipmentNumber = viewModel.ShipmentNumber;
                                newShipment.FromHubId = currentUser.HubId;
                                newShipment.CurrentHubId = currentUser.HubId;
                                var currentHub = _unitOfWork.RepositoryR<Hub>().GetSingle(currentUser.HubId.Value, x => x.District);
                                newShipment.FromProvinceId = currentHub.District.ProvinceId;
                                newShipment.FromDistrictId = currentUser.Hub.DistrictId;
                                newShipment.FromWardId = currentUser.Hub.WardId;
                                newShipment.PickUserId = currentUser.Id;
                                newShipment.OrderDate = DateTime.Now;
                                newShipment.InOutDate = DateTime.Now;
                                newShipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.WaitingToTransfer;
                                var dataNew = await _iGeneralServiceRaw.Create<Shipment, CreateUpdateShipmentViewModel>(newShipment);
                                if (dataNew.IsSuccess)
                                {
                                    var shipmentCreated = dataNew.Data as Shipment;
                                    newShipmentUpdate = shipmentCreated;

                                    // thêm hành trình loại nhập kho
                                    ladingWarehouseNewShip = new LadingSchedule(
                                        shipmentCreated.Id,
                                        currentUser.HubId,
                                        null,
                                        currentUser.Id,
                                        WarehousingUtil.GetStatusWarehousing(viewModel.TypeWarehousing),
                                        newShipment.CurrentLat,
                                        newShipment.CurrentLng,
                                        newShipment.Location,
                                        null,
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
                        else
                        {
                            messages += string.Format("Mã vận đơn {0} không tìm thấy. ", viewModel.ShipmentNumber);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                messages += string.Format("Mã vận đơn {0} lỗi:{1}. ", viewModel.ShipmentNumber, ex.InnerException);
            }

            if (string.IsNullOrEmpty(messages))
            {
                _unitOfWork.Commit();
                ListGoods listGoods = null;
                // check create or update listGoods
                listGoods = _unitOfWork.RepositoryR<ListGoods>().GetSingle(lg => lg.Id == viewModel.ListGoodsId);
                if (Util.IsNull(listGoods))
                {
                    //Create BK
                    listGoods = new ListGoods(
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
                    listGoods.TotalReceived = 1;
                    _unitOfWork.RepositoryCRUD<ListGoods>().Insert(listGoods);
                }
                else
                {
                    listGoods.Note += string.IsNullOrWhiteSpace(viewModel.Note) ? ", " + viewModel.Note : "";
                    listGoods.TotalReceived++;
                    _unitOfWork.RepositoryCRUD<ListGoods>().Update(listGoods);
                }
                await _unitOfWork.CommitAsync();

                if (string.IsNullOrWhiteSpace(listGoods.Code))
                {
                    await _iListGoodsService.UpdateCode(listGoods);
                }
                // update listGoodsId vào vận đơn, update thêm mã BK nhập kho vào note của hành trình vận đơn
                if (isCreateShip)
                {
                    newShipmentUpdate.ListGoodsId = listGoods.Id;
                    _unitOfWork.RepositoryCRUD<Shipment>().Update(newShipmentUpdate);
                    var shipmentListGoods = new ShipmentListGoods(
                        newShipmentUpdate.Id,
                        listGoods.Id
                    );
                    _unitOfWork.RepositoryCRUD<ShipmentListGoods>().Insert(shipmentListGoods);
                    ladingWarehouseNewShip.Note = listGoods.Code + "-" + currentUser.FullName;
                    if (!Util.IsNull(ladingWarehouseExistShip))
                        _unitOfWork.RepositoryCRUD<LadingSchedule>().Update(ladingWarehouseNewShip);
                    await _unitOfWork.CommitAsync();
                    return JsonUtil.Success(listGoods);
                }
                else
                {
                    shipment.ListGoodsId = listGoods.Id;
                    _unitOfWork.RepositoryCRUD<Shipment>().Update(shipment);
                    var shipmentListGoods = new ShipmentListGoods(
                        shipment.Id,
                        listGoods.Id
                    );
                    _unitOfWork.RepositoryCRUD<ShipmentListGoods>().Insert(shipmentListGoods);
                    ladingWarehouseExistShip.Note = listGoods.Code + "-" + currentUser.FullName;
                    if (!Util.IsNull(ladingWarehouseExistShip))
                        _unitOfWork.RepositoryCRUD<LadingSchedule>().Update(ladingWarehouseExistShip);
                    await _unitOfWork.CommitAsync();
                    return JsonUtil.Success(listGoods);
                }
            }
            else
            {
                _unitOfWork.Dispose();
                return JsonUtil.Error(messages);
            }
        }

        [HttpPost("ReceiptWarehousing")]
        public async Task<JsonResult> ReceiptWarehousing([FromBody] AssignShipmentWarehousingViewModel viewModel)
        {
            if (viewModel.ListGoodsId == 0)
            {
                return JsonUtil.Error("Mã nhận kho trống, vui lòng tạo mã nhập kho trước.");
            }
            if (viewModel.IsScan && !viewModel.IsAccept)
            {
                var res = _unitOfWork.Repository<Proc_CheckByShipmentNumbers>()
                 .ExecProcedureSingle(Proc_CheckByShipmentNumbers.GetEntityProc(viewModel.ShipmentNumber));
                if (res == null) return JsonUtil.Error("Không tìm thấy đơn hàng");
                //else if (res.TotalBox > 1)
                //{
                //    return JsonUtil.Success(res, "Đơn hàng hơn 1 kiện");
                //}
            }
            bool isScheduleValid = false;
            var currentUser = GetCurrentUser();
            Shipment shipment = null;
            int[] shipmentDelivered = {
                StatusHelper.ShipmentStatusId.DeliveryComplete,
                StatusHelper.ShipmentStatusId.ReturnComplete
            };
            //trong kho
            int[] shipmentInWarehousing = {
                StatusHelper.ShipmentStatusId.ReadyToDelivery,
                StatusHelper.ShipmentStatusId.WaitingToTransfer,
                StatusHelper.ShipmentStatusId.ReadyToReturn,
            };
            //hàng đang trung chuyển
            int[] shipmentInTransfering = {
                StatusHelper.ShipmentStatusId.AssignEmployeeTransferReturn,
                StatusHelper.ShipmentStatusId.AssignEmployeeTransfer,
                StatusHelper.ShipmentStatusId.TransferReturning,
                StatusHelper.ShipmentStatusId.Transferring
            };
            //ttrạng thái nhập kho lấy hàng
            List<int> shipmentStatusWarehousePickup = new List<int>();
            shipmentStatusWarehousePickup.Add(StatusHelper.ShipmentStatusId.PickupComplete);
            shipmentStatusWarehousePickup.Add(StatusHelper.ShipmentStatusId.NewRequest);
            shipmentStatusWarehousePickup.Add(StatusHelper.ShipmentStatusId.Picking);
            shipmentStatusWarehousePickup.Add(StatusHelper.ShipmentStatusId.Idle);
            shipmentStatusWarehousePickup.Add(StatusHelper.ShipmentStatusId.WaitingHandling); //GSDP
            if (_icompanyInformation.Name == "flashship")
            {
                shipmentStatusWarehousePickup.Add(StatusHelper.ShipmentStatusId.PickupFail); //flashship
            }

            //Trả/giao hàng thất bại.
            List<int> shipmentStatusDeliveryAndReturnFail = new List<int>();
            shipmentStatusDeliveryAndReturnFail.Add(StatusHelper.ShipmentStatusId.DeliveryFail);
            shipmentStatusDeliveryAndReturnFail.Add(StatusHelper.ShipmentStatusId.ReturnFail);
            shipmentStatusDeliveryAndReturnFail.Add(StatusHelper.ShipmentStatusId.WarehouseEmp);

            //if (_icompanyInformation.Name == "flashship")
            //{
            shipmentStatusDeliveryAndReturnFail.Add(StatusHelper.ShipmentStatusId.AssignEmployeeDelivery);
            shipmentStatusDeliveryAndReturnFail.Add(StatusHelper.ShipmentStatusId.AssignEmployeeReturn);
            shipmentStatusDeliveryAndReturnFail.Add(StatusHelper.ShipmentStatusId.Delivering);
            shipmentStatusDeliveryAndReturnFail.Add(StatusHelper.ShipmentStatusId.Returning);
            //};
            //Mới phân đi trả/giao hàng
            int[] shipmentStatusAssignDeliveryOrReturn = {
                StatusHelper.ShipmentStatusId.AssignEmployeeDelivery,
                StatusHelper.ShipmentStatusId.AssignEmployeeReturn
            };

            // kiem tra trang thai don hang truoc khi nhap kho
            int statustBefore = 0;
            //
            using (var context2 = new ApplicationContext())
            {
                try
                {
                    var unit = new UnitOfWork(context2);
                    var checkShipments = unit.Repository<Proc_CheckShipmentNumber>()
                        .ExecProcedure(Proc_CheckShipmentNumber.GetEntityProc(viewModel.ShipmentNumber)).ToList();

                    if (checkShipments.Count() > 0)
                    {
                        var shipmentFD = checkShipments.First();
                        var shipmentCurrentHubId = 0;
                        if (shipmentFD.CurrentHubId.HasValue) shipmentCurrentHubId = shipmentFD.CurrentHubId.Value;

                        if (shipmentFD.IsShipment == 1)
                        {
                            // lay ra trang thai don hang truoc khi nhap kho
                            statustBefore = shipmentFD.ShipmentStatusId;

                            if (shipmentFD.PackageId.HasValue && viewModel.IsPackage == false)
                            {
                                return JsonUtil.Error(string.Format("Vận đơn {0} đã đóng gói, vui lòng thao tác gói.", viewModel.ShipmentNumber));
                            }
                            else if (shipmentDelivered.Contains(shipmentFD.ShipmentStatusId))
                            {
                                return JsonUtil.Error(string.Format("Vận đơn {0} đã hoàn tất.", viewModel.ShipmentNumber));
                            }
                            else
                            {
                                shipment = _unitOfWork.RepositoryR<Shipment>().GetSingle(shipmentFD.Id);
                                if (currentUser.HubId == shipmentCurrentHubId)
                                {
                                    if (shipmentInWarehousing.Contains(shipment.ShipmentStatusId))
                                    {
                                        return JsonUtil.Error(string.Format("Mã vận đơn {0} đã nhập kho trước đó, không được nhập kho lại.", viewModel.ShipmentNumber));
                                    }
                                    else if (shipmentStatusDeliveryAndReturnFail.Contains(shipment.ShipmentStatusId)) // giao/trả khồn thành công
                                    {
                                        isScheduleValid = true;
                                    }
                                    else if (shipmentInTransfering.Contains(shipment.ShipmentStatusId)) // đã phần/đang trung chuyển
                                    {
                                        isScheduleValid = true;
                                    }
                                    else if (shipmentStatusWarehousePickup.Contains(shipment.ShipmentStatusId)) // hàng mới
                                    {
                                        isScheduleValid = true;
                                    }
                                    else if (shipmentStatusAssignDeliveryOrReturn.Contains(shipment.ShipmentStatusId)) // hagf xuất kho giao/trả NV không xác nhận
                                    {
                                        isScheduleValid = true;
                                    }
                                    else
                                    {
                                        return JsonUtil.Error(string.Format("Vận đơn {0} không cho phép nhập kho, vui lòng kiểm tra lại.", viewModel.ShipmentNumber));
                                    }
                                }
                                else
                                {
                                    if (shipmentInTransfering.Contains(shipment.ShipmentStatusId))
                                    {
                                        isScheduleValid = true;
                                    }
                                    else if (shipmentStatusWarehousePickup.Contains(shipment.ShipmentStatusId))
                                    {
                                        isScheduleValid = true;
                                    }
                                    else
                                    {
                                        List<int> listAllowInputVSEOtherHub = new List<int>();
                                        listAllowInputVSEOtherHub.Add(StatusHelper.ShipmentStatusId.WaitingToTransfer);
                                        listAllowInputVSEOtherHub.Add(StatusHelper.ShipmentStatusId.NewRequest);
                                        listAllowInputVSEOtherHub.Add(StatusHelper.ShipmentStatusId.PickupComplete);
                                        if (_icompanyInformation.Name == "vietstar" && listAllowInputVSEOtherHub.Contains(shipment.ShipmentStatusId))
                                        {
                                            isScheduleValid = true; // xử lý tạm cho VSE
                                        }
                                        else
                                        {
                                            return JsonUtil.Error(string.Format("Vận đơn {0} bưu cục / kho khác đang thao tác, bạn không thể nhập kho.", viewModel.ShipmentNumber));
                                        }
                                    }
                                }
                                //
                                if (viewModel.IsCheck == true && (shipment.FromProvinceId.HasValue || shipment.FromDistrictId.HasValue || shipment.FromWardId.HasValue))
                                {
                                    //
                                    shipment.ServiceId = viewModel.ServiceId;
                                    shipment.ToProvinceId = viewModel.ToProvinceId;
                                    shipment.ToDistrictId = viewModel.ToDistrictId;
                                    shipment.ToWardId = viewModel.ToWardId;
                                    shipment.Note = viewModel.Note;
                                    shipment.TotalBox = viewModel.TotalBox;
                                    shipment.Weight = viewModel.Weight;
                                    shipment.CalWeight = viewModel.CalWeight;
                                    if (!Util.IsNull(viewModel.Content)) shipment.Content = viewModel.Content;
                                    if (!Util.IsNull(viewModel.CusNote)) shipment.CusNote = viewModel.CusNote;
                                    shipment.Length = viewModel.Length;
                                    shipment.Width = viewModel.Width;
                                    shipment.Height = viewModel.Height;
                                    shipment.Doc = viewModel.Doc;
                                    if (shipment.IsBox != true)
                                    {
                                        ShipmentCalculateViewModel sh = new ShipmentCalculateViewModel();
                                        sh.COD = shipment.COD;
                                        sh.DefaultPrice = shipment.DefaultPrice;
                                        sh.FromProvinceId = shipment.FromProvinceId;
                                        sh.FromDistrictId = shipment.FromDistrictId;
                                        sh.FromWardId = shipment.FromWardId;
                                        sh.Insured = shipment.Insured;
                                        sh.IsAgreementPrice = shipment.IsAgreementPrice;
                                        sh.OtherPrice = shipment.OtherPrice;
                                        sh.SenderId = shipment.SenderId.Value;
                                        if (!shipment.TotalItem.HasValue) shipment.TotalItem = 0;
                                        var dataDVGT = _unitOfWork.RepositoryR<ShipmentServiceDVGT>().FindBy(f => f.ShipmentId == shipment.Id);
                                        if (dataDVGT.Count() > 0)
                                        {
                                            sh.ServiceDVGTIds = dataDVGT.Where(w => w.ServiceId.HasValue).Select(s => s.ServiceId.Value).ToList();
                                        }
                                        sh.ServiceId = shipment.ServiceId.Value;
                                        sh.StructureId = shipment.StructureId;
                                        sh.ToDistrictId = shipment.ToDistrictId.Value;
                                        sh.TotalItem = shipment.TotalItem.Value;
                                        sh.Distance = shipment.Distance;
                                        if (shipment.Weight > shipment.CalWeight)
                                        {
                                            sh.Weight = shipment.Weight;
                                        }
                                        else
                                        {
                                            sh.Weight = shipment.CalWeight.Value;
                                        }
                                        ResponseViewModel result = PriceUtil.Calculate(sh, _icompanyInformation.Name, true);
                                        if (result.IsSuccess == true)
                                        {
                                            PriceViewModel price = result.Data as PriceViewModel;
                                            shipment.DefaultPrice = price.DefaultPrice;
                                            shipment.TotalDVGT = price.TotalDVGT;
                                            shipment.RemoteAreasPrice = price.RemoteAreasPrice;
                                            shipment.FuelPrice = price.FuelPrice;
                                            shipment.OtherPrice = price.OtherPrice;
                                            shipment.VATPrice = price.VATPrice;
                                            shipment.TotalPrice = price.TotalPrice;
                                        }
                                    }
                                    else
                                    {
                                        if (shipment.ShipmentId.HasValue)
                                        {
                                            //
                                            var shipmentRef = _unitOfWork.RepositoryR<Shipment>().GetSingle(f1828 => f1828.Id == shipment.ShipmentId);
                                            var listShipments = _unitOfWork.RepositoryR<Shipment>().FindBy(f1828 => f1828.ShipmentId == shipment.ShipmentId).ToList();
                                            if (listShipments.Count() > 0)
                                            {
                                                shipmentRef.Weight = listShipments.Sum(s => s.Weight);
                                                shipmentRef.CalWeight = listShipments.Sum(s => s.CalWeight);
                                                shipmentRef.TotalBox = listShipments.Sum(s => s.TotalBox);
                                                shipmentRef.TotalItem = listShipments.Sum(s => s.TotalItem);
                                                shipmentRef.Insured = listShipments.Sum(s => s.Insured);
                                                shipmentRef.COD = listShipments.Sum(s => s.COD);
                                            }
                                            if (!shipmentRef.CalWeight.HasValue) shipmentRef.CalWeight = 0;
                                            //
                                            ShipmentCalculateViewModel sh = new ShipmentCalculateViewModel();
                                            sh.COD = shipmentRef.COD;
                                            sh.DefaultPrice = shipmentRef.DefaultPrice;
                                            sh.FromProvinceId = shipmentRef.FromProvinceId;
                                            sh.FromDistrictId = shipmentRef.FromDistrictId;
                                            sh.FromWardId = shipmentRef.FromWardId;
                                            sh.Insured = shipmentRef.Insured;
                                            sh.IsAgreementPrice = shipmentRef.IsAgreementPrice;
                                            sh.OtherPrice = shipmentRef.OtherPrice;
                                            sh.SenderId = shipmentRef.SenderId.Value;
                                            if (!shipmentRef.TotalItem.HasValue) shipmentRef.TotalItem = 0;
                                            var dataDVGT = _unitOfWork.RepositoryR<ShipmentServiceDVGT>().FindBy(f => f.ShipmentId == shipmentRef.Id);
                                            if (dataDVGT.Count() > 0)
                                            {
                                                sh.ServiceDVGTIds = dataDVGT.Where(w => w.ServiceId.HasValue).Select(s => s.ServiceId.Value).ToList();
                                            }
                                            sh.ServiceId = shipmentRef.ServiceId.Value;
                                            sh.StructureId = shipmentRef.StructureId;
                                            sh.ToDistrictId = shipmentRef.ToDistrictId.Value;
                                            sh.TotalItem = shipmentRef.TotalItem.Value;
                                            sh.Distance = shipment.Distance;
                                            if (shipmentRef.Weight > shipmentRef.CalWeight) sh.Weight = shipmentRef.Weight;
                                            else sh.Weight = shipmentRef.CalWeight.Value;
                                            ResponseViewModel result = PriceUtil.Calculate(sh, _icompanyInformation.Name, true);
                                            if (result.IsSuccess == true)
                                            {
                                                PriceViewModel price = result.Data as PriceViewModel;
                                                shipmentRef.DefaultPrice = price.DefaultPrice;
                                                shipmentRef.TotalDVGT = price.TotalDVGT;
                                                shipmentRef.RemoteAreasPrice = price.RemoteAreasPrice;
                                                shipmentRef.FuelPrice = price.FuelPrice;
                                                shipmentRef.OtherPrice = price.OtherPrice;
                                                shipmentRef.VATPrice = price.VATPrice;
                                                shipmentRef.TotalPrice = price.TotalPrice;
                                            }
                                            _unitOfWork.RepositoryCRUD<Shipment>().Update(shipmentRef);
                                        }
                                    }
                                    //push bill VSE
                                    shipment.CountPushVSE = 0;
                                    shipment.IsPushVSE = false;
                                    shipment.IsPushCustomer = true;
                                    shipment.InOutDate = DateTime.Now;
                                    //_unitOfWork.RepositoryCRUD<Shipment>().Update(shipment);
                                    //_unitOfWork.Commit();
                                }
                                //
                            }
                        }
                        else//bill tổng
                        {
                            var rShipment = _unitOfWork.RepositoryR<RequestShipment>().GetSingle(shipmentFD.Id);
                            if (Util.IsNull(rShipment)) JsonUtil.Error("Thông tin bill tổng không chính xác");
                            else return JsonUtil.Success(rShipment, "RequestShipment");
                        }
                    }
                    else
                    {
                        // tạo bill mới
                        if (_icompanyInformation.Name == "vietstar" || _icompanyInformation.Name == "dlexs")
                        {
                            // check đúng định dạng mã vận đơn mới
                            bool isIsValidShipmentNumberToWarehouse = true;
                            if (_icompanyInformation.Name == "vietstar")
                            {
                                isIsValidShipmentNumberToWarehouse = ShipmentNumberFormatUtil.IsValidShipmentNumberToWarehouse(viewModel.ShipmentNumber);
                                if (isIsValidShipmentNumberToWarehouse == false)
                                {
                                    return JsonUtil.Error(string.Format("Mã vận đơn {0} không hợp lệ", viewModel.ShipmentNumber));
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
                            return JsonUtil.Error(string.Format("Mã vận đơn {0} không tìm thấy. ", viewModel.ShipmentNumber));
                        }
                    }
                    if (shipment != null)
                    {
                        // thêm hành trình loại nhập kho, check trạng thái nhập kho lấy hàng
                        shipment.ListGoodsId = viewModel.ListGoodsId;
                        if (viewModel.TypeWarehousing == ListGoodsTypeHelper.BK_NKLH && shipmentStatusWarehousePickup.Contains(shipment.ShipmentStatusId)
                            && shipment.ShipmentStatusId != StatusHelper.ShipmentStatusId.WaitingHandling)
                        {
                            var ladingWarehouseExistShip = new LadingSchedule(
                                shipment.Id,
                                currentUser.HubId,
                                null,
                                currentUser.Id,
                                WarehousingUtil.GetStatusWarehousing(viewModel.TypeWarehousing),
                                0,
                                0,
                                "",
                                null,
                                0
                            );
                            _unitOfWork.RepositoryCRUD<LadingSchedule>().Insert(ladingWarehouseExistShip);
                            shipment.FromHubId = currentUser.HubId;
                            shipment.PickUserId = currentUser.Id;
                            isScheduleValid = true;
                        }
                        //
                        var resetReceiptWarehousing = false;
                        if (shipment.ShipmentStatusId == StatusHelper.ShipmentStatusId.NewRequest || shipment.ShipmentStatusId == StatusHelper.ShipmentStatusId.PickupComplete)
                        {
                            resetReceiptWarehousing = true;
                        }
                        else if (shipment.ShipmentStatusId == StatusHelper.ShipmentStatusId.WaitingHandling)
                        {
                            if (_icompanyInformation.Name == "gsdp" || _icompanyInformation.Name == "gsdp-staging") resetReceiptWarehousing = true;
                        }
                        if (resetReceiptWarehousing)
                        {
                            shipment.EndPickTime = DateTime.Now;
                            if (shipment.ToDistrictId.HasValue && shipment.ServiceId.HasValue)
                            {
                                var res = _iKPIShipmentDetailService.CalculateTimeForShipment(shipment.SenderId, shipment.ToDistrictId.Value, shipment.ToWardId, shipment.ServiceId.Value, shipment.EndPickTime.Value);
                                shipment.EstimateTimeDelivery = res.Time;
                                shipment.EstimateTimeDeliveryReal = res.RealTime;
                                if (shipment.COD > 0) shipment.PaymentTargetCOD = res.TimeCOD;
                                shipment.DeliveryAppointmentTime = shipment.EndPickTime.Value.AddHours(res.RealTime);
                            }
                        }
                        //
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
                                shipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.ReadyToReturn;
                            }
                            else
                            {
                                shipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.WaitingToTransfer;
                            }
                        }
                        shipment.CurrentEmpId = null;
                        shipment.CurrentHubId = currentUser.HubId;
                        shipment.InOutDate = DateTime.Now;
                        //
                        var lading = new CreateUpdateLadingScheduleViewModel(
                            shipment.Id,
                            currentUser.HubId,
                            currentUser.Id,
                            shipment.ShipmentStatusId,
                            0,
                            0,
                            "",
                            (isScheduleValid == true ? "" : "SCD-ER ") + viewModel.Note,
                            0
                        );
                        try
                        {
                            await _iLadingScheduleService.Create(lading);
                            _unitOfWork.RepositoryCRUD<Shipment>().Update(shipment);
                            _unitOfWork.Commit();
                            //
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                        var shipmentListGoods = new ShipmentListGoods(
                            shipment.Id,
                            viewModel.ListGoodsId
                        );
                        _unitOfWork.RepositoryCRUD<ShipmentListGoods>().Insert(shipmentListGoods);

                        // insert Post_KPIShipmentSAP RealKPIExportSAP = DateTime.Now
                        //viewModel
                        //try
                        //{
                        //    if (statustBefore == 7 || statustBefore == 8 || statustBefore == 49)
                        //    {
                        //        var resUpdate = _unitOfWork.Repository<Proc_UpdateRealExportSAP>()
                        //        .ExecProcedureSingle(Proc_UpdateRealExportSAP.GetEntityProc(shipment.Id, 4, null));
                        //    }
                        //    else if ((statustBefore == 54 || statustBefore == 1 || statustBefore == 2 || statustBefore == 41 || statustBefore == 59) && viewModel.TypeWarehousing == ListGoodsTypeHelper.BK_NKLH)
                        //    {
                        //        var resUpdate = _unitOfWork.Repository<Proc_UpdateRealExportSAP>()
                        //              .ExecProcedureSingle(Proc_UpdateRealExportSAP.GetEntityProc(shipment.Id, 2, null));
                        //    }
                        //}
                        //catch (Exception ex)
                        //{
                        //    Console.Write(ex.Message);
                        //}
                        await _unitOfWork.CommitAsync();

                        return JsonUtil.Success(shipment);
                    }
                    else
                    {
                        return JsonUtil.Error(string.Format("Mã vận đơn {0} không tìm thấy 22. ", viewModel.ShipmentNumber));
                    }
                }
                catch (Exception ex)
                {
                    return JsonUtil.Error(string.Format("Mã vận đơn {0} cảnh báo:{1}. ", viewModel.ShipmentNumber, ex.InnerException));
                }
            }
        }

        [HttpGet("GetShipmentByCode")]
        public JsonResult GetShipmentByCode(string shipmentNumber, string cols)
        {
            return this.FindBy(f => f.ShipmentNumber == shipmentNumber, null, null, cols);
        }

        [HttpGet("GetByRequestShipmentId")]
        public JsonResult GetByRequestShipmentId(int id, int pageNumber, int pageSize, string cols)
        {
            return JsonUtil.Success(_iGeneralService.FindBy(x => x.RequestShipmentId == id, true, pageSize, pageNumber, cols: cols));
        }

        [HttpPost("GetByRequestShipmentIds")]
        public JsonResult GetByRequestShipmentIds([FromBody] GetByRequestShipmentIdsViewModel model)
        {
            string ids = "";
            if (model.Ids != null && model.Ids.Count() > 0) ids = string.Join(",", model.Ids);
            var data = _unitOfWork.Repository<Proc_GetByRequestShipmentIds>().ExecProcedure(Proc_GetByRequestShipmentIds.GetEntityProc(ids, model.SearchText, model.PageNumber, model.PageSize));
            return JsonUtil.Success(data);
        }

        [HttpGet("GetListShipmentCODByPaymentId")]
        public JsonResult GetListShipmentCODByPaymentId(int paymentId, string cols)
        {
            return JsonUtil.Success(_iGeneralService.FindBy(x => x.ListCustomerPaymentCODId == paymentId, cols: cols));
        }
        [HttpGet("GetListShipmentPriceByPaymentId")]
        public JsonResult GetListShipmentPriceByPaymentId(int paymentId, string cols)
        {
            return JsonUtil.Success(_iGeneralService.FindBy(x => x.ListCustomerPaymentTotalPriceId == paymentId, cols: cols));
        }

        [HttpGet("RemoveFromListGoods")]
        public async Task<JsonResult> RemoveFromListGoods(int id)
        {
            var shipment = _unitOfWork.RepositoryR<Shipment>().GetSingle(id);

            if (shipment == null)
                return JsonUtil.Error("Vận đơn không tồn tại!");

            var listGoods = _unitOfWork.RepositoryR<ListGoods>().GetSingle(x => x.Id == shipment.ListGoodsId);

            if (listGoods == null)
                return JsonUtil.Error("Bảng kê không tồn tại!");
            else if (listGoods.ListGoodsStatusId == ListGoodsStatusHelper.TRANSFER_COMPLETE)
            {
                return JsonUtil.Error($"Không thể chỉnh sửa bảng kê {listGoods.Code} đã hoàn tất!");
            }
            else if (listGoods.IsBlock)
            {
                return JsonUtil.Error($"Không thể  chỉnh sửa bảng kê {listGoods.Code} đã bị khoá!");
            }
            //Luu Modified User, Modified Time
            _unitOfWork.RepositoryCRUD<ListGoods>().Update(listGoods);

            shipment.ListGoodsId = null;
            _unitOfWork.RepositoryCRUD<Shipment>().Update(shipment);

            //Tim list shipment va xoa khoi bang ke history
            _unitOfWork.RepositoryCRUD<ShipmentListGoods>().DeleteWhere(x => x.Id == shipment.Id && x.ListGoodsId == listGoods.Id);

            //Conmmit
            await _unitOfWork.CommitAsync();

            return JsonUtil.Success(listGoods);
        }

        [HttpPost("GetByListCode")]
        public JsonResult GetByListCode([FromBody] List<string> list)
        {
            return JsonUtil.Success(_iGeneralService.FindBy(f => list.Contains(f.ShipmentNumber) && !Util.IsNull(f.ShipmentNumber)));
        }

        [HttpGet("GetListShipmentKeeping")]
        public JsonResult GetListShipmentKeeping(int? userId, bool recovery, bool isPickup, DateTime? dateFrom, DateTime? dateTo, string[] cols = null, int? pageSize = null, int? pageNumber = null)
        {
            Expression<Func<Shipment, bool>> predicate = x => x.Id > 0;
            List<int> statusIds = new List<int>();
            if (isPickup == true)
            {
                statusIds.Add(StatusHelper.ShipmentStatusId.PickupComplete);
            }
            else
            {
                statusIds.Add(StatusHelper.ShipmentStatusId.Delivering);
                statusIds.Add(StatusHelper.ShipmentStatusId.DeliveryFail);
                statusIds.Add(StatusHelper.ShipmentStatusId.TransferReturning);
                statusIds.Add(StatusHelper.ShipmentStatusId.Transferring);
                statusIds.Add(StatusHelper.ShipmentStatusId.ReturnFail);
                statusIds.Add(StatusHelper.ShipmentStatusId.Returning);
            }
            if (recovery == true)
            {
                statusIds.Add(StatusHelper.ShipmentStatusId.DeliveryComplete);
                predicate = predicate.And(x => x.IsRecoveryDeliveryComplete != true);
            }
            predicate = predicate.And(x => statusIds.Contains(x.ShipmentStatusId));
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
            else
            {
                var date = DateTime.Now.AddDays(-30);
                predicate = predicate.And(x => x.OrderDate >= date);
            }
            if (!Util.IsNull(dateTo))
            {
                predicate = predicate.And(x => x.OrderDate <= dateTo);
            }

            return JsonUtil.Success(_iGeneralService.FindBy(predicate, pageSize, pageNumber, cols));
        }


        [HttpGet("GetShipmentsReportBroadCastEmployee")]
        public JsonResult GetShipmentsReportBroadCastEmployee(int statusId, int userId, DateTime? dateFrom, DateTime? dateTo)
        {
            var data = new Object();
            switch (statusId)
            {
                case 3:
                    var shipPickupComplete = _unitOfWork.RepositoryR<Shipment>().FindBy(ship => ship.ShipmentStatusId == 3 && ship.PickUserId == userId && ship.EndPickTime >= dateFrom && ship.EndPickTime <= dateTo);
                    data = shipPickupComplete;
                    break;
                case 12:
                    var shipDeliveryComplete = _unitOfWork.RepositoryR<Shipment>().FindBy(ship => ship.ShipmentStatusId == 12 && ship.DeliverUserId == userId && ship.EndDeliveryTime >= dateFrom && ship.EndDeliveryTime <= dateTo);
                    data = shipDeliveryComplete;
                    break;
                case 26:
                    var shipReturnComplete = _unitOfWork.RepositoryR<Shipment>().FindBy(ship => ship.ShipmentStatusId == 26 && ship.ReturnUserId == userId && ship.EndReturnTime >= dateFrom && ship.EndReturnTime <= dateTo);
                    data = shipReturnComplete;
                    break;
            }

            return JsonUtil.Success(data);
        }

        [AllowAnonymous]
        [HttpGet("GetByShipmentNumber")]
        public JsonResult GetByShipmentNumber(string shipmentNumber)
        {
            var data = _unitOfWork.Repository<Proc_GetByShipmentNumber>()
                      .ExecProcedure(Proc_GetByShipmentNumber.GetEntityProc(shipmentNumber));
            if (data.Count() == 0) return JsonUtil.Error("Không tìm thấy đơn hàng");
            var result = data.First();
            return JsonUtil.Success(result);
        }

        [AllowAnonymous]
        [HttpGet("GetLadingSchedule")]
        public JsonResult GetLadingSchedule(int id)
        {
            var ladingSchedules = _unitOfWork.Repository<Proc_LadingSchedule_Joined>().ExecProcedure(Proc_LadingSchedule_Joined.GetEntityProc(id));
            return JsonUtil.Success(ladingSchedules);
        }

        [HttpPost("PrintShipments")]
        public async Task<JsonResult> PrintShipments([FromBody] PrintShipmentViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return JsonUtil.Error(ModelState);
            }
            var currentUser = GetCurrentUser();
            var checkExistShipmentId = _unitOfWork.RepositoryR<Shipment>().FindBy(f2106 => (viewModel.IsHideInPackage == false || (viewModel.IsHideInPackage == true && !f2106.PackageId.HasValue)) && viewModel.ShipmentIds.Contains(f2106.Id)).Select(x2106 => x2106.Id).ToList();
            var arrayNotShipmentId = viewModel.ShipmentIds.Except(checkExistShipmentId);
            if (arrayNotShipmentId.Count() > 0)
            {
                return JsonUtil.Error($"Không tìm thấy vận đơn Id: {String.Join(" ", arrayNotShipmentId)}!");
            }
            List<UserPrintShipment> listUserPrintShipment = new List<UserPrintShipment>();
            var userId = currentUser.Id;
            var printTypeId = viewModel.PrintTypeId;
            foreach (var item in viewModel.ShipmentIds)
            {
                UserPrintShipment userPrintShipment = new UserPrintShipment();
                userPrintShipment.IsEnabled = true;
                userPrintShipment.CreatedWhen = DateTime.Now;
                var isExistUserPrintShipmet = _unitOfWork.RepositoryR<UserPrintShipment>().GetSingle(f => f.PrintTypeId == printTypeId && f.ShipmentId == item);
                if (!Util.IsNull(isExistUserPrintShipmet))
                {
                    // In lại
                    userPrintShipment.StatusPrintId = StatusHelper.StatusPrintShipmentId.RePrint;
                }
                else
                {
                    // In mới
                    userPrintShipment.StatusPrintId = StatusHelper.StatusPrintShipmentId.NewPrint;
                }
                userPrintShipment.UserId = userId;
                userPrintShipment.ShipmentId = item;
                userPrintShipment.PrintTypeId = printTypeId;
                listUserPrintShipment.Add(userPrintShipment);
            }
            if (!Util.IsNull(listUserPrintShipment))
            {
                await _iGeneralServiceRaw.Create<UserPrintShipment>(listUserPrintShipment);
            }
            return JsonUtil.Success();
        }

        [HttpGet("GetAllPrintType")]
        public JsonResult GetAllPrintType()
        {
            var allPrintType = _unitOfWork.RepositoryR<ShipmentPrintType>().GetAll();
            return JsonUtil.Success(allPrintType);
        }

        [HttpGet("GetAllShipmentType")]
        public JsonResult GetAllShipmentType()
        {
            var allShipmentType = _unitOfWork.RepositoryR<ShipmentType>().GetAll();
            return JsonUtil.Success(allShipmentType);
        }

        [HttpPost("UpdateDeliveryCompleteByIds")]
        public async Task<JsonResult> UpdateDeliveryCompleteByIds([FromBody] List<UpdateShipmentDeliveryCompleteViewmodel> viewModel)
        {
            if (!ModelState.IsValid)
            {
                return JsonUtil.Error(ModelState);
            }
            var currentUserId = GetCurrentUserId();
            var updateShipments = new List<Shipment>();
            foreach (var item in viewModel)
            {
                var itemShipment = _unitOfWork.RepositoryR<Shipment>().GetSingle(item.Id);
                itemShipment.RealRecipientName = item.RealRecipientName;
                itemShipment.EndDeliveryTime = item.EndDeliveryTime;
                var data = await _iGeneralServiceRaw.Update<Shipment>(itemShipment);
                if (data.IsSuccess)
                {
                    var shipment = data.Data as Shipment;

                    var lading = new CreateUpdateLadingScheduleViewModel(
                        shipment.Id,
                        shipment.FromHubId,
                        currentUserId,
                        StatusHelper.ShipmentStatusId.DeliveryComplete,
                        0,
                        0,
                        null,
                        "ESH12", // Edit Shipment Status 12 => sửa vận đơn đã giao hàng thành công
                        0
                    );
                    await _iLadingScheduleService.Create(lading);
                }
            }
            return JsonUtil.Success();
        }

        [HttpPost("UpdateStatusShipmentTasetcoTPL")]
        public async Task<JsonResult> UpdateStatusShipmentTasetcoTPL([FromBody] UpdateStatusTasetcoTPLViewModel viewModel)
        {
            var tpl = _unitOfWork.RepositoryR<TPL>().GetSingle(f => f.Code == viewModel.CUSTOMERCODE);
            if (Util.IsNull(tpl))
            {
                return JsonUtil.Error("Mã đối tác không phù hợp!");
            }
            var shipment = _unitOfWork.RepositoryR<Shipment>().FindBy(f2252 => f2252.TPLNumber == viewModel.E_CODE).FirstOrDefault();
            if (Util.IsNull(shipment))
            {
                return JsonUtil.Error("Mã vận đơn không chính xác!");
            }
            if (shipment.TPLId != tpl.Id)
            {
                return JsonUtil.Error("Vận đơn không đủ quyền update!");
            }
            if (shipment.ShipmentStatusId == StatusHelper.ShipmentStatusId.DeliveryComplete)
            {
                return JsonUtil.Error("Vận đơn đã phát thành công!");
            }
            if (!string.IsNullOrWhiteSpace(viewModel.DELIVERYDATE))
            {
                shipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.DeliveryComplete;
                shipment.DeliveryDate = DateTime.ParseExact(viewModel.DELIVERYDATE, "yyyy-MM-dd HH:mm:ss.fff", null);
            }
            else
            {
                shipment.ShipmentStatusId = StatusHelper.ShipmentStatusId.TransferTo3PL;
            }
            _unitOfWork.RepositoryCRUD<Shipment>().Update(shipment);
            var lading = new CreateUpdateLadingScheduleViewModel(
                shipment.Id,
                null,
                GetCurrentUserId(),
                shipment.ShipmentStatusId,
                0,
                0,
                null,
                viewModel.NOTE,
                0
            );
            await _iLadingScheduleService.Create(lading);
            return JsonUtil.Success("SUCCESS");
        }

        [HttpPost("GetReportSumaryByListIds")]
        public JsonResult GetReportSumaryByListIds([FromBody] GetByIdsViewModel viewModel)
        {
            var shipmentIds = String.Join(",", viewModel.Ids);
            var data = _unitOfWork.Repository<Proc_GetExportDataReportSumary>()
                      .ExecProcedure(Proc_GetExportDataReportSumary.GetEntityProc(shipmentIds));
            if (data.Count() == 0) return JsonUtil.Error("Không tìm thấy đơn hàng");
            return JsonUtil.Success(data);
        }

        [HttpPost("GetShipmentToPrint")]
        public JsonResult GetShipmentToPrint([FromBody] GetByIdsViewModel viewModel)
        {
            var shipmentIds = String.Join(",", viewModel.Ids);
            var data = _unitOfWork.Repository<Proc_GetShipmentToPrint>()
                      .ExecProcedure(Proc_GetShipmentToPrint.GetEntityProc(shipmentIds));
            if (data.Count() == 0) return JsonUtil.Error("Không tìm thấy đơn hàng");
            return JsonUtil.Success(data);
        }

        [HttpPost("GetBoxes")]
        public JsonResult GetBoxes([FromBody] GetByIdsViewModel viewModel)
        {
            var shipmentIds = String.Join(",", viewModel.Ids);
            var data = _unitOfWork.Repository<Proc_GetBoxesByShipmentId>()
                      .ExecProcedure(Proc_GetBoxesByShipmentId.GetEntityProc(shipmentIds));
            return JsonUtil.Success(data);
        }

        [HttpPost("ReCalclateShipment")]
        public JsonResult ReCalclateShipment()
        {
            //
            using (var context2 = new ApplicationContext())
            {
                var datas = _unitOfWork.RepositoryR<Shipment>().FindBy(f => f.SenderId == 3835).ToList();
                int count = 0;
                foreach (var data in datas)
                {
                    count++;
                    try
                    {
                        var shipment = _unitOfWork.RepositoryR<Shipment>().GetSingle(data.Id);
                        ShipmentCalculateViewModel sh = new ShipmentCalculateViewModel();
                        sh.COD = shipment.COD;
                        sh.DefaultPrice = shipment.DefaultPrice;
                        sh.FromProvinceId = shipment.FromProvinceId;
                        sh.FromDistrictId = shipment.FromDistrictId;
                        sh.FromWardId = shipment.FromWardId;
                        sh.Insured = shipment.Insured;
                        sh.IsAgreementPrice = shipment.IsAgreementPrice;
                        sh.OtherPrice = shipment.OtherPrice;
                        sh.SenderId = shipment.SenderId.Value;
                        if (!shipment.TotalItem.HasValue) shipment.TotalItem = 0;
                        var dataDVGT = _unitOfWork.RepositoryR<ShipmentServiceDVGT>().FindBy(f => f.ShipmentId == shipment.Id);
                        if (dataDVGT.Count() > 0)
                        {
                            sh.ServiceDVGTIds = dataDVGT.Where(w => w.ServiceId.HasValue).Select(s => s.ServiceId.Value).ToList();
                        }
                        sh.ServiceId = shipment.ServiceId.Value;
                        sh.StructureId = shipment.StructureId;
                        sh.ToDistrictId = shipment.ToDistrictId.Value;
                        sh.TotalItem = shipment.TotalItem.Value;
                        sh.Weight = shipment.Weight;
                        sh.Distance = shipment.Distance;
                        ResponseViewModel result = PriceUtil.Calculate(sh, _icompanyInformation.Name, true);
                        if (result.IsSuccess == true)
                        {
                            PriceViewModel price = result.Data as PriceViewModel;
                            shipment.DefaultPrice = price.DefaultPrice;
                            shipment.TotalDVGT = price.TotalDVGT;
                            shipment.RemoteAreasPrice = price.RemoteAreasPrice;
                            shipment.FuelPrice = price.FuelPrice;
                            shipment.OtherPrice = price.OtherPrice;
                            shipment.VATPrice = price.VATPrice;
                            shipment.TotalPrice = price.TotalPrice;
                            shipment.PriceCOD = price.PriceCOD;
                            shipment.PriceReturn = price.PriceReturn;
                            shipment.TotalPriceSYS = price.TotalPriceSYS;
                        }
                        //push bill VSE
                        shipment.CountPushVSE = 0;
                        shipment.IsPushVSE = false;
                        shipment.IsPushCustomer = true;
                        _unitOfWork.RepositoryCRUD<Shipment>().Update(shipment);
                        _unitOfWork.Commit();
                    }
                    catch (Exception ex)
                    {
                        return JsonUtil.Error(string.Format("cảnh báo:{0}. ", ex.InnerException));
                    }
                }
                return JsonUtil.Success(string.Format("OK {0} vận đơn", datas.Count()));
            }
        }

        #region DANH SACH VANJ DON
        [HttpPost("GetListShipment")]
        public JsonResult GetListShipment([FromBody] ShipmentFilterViewModel filterViewModel)
        {
            var _unitOfWorkRRP = new UnitOfWorkRRP(_contextRRP);
            var currentUserId = this.GetCurrentUserId();
            try
            {
                var data = _unitOfWorkRRP.Repository<Proc_GetListShipment>()
                          .ExecProcedure(Proc_GetListShipment.GetEntityProc(
                filterViewModel.OrderDateFrom,
                filterViewModel.OrderDateTo,
                filterViewModel.SenderId,
                filterViewModel.PaymentTypeId,
                filterViewModel.FromProvinceId,
                filterViewModel.ToProvinceId,
                filterViewModel.WeightFrom,
                filterViewModel.WeightTo,
                filterViewModel.ServiceId,
                filterViewModel.ShipmentNumber,
                filterViewModel.SOENTRY,
                filterViewModel.ShopCode,
                filterViewModel.ReferencesCode,
                filterViewModel.ReShipmentNumber,
                filterViewModel.SearchText,
                filterViewModel.IsExistInfoDelivery,
                filterViewModel.IsExistImagePickup,
                filterViewModel.IsBox,
                filterViewModel.IsPrintBill,
                filterViewModel.UploadExcelHistoryId,
                filterViewModel.GroupStatusId,
                filterViewModel.ShipmentStatusId,
                filterViewModel.FromHubId,
                filterViewModel.ToHubId,
                filterViewModel.CurrentHubId,
                filterViewModel.CurrentEmpId,
                filterViewModel.DeadlineTypeId,
                filterViewModel.pageNumber,
                filterViewModel.pageSize,
                filterViewModel.DeliveryUserId,
                filterViewModel.NumIssueDelivery,
                filterViewModel.IsSuccess,
                filterViewModel.IsGroupEmp,
                filterViewModel.ListGoodsId,
                filterViewModel.IsHideInPackage,
                currentUserId
                ));
                return JsonUtil.Success(data);
            }
            catch (Exception ex)
            {

                return JsonUtil.Error(ex.Message);
            }
        }
        #endregion

        #region BÁO CÁO DANH SÁCH VẬN ĐƠN

        [HttpPost("GetReportListShipment")]
        public JsonResult GetReportListShipment([FromBody] ShipmentFilterViewModel filterViewModel)
        {
            var currentUserId = this.GetCurrentUserId();
            try
            {
                string listGoodsIds = null;
                if (filterViewModel.ListGoodsIds != null) String.Join(",", filterViewModel.ListGoodsIds);
                var unitOfWordRRP = new UnitOfWorkRRP(_contextRRP);
                var data = unitOfWordRRP.Repository<Proc_ReportListShipment>()
                          .ExecProcedure(Proc_ReportListShipment.GetEntityProc(
                filterViewModel.OrderDateFrom,
                filterViewModel.OrderDateTo,
                filterViewModel.SenderId,
                filterViewModel.PaymentTypeId,
                filterViewModel.FromProvinceId,
                filterViewModel.ToProvinceId,
                filterViewModel.ToProvinceIds,
                filterViewModel.WeightFrom,
                filterViewModel.WeightTo,
                filterViewModel.ServiceId,
                filterViewModel.ShipmentNumber,
                filterViewModel.ShopCode,
                filterViewModel.ReferencesCode,
                filterViewModel.ReShipmentNumber,
                filterViewModel.SearchText,
                filterViewModel.IsExistInfoDelivery,
                filterViewModel.IsExistImagePickup,
                filterViewModel.IsBox,
                filterViewModel.IsPrintBill,
                filterViewModel.UploadExcelHistoryId,
                filterViewModel.GroupStatusId,
                filterViewModel.ShipmentStatusId,
                filterViewModel.FromHubId,
                filterViewModel.ToHubId,
                filterViewModel.CurrentHubId,
                filterViewModel.CurrentEmpId,
                filterViewModel.DeadlineTypeId,
                filterViewModel.pageNumber,
                filterViewModel.pageSize,
                filterViewModel.DeliveryUserId,
                filterViewModel.NumIssueDelivery,
                filterViewModel.IsSuccess,
                filterViewModel.IsGroupEmp,
                filterViewModel.ListGoodsId,
                listGoodsIds,
                filterViewModel.IsHideInPackage,
                currentUserId,
                null,
                filterViewModel.IsCreateListReceiptCOD,
                filterViewModel.ListReceiptCODStatusId));
                if (!Util.IsNull(data))
                {
                    return JsonUtil.Success(data);
                }
                else
                {
                    return JsonUtil.Error("Get data error!!!");
                }
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }
        }
        #endregion

        #region DANH SACH CHO NHAP KHO
        [HttpGet("GetListWarehousing")]
        public JsonResult GetListWarehousing(
            int? warehousingType = null,
            int? userId = null,
            int? toUserId = null,
            string listGoodsList = null,
            int? hubId = null,
            int? toHubId = null,
            int? serviceId = null,
            bool? isPrioritize = null,
            bool? isIncidents = null,
            bool? isAllShipment = null,
            int? pageNumber = 1,
            int? pageSize = 20,
            int? senderId = null,
            DateTime? dateFrom = null,
            DateTime? dateTo = null,
            bool? isHideInPackage = null,
            bool? isNullHubRouting = null,
            int? userEmpId = null
            )
        {
            var currentUser = this.GetCurrentUser();
            if (Util.IsNull(userId)) userId = currentUser.Id;
            var unitOfWorkRRP = new UnitOfWorkRRP(_contextRRP);
            try
            {
                var data = unitOfWorkRRP.Repository<Proc_GetListWarehousing>()
                          .ExecProcedure(Proc_GetListWarehousing.GetEntityProc(
                warehousingType,
                currentUser.HubId,
                userId,
                hubId,
                listGoodsList,
                toHubId,
                toUserId,
                serviceId,
                isPrioritize,
                isIncidents,
                isAllShipment,
                pageNumber,
                pageSize,
                senderId,
                dateFrom,
                dateTo,
                isHideInPackage,
                isNullHubRouting,
                userEmpId));
                return JsonUtil.Success(data);
            }
            catch (Exception ex)
            {
                return JsonUtil.Error("Dữ liệu lỗi!!!");
            }
        }

        [HttpPost("GetListWarehousingExportExcel")]
        public dynamic GetListWarehousingExportExcel([FromBody] GetListWarehousingExcelViewModel filterViewModel)
        {
            DataTable dtt = new DataTable();
            var currentUser = this.GetCurrentUser();
            var unitOfWorkRRP = new UnitOfWorkRRP(_contextRRP);
            var data = unitOfWorkRRP.Repository<Proc_GetListWarehousing>()
                      .ExecProcedure(Proc_GetListWarehousing.GetEntityProc(
            filterViewModel.WarehousingType,
            currentUser.HubId,
            filterViewModel.UserId,
            filterViewModel.HubId,
            filterViewModel.ListGoodsList,
            filterViewModel.ToHubId,
            filterViewModel.ToUserId,
            filterViewModel.ServiceId,
            filterViewModel.IsPrioritize,
            filterViewModel.IsIncidents,
            filterViewModel.IsAllShipment,
            filterViewModel.PageNumber,
            filterViewModel.PageSize,
            filterViewModel.SenderId,
            filterViewModel.DateFrom,
            filterViewModel.DateTo,
            filterViewModel.IsHideInPackage,
            filterViewModel.IsNullHubRouting,
            filterViewModel.UserEmpId));
            dtt = data.ToDataTable();
            var bytearray = ExportExcelPartern.ExportExcel(filterViewModel.CustomExportFile, dtt);
            return File(bytearray, "application/xlsx", filterViewModel.CustomExportFile.FileNameReport + ".xlsx");
        }
        #endregion

        #region DANH SACH VAN DON NHHAN VIEN DANG GIU

        [HttpGet("GetByStatusCurrentEmp")]
        public JsonResult GetByStatusCurrentEmp(int? statusId = null, string statusIds = null, string listShipmentIds = null, string searchText = null, int? pageSize = 10, int? pageNumber = 1, string cols = null)
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
            //using (var contextRRP = new ApplicationContextRRP())
            //{
            var _unitOfWorkRRP = new UnitOfWorkRRP(_contextRRP);
            var result = _unitOfWorkRRP.Repository<Proc_GetShipmentCurrentEmp>()
                      .ExecProcedure(Proc_GetShipmentCurrentEmp.GetEntityProc(currentUser.Id, statusIds, listShipmentIds, searchText, pageNumber, pageSize));
            return JsonUtil.Success(result);
            //}
        }
        #endregion

        [HttpGet("GetCountByDeadLineType")]
        public JsonResult GetCountByDeadLineType()
        {
            var currentUser = this.GetCurrentUser();
            var hubId = currentUser.HubId;
            if (!hubId.HasValue) hubId = 0;
            var result = _unitOfWork.Repository<Proc_GetCountShipmentByDealine>()
                     .ExecProcedureSingle(Proc_GetCountShipmentByDealine.GetEntityProc(hubId.Value));
            return JsonUtil.Success(result);
        }

        [HttpPost("DeliveryCompleteVSE")]
        public JsonResult DeliveryCompleteVSE([FromBody] InforBillVSEViewModel viewModel)
        {
            if (Util.IsNull(viewModel)) return JsonUtil.Error("Thông tin báo phát lỗi, báo về IT");
            if (Util.IsNull(viewModel.MANV)) return JsonUtil.Error("Mã nhân viên kết nối trống, báo về IT");
            if (Util.IsNull(viewModel.CREATED)) viewModel.CREATED = viewModel.MANV;
            VSEApi vseAPI = new VSEApi();
            var result = vseAPI.PushImageDelivery(viewModel.VD_ID, viewModel.SVD, viewModel.SMS_ID, viewModel.MANV.Trim(), viewModel.KY_NHAN, viewModel.LY_DO, viewModel.FILENAME, viewModel.IMAGE, viewModel.NGAY);
            string message = result.Result;
            if (message == "success") return JsonUtil.Success("Cập nhật báo phát VSE thành công");
            else return JsonUtil.Error(message);
        }

        [HttpGet("GetListShipmentPushRevenue")]
        public JsonResult GetListShipmentPushRevenue(string shipmentNumber)
        {
            var result = _unitOfWork.Repository<Proc_GetShipmentPushRevenue>()
                     .ExecProcedure(Proc_GetShipmentPushRevenue.GetEntityProc(shipmentNumber));
            return JsonUtil.Success(result);
        }

        [AllowAnonymous]
        [HttpPost("ReceiveWebhooksNinjaVan")]
        public JsonResult ReceiveWebhooksNinjaVan([FromBody] WebhooksNinjaVanViewModel viewModel)
        {
            //save log
            using (var context4 = new ApplicationContext())
            {
                string dataSave = JsonConvert.SerializeObject(viewModel);
                var _unitOfWork4 = new UnitOfWork(context4);
                _unitOfWork4.Repository<Proc_SaveLogReceiveData>()
                .ExecProcedureSingle(Proc_SaveLogReceiveData.GetEntityProc(dataSave));
            }
            var checkShipments = _unitOfWork.Repository<Proc_GetShipmentByShipmentNumber>()
              .ExecProcedure(Proc_GetShipmentByShipmentNumber.GetEntityProc(viewModel.tracking_ref_no)).ToList();
            if (checkShipments.Count() > 0)
            {
                var checkNumber = checkShipments[0];
                if (!Util.IsNull(checkNumber))
                {
                    StatusNinjaVanHelper statusNinjaVan = new StatusNinjaVanHelper();
                    var shipment = _unitOfWork.RepositoryCRUD<Shipment>().GetSingle(checkNumber.Id);
                    List<int> listStatusLockChange = new List<int>();
                    //listStatusLockChange.Add(StatusHelper.ShipmentStatusId.DeliveryComplete);
                    listStatusLockChange.Add(StatusHelper.ShipmentStatusId.ReturnComplete);
                    listStatusLockChange.Add(StatusHelper.ShipmentStatusId.Cancel);
                    listStatusLockChange.Add(StatusHelper.ShipmentStatusId.CollectCancel);
                    listStatusLockChange.Add(StatusHelper.ShipmentStatusId.PickupCancel);
                    listStatusLockChange.Add(StatusHelper.ShipmentStatusId.LotteCancel);
                    if (!listStatusLockChange.Contains(shipment.ShipmentStatusId))
                    {
                        var findStatus = statusNinjaVan.Staging.Find(f => f.Code.ToLower() == viewModel.status.ToLower());
                        if (findStatus != null)
                        {
                            shipment.ShipmentStatusId = findStatus.StatusId;
                            if (findStatus.StatusId == StatusHelper.ShipmentStatusId.DeliveryComplete && shipment.CurrentEmpId.HasValue)
                            {
                                shipment.EndDeliveryTime = DateTime.Now;
                                shipment.DeliverUserId = shipment.CurrentEmpId;
                                if (shipment.COD > 0) shipment.KeepingCODEmpId = shipment.CurrentEmpId;
                                if (shipment.PaymentTypeId == PaymentTypeHelper.NGUOI_NHAN_THANH_TOAN) shipment.KeepingTotalPriceEmpId = shipment.CurrentEmpId;
                            }
                            //
                            var ladingShip = new CreateUpdateLadingScheduleViewModel(
                            shipment.Id,
                                    null,
                                    shipment.CurrentEmpId,
                                    shipment.ShipmentStatusId,
                                    null,
                                    null,
                                    null,
                                    findStatus.Name,
                                    0,
                                    null
                                );
                            _iLadingScheduleService.Create(ladingShip);
                            _unitOfWork.Commit();
                        }
                    }
                }
            }
            return JsonUtil.Success("OK, Completed!");
        }

        [HttpPost("GetReportListShipmentExport")]
        public async Task<dynamic> GetReportListShipmentExport([FromBody] ShipmentFilterViewModel filterViewModel)
        {
            var currentUserId = this.GetCurrentUserId();
            DataTable dtt;
            Image Img = null;
            DataTable DttClone = new DataTable();

            try
            {
                string listGoodsIds = null;
                if (filterViewModel.ListGoodsIds != null && filterViewModel.ListGoodsIds.Count() > 0) listGoodsIds = String.Join(",", filterViewModel.ListGoodsIds);
                var unitOfWordRRP = new UnitOfWorkRRP(_contextRRP);
                var data = await unitOfWordRRP.Repository<Proc_ReportListShipment>()
                          .GetReportListShipmentExport(Proc_ReportListShipment.GetEntityProc(
                filterViewModel.OrderDateFrom,
                filterViewModel.OrderDateTo,
                filterViewModel.SenderId,
                filterViewModel.PaymentTypeId,
                filterViewModel.FromProvinceId,
                filterViewModel.ToProvinceId,
                filterViewModel.ToProvinceIds,
                filterViewModel.WeightFrom,
                filterViewModel.WeightTo,
                filterViewModel.ServiceId,
                filterViewModel.ShipmentNumber,
                filterViewModel.ShopCode,
                filterViewModel.ReferencesCode,
                filterViewModel.ReShipmentNumber,
                filterViewModel.SearchText,
                filterViewModel.IsExistInfoDelivery,
                filterViewModel.IsExistImagePickup,
                filterViewModel.IsBox,
                filterViewModel.IsPrintBill,
                filterViewModel.UploadExcelHistoryId,
                filterViewModel.GroupStatusId,
                filterViewModel.ShipmentStatusId,
                filterViewModel.FromHubId,
                filterViewModel.ToHubId,
                filterViewModel.CurrentHubId,
                filterViewModel.CurrentEmpId,
                filterViewModel.DeadlineTypeId,
                filterViewModel.pageNumber,
                filterViewModel.pageSize = 1,
                filterViewModel.DeliveryUserId,
                filterViewModel.NumIssueDelivery,
                filterViewModel.IsSuccess,
                filterViewModel.IsGroupEmp,
                filterViewModel.ListGoodsId,
                listGoodsIds,
                filterViewModel.IsHideInPackage,
                currentUserId));
                if (!Util.IsNull(data))
                {

                    //Debug.WriteLine("1");
                    var list = data.ToList();
                    if (list.Count == 0)
                    {
                        return ("Not found data!");
                    }
                    dtt = data.ToDataTable();
                    int TotalCount = list[0].TotalCount.Value;
                    var ischecktime = ExportExcelPartern.CheckTimeExport();


                    if (ischecktime == false)
                    {
                        if (ExportExcelPartern.CheckTotalData(TotalCount) && filterViewModel.IsAllowRequest == false)
                        {
                            return JsonUtil.Error("Get data error!!!");
                            //return null;
                        }
                    }
                    var datatable = await unitOfWordRRP.Repository<Proc_ReportListShipment>()
                     .GetReportListShipmentExport(Proc_ReportListShipment.GetEntityProc(
           filterViewModel.OrderDateFrom,
           filterViewModel.OrderDateTo,
           filterViewModel.SenderId,
           filterViewModel.PaymentTypeId,
           filterViewModel.FromProvinceId,
           filterViewModel.ToProvinceId,
           filterViewModel.ToProvinceIds,
           filterViewModel.WeightFrom,
           filterViewModel.WeightTo,
           filterViewModel.ServiceId,
           filterViewModel.ShipmentNumber,
           filterViewModel.ShopCode,
           filterViewModel.ReferencesCode,
           filterViewModel.ReShipmentNumber,
           filterViewModel.SearchText,
           filterViewModel.IsExistInfoDelivery,
           filterViewModel.IsExistImagePickup,
           filterViewModel.IsBox,
           filterViewModel.IsPrintBill,
           filterViewModel.UploadExcelHistoryId,
           filterViewModel.GroupStatusId,
           filterViewModel.ShipmentStatusId,
           filterViewModel.FromHubId,
           filterViewModel.ToHubId,
           filterViewModel.CurrentHubId,
           filterViewModel.CurrentEmpId,
           filterViewModel.DeadlineTypeId,
           filterViewModel.pageNumber,
           filterViewModel.pageSize = TotalCount,
           filterViewModel.DeliveryUserId,
           filterViewModel.NumIssueDelivery,
           filterViewModel.IsSuccess,
           filterViewModel.IsGroupEmp,
           filterViewModel.ListGoodsId,
           listGoodsIds,
           filterViewModel.IsHideInPackage,
           currentUserId));
                    if (!Util.IsNull(datatable))
                    {
                        //Debug.WriteLine();
                        dtt = datatable.ToDataTable();
                    }
                    else
                    {
                        //return JsonUtil.Error("Get data error!!!");
                        return null;
                    }
                    //}
                    var bytearray = ExportExcelPartern.ExportExcel(filterViewModel.CustomExportFile, dtt);
                    return File(bytearray, "application/xlsx", filterViewModel.CustomExportFile.FileNameReport + ".xlsx");
                }
                else
                {
                    //return JsonUtil.Error("Get data error!!!");
                    return null;
                }
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
                //return null;
            }
        }
        [HttpPost("GetListShipmentExport")]
        public async Task<dynamic> GetListShipmentExport([FromBody] ShipmentFilterViewModel filterViewModel)
        {

            var currentUserId = this.GetCurrentUserId();

            int? PageSize = filterViewModel.pageSize;

            Image img = null;

            DataTable dtt;

            DataTable DttClone = new DataTable();

            try
            {
                var unitOfWordRRP = new UnitOfWorkRRP(_contextRRP);
                var data = await unitOfWordRRP.Repository<Proc_GetListShipment>()
                          .GetReportListShipmentExport(Proc_GetListShipment.GetEntityProc(
                filterViewModel.OrderDateFrom,
                filterViewModel.OrderDateTo,
                filterViewModel.SenderId,
                filterViewModel.PaymentTypeId,
                filterViewModel.FromProvinceId,
                filterViewModel.ToProvinceId,
                filterViewModel.WeightFrom,
                filterViewModel.WeightTo,
                filterViewModel.ServiceId,
                filterViewModel.ShipmentNumber,
                filterViewModel.SOENTRY,
                filterViewModel.ShopCode,
                filterViewModel.ReferencesCode,
                filterViewModel.ReShipmentNumber,
                filterViewModel.SearchText,
                filterViewModel.IsExistInfoDelivery,
                filterViewModel.IsExistImagePickup,
                filterViewModel.IsBox,
                filterViewModel.IsPrintBill,
                filterViewModel.UploadExcelHistoryId,
                filterViewModel.GroupStatusId,
                filterViewModel.ShipmentStatusId,
                filterViewModel.FromHubId,
                filterViewModel.ToHubId,
                filterViewModel.CurrentHubId,
                filterViewModel.CurrentEmpId,
                filterViewModel.DeadlineTypeId,
                filterViewModel.pageNumber,
                filterViewModel.pageSize = 1,
                filterViewModel.DeliveryUserId,
                filterViewModel.NumIssueDelivery,
                filterViewModel.IsSuccess,
                filterViewModel.IsGroupEmp,
                filterViewModel.ListGoodsId,
                filterViewModel.IsHideInPackage,
                currentUserId
                ));
                if (!Util.IsNull(data))
                {
                    var list = data.ToList();
                    dtt = data.ToDataTable();
                    int TotalCount = list[0].TotalCount.Value;
                    var ischecktime = ExportExcelPartern.CheckTimeExport();


                    if (ischecktime == false)
                    {
                        if (ExportExcelPartern.CheckTotalData(TotalCount) && filterViewModel.IsAllowRequest == false)
                        {
                            return JsonUtil.Error("Get data error!!!");
                            //return null;
                        }
                    }
                    var datatable = await unitOfWordRRP.Repository<Proc_GetListShipment>()
                     .GetReportListShipmentExport(Proc_GetListShipment.GetEntityProc(
           filterViewModel.OrderDateFrom,
           filterViewModel.OrderDateTo,
           filterViewModel.SenderId,
           filterViewModel.PaymentTypeId,
           filterViewModel.FromProvinceId,
           filterViewModel.ToProvinceId,
           filterViewModel.WeightFrom,
           filterViewModel.WeightTo,
           filterViewModel.ServiceId,
           filterViewModel.ShipmentNumber,
           filterViewModel.SOENTRY,
           filterViewModel.ShopCode,
           filterViewModel.ReferencesCode,
           filterViewModel.ReShipmentNumber,
           filterViewModel.SearchText,
           filterViewModel.IsExistInfoDelivery,
           filterViewModel.IsExistImagePickup,
           filterViewModel.IsBox,
           filterViewModel.IsPrintBill,
           filterViewModel.UploadExcelHistoryId,
           filterViewModel.GroupStatusId,
           filterViewModel.ShipmentStatusId,
           filterViewModel.FromHubId,
           filterViewModel.ToHubId,
           filterViewModel.CurrentHubId,
           filterViewModel.CurrentEmpId,
           filterViewModel.DeadlineTypeId,
           filterViewModel.pageNumber,
           filterViewModel.pageSize = TotalCount,
           filterViewModel.DeliveryUserId,
           filterViewModel.NumIssueDelivery,
           filterViewModel.IsSuccess,
           filterViewModel.IsGroupEmp,
           filterViewModel.ListGoodsId,
           filterViewModel.IsHideInPackage,
           currentUserId
           ));
                    if (!Util.IsNull(datatable))
                    {
                        //Debug.WriteLine();
                        dtt = datatable.ToDataTable();
                    }
                    else
                    {
                        return JsonUtil.Error("Get data error!!!");
                        //return null;
                    }
                    var bytearray = ExportExcelPartern.ExportExcel(filterViewModel.CustomExportFile, dtt);
                    return File(bytearray, "application/xlsx", filterViewModel.CustomExportFile.FileNameReport + ".xlsx");
                }
                else
                {
                    return JsonUtil.Error("Get data error!!!");
                    //return null;
                }
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
                //return null;
            }
        }


        #region Báo cáo chỉnh sửa version
        [HttpGet("CompareShipmentVersion")]
        public JsonResult CompareShipmentVersion(int? shipmentVersionId = null)
        {

            var data = _unitOfWork.Repository<Proc_CompareShipmentVersion>()
                      .ExecProcedureSingle(Proc_CompareShipmentVersion.GetEntityProc(
                          null,
                          shipmentVersionId
                          ));
            return JsonUtil.Success(data);
        }
        #endregion


        #region BÁO CÁO RevenueCustomner
        [HttpPost("RevenueCustomner")]
        public JsonResult RevenueCustomner([FromBody] ShipmentFilterViewModel filterViewModel)
        {
            var currentUserId = this.GetCurrentUserId();
            try
            {
                string listGoodsIds = null;
                if (filterViewModel.ListGoodsIds != null) String.Join(",", filterViewModel.ListGoodsIds);
                var unitOfWordRRP = new UnitOfWorkRRP(_contextRRP);
                var data = unitOfWordRRP.Repository<Proc_ReportRevenueCustomner>()
                          .ExecProcedure(Proc_ReportRevenueCustomner.GetEntityProc(
                filterViewModel.OrderDateFrom,
                filterViewModel.OrderDateTo,
                filterViewModel.SenderId,
                filterViewModel.PaymentTypeId,
                filterViewModel.FromProvinceId,
                filterViewModel.ToProvinceId,
                filterViewModel.ToProvinceIds,
                filterViewModel.WeightFrom,
                filterViewModel.WeightTo,
                filterViewModel.ServiceId,
                filterViewModel.ShipmentNumber,
                filterViewModel.ShopCode,
                filterViewModel.ReferencesCode,
                filterViewModel.ReShipmentNumber,
                filterViewModel.SearchText,
                filterViewModel.IsExistInfoDelivery,
                filterViewModel.IsExistImagePickup,
                filterViewModel.IsBox,
                filterViewModel.IsPrintBill,
                filterViewModel.UploadExcelHistoryId,
                filterViewModel.GroupStatusId,
                filterViewModel.ShipmentStatusId,
                filterViewModel.FromHubId,
                filterViewModel.ToHubId,
                filterViewModel.CurrentHubId,
                filterViewModel.CurrentEmpId,
                filterViewModel.DeadlineTypeId,
                filterViewModel.pageNumber,
                filterViewModel.pageSize,
                filterViewModel.DeliveryUserId,
                filterViewModel.NumIssueDelivery,
                filterViewModel.IsSuccess,
                filterViewModel.IsGroupEmp,
                filterViewModel.ListGoodsId,
                listGoodsIds,
                filterViewModel.IsHideInPackage,
                currentUserId, null, filterViewModel.truckIds));
                if (!Util.IsNull(data))
                {
                    return JsonUtil.Success(data);
                }
                else
                {
                    return JsonUtil.Error("Get data error!!!");
                }
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }
        }
        #endregion

        #region BÁO CÁO RevenueCustomner EXPORT
        [HttpPost("RevenueCustomnerExport")]
        public dynamic RevenueCustomnerExport([FromBody] ShipmentFilterViewModel filterViewModel)
        {
            DataTable dtt = new DataTable();
            var currentUserId = this.GetCurrentUserId();
            try
            {
                string listGoodsIds = null;
                if (filterViewModel.ListGoodsIds != null) String.Join(",", filterViewModel.ListGoodsIds);
                var unitOfWordRRP = new UnitOfWorkRRP(_contextRRP);
                var data = unitOfWordRRP.Repository<Proc_ReportRevenueCustomner>()
                          .ExecProcedure(Proc_ReportRevenueCustomner.GetEntityProc(
                filterViewModel.OrderDateFrom,
                filterViewModel.OrderDateTo,
                filterViewModel.SenderId,
                filterViewModel.PaymentTypeId,
                filterViewModel.FromProvinceId,
                filterViewModel.ToProvinceId,
                filterViewModel.ToProvinceIds,
                filterViewModel.WeightFrom,
                filterViewModel.WeightTo,
                filterViewModel.ServiceId,
                filterViewModel.ShipmentNumber,
                filterViewModel.ShopCode,
                filterViewModel.ReferencesCode,
                filterViewModel.ReShipmentNumber,
                filterViewModel.SearchText,
                filterViewModel.IsExistInfoDelivery,
                filterViewModel.IsExistImagePickup,
                filterViewModel.IsBox,
                filterViewModel.IsPrintBill,
                filterViewModel.UploadExcelHistoryId,
                filterViewModel.GroupStatusId,
                filterViewModel.ShipmentStatusId,
                filterViewModel.FromHubId,
                filterViewModel.ToHubId,
                filterViewModel.CurrentHubId,
                filterViewModel.CurrentEmpId,
                filterViewModel.DeadlineTypeId,
                filterViewModel.pageNumber,
                filterViewModel.pageSize,
                filterViewModel.DeliveryUserId,
                filterViewModel.NumIssueDelivery,
                filterViewModel.IsSuccess,
                filterViewModel.IsGroupEmp,
                filterViewModel.ListGoodsId,
                listGoodsIds,
                filterViewModel.IsHideInPackage,
                currentUserId, null, filterViewModel.truckIds));

                if (!Util.IsNull(data))
                {
                    //Debug.WriteLine();
                    dtt = data.ToDataTable();
                }
                else
                {
                    //return JsonUtil.Error("Get data error!!!");
                    return null;
                }
                //}
                var bytearray = ExportExcelPartern.ExportExcel(filterViewModel.CustomExportFile, dtt);
                return File(bytearray, "application/xlsx", filterViewModel.CustomExportFile.FileNameReport + ".xlsx");
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }
        }
        #endregion

        #region BÁO CÁO Toàn Trình
        [HttpPost("ReportAllChedule")]
        public JsonResult ReportAllChedule([FromBody] ShipmentFilterViewModel filterViewModel)
        {
            var currentUserId = this.GetCurrentUserId();
            try
            {
                string listGoodsIds = null;
                if (filterViewModel.ListGoodsIds != null) String.Join(",", filterViewModel.ListGoodsIds);
                var unitOfWordRRP = new UnitOfWorkRRP(_contextRRP);
                var data = unitOfWordRRP.Repository<Proc_ReportLandingScheduleReport>()
                          .ExecProcedure(Proc_ReportLandingScheduleReport.GetEntityProc(
                filterViewModel.OrderDateFrom,
                filterViewModel.OrderDateTo,
                filterViewModel.SenderId,
                filterViewModel.PaymentTypeId,
                filterViewModel.FromProvinceId,
                filterViewModel.ToProvinceId,
                filterViewModel.ToProvinceIds,
                filterViewModel.WeightFrom,
                filterViewModel.WeightTo,
                filterViewModel.ServiceId,
                filterViewModel.ShipmentNumber,
                filterViewModel.ShopCode,
                filterViewModel.ReferencesCode,
                filterViewModel.ReShipmentNumber,
                filterViewModel.SearchText,
                filterViewModel.IsExistInfoDelivery,
                filterViewModel.IsExistImagePickup,
                filterViewModel.IsBox,
                filterViewModel.IsPrintBill,
                filterViewModel.UploadExcelHistoryId,
                filterViewModel.GroupStatusId,
                filterViewModel.ShipmentStatusId,
                filterViewModel.FromHubId,
                filterViewModel.ToHubId,
                filterViewModel.CurrentHubId,
                filterViewModel.CurrentEmpId,
                filterViewModel.DeadlineTypeId,
                filterViewModel.pageNumber,
                filterViewModel.pageSize,
                filterViewModel.DeliveryUserId,
                filterViewModel.NumIssueDelivery,
                filterViewModel.IsSuccess,
                filterViewModel.IsGroupEmp,
                filterViewModel.ListGoodsId,
                listGoodsIds,
                filterViewModel.IsHideInPackage,
                currentUserId));
                if (!Util.IsNull(data))
                {
                    return JsonUtil.Success(data);
                }
                else
                {
                    return JsonUtil.Error("Get data error!!!");
                }
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }
        }
        #endregion

        #region BÁO CÁO Toàn trình EXPORT
        [HttpPost("ReportAllCheduleExport")]
        public dynamic ReportAllCheduleExport([FromBody] ShipmentFilterViewModel filterViewModel)
        {
            DataTable dtt = new DataTable();
            var currentUserId = this.GetCurrentUserId();
            try
            {
                string listGoodsIds = null;
                if (filterViewModel.ListGoodsIds != null) String.Join(",", filterViewModel.ListGoodsIds);
                var unitOfWordRRP = new UnitOfWorkRRP(_contextRRP);
                var data = unitOfWordRRP.Repository<Proc_ReportLandingScheduleReport>()
                          .ExecProcedure(Proc_ReportLandingScheduleReport.GetEntityProc(
                filterViewModel.OrderDateFrom,
                filterViewModel.OrderDateTo,
                filterViewModel.SenderId,
                filterViewModel.PaymentTypeId,
                filterViewModel.FromProvinceId,
                filterViewModel.ToProvinceId,
                filterViewModel.ToProvinceIds,
                filterViewModel.WeightFrom,
                filterViewModel.WeightTo,
                filterViewModel.ServiceId,
                filterViewModel.ShipmentNumber,
                filterViewModel.ShopCode,
                filterViewModel.ReferencesCode,
                filterViewModel.ReShipmentNumber,
                filterViewModel.SearchText,
                filterViewModel.IsExistInfoDelivery,
                filterViewModel.IsExistImagePickup,
                filterViewModel.IsBox,
                filterViewModel.IsPrintBill,
                filterViewModel.UploadExcelHistoryId,
                filterViewModel.GroupStatusId,
                filterViewModel.ShipmentStatusId,
                filterViewModel.FromHubId,
                filterViewModel.ToHubId,
                filterViewModel.CurrentHubId,
                filterViewModel.CurrentEmpId,
                filterViewModel.DeadlineTypeId,
                filterViewModel.pageNumber,
                filterViewModel.pageSize,
                filterViewModel.DeliveryUserId,
                filterViewModel.NumIssueDelivery,
                filterViewModel.IsSuccess,
                filterViewModel.IsGroupEmp,
                filterViewModel.ListGoodsId,
                listGoodsIds,
                filterViewModel.IsHideInPackage,
                currentUserId));
                if (!Util.IsNull(data))
                {
                    //Debug.WriteLine();
                    dtt = data.ToDataTable();
                }
                else
                {
                    //return JsonUtil.Error("Get data error!!!");
                    return null;
                }
                //}
                var bytearray = ExportExcelPartern.ExportExcel(filterViewModel.CustomExportFile, dtt);
                return File(bytearray, "application/xlsx", filterViewModel.CustomExportFile.FileNameReport + ".xlsx");
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }
        }
        #endregion

        #region báo cáo toàn trình
        [HttpPost("GetAllScheeduleExport")]
        public async Task<dynamic> GetAllScheeduleExport([FromBody] ShipmentFilterViewModel filterViewModel)
        {
            var currentUserId = this.GetCurrentUserId();
            DataTable dtt;
            Image Img = null;
            DataTable DttClone = new DataTable();

            try
            {
                string listGoodsIds = null;
                if (filterViewModel.ListGoodsIds != null && filterViewModel.ListGoodsIds.Count() > 0) listGoodsIds = String.Join(",", filterViewModel.ListGoodsIds);
                var unitOfWordRRP = new UnitOfWorkRRP(_contextRRP);
                var data = await unitOfWordRRP.Repository<Proc_ReportAllSchedule2>()
                          .GetReportListShipmentExport(Proc_ReportAllSchedule2.GetEntityProc(
                filterViewModel.OrderDateFrom,
                filterViewModel.OrderDateTo,
                filterViewModel.SenderId,
                filterViewModel.PaymentTypeId,
                filterViewModel.FromProvinceId,
                filterViewModel.ToProvinceId,
                filterViewModel.ToProvinceIds,
                filterViewModel.WeightFrom,
                filterViewModel.WeightTo,
                filterViewModel.ServiceId,
                filterViewModel.ShipmentNumber,
                filterViewModel.ShopCode,
                filterViewModel.ReferencesCode,
                filterViewModel.ReShipmentNumber,
                filterViewModel.SearchText,
                filterViewModel.IsExistInfoDelivery,
                filterViewModel.IsExistImagePickup,
                filterViewModel.IsBox,
                filterViewModel.IsPrintBill,
                filterViewModel.UploadExcelHistoryId,
                filterViewModel.GroupStatusId,
                filterViewModel.ShipmentStatusId,
                filterViewModel.FromHubId,
                filterViewModel.ToHubId,
                filterViewModel.CurrentHubId,
                filterViewModel.CurrentEmpId,
                filterViewModel.DeadlineTypeId,
                filterViewModel.pageNumber,
                filterViewModel.pageSize = 1,
                filterViewModel.DeliveryUserId,
                filterViewModel.NumIssueDelivery,
                filterViewModel.IsSuccess,
                filterViewModel.IsGroupEmp,
                filterViewModel.ListGoodsId,
                listGoodsIds,
                filterViewModel.IsHideInPackage,
                currentUserId));
                if (!Util.IsNull(data))
                {

                    //Debug.WriteLine("1");
                    var list = data.ToList();
                    if (list.Count == 0)
                    {
                        return ("Not found data!");
                    }
                    dtt = data.ToDataTable();
                    int TotalCount = list[0].TotalCount.Value;
                    var ischecktime = ExportExcelPartern.CheckTimeExport();


                    if (ischecktime == false)
                    {
                        if (ExportExcelPartern.CheckTotalData(TotalCount) && filterViewModel.IsAllowRequest == false)
                        {
                            return JsonUtil.Error("Get data error!!!");
                            //return null;
                        }
                    }
                    var datatable = await unitOfWordRRP.Repository<Proc_ReportAllSchedule2>()
                     .GetReportListShipmentExport(Proc_ReportAllSchedule2.GetEntityProc(
           filterViewModel.OrderDateFrom,
           filterViewModel.OrderDateTo,
           filterViewModel.SenderId,
           filterViewModel.PaymentTypeId,
           filterViewModel.FromProvinceId,
           filterViewModel.ToProvinceId,
           filterViewModel.ToProvinceIds,
           filterViewModel.WeightFrom,
           filterViewModel.WeightTo,
           filterViewModel.ServiceId,
           filterViewModel.ShipmentNumber,
           filterViewModel.ShopCode,
           filterViewModel.ReferencesCode,
           filterViewModel.ReShipmentNumber,
           filterViewModel.SearchText,
           filterViewModel.IsExistInfoDelivery,
           filterViewModel.IsExistImagePickup,
           filterViewModel.IsBox,
           filterViewModel.IsPrintBill,
           filterViewModel.UploadExcelHistoryId,
           filterViewModel.GroupStatusId,
           filterViewModel.ShipmentStatusId,
           filterViewModel.FromHubId,
           filterViewModel.ToHubId,
           filterViewModel.CurrentHubId,
           filterViewModel.CurrentEmpId,
           filterViewModel.DeadlineTypeId,
           filterViewModel.pageNumber,
           filterViewModel.pageSize = TotalCount,
           filterViewModel.DeliveryUserId,
           filterViewModel.NumIssueDelivery,
           filterViewModel.IsSuccess,
           filterViewModel.IsGroupEmp,
           filterViewModel.ListGoodsId,
           listGoodsIds,
           filterViewModel.IsHideInPackage,
           currentUserId));
                    if (!Util.IsNull(datatable))
                    {
                        //Debug.WriteLine();
                        dtt = datatable.ToDataTable();
                    }
                    else
                    {
                        //return JsonUtil.Error("Get data error!!!");
                        return null;
                    }
                    //}
                    var bytearray = ExportExcelPartern.ExportExcel(filterViewModel.CustomExportFile, dtt);
                    return File(bytearray, "application/xlsx", filterViewModel.CustomExportFile.FileNameReport + ".xlsx");
                }
                else
                {
                    //return JsonUtil.Error("Get data error!!!");
                    return null;
                }
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
                //return null;
            }
        }

        [HttpPost("GetAllScheedule")]
        public async Task<dynamic> GetAllScheedule([FromBody] ShipmentFilterViewModel filterViewModel)
        {
            var currentUserId = this.GetCurrentUserId();
            try
            {
                string listGoodsIds = null;
                if (filterViewModel.ListGoodsIds != null && filterViewModel.ListGoodsIds.Count() > 0) listGoodsIds = String.Join(",", filterViewModel.ListGoodsIds);
                var unitOfWordRRP = new UnitOfWorkRRP(_contextRRP);
                var data = await unitOfWordRRP.Repository<Proc_ReportAllSchedule2>()
                          .GetReportListShipmentExport(Proc_ReportAllSchedule2.GetEntityProc(
                filterViewModel.OrderDateFrom,
                filterViewModel.OrderDateTo,
                filterViewModel.SenderId,
                filterViewModel.PaymentTypeId,
                filterViewModel.FromProvinceId,
                filterViewModel.ToProvinceId,
                filterViewModel.ToProvinceIds,
                filterViewModel.WeightFrom,
                filterViewModel.WeightTo,
                filterViewModel.ServiceId,
                filterViewModel.ShipmentNumber,
                filterViewModel.ShopCode,
                filterViewModel.ReferencesCode,
                filterViewModel.ReShipmentNumber,
                filterViewModel.SearchText,
                filterViewModel.IsExistInfoDelivery,
                filterViewModel.IsExistImagePickup,
                filterViewModel.IsBox,
                filterViewModel.IsPrintBill,
                filterViewModel.UploadExcelHistoryId,
                filterViewModel.GroupStatusId,
                filterViewModel.ShipmentStatusId,
                filterViewModel.FromHubId,
                filterViewModel.ToHubId,
                filterViewModel.CurrentHubId,
                filterViewModel.CurrentEmpId,
                filterViewModel.DeadlineTypeId,
                filterViewModel.pageNumber,
                filterViewModel.pageSize,
                filterViewModel.DeliveryUserId,
                filterViewModel.NumIssueDelivery,
                filterViewModel.IsSuccess,
                filterViewModel.IsGroupEmp,
                filterViewModel.ListGoodsId,
                listGoodsIds,
                filterViewModel.IsHideInPackage,
                currentUserId));
                if (!Util.IsNull(data))
                {
                    return JsonUtil.Success(data);
                }
                return JsonUtil.Success();
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
                //return null;
            }
        }
        #endregion
        #region BÁO CÁO DANH THU KHÁCH HANG

        [HttpPost("GetReportRevenueCustomer")]
        public JsonResult GetReportRevenueCustomer([FromBody] ShipmentFilterViewModel filterViewModel)
        {
            var currentUserId = this.GetCurrentUserId();
            try
            {
                var unitOfWordRRP = new UnitOfWorkRRP(_contextRRP);
                var data = unitOfWordRRP.Repository<Proc_ReportRevenueCustomer>()
                          .ExecProcedure(Proc_ReportRevenueCustomer.GetEntityProc(
                filterViewModel.OrderDateFrom,
                filterViewModel.OrderDateTo,
                filterViewModel.SenderId,
                filterViewModel.pageNumber,
                filterViewModel.pageSize));
                if (!Util.IsNull(data))
                {
                    return JsonUtil.Success(data);
                }
                else
                {
                    return JsonUtil.Error("Get data error!!!");
                }
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }
        }
        //
        [HttpPost("GetReportRevenueCustomerExport")]
        public async Task<dynamic> GetReportRevenueCustomerExport([FromBody] ShipmentFilterViewModel filterViewModel)
        {

            var currentUserId = this.GetCurrentUserId();

            int? PageSize = filterViewModel.pageSize;

            Image img = null;

            DataTable dtt;

            DataTable DttClone = new DataTable();

            try
            {
                var unitOfWordRRP = new UnitOfWorkRRP(_contextRRP);
                var data = await unitOfWordRRP.Repository<Proc_ReportRevenueCustomer>()
                          .GetReportListShipmentExport(Proc_ReportRevenueCustomer.GetEntityProc(
                filterViewModel.OrderDateFrom,
                filterViewModel.OrderDateTo,
                filterViewModel.SenderId,
                filterViewModel.pageNumber,
                filterViewModel.pageSize = 1));
                if (!Util.IsNull(data))
                {
                    var list = data.ToList();
                    dtt = data.ToDataTable();
                    int TotalCount = list[0].TotalCount.Value;
                    var ischecktime = ExportExcelPartern.CheckTimeExport();


                    if (ischecktime == false)
                    {
                        if (ExportExcelPartern.CheckTotalData(TotalCount) && filterViewModel.IsAllowRequest == false)
                        {
                            return JsonUtil.Error("Get data error!!!");
                            //return null;
                        }
                    }
                    var datatable = await unitOfWordRRP.Repository<Proc_ReportRevenueCustomer>()
                     .GetReportListShipmentExport(Proc_ReportRevenueCustomer.GetEntityProc(
           filterViewModel.OrderDateFrom,
           filterViewModel.OrderDateTo,
           filterViewModel.SenderId,
           filterViewModel.pageNumber,
           filterViewModel.pageSize = TotalCount));
                    if (!Util.IsNull(datatable))
                    {
                        //Debug.WriteLine();
                        dtt = datatable.ToDataTable();
                    }
                    else
                    {
                        return JsonUtil.Error("Get data error!!!");
                        //return null;
                    }
                    var bytearray = ExportExcelPartern.ExportExcel(filterViewModel.CustomExportFile, dtt);
                    return File(bytearray, "application/xlsx", filterViewModel.CustomExportFile.FileNameReport + ".xlsx");
                }
                else
                {
                    return JsonUtil.Error("Get data error!!!");
                    //return null;
                }
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
                //return null;
            }
        }
        #endregion


        // test one shipment SAP TO GSDP

        #region CHECK VALID IMPORT EXCEL
        [HttpPost("CheckUpLoadExcel")]
        public async Task<JsonResult> CheckUpLoadExcel([FromBody]List<CreateUpdateShipmentViewModel> viewModels)
        {
            string shipmentNumbers = "";
            string requestCodes = "";
            foreach (var item in viewModels)
            {
                if (item.ShipmentNumber != null)
                {
                    shipmentNumbers = shipmentNumbers + item.ShipmentNumber + ",";
                }
                //if (item.RequestShipment != null)
                //{
                //    requestCodes = requestCodes + item.RequestShipment.ShipmentNumber + ",";
                //}
            }
            try
            {
                if (shipmentNumbers != "" || requestCodes != "")
                {
                    var res = _unitOfWork.Repository<Proc_CheckUpLoadExcelShipment>()
                    .ExecProcedureSingle(Proc_CheckUpLoadExcelShipment.GetEntityProc(shipmentNumbers, requestCodes));
                    if (res.Result == true)
                    {
                        return JsonUtil.Success(res);
                    }
                    else
                    {
                        return JsonUtil.Error(res.Message);
                    }
                }
                else
                {
                    return JsonUtil.Success();
                }
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }
        }
        #endregion
        [HttpPost("GetListHistoryShipment")]
        public JsonResult GetListHistoryShipment([FromBody] ListHistoryShipmentViewModel viewModel)
        {
            var result = _unitOfWork.Repository<Proc_GetListHistoryShipment>()
                     .ExecProcedure(Proc_GetListHistoryShipment.GetEntityProc(viewModel.FromDate, viewModel.ToDate, viewModel.SearchText, viewModel.PageNumber, viewModel.PageSize));
            return JsonUtil.Success(result);
        }

        [HttpPost("GetListHistoryShipmentByShipmentId")]
        public JsonResult GetListHistoryShipmentByShipmentId([FromBody] GetListHistoryShipmentByShipmentIdViewModel viewModel)
        {
            var result = _unitOfWork.Repository<Proc_GetListHistoryShipmentByShipmentId>()
                     .ExecProcedure(Proc_GetListHistoryShipmentByShipmentId.GetEntityProc(viewModel.ShipmentId, viewModel.Id));
            return JsonUtil.Success(result);
        }

        [HttpPost("PickupFail")]
        public async Task<JsonResult> PickupFail([FromBody] UpdateStatusViewModel viewModel)
        {
            var currentuser = GetCurrentUser();
            Shipment shipment = _unitOfWork.RepositoryR<Shipment>().GetSingle(viewModel.Id);

            if (shipment != null)
            {
                shipment.ShipmentStatusId = 4;
                shipment.CurrentHubId = currentuser.HubId;
                shipment.CurrentEmpId = currentuser.Id;
                shipment.ReasonId = viewModel.ReasonId;
                _unitOfWork.Commit();

                var ladingShip = new CreateUpdateLadingScheduleViewModel(
                    shipment.Id,
                    currentuser.HubId,
                    currentuser.Id,
                    shipment.ShipmentStatusId,
                    viewModel.CurrentLat,
                    viewModel.CurrentLng,
                    null,
                    viewModel.Note,
                    0,
                    viewModel.ReasonId
                );
                await _iLadingScheduleService.Create(ladingShip);
            }
            return JsonUtil.Success(null, "Cập nhật trạng thái đơn hàng lấy không thành công");
        }

        // check shipmentNumber của requestShipment
        [HttpGet("CheckByShipmentNumber")]
        public async Task<JsonResult> CheckByShipmentNumber(string shipmentNumber, int? requestShipmentId)
        {
            var currentuser = GetCurrentUser();
            var data = _unitOfWork.Repository<Proc_CheckByShipmentNumber>()
                     .ExecProcedureSingle(Proc_CheckByShipmentNumber.GetEntityProc(shipmentNumber, currentuser.Id, requestShipmentId));
            if (data == null) return JsonUtil.Error("Không tìm thấy đơn hàng");
            else if (data.ShipmentStatusId == 3) return JsonUtil.Error(string.Format("Mã đơn hàng {0} đã được lấy thành công", data.ShipmentNumber));
            else if (data.ShipmentStatusId == 4) return JsonUtil.Error(string.Format("Mã đơn hàng {0} đã lấy không thành công", data.ShipmentNumber));
            var result = data;
            return JsonUtil.Success(result);
        }

        // chec shipmentNumber của nhiều requestSHipment
        [HttpPost("CheckByRequestShipmentIds")]
        public JsonResult CheckByRequestShipmentIds([FromBody] CheckByRequestShipmentIdsViewModel viewModel)
        {
            string requestShipmentIds = "";
            if (viewModel.RequestShipmentIds != null && viewModel.RequestShipmentIds.Count() > 0)
            {
                requestShipmentIds = string.Join(",", viewModel.RequestShipmentIds);
            }
            var currentuser = GetCurrentUser();
            var data = _unitOfWork.Repository<Proc_CheckByRequestShipmentIds>()
                     .ExecProcedureSingle(Proc_CheckByRequestShipmentIds.GetEntityProc(viewModel.ShipmentNumber, currentuser.Id, requestShipmentIds));
            if (data == null) return JsonUtil.Error("Không tìm thấy đơn hàng");
            else if (data.ShipmentStatusId == 3) return JsonUtil.Error(string.Format("Mã đơn hàng {0} đã được lấy thành công", data.ShipmentNumber));
            else if (data.ShipmentStatusId == 4) return JsonUtil.Error(string.Format("Mã đơn hàng {0} đã lấy không thành công", data.ShipmentNumber));
            var result = data;
            return JsonUtil.Success(result);
        }

        [HttpGet("CheckByShipmentNumberWarhouse")]
        public async Task<JsonResult> CheckByShipmentNumberWarhouse(string shipmentNumber)
        {
            var currentuser = GetCurrentUser();
            var data = _unitOfWork.Repository<Proc_CheckByShipmentNumberWarhouse>()
                     .ExecProcedureSingle(Proc_CheckByShipmentNumberWarhouse.GetEntityProc(shipmentNumber));
            if (data == null) return JsonUtil.Error("Không tìm thấy đơn hàng");
            var result = data;
            return JsonUtil.Success(result);
        }

        // check shipmentNUmbet
        [HttpGet("CheckByShipmentNumbers")]
        public async Task<JsonResult> CheckByShipmentNumbers(string shipmentNumber)
        {
            var currentuser = GetCurrentUser();
            var data = _unitOfWork.Repository<Proc_CheckByShipmentNumbers>()
                     .ExecProcedureSingle(Proc_CheckByShipmentNumbers.GetEntityProc(shipmentNumber));
            if (data == null) return JsonUtil.Error("Không tìm thấy đơn hàng");
            var result = data;
            return JsonUtil.Success(result);

        }

    }
}