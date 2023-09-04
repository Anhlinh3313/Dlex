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
using static Core.Api.Infrastruture.GSDPApi;
using Core.Api.Infrastruture;
using Core.Data.Core;
using Core.Data;
using Core.Business.ViewModels.General;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class ListReceiptMoneyController : GeneralController<ListReceiptMoneyViewModel, ListReceiptMoneyInfoViewModel, ListReceiptMoney>
    {
        private readonly CompanyInformation _icompanyInformation;
        private readonly IListReceiptMoneyService _iListReceiptMoneyService;
        private readonly IGeneralService<ListReceiptMoneyShipmentInfoViewModel, ListReceiptMoneyShipment> _iListReceiptMoneyShipmentService;
        private readonly IGeneralService _iGeneralServiceRaw;
        public ListReceiptMoneyController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor, IOptions<JwtIssuerOptions> jwtOptions,
            IUnitOfWork unitOfWork,
            IGeneralService<ListReceiptMoneyViewModel, ListReceiptMoneyInfoViewModel, ListReceiptMoney> iGeneralService,
            IListReceiptMoneyService iListReceiptMoneyService,
            IOptions<CompanyInformation> companyInformation,
            IGeneralService<ListReceiptMoneyShipmentInfoViewModel, ListReceiptMoneyShipment> iListReceiptMoneyShipmentService,
            IGeneralService iGeneralServiceRaw) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
            _icompanyInformation = companyInformation.Value;
            _iListReceiptMoneyService = iListReceiptMoneyService;
            _iListReceiptMoneyShipmentService = iListReceiptMoneyShipmentService;
            _iGeneralServiceRaw = iGeneralServiceRaw;
        }
        #region Xác nhận tiền nhân viên
        [HttpGet("GetListShipmentKeepingByEmployee")]
        public JsonResult GetListShipmentKeepingByEmployee(int? empId)
        {
            var data = _unitOfWork.Repository<Proc_ShipmentEmployeeKeeping>()
                      .ExecProcedure(Proc_ShipmentEmployeeKeeping.GetEntityProc(GetCurrentUser().HubId ?? 0, empId));
            return JsonUtil.Success(data);
        }

        [HttpGet("GetListShipmentKeepingByLRMId")]
        public JsonResult GetListShipmentKeepingByLRMId(int id)
        {
            var data = _unitOfWork.Repository<Proc_ShipmentEmployeeByLRMId>()
                      .ExecProcedure(Proc_ShipmentEmployeeByLRMId.GetEntityProc(id));
            return JsonUtil.Success(data);
        }

        [HttpGet("GetListShipmentKeepingByHub")]
        public JsonResult GetListShipmentKeepingByHub(int? otherHubId, int? listReceitMoneyTypeId)
        {
            int hubId = GetCurrentUser().HubId ?? 0;
            var data = _unitOfWork.Repository<Proc_ShipmentHubKeeping>()
                      .ExecProcedure(Proc_ShipmentHubKeeping.GetEntityProc(hubId, otherHubId, listReceitMoneyTypeId));
            return JsonUtil.Success(data);
        }
        [HttpPost("CreateListReceiptMoneyFromRider")]
        public async Task<JsonResult> CreateListReceiptMoneyFromRider([FromBody]ListReceiptMoneyViewModel viewModel)
        {
            viewModel.ListReceiptMoneyTypeId = ListReceiptMoneyTypeHelper.EMPLOYEE;
            viewModel.FromHubId = GetCurrentUser().HubId;
            viewModel.ToHubId = GetCurrentUser().HubId;
            var result = await _iListReceiptMoneyService.Create(viewModel);
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
        #region Nộp tiền về Hub cha
        [HttpPost("CreateListReceiptMoneyToHub")]
        public async Task<JsonResult> CreateListReceiptMoneyToHub([FromBody]ListReceiptMoneyViewModel viewModel)
        {
            viewModel.ListReceiptMoneyTypeId = ListReceiptMoneyTypeHelper.HUB;
            var currentUser = GetCurrentUser();
            viewModel.FromHubId = currentUser.HubId;
            viewModel.PaidByEmpId = currentUser.Id;
            if (!viewModel.ToHubId.HasValue)
            {
                if (viewModel.FromHubId == viewModel.ToHubId && viewModel.FromHubId.HasValue)
                {
                    var hub = _unitOfWork.RepositoryR<Hub>().GetSingle(viewModel.FromHubId.Value);
                    if (!Util.IsNull(hub))
                    {
                        if (hub.PoHubId.HasValue) viewModel.ToHubId = hub.PoHubId;
                    }
                }
            }
            var result = await _iListReceiptMoneyService.Create(viewModel);
            if (result.IsSuccess)
            {
                return JsonUtil.Success(result.Data);
            }
            else
            {
                return JsonUtil.Error(result.Message);
            }
        }

        [HttpPost("UpdateListReceiptMoneyToHub")]
        public async Task<JsonResult> UpdateListReceiptMoneyToHub([FromBody]ListReceiptMoneyViewModel viewModel)
        {
            viewModel.ListReceiptMoneyTypeId = ListReceiptMoneyTypeHelper.HUB;
            var currentUser = GetCurrentUser();
            viewModel.FromHubId = currentUser.HubId;
            viewModel.PaidByEmpId = currentUser.Id;
            if (!viewModel.ToHubId.HasValue)
            {
                if (viewModel.FromHubId == viewModel.ToHubId && viewModel.FromHubId.HasValue)
                {
                    var hub = _unitOfWork.RepositoryR<Hub>().GetSingle(viewModel.FromHubId.Value);
                    if (!Util.IsNull(hub))
                    {
                        if (hub.PoHubId.HasValue) viewModel.ToHubId = hub.PoHubId;
                    }
                }
            }
            var result = await _iListReceiptMoneyService.Update(viewModel);
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
        #region Nộp tiền về thủ quỹ
        [HttpPost("CreateListReceiptMoneyToTreasurer")]
        public async Task<JsonResult> CreateListReceiptMoneyToTreasurer([FromBody]ListReceiptMoneyViewModel viewModel)
        {
            viewModel.ListReceiptMoneyTypeId = ListReceiptMoneyTypeHelper.TREASURER;
            var currentUser = GetCurrentUser();
            viewModel.FromHubId = currentUser.HubId;
            viewModel.ToHubId = currentUser.HubId;
            viewModel.PaidByEmpId = currentUser.Id;
            var result = await _iListReceiptMoneyService.Create(viewModel);
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
        #region Trả tiền về bưu cục khác
        [HttpPost("CreateListReceiptMoneyToHubOther")]
        public async Task<JsonResult> CreateListReceiptMoneyToHubOther([FromBody]ListReceiptMoneyViewModel viewModel)
        {
            var currentUser = GetCurrentUser();
            viewModel.FromHubId = currentUser.HubId;
            viewModel.PaidByEmpId = currentUser.Id;
            viewModel.ListReceiptMoneyTypeId = ListReceiptMoneyTypeHelper.HUBOTHER;
            var result = await _iListReceiptMoneyService.Create(viewModel);
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

        [HttpGet("GetListReceiptMoneyWarningNote")]
        public JsonResult GetListReceiptMoneyWarningNote()
        {
            var data = _unitOfWork.RepositoryR<ListReceiptMoney>().FindBy(x => x.PaidByEmpId == GetCurrentUserId() && !string.IsNullOrEmpty(x.WarningNote) && x.Seen != true).OrderByDescending(x => x.CreatedWhen);

            return JsonUtil.Success(data);
        }

        [HttpPost("SeenWarningNote")]
        public JsonResult SeenWarningNote([FromBody] ListReceiptMoney model)
        {
            var data = _unitOfWork.RepositoryR<ListReceiptMoney>().GetSingle(model.Id);
            data.Seen = true;
            _unitOfWork.Commit();

            return JsonUtil.Success();
        }

        #region Lock - Confirm - Cancel
        [HttpPost("Lock")]
        public async Task<JsonResult> Lock([FromBody]ListReceiptMoneyViewModel viewModel)
        {
            var listReceiptMoney = _unitOfWork.RepositoryR<ListReceiptMoney>().GetSingle(viewModel.Id);

            if (listReceiptMoney == null)
            {
                return JsonUtil.Error("Bảng kê nộp tiền không tồn tại");
            }
            ListReceiptMoneyLog listReceiptMoneyLog = new ListReceiptMoneyLog();
            listReceiptMoneyLog.DataChanged = "Khóa bảng kê";
            listReceiptMoneyLog.ListReceiptMoneyId = listReceiptMoney.Id;
            listReceiptMoneyLog.Reason = listReceiptMoney.CancelReason;
            if (listReceiptMoney.ListReceiptMoneyStatusId.HasValue)
            {
                listReceiptMoneyLog.StatusId = listReceiptMoney.ListReceiptMoneyStatusId.Value;
            }
            listReceiptMoneyLog.GrandTotal = listReceiptMoney.GrandTotal;
            listReceiptMoneyLog.FeeBank = listReceiptMoney.FeeBank;
            listReceiptMoneyLog.GrandTotalReal = listReceiptMoney.GrandTotalReal;
            listReceiptMoneyLog.AccountingAccountId = listReceiptMoney.AccountingAccountId;
            listReceiptMoneyLog.AcceptDate = listReceiptMoney.AcceptDate;
            listReceiptMoneyLog.CashFlowId = listReceiptMoney.CashFlowId;
            listReceiptMoneyLog.WarningNote = listReceiptMoney.WarningNote;
            listReceiptMoneyLog.TotalShipment = listReceiptMoney.TotalShipment;
            listReceiptMoneyLog.IsTransfer = listReceiptMoney.IsTransfer;
            _unitOfWork.RepositoryCRUD<ListReceiptMoneyLog>().Insert(listReceiptMoneyLog);
            _unitOfWork.Commit();
            _unitOfWork.Repository<Proc_SaveLogReceiptMoneyDetail>().ExecProcedureSingle(
                Proc_SaveLogReceiptMoneyDetail.GetEntityProc(listReceiptMoneyLog.Id, listReceiptMoney.Id));
            //
            if (!listReceiptMoney.CreatedBy.HasValue) viewModel.CreatedBy = GetCurrentUserId();
            else viewModel.CreatedBy = listReceiptMoney.CreatedBy;
            var result = await _iListReceiptMoneyService.Lock(viewModel);
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
        public async Task<JsonResult> Unlock([FromBody]ListReceiptMoneyViewModel viewModel)
        {
            var userId = GetCurrentUser().Id;
            var userRoleIds = _unitOfWork.RepositoryR<UserRole>().FindBy(u => u.UserId == userId).Select(u => u.RoleId).ToArray();
            if (Util.IsNull(userRoleIds) || !userRoleIds.Contains(RoleHelper.ChiefAccountant))
            {
                return JsonUtil.Error("Kế toán trưởng mới được phép mở khóa");
            }
            var listReceiptMoney = _unitOfWork.RepositoryR<ListReceiptMoney>().GetSingle(viewModel.Id);
            if (listReceiptMoney == null)
            {
                return JsonUtil.Error("Bảng kê nộp tiền không tồn tại");
            }
            ListReceiptMoneyLog listReceiptMoneyLog = new ListReceiptMoneyLog();
            listReceiptMoneyLog.DataChanged = "Mở khóa bảng kê";
            listReceiptMoneyLog.ListReceiptMoneyId = listReceiptMoney.Id;
            listReceiptMoneyLog.Reason = listReceiptMoney.CancelReason;
            if (listReceiptMoney.ListReceiptMoneyStatusId.HasValue)
            {
                listReceiptMoneyLog.StatusId = listReceiptMoney.ListReceiptMoneyStatusId.Value;
            }
            listReceiptMoneyLog.GrandTotal = listReceiptMoney.GrandTotal;
            listReceiptMoneyLog.FeeBank = listReceiptMoney.FeeBank;
            listReceiptMoneyLog.GrandTotalReal = listReceiptMoney.GrandTotalReal;
            listReceiptMoneyLog.AccountingAccountId = listReceiptMoney.AccountingAccountId;
            listReceiptMoneyLog.AcceptDate = listReceiptMoney.AcceptDate;
            listReceiptMoneyLog.CashFlowId = listReceiptMoney.CashFlowId;
            listReceiptMoneyLog.WarningNote = listReceiptMoney.WarningNote;
            listReceiptMoneyLog.TotalShipment = listReceiptMoney.TotalShipment;
            listReceiptMoneyLog.IsTransfer = listReceiptMoney.IsTransfer;
            _unitOfWork.RepositoryCRUD<ListReceiptMoneyLog>().Insert(listReceiptMoneyLog);
            _unitOfWork.Commit();
            _unitOfWork.Repository<Proc_SaveLogReceiptMoneyDetail>().ExecProcedureSingle(
                Proc_SaveLogReceiptMoneyDetail.GetEntityProc(listReceiptMoneyLog.Id, listReceiptMoney.Id));
            //
            var result = await _iListReceiptMoneyService.Unlock(viewModel);
            if (result.IsSuccess)
            {
                string reasonUnLock = "";
                if (listReceiptMoney.ReasonListGoodsId.HasValue)
                {
                    var reason = _unitOfWork.RepositoryR<Reason>().GetSingle(listReceiptMoney.ReasonListGoodsId.Value);
                    if (reason != null) reasonUnLock = reason.Name;
                }
                var firebasetoken = _unitOfWork.RepositoryR<User>().GetSingle(u => u.Id == listReceiptMoney.PaidByEmpId.GetValueOrDefault()).FireBaseToken;
                await FireBaseUtil.SendNotification(FireBaseUtil.BrowserAPIKey, firebasetoken, "Bảng kê " + listReceiptMoney.Code + " kế toán không xác nhận, lý do: " + reasonUnLock, 1);
                return JsonUtil.Success(result.Data);
            }
            else
            {
                return JsonUtil.Error(result.Message);
            }

        }
        [HttpPost("Confirm")]
        public async Task<JsonResult> Confirm([FromBody]ListReceiptMoneyViewModel viewModel)
        {
            var currentUser = GetCurrentUser();
            viewModel.AcceptByUserId = currentUser.Id;
            var listReceiptMoney = _unitOfWork.RepositoryR<ListReceiptMoney>().GetSingle(viewModel.Id);

            if (listReceiptMoney == null)
            {
                return JsonUtil.Error("Bảng kê nộp tiền không tồn tại");
            }

            ListReceiptMoneyLog listReceiptMoneyLog = new ListReceiptMoneyLog();
            listReceiptMoneyLog.DataChanged = "Xác nhận bảng kê";
            listReceiptMoneyLog.ListReceiptMoneyId = listReceiptMoney.Id;
            listReceiptMoneyLog.Reason = listReceiptMoney.CancelReason;
            if (listReceiptMoney.ListReceiptMoneyStatusId.HasValue)
            {
                listReceiptMoneyLog.StatusId = listReceiptMoney.ListReceiptMoneyStatusId.Value;
            }
            listReceiptMoneyLog.GrandTotal = listReceiptMoney.GrandTotal;
            listReceiptMoneyLog.FeeBank = listReceiptMoney.FeeBank;
            listReceiptMoneyLog.GrandTotalReal = listReceiptMoney.GrandTotalReal;
            listReceiptMoneyLog.AccountingAccountId = listReceiptMoney.AccountingAccountId;
            listReceiptMoneyLog.AcceptDate = listReceiptMoney.AcceptDate;
            listReceiptMoneyLog.CashFlowId = listReceiptMoney.CashFlowId;
            listReceiptMoneyLog.WarningNote = listReceiptMoney.WarningNote;
            listReceiptMoneyLog.TotalShipment = listReceiptMoney.TotalShipment;
            listReceiptMoneyLog.IsTransfer = listReceiptMoney.IsTransfer;
            _unitOfWork.RepositoryCRUD<ListReceiptMoneyLog>().Insert(listReceiptMoneyLog);
            _unitOfWork.Commit();
            _unitOfWork.Repository<Proc_SaveLogReceiptMoneyDetail>().ExecProcedureSingle(
                Proc_SaveLogReceiptMoneyDetail.GetEntityProc(listReceiptMoneyLog.Id, listReceiptMoney.Id));
            //
            var result = await _iListReceiptMoneyService.Confirm(viewModel);
            if (result.IsSuccess)
            {
                return JsonUtil.Success(result.Data);
            }
            else
            {
                return JsonUtil.Error(result.Message);
            }
        }
        [HttpPost("ReConfirm")]
        public async Task<JsonResult> ReConfirm([FromBody]ListReceiptMoneyViewModel viewModel)
        {
            var currentUser = GetCurrentUser();
            viewModel.AcceptByUserId = currentUser.Id;
            var listReceiptMoney = _unitOfWork.RepositoryR<ListReceiptMoney>().GetSingle(viewModel.Id);

            if (listReceiptMoney == null)
            {
                return JsonUtil.Error("Bảng kê nộp tiền không tồn tại");
            }

            ListReceiptMoneyLog listReceiptMoneyLog = new ListReceiptMoneyLog();
            listReceiptMoneyLog.DataChanged = "Xác nhận lại bảng kê";
            listReceiptMoneyLog.ListReceiptMoneyId = listReceiptMoney.Id;
            listReceiptMoneyLog.Reason = listReceiptMoney.CancelReason;
            if (listReceiptMoney.ListReceiptMoneyStatusId.HasValue)
            {
                listReceiptMoneyLog.StatusId = listReceiptMoney.ListReceiptMoneyStatusId.Value;
            }
            listReceiptMoneyLog.GrandTotal = listReceiptMoney.GrandTotal;
            listReceiptMoneyLog.FeeBank = listReceiptMoney.FeeBank;
            listReceiptMoneyLog.GrandTotalReal = listReceiptMoney.GrandTotalReal;
            listReceiptMoneyLog.AccountingAccountId = listReceiptMoney.AccountingAccountId;
            listReceiptMoneyLog.AcceptDate = listReceiptMoney.AcceptDate;
            listReceiptMoneyLog.CashFlowId = listReceiptMoney.CashFlowId;
            listReceiptMoneyLog.WarningNote = listReceiptMoney.WarningNote;
            listReceiptMoneyLog.TotalShipment = listReceiptMoney.TotalShipment;
            listReceiptMoneyLog.IsTransfer = listReceiptMoney.IsTransfer;
            _unitOfWork.RepositoryCRUD<ListReceiptMoneyLog>().Insert(listReceiptMoneyLog);
            _unitOfWork.Commit();
            _unitOfWork.Repository<Proc_SaveLogReceiptMoneyDetail>().ExecProcedureSingle(
                Proc_SaveLogReceiptMoneyDetail.GetEntityProc(listReceiptMoneyLog.Id, listReceiptMoney.Id));
            //
            var result = await _iListReceiptMoneyService.ReConfirm(viewModel);
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
        public async Task<JsonResult> Cancel([FromBody]ListReceiptMoneyViewModel viewModel)
        {
            var listReceiptMoney = _unitOfWork.RepositoryR<ListReceiptMoney>().GetSingle(viewModel.Id);
            listReceiptMoney.CancelReason = viewModel.CancelReason;
            if (listReceiptMoney == null)
            {
                return JsonUtil.Error("Bảng kê nộp tiền không tồn tại");
            }
            ListReceiptMoneyLog listReceiptMoneyLog = new ListReceiptMoneyLog();
            listReceiptMoneyLog.DataChanged = "Hủy bảng kê";
            listReceiptMoneyLog.ListReceiptMoneyId = listReceiptMoney.Id;
            listReceiptMoneyLog.Reason = listReceiptMoney.CancelReason;
            if (listReceiptMoney.ListReceiptMoneyStatusId.HasValue)
            {
                listReceiptMoneyLog.StatusId = listReceiptMoney.ListReceiptMoneyStatusId.Value;
            }
            listReceiptMoneyLog.GrandTotal = listReceiptMoney.GrandTotal;
            listReceiptMoneyLog.FeeBank = listReceiptMoney.FeeBank;
            listReceiptMoneyLog.GrandTotalReal = listReceiptMoney.GrandTotalReal;
            listReceiptMoneyLog.AccountingAccountId = listReceiptMoney.AccountingAccountId;
            listReceiptMoneyLog.AcceptDate = listReceiptMoney.AcceptDate;
            listReceiptMoneyLog.CashFlowId = listReceiptMoney.CashFlowId;
            listReceiptMoneyLog.WarningNote = listReceiptMoney.WarningNote;
            listReceiptMoneyLog.TotalShipment = listReceiptMoney.TotalShipment;
            listReceiptMoneyLog.IsTransfer = listReceiptMoney.IsTransfer;
            _unitOfWork.RepositoryCRUD<ListReceiptMoneyLog>().Insert(listReceiptMoneyLog);
            _unitOfWork.Commit();
            _unitOfWork.Repository<Proc_SaveLogReceiptMoneyDetail>().ExecProcedureSingle(
                Proc_SaveLogReceiptMoneyDetail.GetEntityProc(listReceiptMoneyLog.Id, listReceiptMoney.Id));
            //
            var result = await _iListReceiptMoneyService.Cancel(viewModel);
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
        public JsonResult GetListByType(int type, int? empId = null, DateTime? fromDate = null, DateTime? toDate = null, int? pageSize = null, int? pageNumber = null, string cols = null)
        {
            return JsonUtil.Create(_iListReceiptMoneyService.GetByType(GetCurrentUser().HubId ?? 0, type, empId, fromDate, toDate, pageSize, pageNumber, cols));
        }
        [HttpGet("GetByTypeConfirm")]
        public JsonResult GetByTypeConfirm(int type, int? empId = null, DateTime? fromDate = null, DateTime? toDate = null, int? pageSize = null, int? pageNumber = null, string cols = null)
        {
            return JsonUtil.Create(_iListReceiptMoneyService.GetByTypeConfirm(GetCurrentUser().HubId ?? 0, type, empId, fromDate, toDate, pageSize, pageNumber, cols));
        }

        [HttpGet("GetListByShipmentId")]
        public JsonResult GetListByShipmentId(int shipmentId)
        {
            return JsonUtil.Create(_iListReceiptMoneyService.GetListReceiptByShipmentId(shipmentId));
        }
        [HttpGet("GetListReceiptByShipmentNumber")]
        public JsonResult GetListReceiptByShipmentNumber(string shipmentNumber)
        {
            return JsonUtil.Create(_iListReceiptMoneyService.GetListReceiptByShipmentNumber(shipmentNumber));
        }
        [HttpGet("GetListToConfirmByType")]
        public JsonResult GetListToConfirmByType(int type, DateTime? fromDate = null, DateTime? toDate = null, string bankAccount = null, int? pageSize = null, int? pageNumber = null, string cols = null)
        {
            return JsonUtil.Create(_iListReceiptMoneyService.GetToConfirmByType(GetCurrentUser().HubId ?? 0, type, fromDate, toDate, bankAccount, pageSize, pageNumber, cols));
        }
        #endregion

        #region [Other]
        [HttpGet("GetListReceiptMoneyShipmentByListReceiptMoney")]
        public JsonResult GetListReceiptMoneyShipmentByListReceiptMoney(int id, string cols = null)
        {
            return JsonUtil.Create(_iListReceiptMoneyShipmentService.FindBy(x => x.ListReceiptMoneyId == id, null, null, cols));
        }
        [HttpGet("GetListEmployeeKeepingMoney")]
        public JsonResult GetListEmployeeKeepingMoney(string cols = null)
        {
            int currentHubId = GetCurrentUser().HubId ?? 0;
            var shipmentRepository = _unitOfWork.RepositoryR<Shipment>();
            var hubRepository = _unitOfWork.RepositoryR<Hub>();
            var hubIds = hubRepository.FindBy(x => x.CenterHubId.Value == currentHubId || x.PoHubId == currentHubId || x.Id == currentHubId)
                                    .Select(x => x.Id);
            var userIds = new List<int>();
            var empIds = shipmentRepository.FindBy(x => ((x.TotalPrice > 0 && !x.ListReceiptMoneyTotalPriceId.HasValue && x.KeepingTotalPriceEmpId.HasValue) ||
                                                        (x.COD > 0 && !x.ListReceiptMoneyCODId.HasValue && x.KeepingCODEmpId.HasValue))
                                                        && (x.PaymentTypeId == PaymentTypeHelper.NGUOI_GUI_THANH_TOAN || x.PaymentTypeId == PaymentTypeHelper.NGUOI_NHAN_THANH_TOAN))
                                           .GroupBy(x => new { x.KeepingCODEmpId, x.KeepingTotalPriceEmpId })
                                           .Select(x => new { userId1 = x.FirstOrDefault().KeepingCODEmpId, userId2 = x.FirstOrDefault().KeepingTotalPriceEmpId }).ToList();
            empIds.ForEach(x =>
            {
                if (x.userId1.HasValue && !userIds.Contains(x.userId1.Value)) userIds.Add(x.userId1.Value);
                if (x.userId2.HasValue && !userIds.Contains(x.userId2.Value)) userIds.Add(x.userId2.Value);
            });

            return JsonUtil.Create(_iGeneralServiceRaw.FindBy<User, UserInfoViewModel>(x => hubIds.Contains(x.HubId.Value) && userIds.Contains(x.Id), null, null, cols));
        }
        [HttpGet("GetShipmentHistoryIncomingPayment")]
        public JsonResult GetShipmentHistoryIncomingPayment(int id)
        {
            var datas = _unitOfWork.Repository<Proc_ShipmentHistoryIncomingPayment>().ExecProcedure(
                Proc_ShipmentHistoryIncomingPayment.GetEntityProc(id));
            return JsonUtil.Success(datas);
        }
        [HttpGet("GetListShipmentPushIncomming")]
        public JsonResult GetListShipmentPushIncomming(int id)
        {
            var datas = _unitOfWork.Repository<Proc_GetListShipmentPushIncoming>().ExecProcedure(
                Proc_GetListShipmentPushIncoming.GetEntityProc(id));
            return JsonUtil.Success(datas);
        }
        [HttpPost("PushIncomingPaymentCheck")]
        public async Task<JsonResult> PushIncomingPaymentCheck([FromBody]List<IncomingPaymentViewModel> models)
        {
            if (models == null || models.Count() == 0) return JsonUtil.Error("Dữ liệu xác nhận không hợp lệ, vui lòng kiểm tra lại");
            var _user = this.GetCurrentUser();
            string typePush = "IC";
            List<IncomingPayment> listDataPush = new List<IncomingPayment>();
            foreach (var model in models)
            {
                var data = new IncomingPayment();
                data.CreateUser = _user.VSEOracleCode;
                data.CashFlow = model.CashFlowCode;
                data.CustomerCode = model.CustomerCode;
                data.DocTotal = model.DocTotal;
                data.Total = model.Total;
                data.DocDate = model.PaymentDate; // model.DocDate;
                data.PaymentType = model.PaymentType;
                data.BankAccount = model.BankAccount;
                data.TransferDate = model.PaymentDate;
                data.DocumentNo = model.DocumentNo.Replace("HDN", "");
                data.DocumentNo = data.DocumentNo.Replace("HDCN", "");
                data.Flag = model.Flag;
                data.TMSNumber = model.DocumentNo;
                data.IsTest = 1;
                if (data.Flag == 1) typePush = "JE";
                data.Remarks = string.Format("{0}-{1}-{2}-{3}-{4}", typePush, model.CustomerCode, Util.AcronymFullname(model.CreatedByFullName), model.ListReceiveMoneyCode, model.GrandTotalReal);
                listDataPush.Add(data);
            }
            //
            GSDPApi gSDPApi = new GSDPApi();
            var res = await gSDPApi.PushIncomingPaymentGSDP(listDataPush, _icompanyInformation.Name);
            //
            string message = string.Format("PUSH InCommingPayment GSDP - ErrorCode:{0}, ErrorMessage: {1}, Message: {2}", res.ErrorCode, res.ErrorMessage, res.Message);
            if (res.ErrorCode == 0)
            {
                _unitOfWork.Repository<Proc_SaveLogPushData>()
                .ExecProcedureSingle(Proc_SaveLogPushData.GetEntityProc(res.body, message, true));
                return JsonUtil.Success();
            }
            else
            {
                _unitOfWork.Repository<Proc_SaveLogPushData>()
                .ExecProcedureSingle(Proc_SaveLogPushData.GetEntityProc(res.body, message, false));
                return JsonUtil.Error("Push incoming không thành công, vui lòng thử lại", res.ErrorDatas);
            }
        }
        [HttpPost("PushIncomingPayment")]
        public async Task<JsonResult> PushIncomingPayment([FromBody]List<IncomingPaymentViewModel> models)
        {
            if (models == null || models.Count() == 0) return JsonUtil.Error("Dữ liệu xác nhận không hợp lệ, vui lòng kiểm tra lại");
            var _user = this.GetCurrentUser();
            string typePush = "IC";
            List<IncomingPayment> listDataPush = new List<IncomingPayment>();
            foreach (var model in models)
            {
                var data = new IncomingPayment();
                data.CreateUser = _user.VSEOracleCode;
                data.CashFlow = model.CashFlowCode;
                data.CustomerCode = model.CustomerCode;
                data.DocTotal = model.DocTotal;
                data.Total = model.Total;
                data.DocDate = model.PaymentDate; // model.DocDate;
                data.PaymentType = model.PaymentType;
                data.BankAccount = model.BankAccount;
                data.TransferDate = model.PaymentDate;
                data.DocumentNo = model.DocumentNo.Replace("HDN", "");
                data.DocumentNo = data.DocumentNo.Replace("HDCN", "");
                data.Flag = model.Flag;
                data.TMSNumber = model.DocumentNo;
                data.IsTest = 0;
                if (data.Flag == 1) typePush = "JE";
                data.Remarks = string.Format("{0}-{1}-{2}-{3}-{4}", typePush, model.CustomerCode, Util.AcronymFullname(model.CreatedByFullName), model.ListReceiveMoneyCode, model.GrandTotalReal);
                listDataPush.Add(data);
            }
            //
            GSDPApi gSDPApi = new GSDPApi();
            var res = await gSDPApi.PushIncomingPaymentGSDP(listDataPush, _icompanyInformation.Name);
            //
            string message = string.Format("PUSH InCommingPayment GSDP - ErrorCode:{0}, ErrorMessage: {1}, Message: {2}", res.ErrorCode, res.ErrorMessage, res.Message);
            if (res.ErrorCode == 0)
            {
                _unitOfWork.Repository<Proc_SaveLogPushData>()
                .ExecProcedureSingle(Proc_SaveLogPushData.GetEntityProc(res.body, message, true));
                //
                foreach (var model in models)
                {
                    _unitOfWork.Repository<Proc_UpdateCountPushVSE>()
                    .ExecProcedureSingle(Proc_UpdateCountPushVSE.GetEntityProc(model.Id, true, null, null, model.Flag));
                }
                //
                return JsonUtil.Success();
            }
            else
            {
                _unitOfWork.Repository<Proc_SaveLogPushData>()
                .ExecProcedureSingle(Proc_SaveLogPushData.GetEntityProc(res.body, message, false));
                return JsonUtil.Error("Push incoming không thành công, vui lòng thử lại", res.ErrorDatas);
            }
        }

        [HttpPost("PushIncomingPaymentToGSDP")]
        public async Task<JsonResult> PushIncomingPaymentToGSDP([FromBody]IncomingPaymentViewModel model)
        {
            var errorMessage = "";
            var _user = this.GetCurrentUser();
            var listData = _unitOfWork.Repository<Proc_GetListShipmentIncomingPayment>().ExecProcedure(
                Proc_GetListShipmentIncomingPayment.GetEntityProc(model.Id));
            if (listData != null && listData.Count() > 0)
            {
                bool isPushFail = false;
                List<IncomingPayment> listDataPush = new List<IncomingPayment>();
                foreach (var item in listData)
                {
                    var data = new IncomingPayment();
                    data.CreateUser = _user.VSEOracleCode;
                    data.CashFlow = model.CashFlowCode;
                    data.CustomerCode = item.CustomerCode;
                    data.DocTotal = item.DocTotal;
                    data.Total = item.Total;
                    data.DocDate = item.DocDate;
                    data.PaymentType = item.PaymentType;
                    data.BankAccount = model.BankAccount;
                    data.TransferDate = model.PaymentDate;
                    data.DocumentNo = item.DocumentNo.Replace("HDN", "");
                    data.DocumentNo = data.DocumentNo.Replace("HDCN", "");
                    //data.DocumentNo = item.DocumentNo.Replace("HD", "");
                    data.Remarks = string.Format("IC-{0}-{1}-{2}-{3}", item.CustomerCode, Util.AcronymFullname(item.CreatedByFullName), item.ListReceiveMoneyCode, model.GrandTotalReal);
                    listDataPush.Add(data);
                }
                //
                GSDPApi gSDPApi = new GSDPApi();
                //
                var res = await gSDPApi.PushIncomingPaymentGSDP(listDataPush, _icompanyInformation.Name);
                //
                string message = string.Format("PUSH InCommingPayment GSDP - ListReceivepMoneyId:{0}, ErrorMessage: {1}", model.Id, res.ErrorMessage);
                bool isSuccess = false;
                if (res.ErrorCode == 0)
                {
                    isSuccess = true;
                }
                else
                {
                    if (res.ErrorMessage.Contains("đã hoàn tất thanh toán trước đó"))
                    {
                        isSuccess = true;
                    }
                    else
                    {
                        isPushFail = true;
                    }
                    errorMessage = res.ErrorMessage;
                }
                using (var context2 = new ApplicationContext())
                {
                    var _unitOfWork2 = new UnitOfWork(context2);
                    _unitOfWork2.Repository<Proc_UpdateCountPushVSE>()
                    .ExecProcedureSingle(Proc_UpdateCountPushVSE.GetEntityProc(model.Id, isSuccess, res.body, message));
                    _unitOfWork2.Commit();
                }
                if (isPushFail != true) return JsonUtil.Success();
                else return JsonUtil.Error(string.Format("Push inCommingPayment error: {0}", errorMessage));
            }
            else
            {
                return JsonUtil.Success("Không có đơn hàng của HDN.");
            }
        }

        #endregion
        [HttpPost("GetListHistoryReceiptMoney")]
        public JsonResult GetListHistoryReceiptMoney([FromBody] ListHistoryReceiptMoneyViewModel viewModel)
        {
            var result = _unitOfWork.Repository<Proc_GetListHistoryReceiptMoney>()
                     .ExecProcedure(Proc_GetListHistoryReceiptMoney.GetEntityProc(viewModel.FromDate, viewModel.ToDate, viewModel.SearchText, viewModel.PageNumber, viewModel.PageSize));
            return JsonUtil.Success(result);
        }
        [HttpGet("GetReceiptMoneyListShipment")]
        public JsonResult GetReceiptMoneyListShipment(int id)
        {
            var result = _unitOfWork.Repository<Proc_GetReceiptMoneyListShipment>()
                     .ExecProcedure(Proc_GetReceiptMoneyListShipment.GetEntityProc(id));
            return JsonUtil.Success(result);
        }

        [HttpPost("GetListHistoryReceiptMoneyDetail")]
        public JsonResult GetListHistoryReceiptMoneyDetail([FromBody] ReceiptMoneyDetailViewModel viewModel)
        {
            var result = _unitOfWork.Repository<Proc_GetListHistoryReceiptMoneyDetail>()
                     .ExecProcedure(Proc_GetListHistoryReceiptMoneyDetail.GetEntityProc(viewModel.ListReceiptMoneyId, viewModel.Id));
            return JsonUtil.Success(result);
        }
    }
}
