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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class ListCustomerPaymentController : GeneralController<ListCustomerPaymentViewModel, ListCustomerPaymentInfoViewModel, ListCustomerPayment>
    {
        private readonly IListCustomerPaymentService _iListCustomerPaymentService;
        private readonly IShipmentService _iShipmentService;
        private readonly IGeneralService _iGeneralServiceRaw;

        public ListCustomerPaymentController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor, IOptions<JwtIssuerOptions> jwtOptions,
            IUnitOfWork unitOfWork,
            IGeneralService<ListCustomerPaymentViewModel, ListCustomerPaymentInfoViewModel, ListCustomerPayment> iGeneralService,
            IGeneralService iGeneralServiceRaw,
            IListCustomerPaymentService iListCustomerPaymentService,
            IShipmentService iShipmentService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
            _iListCustomerPaymentService = iListCustomerPaymentService;
            _iShipmentService = iShipmentService;
            _iGeneralServiceRaw = iGeneralServiceRaw;
        }

        [HttpGet("GetListShipmentToPayment")]
        public JsonResult GetListShipmentToPayment(int? categoryPaymentId, DateTime? formDate = null, DateTime? toDate = null, DateTime? dateFromAccept = null, DateTime? dateToAccept = null, string searchText = null, int? senderId = null, bool? isAccept = null, bool? isSuccess = null, int? pageSize = null, int? pageNumber = null)
        {
            var data = _unitOfWork.Repository<Proc_GetListShipmentPayment>()
                .ExecProcedure(Proc_GetListShipmentPayment.GetEntityProc(categoryPaymentId, formDate, toDate, dateFromAccept, dateToAccept, senderId, searchText, isSuccess, isAccept, pageNumber, pageSize));
            return JsonUtil.Success(data);
        }

        [HttpGet("CheckCustomerPayment")]
        public JsonResult CheckCustomerPayment(int? categoryPaymentId, DateTime? formDate = null, DateTime? toDate = null, int? senderId = null,  bool? isSuccess = null, int? pageSize = null, int? pageNumber = null)
        {
            var data = _unitOfWork.Repository<Proc_CheckCustomerPayment>()
                .ExecProcedure(Proc_CheckCustomerPayment.GetEntityProc(categoryPaymentId, formDate, toDate, senderId, isSuccess, pageNumber, pageSize));
            return JsonUtil.Success(data);
        }

        [HttpGet("GetShipmentListPaymentCustomer")]
        public JsonResult GetShipmentListPaymentCustomer(int listPaymentId, string searchText = null, int? pageSize = null, int? pageNumber = null)
        {
            var data = _unitOfWork.Repository<Proc_GetShipmentListPaymentCustomer>()
                .ExecProcedure(Proc_GetShipmentListPaymentCustomer.GetEntityProc(listPaymentId, searchText, pageNumber, pageSize));
            return JsonUtil.Success(data);
        }

        [HttpGet("GetCustomerWaitingPayment")]
        public JsonResult GetCustomerWaitingPayment(int type, string cols = null)
        {
            var shipmentRepository = _unitOfWork.RepositoryR<Shipment>();
            var customer = _unitOfWork.RepositoryR<Customer>();
            if (type == ListCustomerPaymentTypeHelper.PAYMENT_COD)
            {
                return JsonUtil.Success(shipmentRepository.FindBy(x => x.COD > 0 && x.IsCreditTransfer == false && !x.ListCustomerPaymentCODId.HasValue && x.ShipmentStatusId == StatusHelper.ShipmentStatusId.DeliveryComplete)
                    .GroupBy(x => x.SenderId)
                    .Join(customer.GetAll(), x => x.FirstOrDefault().SenderId, y => y.Id, (x, y) => y));
            }
            else
            {
                return JsonUtil.Success(shipmentRepository.FindBy(x => x.TotalPrice > 0 && !x.ListCustomerPaymentTotalPriceId.HasValue && x.PaymentTypeId == PaymentTypeHelper.THANH_TOAN_CUOI_THANG)
                     .GroupBy(x => x.SenderId)
                    .Join(customer.GetAll(), x => x.FirstOrDefault().SenderId, y => y.Id, (x, y) => y));
            }
        }
        #region Create
        [HttpPost("Create")]
        public override async Task<JsonResult> Create([FromBody]ListCustomerPaymentViewModel viewModel)
        {
            //viewModel.ListCustomerPaymentTypeId = ListCustomerPaymentTypeHelper.PAYMENT_COD;
            viewModel.HubCreatedId = GetCurrentUser().HubId;
            var result = await _iListCustomerPaymentService.Create(viewModel);
            if (result.IsSuccess)
            {
                return JsonUtil.Success(result.Data);
            }
            else
            {
                return JsonUtil.Error(result.Message);
            }
        }
        #endregion

        #region CreateNew
        [HttpPost("CreateNew")]
        public async Task<JsonResult> CreateNew([FromBody]ListCustomerPaymentViewModel viewModel)
        {
            //viewModel.ListCustomerPaymentTypeId = ListCustomerPaymentTypeHelper.PAYMENT_COD;
            if (!viewModel.AdjustPrice.HasValue) viewModel.AdjustPrice = 0;
            viewModel.HubCreatedId = GetCurrentUser().HubId;
            var result = await _iListCustomerPaymentService.CreateNew(viewModel);
            if (result.IsSuccess)
            {
                return JsonUtil.Success(result.Data);
            }
            else
            {
                return JsonUtil.Error(result.Message);
            }
        }
        #endregion

        #region Lock - Pay - Cancel
        [HttpPost("Lock")]
        public async Task<JsonResult> Lock([FromBody]ListCustomerPaymentViewModel viewModel)
        {
            var listCustomerPayment = _unitOfWork.RepositoryR<ListCustomerPayment>().GetSingle(viewModel.Id);
            if (listCustomerPayment == null)
            {
                return JsonUtil.Error("Bảng kê nộp tiền không tồn tại");
            }
            var result = await _iListCustomerPaymentService.Lock(viewModel);
            if (result.IsSuccess)
            {
                return JsonUtil.Success(result.Data);
            }
            else
            {
                return JsonUtil.Error(result.Message);
            }

        }
        [HttpPost("Unlock")]
        public async Task<JsonResult> Unlock([FromBody]ListCustomerPaymentViewModel viewModel)
        {
            var userId = GetCurrentUser().Id;
            var userRoleIds = _unitOfWork.RepositoryR<UserRole>().FindBy(u => u.UserId == userId).Select(u => u.RoleId).ToArray();
            if (Util.IsNull(userRoleIds) || !userRoleIds.Contains(RoleHelper.ChiefAccountant))
            {
                return JsonUtil.Error("Kế toán trưởng mới được phép mở khóa");
            }
            var listCustomerPayment = _unitOfWork.RepositoryR<ListCustomerPayment>().GetSingle(viewModel.Id);
            if (listCustomerPayment == null)
            {
                return JsonUtil.Error("Bảng kê nộp tiền không tồn tại");
            }
            var result = await _iListCustomerPaymentService.Unlock(viewModel);
            if (result.IsSuccess)
            {
                return JsonUtil.Success(result.Data);
            }
            else
            {
                return JsonUtil.Error(result.Message);
            }

        }

        [HttpPost("Pay")]
        public async Task<JsonResult> Pay([FromBody]ListCustomerPaymentViewModel viewModel)
        {
            var listCustomerPayment = _unitOfWork.RepositoryR<ListCustomerPayment>().GetSingle(viewModel.Id);

            if (listCustomerPayment == null)
            {
                return JsonUtil.Error("Bảng kê nộp tiền không tồn tại");
            }
            var result = await _iListCustomerPaymentService.Pay(viewModel);
            if (result.IsSuccess)
            {
                return JsonUtil.Success(result.Data);
            }
            else
            {
                return JsonUtil.Error(result.Message);
            }
        }
        [HttpPost("Cancel")]
        public async Task<JsonResult> Cancel([FromBody]ListCustomerPaymentViewModel viewModel)
        {
            var listCustomerPayment = _unitOfWork.RepositoryR<ListCustomerPayment>().GetSingle(viewModel.Id);
            if (listCustomerPayment == null)
            {
                return JsonUtil.Error("Bảng kê nộp tiền không tồn tại");
            }
            var result = await _iListCustomerPaymentService.Cancel(viewModel);
            if (result.IsSuccess)
            {
                return JsonUtil.Success(result.Data);
            }
            else
            {
                return JsonUtil.Error(result.Message);
            }
        }
        #endregion
        #region GET
        [HttpGet("GetListByType")]
        public JsonResult GetListByType(int? type = null, DateTime? fromDate = null, DateTime? toDate = null, int? pageSize = null, int? pageNumber = null, string cols = null)
        {
            return JsonUtil.Create(_iListCustomerPaymentService.GetByType(GetCurrentUser().HubId ?? 0, type, fromDate, toDate, pageSize, pageNumber, cols));
        }
        [HttpGet("getListByTypeNew")]
        public JsonResult getListByTypeNew(int senderId, int? typePaymentId = null)
        {
            return JsonUtil.Create(_iListCustomerPaymentService.GetByTypeNew(GetCurrentUser().HubId ?? 0, senderId, typePaymentId));
        }
        [HttpGet("GetListCustomerPaymentByShipmentNumberAndType")]
        public JsonResult GetListCustomerPaymentByShipmentNumberAndType(string shipmentNumber, int type)
        {
            return JsonUtil.Create(_iListCustomerPaymentService.GetListCustomerPaymentByShipmentNumberAndType(shipmentNumber, type));
        }
        #endregion

        [HttpPost("AdjustPriceCustomerPayment")]
        public async Task<JsonResult> AdjustPriceCustomerPayment([FromBody]ListCustomerPaymentViewModel viewModel)
        {
            var listCustomerPayment = _unitOfWork.RepositoryR<ListCustomerPayment>().GetSingle(viewModel.Id);
            if (listCustomerPayment == null)
            {
                return JsonUtil.Error("Bảng kê nộp tiền không tồn tại");
            }
            if (!string.IsNullOrWhiteSpace(viewModel.Note) && viewModel.AdjustPrice.HasValue)
            {
                listCustomerPayment.AdjustPrice = viewModel.AdjustPrice;
                if (!string.IsNullOrWhiteSpace(listCustomerPayment.Note))
                {
                    listCustomerPayment.Note = listCustomerPayment.Note + ", " + viewModel.Note;
                }
                else
                {
                    listCustomerPayment.Note = viewModel.Note;
                }
                var data = await _iGeneralServiceRaw.Update<ListCustomerPayment>(listCustomerPayment);
                return JsonUtil.Create(data);
            }
            else
            {
                return JsonUtil.Error("Chưa nhập giá điều chỉnh hoặc ghi chú");
            }
        }

        #region Add Shipment To Payment
        [HttpPost("AddShipmentToPayment")]
        public async Task<JsonResult> AddShipmentToPayment([FromBody]ListCustomerPaymentViewModel viewModel)
        {
            var listCustomerPayment = _unitOfWork.RepositoryR<ListCustomerPayment>().GetSingle(viewModel.Id);
            if (listCustomerPayment == null)
            {
                return JsonUtil.Error("Bảng kê nộp tiền không tồn tại");
            }
            var result = await _iListCustomerPaymentService.AddShipmentToPayment(viewModel);
            if (result.IsSuccess)
            {
                return JsonUtil.Success(result.Data);
            }
            else
            {
                return JsonUtil.Error(result.Message);
            }

        }
        #endregion

        [HttpPost("UnInstall")]
        public JsonResult UnInstall([FromBody]LCPShipmentViewModel viewModel)
        {
            var res = _unitOfWork.Repository<Proc_UnInstallShipmentInListCustomerPayment>().ExecProcedureSingle(
                Proc_UnInstallShipmentInListCustomerPayment.GetEntityProc(viewModel.ListCustomerPaymentId, viewModel.ShipmentId));
            if (res.IsSuccess==true)
            {
                return JsonUtil.Success();
            }
            else
            {
                return JsonUtil.Error(res.Message);
            }
        }

    }
}
