using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Business.ViewModels;
using Core.Data.Abstract;
using Core.Entity.Entities;
using Core.Infrastructure.Helper;
using Core.Infrastructure.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Core.Entity.Procedures;
using System.Linq.Expressions;
using LinqKit;
using Core.Business.ViewModels.Shipments;
using AutoMapper;
using Core.Infrastructure.ViewModels;
using Core.Data.Core;
using Core.Data;
using Core.Api.Library;
using Core.Business.ViewModels.ExportExcelModel;
using MoreLinq;
using System.Data;
using Core.Business.ViewModels.Report;
using Core.Business.ViewModels.TruckTransfer;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class ReportController : GeneralController<ListCustomerPaymentViewModel, ListCustomerPaymentInfoViewModel, ListCustomerPayment>
    {
        private ApplicationContextRRP _contextRRP;
        private readonly IShipmentService _iShipmentService;
        private readonly IGeneralService _iGeneralServiceRaw;
        private readonly IGeneralService<CreateUpdateShipmentViewModel, ShipmentInfoViewModel, Shipment> _iGeneralServiceShip;
        public ReportController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            ApplicationContextRRP contextRRP,
            IOptions<AppSettings> optionsAccessor, IOptions<JwtIssuerOptions> jwtOptions,
            IUnitOfWork unitOfWork,
            IGeneralService<ListCustomerPaymentViewModel, ListCustomerPaymentInfoViewModel, ListCustomerPayment> iGeneralService,
            IShipmentService iShipmentService,
            IGeneralService iGeneralServiceRaw,
            IGeneralService<CreateUpdateShipmentViewModel, ShipmentInfoViewModel, Shipment> iGeneralServiceShip) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
            _contextRRP = contextRRP;
            _iGeneralServiceRaw = iGeneralServiceRaw;
            _iGeneralServiceShip = iGeneralServiceShip;
            _iShipmentService = iShipmentService;
        }
        #region Báo cáo tổng hợp gửi
        [HttpGet("GetReportSumary")]
        public JsonResult GetReportSumary(DateTime? fromDate = null, DateTime? toDate = null, int? pageSize = null, int? pageNumber = null, string cols = null, string type = null)
        {
            Expression<Func<Shipment, bool>> predicate = x => x.CreatedWhen.HasValue;
            if (type == "success")
            {
                if (fromDate.HasValue)
                {
                    predicate = predicate.And(x => fromDate.Value.Date <= x.EndDeliveryTime.Value.Date);
                }
                if (toDate.HasValue)
                {
                    predicate = predicate.And(x => toDate.Value.Date >= x.EndDeliveryTime.Value.Date);
                }
            }
            else
            {
                if (fromDate.HasValue)
                {
                    predicate = predicate.And(x => fromDate.Value.Date <= x.OrderDate.Date);
                }
                if (toDate.HasValue)
                {
                    predicate = predicate.And(x => toDate.Value.Date >= x.OrderDate.Date);
                }
            }
            var data = _iGeneralServiceShip.FindBy(predicate, pageSize, pageNumber, cols);
            return JsonUtil.Create(data);
        }
        #endregion
        #region Báo cáo tổng hợp khách hàng
        [HttpGet("GetReportCustomer")]
        public JsonResult GetReportCustomer(int? type = null, int? customerId = null, DateTime? fromDate = null, DateTime? toDate = null, int? hubId = null, int? pageSize = null, int? pageNumber = null)
        {
            var data = _unitOfWork.Repository<Proc_ReportByCustomer>()
                      .ExecProcedure(Proc_ReportByCustomer.GetEntityProc(type, customerId, fromDate, toDate, hubId));
            return JsonUtil.Success(data);

        }
        #endregion
        #region Báo cáo nhân viên đang giữ
        [HttpGet("GetReportEmployee")]
        public JsonResult GetReportEmployee(int? hubId = null, int? empId = null)
        {
            var data = _unitOfWork.Repository<Proc_ReportEmployee>()
                      .ExecProcedure(Proc_ReportEmployee.GetEntityProc(hubId, empId));
            return JsonUtil.Success(data);

        }
        #endregion

        #region Báo cáo báo phát nhân viên
        [HttpGet("GetReportBroadcastEmployee")]
        public JsonResult GetReportBroadcastEmployee(int? hubId = null, int? empId = null, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            var data = _unitOfWork.Repository<Proc_ReportBroadcastEmployee>()
                      .ExecProcedure(Proc_ReportBroadcastEmployee.GetEntityProc(hubId, empId, dateFrom, dateTo));
            return JsonUtil.Success(data);

        }
        #endregion

        #region Báo cáo trung chuyển
        [HttpGet("GetReportTransfer")]
        public JsonResult GetReportTransfer(bool? isAllowChild = false, int? hubId = null, DateTime? fromDate = null, DateTime? toDate = null, int? pageSize = null, int? pageNumber = null)
        {
            var data = _unitOfWork.Repository<Proc_ReportTransfer>()
                      .ExecProcedure(Proc_ReportTransfer.GetEntityProc(isAllowChild, hubId, fromDate, toDate));
            return JsonUtil.Success(data);

        }
        #endregion

        #region Báo cáo giao hàng theo bảng kê
        [HttpGet("GetReportDeliveryByListGoods")]
        public JsonResult GetReportDeliveryByListGoods(bool? isAllowChild = false, int? hubId = null, int? userId = null, DateTime? fromDate = null, DateTime? toDate = null, int? pageSize = null, int? pageNumber = null)
        {
            var data = _unitOfWork.Repository<Proc_ReportDeliveryByListGoods>()
                      .ExecProcedure(Proc_ReportDeliveryByListGoods.GetEntityProc(isAllowChild, hubId, userId, fromDate, toDate));
            return JsonUtil.Success(data);

        }
        #endregion

        #region Báo cáo tổng hợp giao nhận
        [HttpGet("GetReportPickupDelivery")]
        public JsonResult GetReportPickupDelivery(int? hubId = null, int? userId = null, DateTime? fromDate = null, DateTime? toDate = null, int? pageSize = null, int? pageNumber = null, string toProvinceIds = null)
        {
            var data = _unitOfWork.Repository<Proc_ReportByPickupDelivery>()
                      .ExecProcedure(Proc_ReportByPickupDelivery.GetEntityProc(userId, fromDate, toDate, hubId, toProvinceIds));
            return JsonUtil.Success(data);

        }
        [HttpGet("GetReportPickupDetail")]
        public JsonResult GetReportPickupDetail(int? userId = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var data = _unitOfWork.Repository<Proc_ReportPickupDeltail>()
                      .ExecProcedure(Proc_ReportPickupDeltail.GetEntityProc(userId, fromDate, toDate));
            return JsonUtil.Success(data);
        }
        [HttpGet("GetReportDeliveryDetail")]
        public JsonResult GetReportDeliveryDetail(int? userId = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var data = _unitOfWork.Repository<Proc_ReportDeliveryDeltail>()
                      .ExecProcedure(Proc_ReportDeliveryDeltail.GetEntityProc(userId, fromDate, toDate));
            return JsonUtil.Success(data);
        }
        [HttpGet("GetReportPercentDeliveryEmp")]
        public JsonResult GetReportPercentDeliveryEmp()
        {
            var currentUserId = GetCurrentUserId();
            var unitOfWordRRP = new UnitOfWorkRRP(_contextRRP);
            var data = unitOfWordRRP.Repository<Proc_ReportPercentDeliveryEmp>()
                      .ExecProcedure(Proc_ReportPercentDeliveryEmp.GetEntityProc(currentUserId));
            return JsonUtil.Success(data);
        }
        [HttpGet("GetReportDeliveryFail")]
        public JsonResult GetReportDeliveryFail(int? userId = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var data = _unitOfWork.Repository<Proc_ReportDeliveryFail>()
                      .ExecProcedure(Proc_ReportDeliveryFail.GetEntityProc(userId, fromDate, toDate));
            return JsonUtil.Success(data);
        }
        #region BÁO CÁO SẢN LƯỢNG
        [HttpGet("GetReportShipmentQuantity")]
        public JsonResult GetReportShipmentQuantity(int? fromHubId = null, DateTime? fromDate = null, DateTime? toDate = null, int? pageSize = null, int? pageNum = null)
        {
            var data = _unitOfWork.Repository<Proc_ReportShipmentQuantity>()
                      .ExecProcedure(Proc_ReportShipmentQuantity.GetEntityProc(fromHubId, fromDate, toDate, pageSize, pageNum));
            return JsonUtil.Success(data);
        }
        #endregion
        #endregion

        #region Báo cáo khiếu nại []
        [HttpGet("ReportComplain")]
        public JsonResult ReportComplain(DateTime? dateFrom = null, DateTime? dateTo = null, int? pageSize = null, int? pageNumber = null, int? salerId = null, int? handleEmpId = null, bool? isCompensation = null)
        {
            var data = _unitOfWork.Repository<Proc_ReportComplain>()
                      .ExecProcedure(Proc_ReportComplain.GetEntityProc(dateFrom, dateTo, pageNumber, pageSize, salerId, handleEmpId, isCompensation));
            return JsonUtil.Success(data);
        }
        #endregion

        #region BAO CAO HANH TRINH
        [HttpGet("ReportLadingSchedule")]
        public JsonResult ReportLadingSchedule(DateTime? fromDate = null, DateTime? toDate = null, int? fromProvinceId = null, int? toProvinceId = null, int? fromHubId = null, int? toHubId = null, int? deliveryUserId = null, int? pageNumber = 1, int? pageSize = 20)
        {
            var data = _unitOfWork.Repository<Proc_ReportLadingSchedule>()
                     .ExecProcedure(Proc_ReportLadingSchedule.GetEntityProc(pageNumber, pageSize, fromDate, toDate, fromProvinceId, toProvinceId, fromHubId, toHubId, deliveryUserId));
            return JsonUtil.Success(data);
        }
        #endregion

        #region BAO CAO NANG XUAT NV KHO
        [HttpGet("ReportEmpReceiptIssue")]
        public JsonResult ReportEmpReceiptIssue(DateTime? fromDate = null, DateTime? toDate = null, int? hubId = null, int? userId = null)
        {
            var data = _unitOfWork.Repository<Proc_ReportEmpReceiptIssue>()
                     .ExecProcedure(Proc_ReportEmpReceiptIssue.GetEntityProc(fromDate, toDate, hubId, userId));
            return JsonUtil.Success(data);
        }
        #endregion

        #region BAO CAO THU NGAY
        [HttpGet("ReportPaymentPickupUser")]
        public JsonResult ReportPaymentPickupUser(DateTime? fromDate = null, DateTime? toDate = null, int? hubId = null, int? userId = null, int? pageNumber = 1, int? pageSize = 20)
        {
            var data = _unitOfWork.Repository<Proc_ReportPaymentPickupUser>()
                     .ExecProcedure(Proc_ReportPaymentPickupUser.GetEntityProc(fromDate, toDate, hubId, userId, pageNumber, pageSize));
            return JsonUtil.Success(data);
        }
        #endregion

        #region BAO CAO NANG XUAT XE
        [HttpGet("ReportTruckTransfer")]
        public JsonResult ReportTruckTransfer(DateTime? fromDate = null, DateTime? toDate = null, int? fromProvinceId = null, int? toProvinceId = null, int? truckId = null, int? pageNumber = 1, int? pageSize = 20)
        {
            var data = _unitOfWork.Repository<Proc_ReportTruckTransfer>()
                     .ExecProcedure(Proc_ReportTruckTransfer.GetEntityProc(fromDate, toDate, fromProvinceId, toProvinceId, truckId, pageNumber, pageSize));
            return JsonUtil.Success(data);
        }
        #endregion
        #region BAO CAO NANG XUAT XE EXCEl
        [HttpPost("ReportTruckTransferExport")]
        public dynamic ReportTruckTransferExport([FromBody]TruckTransferReportExport request)
        {
            DataTable dtt = new DataTable();
            var data = _unitOfWork.Repository<Proc_ReportTruckTransfer>()
                     .ExecProcedure(Proc_ReportTruckTransfer.GetEntityProc(request.fromDate, request.toDate, request.fromProvinceId, request.toProvinceId, request.truckId, request.pageNumber, request.pageSize));
            dtt = data.ToList().ToDataTable();
            if (data.ToList().Count() == 0)
            {
                return JsonUtil.Error("Không tìm thấy dữ liệu");
            }
            var dataExport = ExportExcelPartern.ExportExcel(request.CustomExportFile, dtt);
            return File(dataExport, "application/xlsx", request.CustomExportFile.FileNameReport + ".xlsx");
        }
        #endregion
        #region BAO CAO THU HO
        [HttpGet("ReportShipmentCOD")]
        public JsonResult ReportShipmentCOD(DateTime? fromDate = null, DateTime? toDate = null, int? isReturn = null, int? tohubId = null, int? empId = null, int? pageNumber = 1, int? pageSize = 20)
        {
            var data = _unitOfWork.Repository<Proc_ReportShipmentCOD>()
                     .ExecProcedure(Proc_ReportShipmentCOD.GetEntityProc(fromDate, toDate, isReturn, tohubId, empId, pageNumber, pageSize));
            return JsonUtil.Success(data);
        }
        #endregion

        #region GIẢM GIÁ
        [HttpGet("ReportDiscountCustomer")]
        public JsonResult ReportDiscountCustomer(DateTime? dateFrom = null, DateTime? dateTo = null, int? senderId = null, int? salerId = null)
        {
            var data = _unitOfWork.Repository<Proc_ReportDiscountCustomer>()
                      .ExecProcedure(Proc_ReportDiscountCustomer.GetEntityProc(dateFrom, dateTo, senderId, salerId));
            return JsonUtil.Success(data);
        }
        #endregion

        #region Báo cáo KPI khachs hangf
        [HttpGet("ReportKPICustomer")]
        public JsonResult ReportKPICustomer(DateTime? dateFrom = null, DateTime? dateTo = null, int? hubId = null, int? senderId = null)
        {
            var data = _unitOfWork.Repository<Proc_ReportKPICustomer>()
                      .ExecProcedure(Proc_ReportKPICustomer.GetEntityProc(dateFrom, dateTo, hubId, senderId));
            return JsonUtil.Success(data);
        }
        #endregion

        #region KPI NHÂN VIÊN KINH DOANH
        [HttpGet("ReportKPIBusiness")]
        public JsonResult ReportKPIBusiness(DateTime? dateFrom = null, DateTime? dateTo = null, int? hubId = null)
        {
            var data = _unitOfWork.Repository<Proc_ReportKPIBusiness>()
                      .ExecProcedure(Proc_ReportKPIBusiness.GetEntityProc(dateFrom, dateTo, hubId));
            return JsonUtil.Success(data);
        }
        #endregion

        #region KPI KẾT QUẢ KINH DOANH
        [HttpGet("ReportResultBusiness")]
        public JsonResult ReportResultBusiness(DateTime? dateFrom = null, DateTime? dateTo = null, int? hubId = null, int? fromProvinceId = null, int? userId = null)
        {
            var data = _unitOfWork.Repository<Proc_ReportResultBusiness>()
                      .ExecProcedure(Proc_ReportResultBusiness.GetEntityProc(dateFrom, dateTo, hubId, fromProvinceId, userId));
            return JsonUtil.Success(data);
        }
        #endregion

        #region Báo cáo công nợ khách hàng
        [HttpGet("GetReportPayablesAndReceivablesByCustomer")]
        public JsonResult GetReportPayablesAndReceivablesByCustomer(int? type = null, int? customerId = null, DateTime? fromDate = null, DateTime? toDate = null, int? pageSize = null, int? pageNumber = null)
        {
            var data = _unitOfWork.Repository<Proc_ReportPayablesAndReceivablesByCustomer>()
                      .ExecProcedure(Proc_ReportPayablesAndReceivablesByCustomer.GetEntityProc(type, customerId, fromDate, toDate));
            return JsonUtil.Success(data);
        }
        #endregion

        #region Báo cáo yếu cầu phải đi lấy, đã đi lấy, vận đơn đã được phân đi giao, vận đơn đang giữ 
        [HttpGet("GetReportRequestPickupAndShipmentDelivery")]
        public JsonResult GetReportRequestPickupAndShipmentDelivery()
        {
            var countTotalRequestPickup = 0;
            var countTotalRequestPickuped = 0;
            var countTotalShipmentDelivered = 0;
            var countTotalShipmentKeeping = 0;
            var totalRequestPickup = _unitOfWork.RepositoryR<RequestShipment>().FindBy(requesst => requesst.ShipmentStatusId == 41);
            if (totalRequestPickup != null)
            {
                countTotalRequestPickup = totalRequestPickup.Count();
            }
            var totalRequestPickuped = _unitOfWork.RepositoryR<RequestShipment>().FindBy(requesst => requesst.ShipmentStatusId == 3 || requesst.ShipmentStatusId == 10);
            if (totalRequestPickuped != null)
            {
                countTotalRequestPickuped = totalRequestPickuped.Count();
            }
            var totalShipmentDelivered = _unitOfWork.RepositoryR<Shipment>().FindBy(shipment => shipment.ShipmentStatusId == 48 || shipment.ShipmentStatusId == 51);
            if (totalShipmentDelivered != null)
            {
                countTotalShipmentDelivered = totalShipmentDelivered.Count();
            }
            var totalShipmentKeeping = _unitOfWork.RepositoryR<Shipment>().FindBy(shipment =>
                shipment.ShipmentStatusId == 3 ||
                shipment.ShipmentStatusId == 8 ||
                shipment.ShipmentStatusId == 11 ||
                shipment.ShipmentStatusId == 13 ||
                shipment.ShipmentStatusId == 28 ||
                shipment.ShipmentStatusId == 31 ||
                shipment.ShipmentStatusId == 31
                );
            if (totalShipmentKeeping != null)
            {
                countTotalShipmentKeeping = totalShipmentKeeping.Count();
            }
            var data = new ReportRequestPickupAndShipmentDeliveryViewModel(
                countTotalRequestPickup,
                countTotalRequestPickuped,
                countTotalShipmentDelivered,
                countTotalShipmentKeeping
            );
            return JsonUtil.Success(data);
        }
        #endregion

        #region Báo cáo phát thành công 
        [HttpGet("GetReportDeliveryComplete")]
        public JsonResult GetReportDeliveryComplete(DateTime? date, int? month, int? year)
        {
            var curentUserId = GetCurrentUserId();
            var data = _unitOfWork.Repository<Proc_ReportPickupDelivery>()
                     .ExecProcedureSingle(Proc_ReportPickupDelivery.GetEntityProc(curentUserId, date));
            return JsonUtil.Success(data);
        }
        #endregion

        #region Báo cáo In vận đơn 
        [HttpGet("GetReportPrintShipment")]
        public ResponseViewModel GetReportPrintShipment(int? hubId = null, int? empId = null, int? typePrintId = null, DateTime? dateFrom = null, DateTime? dateTo = null, string searchText = null, int? pageSize = null, int? pageNumber = null)
        {
            var data = _unitOfWork.Repository<Proc_ReportPrintShipment>()
                      .ExecProcedure(Proc_ReportPrintShipment.GetEntityProc(hubId, empId, typePrintId, dateFrom, dateTo, searchText));
            var totalCount = data.Count();
            var result = PaginatorUtil.GetDataPaginator(data, pageSize, pageNumber);
            return ResponseViewModel.CreateSuccess(result, null, totalCount);
        }
        #endregion

        #region Báo cáo In vận đơn 
        [HttpGet("GetHistoryPrintShipmentId")]
        public ResponseViewModel GetHistoryPrintShipmentId(int shipmentId, int? hubId, int? empId, int? typePrintId, DateTime? dateFrom, DateTime? dateTo, int? pageSize = null, int? pageNumber = null)
        {
            var data = _unitOfWork.Repository<Proc_GetHistoryPrintShipmentId>()
                      .ExecProcedure(Proc_GetHistoryPrintShipmentId.GetEntityProc(shipmentId, hubId, empId, typePrintId, dateFrom, dateTo));
            var totalCount = data.Count();
            var result = PaginatorUtil.GetDataPaginator(data, pageSize, pageNumber);
            return ResponseViewModel.CreateSuccess(result, null, totalCount);
        }
        #endregion

        #region Báo cáo Hủy vận đơn 
        [HttpGet("GetReportCancelShipment")]
        public ResponseViewModel GetReportCancelShipment(int? hubId = null, int? empId = null, int? senderId = null, DateTime? dateFrom = null, DateTime? dateTo = null, string searchText = null, int? pageSize = null, int? pageNumber = null)
        {
            var data = _unitOfWork.Repository<Proc_ReportCancelShipment>()
                      .ExecProcedure(Proc_ReportCancelShipment.GetEntityProc(hubId, empId, senderId, dateFrom, dateTo, searchText));
            var totalCount = data.Count();
            var result = PaginatorUtil.GetDataPaginator(data, pageSize, pageNumber);
            return ResponseViewModel.CreateSuccess(result, null, totalCount);
        }
        #endregion

        #region BAO CAO TRE DEADLINE 
        [HttpGet("GetReportDeadline")]
        public JsonResult GetReportDeadline(DateTime? fromDate = null, DateTime? toDate = null, int? fromProvinceId = null, int? toProvinceId = null, int? fromHubId = null, int? toHubId = null, int? deliveryUserId = null, int? senderId = null, int? pageNumber = 1, int? pageSize = 20)
        {
            var data = _unitOfWork.Repository<Proc_ReportDeadline>()
                      .ExecProcedure(Proc_ReportDeadline.GetEntityProc(fromDate, toDate, fromProvinceId, toProvinceId, fromHubId, toHubId, deliveryUserId, senderId, pageNumber, pageSize));
            return JsonUtil.Success(data);
        }
        #endregion

        #region BAO CAO VAN DON UU TIEN
        [HttpGet("GetReportPrioritize")]
        public JsonResult GetReportPrioritize(DateTime? fromDate = null, DateTime? toDate = null, int? fromProvinceId = null, int? toProvinceId = null, int? fromHubId = null, int? toHubId = null, int? deliveryUserId = null, int? senderId = null, int? pageNumber = 1, int? pageSize = 20)
        {
            var data = _unitOfWork.Repository<Proc_ReportPrioritize>()
                      .ExecProcedure(Proc_ReportPrioritize.GetEntityProc(fromDate, toDate, fromProvinceId, toProvinceId, fromHubId, toHubId, deliveryUserId, senderId, pageNumber, pageSize));
            return JsonUtil.Success(data);
        }
        #endregion

        #region BAO CAO SỰ CỐ
        [HttpGet("GetReportIncidents")]
        public JsonResult GetReportIncidents(DateTime? dateFrom = null, DateTime? dateTo = null, int? customerId = null, int? incidentEmpId = null, int? handleEmpId = null, bool? isCompensation = null, int? pageNumber = 1, int? pageSize = 20)
        {
            var data = _unitOfWork.Repository<Proc_ReportIncidents>()
                      .ExecProcedure(Proc_ReportIncidents.GetEntityProc(dateFrom, dateTo, customerId, incidentEmpId, handleEmpId, isCompensation, pageNumber, pageSize));
            return JsonUtil.Success(data);
        }
        #endregion

        #region Báo cáo công nợ cước chi tiết theo khách hàng [PCSPOST]
        [HttpGet("GetReportDebtPriceDetailByCustomer")]
        public ResponseOfReportViewModel GetReportDebtPriceDetailByCustomer(int? type = null, int? customerId = null, DateTime? dateFrom = null, DateTime? dateTo = null, int? pageSize = null, int? pageNumber = null)
        {
            var data = _unitOfWork.Repository<Proc_ReportDebtPriceDetailByCustomer>()
                      .ExecProcedure(Proc_ReportDebtPriceDetailByCustomer.GetEntityProc(type, customerId, dateFrom, dateTo));
            var totalCount = data.Count();
            var sumOfReport = new SumOfReport();
            if (totalCount == 0)
            {
                sumOfReport.SumOfTotalBeforeByReportDebtPriceDetailCustomer = 0;
                sumOfReport.SumOfTotalPriceByReportDebtPriceDetailCustomer = 0;
                sumOfReport.SumOfTotalPricePaidByReportDebtPriceDetailCustomer = 0;
                sumOfReport.SumOfTotalAfterPaidByReportDebtPriceDetailCustomer = 0;
            }
            else
            {
                sumOfReport.SumOfTotalBeforeByReportDebtPriceDetailCustomer = data.Sum(rp => rp.TotalBefore);
                sumOfReport.SumOfTotalPriceByReportDebtPriceDetailCustomer = data.Sum(rp => rp.TotalPrice);
                sumOfReport.SumOfTotalPricePaidByReportDebtPriceDetailCustomer = data.Sum(rp => rp.TotalPricePaid);
                sumOfReport.SumOfTotalAfterPaidByReportDebtPriceDetailCustomer = data.Sum(rp => rp.TotalAfter);
            }
            var result = PaginatorUtil.GetDataPaginator(data, pageSize, pageNumber);
            return ResponseOfReportViewModel.CreateSuccess(result, null, totalCount, sumOfReport);
        }
        #endregion

        #region Báo cáo công nợ cước chi tiết theo khách hàng - chi tiết [PCSPOST]
        [HttpGet("GetReportDebtPriceDetailByCustomerDetail")]
        public ResponseOfReportViewModel GetReportDebtPriceDetailByCustomerDetail(int customerId, DateTime? dateFrom = null, DateTime? dateTo = null, int? pageSize = 20, int? pageNumber = 1)
        {
            var data = _unitOfWork.Repository<Proc_ReportDebtPriceDetailByCustomerDetail>()
                      .ExecProcedure(Proc_ReportDebtPriceDetailByCustomerDetail.GetEntityProc(customerId, dateFrom, dateTo));
            var totalCount = data.Count();
            var sumOfReport = new SumOfReport();
            if (totalCount == 0)
            {
                sumOfReport.SumOfTotalBeforeByReportDebtPriceDetailCustomerDetail = 0;
                sumOfReport.SumOfTotalCreditBalanceBeforeByReportDebtPriceDetailCustomerDetail = 0;
                sumOfReport.SumOfTotalPriceByReportDebtPriceDetailCustomerDetail = 0;
                sumOfReport.SumOfTotalPricePaidByReportDebtPriceDetailCustomerDetail = 0;
            }
            else
            {
                sumOfReport.SumOfTotalBeforeByReportDebtPriceDetailCustomerDetail = data.Sum(rp => rp.TotalPrice);
                sumOfReport.SumOfTotalCreditBalanceBeforeByReportDebtPriceDetailCustomerDetail = data.Sum(rp => rp.TotalPrice);
                sumOfReport.SumOfTotalPriceByReportDebtPriceDetailCustomerDetail = data.Sum(rp => rp.TotalPrice);
                sumOfReport.SumOfTotalPricePaidByReportDebtPriceDetailCustomerDetail = data.Sum(rp => rp.TotalPrice);
            }
            var result = PaginatorUtil.GetDataPaginator(data, pageSize, pageNumber);
            return ResponseOfReportViewModel.CreateSuccess(result, null, totalCount, sumOfReport);
        }
        #endregion

        #region Báo cáo công nợ cod chi tiết theo khách hàng [PCSPOST]
        [HttpGet("GetReportDebtCODDetailByCustomer")]
        public ResponseOfReportViewModel GetReportDebtCODDetailByCustomer(int? type = null, int? customerId = null, DateTime? dateFrom = null, DateTime? dateTo = null, int? pageSize = null, int? pageNumber = null)
        {
            var data = _unitOfWork.Repository<Proc_ReportDebtCODDetailByCustomer>()
                      .ExecProcedure(Proc_ReportDebtCODDetailByCustomer.GetEntityProc(type, customerId, dateFrom, dateTo));
            var totalCount = data.Count();
            var sumOfReport = new SumOfReport();
            if (totalCount == 0)
            {
                sumOfReport.SumOfTotalCODByReportDebtCODDetailCustomer = 0;
                sumOfReport.SumOfTotalCODReturnByReportDebtCODDetailCustomer = 0;
                sumOfReport.SumOfTotalCODChargedByReportDebtCODDetailCustomer = 0;
                sumOfReport.SumOfTotalCODNotChargedByReportDebtCODDetailCustomer = 0;
                sumOfReport.SumOfTotalCODPaidByReportDebtCODDetailCustomer = 0;
                sumOfReport.SumOfTotalCODNotPaidByReportDebtCODDetailCustomer = 0;
                sumOfReport.SumOfTotalPriceByReportDebtCODDetailCustomer = 0;
                sumOfReport.SumOfTotalPricePaidByReportDebtCODDetailCustomer = 0;
                sumOfReport.SumOfTotalPriceNotPaidByReportDebtCODDetailCustomer = 0;

            }
            else
            {
                sumOfReport.SumOfTotalCODByReportDebtCODDetailCustomer = data.Sum(rp => rp.TotalCOD);
                sumOfReport.SumOfTotalCODReturnByReportDebtCODDetailCustomer = data.Sum(rp => rp.TotalCOD);
                sumOfReport.SumOfTotalCODChargedByReportDebtCODDetailCustomer = data.Sum(rp => rp.TotalCOD);
                sumOfReport.SumOfTotalCODNotChargedByReportDebtCODDetailCustomer = data.Sum(rp => rp.TotalCOD);
                sumOfReport.SumOfTotalCODPaidByReportDebtCODDetailCustomer = data.Sum(rp => rp.TotalCOD);
                sumOfReport.SumOfTotalCODNotPaidByReportDebtCODDetailCustomer = data.Sum(rp => rp.TotalCOD);
                sumOfReport.SumOfTotalPriceByReportDebtCODDetailCustomer = data.Sum(rp => rp.TotalPrice);
                sumOfReport.SumOfTotalPricePaidByReportDebtCODDetailCustomer = data.Sum(rp => rp.TotalPricePaid);
                sumOfReport.SumOfTotalPriceNotPaidByReportDebtCODDetailCustomer = data.Sum(rp => rp.TotalPriceNotPaid);
            }
            var result = PaginatorUtil.GetDataPaginator(data, pageSize, pageNumber);
            return ResponseOfReportViewModel.CreateSuccess(result, null, totalCount, sumOfReport);
        }
        #endregion

        #region Báo cáo công nợ cod chi tiết theo khách hàng - chi tiết [PCSPOST]
        [HttpGet("GetReportDebtCODDetailByCustomerDetail")]
        public ResponseOfReportViewModel GetReportDebtCODDetailByCustomerDetail(int customerId, DateTime? dateFrom = null, DateTime? dateTo = null, int? pageSize = 20, int? pageNumber = 1)
        {
            var data = _unitOfWork.Repository<Proc_ReportDebtCODDetailByCustomerDetail>()
                      .ExecProcedure(Proc_ReportDebtCODDetailByCustomerDetail.GetEntityProc(customerId, dateFrom, dateTo));
            var totalCount = data.Count();
            var sumOfReport = new SumOfReport();
            if (totalCount == 0)
            {
                sumOfReport.SumOfTotalCODByReportDebtCODDetailCustomerDetail = 0;
                sumOfReport.SumOfTotalCODReturnByReportDebtCODDetailCustomerDetail = 0;
                sumOfReport.SumOfTotalCODChargedByReportDebtCODDetailCustomerDetail = 0;
                sumOfReport.SumOfTotalCODNotChargedByReportDebtCODDetailCustomerDetail = 0;
                sumOfReport.SumOfTotalCODPaidByReportDebtCODDetailCustomerDetail = 0;
                sumOfReport.SumOfTotalCODNotPaidByReportDebtCODDetailCustomerDetail = 0;
                sumOfReport.SumOfTotalPriceByReportDebtCODDetailCustomerDetail = 0;
                sumOfReport.SumOfTotalPricePaidByReportDebtCODDetailCustomerDetail = 0;
                sumOfReport.SumOfTotalPriceNotPaidByReportDebtCODDetailCustomerDetail = 0;

            }
            else
            {
                sumOfReport.SumOfTotalCODByReportDebtCODDetailCustomerDetail = data.Sum(rp => rp.COD);
                sumOfReport.SumOfTotalCODReturnByReportDebtCODDetailCustomerDetail = data.Sum(rp => rp.COD);
                sumOfReport.SumOfTotalCODChargedByReportDebtCODDetailCustomerDetail = data.Sum(rp => rp.COD);
                sumOfReport.SumOfTotalCODNotChargedByReportDebtCODDetailCustomerDetail = data.Sum(rp => rp.COD);
                sumOfReport.SumOfTotalCODPaidByReportDebtCODDetailCustomerDetail = data.Sum(rp => rp.COD);
                sumOfReport.SumOfTotalCODNotPaidByReportDebtCODDetailCustomerDetail = data.Sum(rp => rp.COD);
                sumOfReport.SumOfTotalPriceByReportDebtCODDetailCustomerDetail = data.Sum(rp => rp.TotalPrice);
                sumOfReport.SumOfTotalPricePaidByReportDebtCODDetailCustomerDetail = data.Sum(rp => rp.TotalPrice);
                sumOfReport.SumOfTotalPriceNotPaidByReportDebtCODDetailCustomerDetail = data.Sum(rp => rp.TotalPrice);
            }
            var result = PaginatorUtil.GetDataPaginator(data, pageSize, pageNumber);
            return ResponseOfReportViewModel.CreateSuccess(result, null, totalCount, sumOfReport);
        }
        #endregion

        #region Báo cáo bảng kê cước chi tiết theo khách hàng [PCSPOST]
        [HttpGet("GetReportListGoodsPriceDetailByCustomer")]
        public ResponseOfReportViewModel GetReportListGoodsPriceDetailByCustomer(int? type = null, int? customerId = null, DateTime? dateFrom = null, DateTime? dateTo = null, int? pageSize = null, int? pageNumber = null)
        {
            var data = _unitOfWork.Repository<Proc_ReportListGoodsPriceDetailByCustomer>()
                      .ExecProcedure(Proc_ReportListGoodsPriceDetailByCustomer.GetEntityProc(type, customerId, dateFrom, dateTo));
            var totalCount = data.Count();
            var sumOfReport = new SumOfReport();
            if (totalCount == 0)
            {
                sumOfReport.SumOfTotalPriceByReportListGoodsPriceCustomer = 0;
            }
            else
            {
                sumOfReport.SumOfTotalPriceByReportListGoodsPriceCustomer = data.Sum(rp => rp.TotalPrice);
            }
            var result = PaginatorUtil.GetDataPaginator(data, pageSize, pageNumber);
            return ResponseOfReportViewModel.CreateSuccess(result, null, totalCount, sumOfReport);
        }
        #endregion

        #region DASHBOARD
        [HttpGet("GetDashboardPickup")]
        public JsonResult GetDashboardPickup(DateTime? fromDate = null, DateTime? toDate = null, int? hubId = null)
        {
            var data = _unitOfWork.Repository<Proc_DashboardPickup>()
                      .ExecProcedureSingle(Proc_DashboardPickup.GetEntityProc(fromDate, toDate, hubId));
            return JsonUtil.Success(data);
        }

        [HttpGet("GetDashboardTransfer")]
        public JsonResult GetDashboardTransfer(DateTime? fromDate = null, DateTime? toDate = null, int? hubId = null)
        {
            var data = _unitOfWork.Repository<Proc_DashboardTransfer>()
                      .ExecProcedureSingle(Proc_DashboardTransfer.GetEntityProc(fromDate, toDate, hubId));
            return JsonUtil.Success(data);
        }

        [HttpGet("GetDashboardDeliveryAndReturn")]
        public JsonResult GetDashboardDeliveryAndReturn(DateTime? fromDate = null, DateTime? toDate = null, int? hubId = null)
        {
            var data = _unitOfWork.Repository<Proc_DashboardDeliveryAndReturn>()
                      .ExecProcedureSingle(Proc_DashboardDeliveryAndReturn.GetEntityProc(fromDate, toDate, hubId));
            return JsonUtil.Success(data);
        }

        [HttpGet("GetDashboardService")]
        public JsonResult GetDashboardService(DateTime? fromDate = null, DateTime? toDate = null, int? hubId = null)
        {
            var data = _unitOfWork.Repository<Proc_DashboardService>()
                      .ExecProcedureSingle(Proc_DashboardService.GetEntityProc(fromDate, toDate, hubId));
            return JsonUtil.Success(data);
        }
        #endregion

        #region LICH SU NHAP KHO CHI TIET
        [HttpGet("GetReportListGoodsShipment")]
        public JsonResult GetReportListGoodsShipment(int? typeId = null, int? createdByHubId = null, int? fromHubId = null, int? toHubId = null,
            int? userId = null, int? statusId = null, int? transportTypeId = null, int? tplId = null, int? senderId = null, DateTime? dateFrom = null,
            DateTime? dateTo = null, string searchText = null, int? pageNumber = 1, int? pageSize = 20)
        {
            var data = _unitOfWork.Repository<Proc_ReportListGoodsShipment>()
                      .ExecProcedure(Proc_ReportListGoodsShipment.GetEntityProc(typeId, createdByHubId, fromHubId, toHubId,
            userId, statusId, transportTypeId, tplId, senderId, dateFrom, dateTo, searchText, pageNumber, pageSize));
            return JsonUtil.Success(data);
        }
        #endregion

        #region Báo cáo tiền nhân viên đã thu
        [HttpGet("GetReportEmployeeCollected")]
        public JsonResult GetReportEmployeeCollected(int userId, int? pageSize = 10, int? pageNumber = 1)
        {
            var data = _unitOfWork.Repository<Proc_ReportEmployeeCollected>()
                      .ExecProcedure(Proc_ReportEmployeeCollected.GetEntityProc(userId, pageSize, pageNumber));
            return JsonUtil.Success(data);

        }
        #endregion

        #region Báo cáo tiền nhân viên đang thu
        [HttpGet("GetReportEmployeeCollecting")]
        public JsonResult GetReportEmployeeCollecting(int userId, int? pageSize = 10, int? pageNumber = 1)
        {
            var data = _unitOfWork.Repository<Proc_ReportEmployeeCollecting>()
                      .ExecProcedure(Proc_ReportEmployeeCollecting.GetEntityProc(userId, pageSize, pageNumber));
            return JsonUtil.Success(data);

        }
        #endregion

        #region 
        [HttpPost("GetReportCODConfirm")]
        public JsonResult GetReportCODConfirm([FromBody]ReportCODConfirmViewModel filterViewModel)
        {
            var data = _unitOfWork.Repository<Proc_ReportCODConfirm>()
                      .ExecProcedure(Proc_ReportCODConfirm.GetEntityProc(
                          filterViewModel.FromHubId,
                          filterViewModel.ToHubId,
                          filterViewModel.CurrentHubId,
                          filterViewModel.ServiceId,
                          filterViewModel.ShipmentStatusId,
                          filterViewModel.SenderId,
                          filterViewModel.CurrentUserId,
                          filterViewModel.AccountingAccountId,
                          filterViewModel.FromProvinceId,
                          filterViewModel.ToProvinceId,
                          filterViewModel.DateFrom,
                          filterViewModel.DateTo,
                          filterViewModel.PageNumber,
                          filterViewModel.PageSize,
                          filterViewModel.SearchText
                          ));
            return JsonUtil.Success(data);
        }

        [HttpPost("GetReportCODConfirmExcel")]
        public dynamic GetReportCODConfirmExcel([FromBody]ReportCODConfirmViewModel filterViewModel)
        {
            DataTable dtt = new DataTable();
            var data = _unitOfWork.Repository<Proc_ReportCODConfirm>()
                      .ExecProcedure(Proc_ReportCODConfirm.GetEntityProc(
                          filterViewModel.FromHubId,
                          filterViewModel.ToHubId,
                          filterViewModel.CurrentHubId,
                          filterViewModel.ServiceId,
                          filterViewModel.ShipmentStatusId,
                          filterViewModel.SenderId,
                          filterViewModel.CurrentUserId,
                          filterViewModel.AccountingAccountId,
                          filterViewModel.FromProvinceId,
                          filterViewModel.ToProvinceId,
                          filterViewModel.DateFrom,
                          filterViewModel.DateTo,
                          filterViewModel.PageNumber,
                          filterViewModel.PageSize,
                          filterViewModel.SearchText
                          ));
            dtt = data.ToDataTable();
            var bytearray = ExportExcelPartern.ExportExcel(filterViewModel.CustomExportFile, dtt);
            return File(bytearray, "application/xlsx", filterViewModel.CustomExportFile.FileNameReport + ".xlsx");
        }
        #endregion

        #region Báo cáo phan tich kinh doanh
        [HttpGet("GetReportByCus")]
        public JsonResult GetReportByCus(int? senderId = null,
            DateTime? dateFrom = null, DateTime? dateTo = null, string listProvinceIds = null, string listDeliveryIds = null,
            int? pageNumber = 1, int? pageSize = 20)
        {
            var data = _unitOfWork.Repository<Proc_ReportByCus>()
                      .ExecProcedure(Proc_ReportByCus.GetEntityProc(senderId, dateFrom, dateTo, listProvinceIds, listDeliveryIds));
            return JsonUtil.Success(data);
        }
        #endregion
        [HttpPost("GetReportByCusExportExcel")]
        public dynamic GetReportByCusExportExcel([FromBody]GetReportByCusExportExcelViewModel filterViewModel)
        {
            DataTable dtt = new DataTable();
            var data = _unitOfWork.Repository<Proc_ReportByCus>()
                      .ExecProcedure(Proc_ReportByCus.GetEntityProc(filterViewModel.senderId, filterViewModel.dateFrom, filterViewModel.dateTo, filterViewModel.listProvinceIds, filterViewModel.listDeliveryIds));
            var ListData = data.ToList();
            dtt = data.ToDataTable();
            var bytearray = ExportExcelPartern.ExportExcel(filterViewModel.CustomExportFile, dtt);
            return File(bytearray, "application/xlsx", filterViewModel.CustomExportFile.FileNameReport + ".xlsx");
        }
        #region Báo cáo daonh thu thang
        [HttpGet("GetReportByRevenueMonth")]
        public JsonResult GetReportByRevenueMonth(DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            var data = _unitOfWork.Repository<Proc_ReportByRevenueMonth>()
                      .ExecProcedure(Proc_ReportByRevenueMonth.GetEntityProc(dateFrom, dateTo));
            return JsonUtil.Success(data);
        }
        #endregion

        [HttpPost("GetReportByRevenueMonthExportExcel")]
        public dynamic GetReportByRevenueMonthExportExcel([FromBody]GetReportByRevenueMonthExportExcelViewModel filterViewModel)
        {
            DataTable dtt = new DataTable();
            var data = _unitOfWork.Repository<Proc_ReportByRevenueMonth>()
                      .ExecProcedure(Proc_ReportByRevenueMonth.GetEntityProc(filterViewModel.dateFrom, filterViewModel.dateTo));
            var ListData = data.ToList();
            dtt = data.ToDataTable();
            var bytearray = ExportExcelPartern.ExportExcel(filterViewModel.CustomExportFile, dtt);
            return File(bytearray, "application/xlsx", filterViewModel.CustomExportFile.FileNameReport + ".xlsx");

        }

        #region Báo cáo daonh thu nam
        [HttpGet("GetReportByRevenueYear")]
        public JsonResult GetReportByRevenueYear(int year)
        {
            int gYear = DateTime.Now.Year;
            if (year > gYear || year < (gYear - 10))
            {
                return JsonUtil.Error(string.Format("Năm lấy báo cáo không hợp lệ '{0}'", year));
            }
            DateTime dateGet = DateTime.Parse(string.Format("{0}-01-01 00:00:00", year));
            var data = _unitOfWork.Repository<Proc_ReportByRevenueYear>()
                      .ExecProcedure(Proc_ReportByRevenueYear.GetEntityProc(dateGet));
            return JsonUtil.Success(data);
        }
        #endregion

        [HttpPost("GetReportByRevenueYearExportExcel")]
        public dynamic GetReportByRevenueYearExportExcel([FromBody]GetReportByRevenueYearExportExcelViewModel filterViewModel)
        {
            DataTable dtt = new DataTable();
            int gYear = DateTime.Now.Year;
            if (filterViewModel.year > gYear || filterViewModel.year < (gYear - 10))
            {
                return JsonUtil.Error(string.Format("Năm lấy báo cáo không hợp lệ '{0}'", filterViewModel.year));
            }
            DateTime dateGet = DateTime.Parse(string.Format("{0}-01-01 00:00:00", filterViewModel.year));
            var data = _unitOfWork.Repository<Proc_ReportByRevenueYear>()
                      .ExecProcedure(Proc_ReportByRevenueYear.GetEntityProc(dateGet));
            var ListData = data.ToList();
            dtt = data.ToDataTable();
            var bytearray = ExportExcelPartern.ExportExcel(filterViewModel.CustomExportFile, dtt);
            return File(bytearray, "application/xlsx", filterViewModel.CustomExportFile.FileNameReport + ".xlsx");
        }

        #region Báo cáo giam sat nv
        [HttpGet("GetReportHandleEmployee")]
        public JsonResult GetReportHandleEmployee(int? hubId = null, int? userId = null, bool? isGroupEmp = null, int? groupStatusId = null,
            int? timeCompare = null, DateTime? dateFrom = null, DateTime? dateTo = null, int? pageNumber = null, int? pageSize = null)
        {

            var data = _unitOfWork.Repository<Proc_ReportHandleEmployee>()
                      .ExecProcedure(Proc_ReportHandleEmployee.GetEntityProc(hubId, userId, isGroupEmp, groupStatusId,
            timeCompare, dateFrom, dateTo, pageNumber, pageSize));
            return JsonUtil.Success(data);
        }
        #endregion

        #region Báo cáo cập nhật thông tin nhận
        [HttpPost("GetReportUpdateReceiveInformation")]
        public JsonResult GetReportUpdateReceiveInformation([FromBody]ReportEmployeesUpdateAndPaymentFilterViewModel filterViewModel)
        {
            var data = _unitOfWork.Repository<Proc_ReportUpdateReceiveInformation>()
                      .ExecProcedure(Proc_ReportUpdateReceiveInformation.GetEntityProc(
                          filterViewModel.dateFrom,
                          filterViewModel.dateTo,
                          filterViewModel.hubId,
                          filterViewModel.empId));
            return JsonUtil.Success(data);
        }
        #endregion

        #region Báo cáo nộp tiền nhân viên
        [HttpPost("GetReportPaymentEmployees")]
        public JsonResult GetReportPaymentEmployees([FromBody]ReportEmployeesUpdateAndPaymentFilterViewModel filterViewModel)
        {
            var data = _unitOfWork.Repository<Proc_ReportPaymentEmployees>()
                      .ExecProcedure(Proc_ReportPaymentEmployees.GetEntityProc(
                          filterViewModel.dateFrom,
                          filterViewModel.dateTo,
                          filterViewModel.hubId,
                          filterViewModel.empId));
            return JsonUtil.Success(data);
        }
        #endregion

        #region Báo cáo cập nhật thông tin nhận Excel
        [HttpPost("GetReportUpdateReceiveInformationExportExcel")]
        public dynamic GetReportUpdateReceiveInformationExportExcel([FromBody]ReportEmployeesUpdateAndPaymentFilterViewModel filterViewModel)
        {
            DataTable dtt = new DataTable();
            var data = _unitOfWork.Repository<Proc_ReportUpdateReceiveInformation>()
                      .ExecProcedure(Proc_ReportUpdateReceiveInformation.GetEntityProc(
                          filterViewModel.dateFrom,
                          filterViewModel.dateTo,
                          filterViewModel.hubId,
                          filterViewModel.empId));
            var ListData = data.ToList();
            dtt = data.ToDataTable();
            var bytearray = ExportExcelPartern.ExportExcel(filterViewModel.CustomExportFile, dtt);
            return File(bytearray, "application/xlsx", filterViewModel.CustomExportFile.FileNameReport + ".xlsx");
        }
        #endregion

        #region Báo cáo nộp tiền nhân viên Excel
        [HttpPost("GetReportPaymentEmployeesExportExcel")]
        public dynamic GetReportPaymentEmployeesExportExcel([FromBody]ReportEmployeesUpdateAndPaymentFilterViewModel filterViewModel)
        {
            DataTable dtt = new DataTable();
            var data = _unitOfWork.Repository<Proc_ReportPaymentEmployees>()
                      .ExecProcedure(Proc_ReportPaymentEmployees.GetEntityProc(
                          filterViewModel.dateFrom,
                          filterViewModel.dateTo,
                          filterViewModel.hubId,
                          filterViewModel.empId));
            var ListData = data.ToList();
            dtt = data.ToDataTable();
            var bytearray = ExportExcelPartern.ExportExcel(filterViewModel.CustomExportFile, dtt);
            return File(bytearray, "application/xlsx", filterViewModel.CustomExportFile.FileNameReport + ".xlsx");
        }
        #endregion

        #region Báo cáo chỉnh sửa version
        [HttpPost("GetReportShipmentVersion")]
        public JsonResult GetReportShipmentVersion([FromBody]ReportShipmentVersionFilterViewModel filterViewModel)
        {

            var data = _unitOfWork.Repository<Proc_ReportShipmentVersion>()
                      .ExecProcedure(Proc_ReportShipmentVersion.GetEntityProc(
                          filterViewModel.DateFrom,
                          filterViewModel.DateTo,
                          filterViewModel.HubId,
                          filterViewModel.SenderId,
                          filterViewModel.EmpId,
                          filterViewModel.ShipmentId,
                          filterViewModel.ShipmentNumber,
                          filterViewModel.PageNumber,
                          filterViewModel.PageSize,
                          filterViewModel.IsSortByCOD
                          ));
            return JsonUtil.Success(data);
        }
        #endregion

        #region Xuất Excel báo cáo chỉnh sửa version
        [HttpPost("GetReportShipmentVersionExcel")]
        public async Task<dynamic> GetReportShipmentVersionExcel([FromBody]ReportShipmentVersionFilterViewModel filterViewModel)
        {
            var currentUserId = this.GetCurrentUserId();

            int? PageSize = filterViewModel.PageSize;

            DataTable dtt;

            DataTable DttClone = new DataTable();

            try
            {
                var unitOfWordRRP = new UnitOfWorkRRP(_contextRRP);
                var data = await unitOfWordRRP.Repository<Proc_ReportShipmentVersion>()
                          .GetReportListShipmentExport(Proc_ReportShipmentVersion.GetEntityProc(
                          filterViewModel.DateFrom,
                          filterViewModel.DateTo,
                          filterViewModel.HubId,
                          filterViewModel.SenderId,
                          filterViewModel.EmpId,
                          filterViewModel.ShipmentId,
                          filterViewModel.ShipmentNumber,
                          filterViewModel.PageNumber,
                          filterViewModel.PageSize,
                          filterViewModel.IsSortByCOD
                ));
                if (!Util.IsNull(data))
                {
                    var list = data.ToList();
                    dtt = data.ToDataTable();
                    int TotalCount = list[0].TotalCount.Value;
                    var ischecktime = ExportExcelPartern.CheckTimeExport();


                    if (ischecktime == false)
                    {
                        if (ExportExcelPartern.CheckTotalData(TotalCount))
                        {
                            return JsonUtil.Error("Get data error!!!");
                            //return null;
                        }
                    }
                    var datatable = await unitOfWordRRP.Repository<Proc_ReportShipmentVersion>()
                     .GetReportListShipmentExport(Proc_ReportShipmentVersion.GetEntityProc(
                          filterViewModel.DateFrom,
                          filterViewModel.DateTo,
                          filterViewModel.HubId,
                          filterViewModel.SenderId,
                          filterViewModel.EmpId,
                          filterViewModel.ShipmentId,
                          filterViewModel.ShipmentNumber,
                          filterViewModel.PageNumber,
                          filterViewModel.PageSize,
                          filterViewModel.IsSortByCOD));
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

        #region Báo cáo chi phí
        [HttpPost("GetReportExpenseReceiveMoney")]
        public JsonResult GetReportExpenseReceiveMoney([FromBody]ReportExpenseReceiveMoneyFilterViewModel filterViewModel)
        {
            var data = _unitOfWork.Repository<Proc_ReportExpenseReceiveMoney>()
                      .ExecProcedure(Proc_ReportExpenseReceiveMoney.GetEntityProc(
                          filterViewModel.DateFrom,
                          filterViewModel.DateTo,
                          filterViewModel.HubId,
                          filterViewModel.UserId,
                          filterViewModel.AccountingAccountId,
                          filterViewModel.PageNumber,
                          filterViewModel.PageSize
                          ));
            return JsonUtil.Success(data);
        }
        #endregion


        #region Xuất Excel báo cáo chi phí
        [HttpPost("GetReportExpenseReceiveMoneyExcel")]
        public async Task<dynamic> GetReportExpenseReceiveMoneyExcel([FromBody]ReportExpenseReceiveMoneyFilterViewModel filterViewModel)
        {
            DataTable dtt = new DataTable();
            var data = _unitOfWork.Repository<Proc_ReportExpenseReceiveMoney>()
                          .ExecProcedure(Proc_ReportExpenseReceiveMoney.GetEntityProc(
                          filterViewModel.DateFrom,
                          filterViewModel.DateTo,
                          filterViewModel.HubId,
                          filterViewModel.UserId,
                          filterViewModel.AccountingAccountId,
                          filterViewModel.PageNumber,
                          filterViewModel.PageSize));
            var ListData = data.ToList();
            dtt = ListData.ToDataTable();
            if (data.ToList().Count() == 0)
            {
                return JsonUtil.Error("Không tìm thấy dữ liệu");
            }
            var dataExport = ExportExcelPartern.ExportExcel(filterViewModel.CustomExportFile, dtt);
            return File(dataExport, "application/xlsx", filterViewModel.CustomExportFile.FileNameReport + ".xlsx");
        }
        #endregion


        [HttpGet("GetKPIFullLadingReport")]
        public JsonResult GetKPIFullLadingReport(GetKPIFullLadingReportViewModel model)
        {
            try
            {
                var res = _unitOfWork.Repository<Proc_GetKPIFullLadingReport>()
                       .ExecProcedure(Proc_GetKPIFullLadingReport.GetEntityProc(model.centerHubId, model.poHubId, model.stationId, model.customerId, model.fromDate, model.toDate,
                       model.searchText, model.pageNumber, model.pageSize, model.isSortDescending));


                return JsonUtil.Success(res);
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }
        }
        //
        [HttpPost("GetKPIFullLadingReportExportExcel")]
        public dynamic GetKPIFullLadingReportExportExcel([FromBody]GetKPIFullLadingReportViewModel model)
        {
            DataTable dtt = new DataTable();
            var data = _unitOfWork.Repository<Proc_GetKPIFullLadingReport>()
                     .ExecProcedure(Proc_GetKPIFullLadingReport.GetEntityProc(model.centerHubId, model.poHubId, model.stationId, model.customerId, model.fromDate, model.toDate,
                       model.searchText, model.pageNumber, model.pageSize, model.isSortDescending));
            var ListData = data.ToList();
            dtt = ListData.ToDataTable();
            if (data.ToList().Count() == 0)
            {
                return JsonUtil.Error("Không tìm thấy dữ liệu");
            }
            var dataExport = ExportExcelPartern.ExportExcel(model.CustomExportFile, dtt);
            return File(dataExport, "application/xlsx", model.CustomExportFile.FileNameReport + ".xlsx");
        }
        //


        [HttpGet("GetKPIExportSAPReport")]
        public JsonResult GetKPIExportSAPReport(GetKPIFullLadingReportViewModel model)
        {
            try
            {
                var res = _unitOfWork.Repository<Proc_GetKPIExportSAPReport>()
                       .ExecProcedure(Proc_GetKPIExportSAPReport.GetEntityProc(model.centerHubId, model.poHubId, model.stationId, model.customerId, model.fromDate, model.toDate,
                       model.searchText, model.pageNumber, model.pageSize, model.isSortDescending));


                return JsonUtil.Success(res);
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }
        }
        //
        [HttpPost("GetKPIExportSAPReportExportExcel")]
        public dynamic GetKPIExportSAPReportExportExcel([FromBody]GetKPIFullLadingReportViewModel model)
        {
            DataTable dtt = new DataTable();
            var data = _unitOfWork.Repository<Proc_GetKPIExportSAPReport>()
                     .ExecProcedure(Proc_GetKPIExportSAPReport.GetEntityProc(model.centerHubId, model.poHubId, model.stationId, model.customerId, model.fromDate, model.toDate,
                       model.searchText, model.pageNumber, model.pageSize, model.isSortDescending));
            var ListData = data.ToList();
            dtt = ListData.ToDataTable();
            if (data.ToList().Count() == 0)
            {
                return JsonUtil.Error("Không tìm thấy dữ liệu");
            }
            var dataExport = ExportExcelPartern.ExportExcel(model.CustomExportFile, dtt);
            return File(dataExport, "application/xlsx", model.CustomExportFile.FileNameReport + ".xlsx");
        }
        //

        [HttpGet("GetKPITransferReport")]
        public JsonResult GetKPITransferReport(GetKPIFullLadingReportViewModel model)
        {
            try
            {
                var res = _unitOfWork.Repository<Proc_GetKPITransferReport>()
                       .ExecProcedure(Proc_GetKPITransferReport.GetEntityProc(model.centerHubId, model.poHubId, model.stationId, model.customerId, model.fromDate, model.toDate,
                       model.searchText, model.pageNumber, model.pageSize, model.isSortDescending));


                return JsonUtil.Success(res);
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }
        }
        //
        [HttpPost("GetKPITransferReportExportExcel")]
        public dynamic GetKPITransferReportExportExcel([FromBody]GetKPIFullLadingReportViewModel model)
        {
            DataTable dtt = new DataTable();
            var data = _unitOfWork.Repository<Proc_GetKPITransferReport>()
                     .ExecProcedure(Proc_GetKPITransferReport.GetEntityProc(model.centerHubId, model.poHubId, model.stationId, model.customerId, model.fromDate, model.toDate,
                       model.searchText, model.pageNumber, model.pageSize, model.isSortDescending));
            var ListData = data.ToList();
            dtt = ListData.ToDataTable();
            if (data.ToList().Count() == 0)
            {
                return JsonUtil.Error("Không tìm thấy dữ liệu");
            }
            var dataExport = ExportExcelPartern.ExportExcel(model.CustomExportFile, dtt);
            return File(dataExport, "application/xlsx", model.CustomExportFile.FileNameReport + ".xlsx");
        }
        //
        [HttpGet("GetKPIDeliveryReport")]
        public JsonResult GetKPIDeliveryReport(GetKPIFullLadingReportViewModel model)
        {
            try
            {
                var res = _unitOfWork.Repository<Proc_GetKPIDeliveryReport>()
                       .ExecProcedure(Proc_GetKPIDeliveryReport.GetEntityProc(model.centerHubId, model.poHubId, model.stationId, model.customerId, model.fromDate, model.toDate,
                       model.searchText, model.pageNumber, model.pageSize, model.isSortDescending));


                return JsonUtil.Success(res);
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }
        }
        //
        [HttpPost("GetKPIDeliveryReportExportExcel")]
        public dynamic GetKPIDeliveryReportExportExcel([FromBody]GetKPIFullLadingReportViewModel model)
        {
            DataTable dtt = new DataTable();
            var data = _unitOfWork.Repository<Proc_GetKPIDeliveryReport>()
                     .ExecProcedure(Proc_GetKPIDeliveryReport.GetEntityProc(model.centerHubId, model.poHubId, model.stationId, model.customerId, model.fromDate, model.toDate,
                       model.searchText, model.pageNumber, model.pageSize, model.isSortDescending));
            var ListData = data.ToList();
            dtt = ListData.ToDataTable();
            if (data.ToList().Count() == 0)
            {
                return JsonUtil.Error("Không tìm thấy dữ liệu");
            }
            var dataExport = ExportExcelPartern.ExportExcel(model.CustomExportFile, dtt);
            return File(dataExport, "application/xlsx", model.CustomExportFile.FileNameReport + ".xlsx");
        }
        //
        [HttpGet("GetKPICashSubmitionReport")]
        public JsonResult GetKPICashSubmitionReport(GetKPIFullLadingReportViewModel model)
        {
            try
            {
                var res = _unitOfWork.Repository<Proc_GetKPICashSubmitionReport>()
                       .ExecProcedure(Proc_GetKPICashSubmitionReport.GetEntityProc(model.centerHubId, model.poHubId, model.stationId, model.customerId, model.fromDate, model.toDate,
                       model.searchText, model.pageNumber, model.pageSize, model.isSortDescending));


                return JsonUtil.Success(res);
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }
        }
        //
        [HttpPost("GetKPICashSubmitionReportExportExcel")]
        public dynamic GetKPICashSubmitionReportExportExcel([FromBody]GetKPIFullLadingReportViewModel model)
        {
            DataTable dtt = new DataTable();
            var data = _unitOfWork.Repository<Proc_GetKPICashSubmitionReport>()
                     .ExecProcedure(Proc_GetKPICashSubmitionReport.GetEntityProc(model.centerHubId, model.poHubId, model.stationId, model.customerId, model.fromDate, model.toDate,
                       model.searchText, model.pageNumber, model.pageSize, model.isSortDescending));
            var ListData = data.ToList();
            dtt = ListData.ToDataTable();
            if (data.ToList().Count() == 0)
            {
                return JsonUtil.Error("Không tìm thấy dữ liệu");
            }
            var dataExport = ExportExcelPartern.ExportExcel(model.CustomExportFile, dtt);
            return File(dataExport, "application/xlsx", model.CustomExportFile.FileNameReport + ".xlsx");
        }
        //
        [HttpGet("GetKPIConfirmCashSubmitionReport")]
        public JsonResult GetKPIConfirmCashSubmitionReport(GetKPIFullLadingReportViewModel model)
        {
            try
            {
                var res = _unitOfWork.Repository<Proc_GetKPIConfirmCashSubmitionReport>()
                       .ExecProcedure(Proc_GetKPIConfirmCashSubmitionReport.GetEntityProc(model.centerHubId, model.poHubId, model.stationId, model.customerId, model.fromDate, model.toDate,
                       model.searchText, model.pageNumber, model.pageSize, model.isSortDescending));


                return JsonUtil.Success(res);
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }
        }
        //
        [HttpPost("GetKPIConfirmCashSubmitionReportExportExcel")]
        public dynamic GetKPIConfirmCashSubmitionReportExportExcel([FromBody]GetKPIFullLadingReportViewModel model)
        {
            DataTable dtt = new DataTable();
            var data = _unitOfWork.Repository<Proc_GetKPIConfirmCashSubmitionReport>()
                     .ExecProcedure(Proc_GetKPIConfirmCashSubmitionReport.GetEntityProc(model.centerHubId, model.poHubId, model.stationId, model.customerId, model.fromDate, model.toDate,
                       model.searchText, model.pageNumber, model.pageSize, model.isSortDescending));
            var ListData = data.ToList();
            dtt = ListData.ToDataTable();
            if (data.ToList().Count() == 0)
            {
                return JsonUtil.Error("Không tìm thấy dữ liệu");
            }
            var dataExport = ExportExcelPartern.ExportExcel(model.CustomExportFile, dtt);
            return File(dataExport, "application/xlsx", model.CustomExportFile.FileNameReport + ".xlsx");
        }

        [HttpGet("GetReturnLicenseReport")]
        public JsonResult GetReturnLicenseReport(ReturnLicenseReportViewModel model)
        {
            var data = _unitOfWork.Repository<Proc_GetReturnLicenseReport>()
                .ExecProcedure(Proc_GetReturnLicenseReport.GetEntityProc(model.CenterHubId, model.POHubId, model.StationHubId, model.CustomerId, model.DeliveryUserId,
                                model.FromDate, model.ToDate, model.SearchText, model.PageNumber, model.PageSize, model.IsSortDescending));
            return JsonUtil.Success(data);
        }

        [HttpPost("GetReturnLicenseReportExportExcel")]
        public dynamic GetReturnLicenseReportExportExcel([FromBody]ReturnLicenseReportViewModel model)
        {
            DataTable dtt = new DataTable();
            var data = _unitOfWork.Repository<Proc_GetReturnLicenseReport>()
                .ExecProcedure(Proc_GetReturnLicenseReport.GetEntityProc(model.CenterHubId, model.POHubId, model.StationHubId, model.CustomerId, model.DeliveryUserId,
                                model.FromDate, model.ToDate, model.SearchText, model.PageNumber, model.PageSize, model.IsSortDescending));
            var ListData = data.ToList();
            dtt = data.ToDataTable();
            var bytearray = ExportExcelPartern.ExportExcel(model.CustomExportFile, dtt);
            return File(bytearray, "application/xlsx", model.CustomExportFile.FileNameReport + ".xlsx");

        }

        [HttpPost("ReportListGoodsUnconfirm")]
        public JsonResult ReportListGoodsUnconfirm([FromBody]ReportListGoodsViewModel model)
        {
            var data = _unitOfWork.Repository<Proc_ReportListGoodsUnconfirm>()
                .ExecProcedure(Proc_ReportListGoodsUnconfirm.GetEntityProc(model.FromDate, model.ToDate, model.HubId, model.PageNumber, model.PageSize, model.SearchText)).ToList();
            return JsonUtil.Success(data);
        }

        [HttpPost("ReportListGoodsUnconfirmExportExcel")]
        public dynamic ReportListGoodsUnconfirmExportExcel([FromBody]ReportListGoodsViewModel model)
        {
            DataTable dtt = new DataTable();
            var data = _unitOfWork.Repository<Proc_ReportListGoodsUnconfirm>()
                .ExecProcedure(Proc_ReportListGoodsUnconfirm.GetEntityProc(model.FromDate, model.ToDate, model.HubId, model.PageNumber, model.PageSize, model.SearchText)).ToList();
            var ListData = data.ToList();
            dtt = data.ToDataTable();
            var bytearray = ExportExcelPartern.ExportExcel(model.CustomExportFile, dtt);
            return File(bytearray, "application/xlsx", model.CustomExportFile.FileNameReport + ".xlsx");

        }

        [HttpPost("ReportListGoodsByCustomer")]
        public JsonResult ReportListGoodsByCustomer([FromBody]ReportListGoodsByCustomerViewModel model)
        {
            var data = _unitOfWork.Repository<Proc_ReportListGoodsByCustomer>()
                .ExecProcedure(Proc_ReportListGoodsByCustomer.GetEntityProc(model.FromDate, model.ToDate, model.CustomerId, model.IsCreatedPayment, model.PageNumber, model.PageSize)).ToList();
            return JsonUtil.Success(data);
        }

        [HttpPost("ReportListGoodsByCustomerExportExcel")]
        public dynamic ReportListGoodsByCustomerExportExcel([FromBody]ReportListGoodsByCustomerViewModel model)
        {
            DataTable dtt = new DataTable();
            var data = _unitOfWork.Repository<Proc_ReportListGoodsByCustomer>()
                .ExecProcedure(Proc_ReportListGoodsByCustomer.GetEntityProc(model.FromDate, model.ToDate, model.CustomerId, model.IsCreatedPayment, model.PageNumber, model.PageSize)).ToList();
            var ListData = data.ToList();
            dtt = data.ToDataTable();
            var bytearray = ExportExcelPartern.ExportExcel(model.CustomExportFile, dtt);
            return File(bytearray, "application/xlsx", model.CustomExportFile.FileNameReport + ".xlsx");

        }

        [HttpPost("ReportListReceiptAccepted")]
        public JsonResult ReportListReceiptAccepted([FromBody]ReportListGoodsByCustomerViewModel model)
        {
            var data = _unitOfWork.Repository<Proc_ReportListReceiptAccepted>()
                .ExecProcedure(Proc_ReportListReceiptAccepted.GetEntityProc(model.FromDate, model.ToDate, model.HubId, model.AccountingAcountId, model.PageNumber, model.PageSize)).ToList();
            return JsonUtil.Success(data);
        }

        [HttpPost("ReportListReceiptAcceptedExportExcel")]
        public dynamic ReportListReceiptAcceptedExportExcel([FromBody]ReportListGoodsByCustomerViewModel model)
        {
            DataTable dtt = new DataTable();
            var data = _unitOfWork.Repository<Proc_ReportListReceiptAccepted>()
                .ExecProcedure(Proc_ReportListReceiptAccepted.GetEntityProc(model.FromDate, model.ToDate, model.HubId, model.AccountingAcountId, model.PageNumber, model.PageSize)).ToList();
            var ListData = data.ToList();
            dtt = data.ToDataTable();
            var bytearray = ExportExcelPartern.ExportExcel(model.CustomExportFile, dtt);
            return File(bytearray, "application/xlsx", model.CustomExportFile.FileNameReport + ".xlsx");

        }


        [HttpPost("ReportListGoodsByEmployee")]
        public JsonResult ReportListGoodsByEmpoyee([FromBody]ReportListGoodsByEmployeeViewModel model)
        {
            var data = _unitOfWork.Repository<Proc_ReportListGoodsByEmployee>()
                .ExecProcedure(Proc_ReportListGoodsByEmployee.GetEntityProc(model.FromDate, model.ToDate, model.EmpId, model.IsGroupEmp, 
                model.SearchText, model.SenderId, model.IsCreateListReceiptCOD, model.ListReceiptCODStatusId, model.IsSubmitCOD, model.PageNumber, model.PageSize)).ToList();
            return JsonUtil.Success(data);
        }

        [HttpPost("ReportListGoodsByEmpoyeeExportExcel")]
        public dynamic ReportListGoodsByEmpoyeeExportExcel([FromBody]ReportListGoodsByEmployeeViewModel model)
        {
            DataTable dtt = new DataTable();
            var data = _unitOfWork.Repository<Proc_ReportListGoodsByEmployee>()
                .ExecProcedure(Proc_ReportListGoodsByEmployee.GetEntityProc(model.FromDate, model.ToDate, model.EmpId, model.IsGroupEmp,
                model.SearchText, model.SenderId, model.IsCreateListReceiptCOD, model.ListReceiptCODStatusId, model.IsSubmitCOD, model.PageNumber, model.PageSize)).ToList();
            var ListData = data.ToList();
            dtt = data.ToDataTable();
            var bytearray = ExportExcelPartern.ExportExcel(model.CustomExportFile, dtt);
            return File(bytearray, "application/xlsx", model.CustomExportFile.FileNameReport + ".xlsx");

        }

        [HttpPost("GetReportListGoodsConfirmTSM")]
        public JsonResult GetReportListGoodsConfirmTSM([FromBody] ReportListGoodsConfirmTSMViewModel filterViewModel)
        {
            var data = _unitOfWork.Repository<Proc_ReportListGoodsConfirmTSM>()
                      .ExecProcedure(Proc_ReportListGoodsConfirmTSM.GetEntityProc(
                          filterViewModel.FromHubId,
                          filterViewModel.ToHubId,
                          filterViewModel.CurrentUserId,
                          filterViewModel.AccountingAccountId,
                          filterViewModel.DateFrom,
                          filterViewModel.DateTo,
                          filterViewModel.PageNumber,
                          filterViewModel.PageSize,
                          filterViewModel.SearchText
                          ));
            return JsonUtil.Success(data);
        }

        [HttpPost("GetReportListGoodsConfirmTSMExcel")]
        public dynamic GetReportListGoodsConfirmTSMExcel([FromBody] ReportListGoodsConfirmTSMViewModel model)
        {
            DataTable dtt = new DataTable();
            var data = _unitOfWork.Repository<Proc_ReportListGoodsConfirmTSM>()
                .ExecProcedure(Proc_ReportListGoodsConfirmTSM.GetEntityProc(
                    model.FromHubId,
                    model.ToHubId,
                    model.CurrentUserId,
                    model.AccountingAccountId,
                    model.DateFrom,
                    model.DateTo,
                    model.PageNumber,
                    model.PageSize,
                    model.SearchText)).ToList();
            var ListData = data.ToList();
            dtt = data.ToDataTable();
            var bytearray = ExportExcelPartern.ExportExcel(model.CustomExportFile, dtt);
            return File(bytearray, "application/xlsx", model.CustomExportFile.FileNameReport + ".xlsx");

        }

        [HttpPost("ReportListGoodsOpenLock")]
        public JsonResult ReportListGoodsOpenLock([FromBody] ReportListGoodsViewModel model)
        {
            var data = _unitOfWork.Repository<Proc_ReportListGoodsOpenLock>()
                .ExecProcedure(Proc_ReportListGoodsOpenLock.GetEntityProc(model.FromDate, model.ToDate, model.HubId, model.PageNumber, model.PageSize)).ToList();
            return JsonUtil.Success(data);
        }

        [HttpPost("ReportListGoodsOpenLockExportExcel")]
        public dynamic ReportListGoodsOpenLockExportExcel([FromBody] ReportListGoodsViewModel model)
        {
            DataTable dtt = new DataTable();
            var data = _unitOfWork.Repository<Proc_ReportListGoodsOpenLock>()
                .ExecProcedure(Proc_ReportListGoodsOpenLock.GetEntityProc(model.FromDate, model.ToDate, model.HubId, model.PageNumber, model.PageSize)).ToList();
            var ListData = data.ToList();
            dtt = data.ToDataTable();
            var bytearray = ExportExcelPartern.ExportExcel(model.CustomExportFile, dtt);
            return File(bytearray, "application/xlsx", model.CustomExportFile.FileNameReport + ".xlsx");

        }
    }
}
