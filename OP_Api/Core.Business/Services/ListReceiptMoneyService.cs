using System;
using System.Linq;
using System.Linq.Expressions;
using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Business.ViewModels;
using Core.Business.ViewModels.Shipments;
using Core.Data.Abstract;
using Core.Entity.Entities;
using Core.Entity.Procedures;
using Core.Infrastructure.Helper;
using Core.Infrastructure.Helper.ExceptionHelper;
using Core.Infrastructure.ViewModels;
using LinqKit;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using AutoMapper;
using System.Collections.Generic;
using Core.Infrastructure.Utils;
using Core.Data;
using Core.Data.Core;
using System.Transactions;

namespace Core.Business.Services
{
    public class ListReceiptMoneyService : GeneralService<ListReceiptMoneyViewModel, ListReceiptMoneyInfoViewModel, ListReceiptMoney>, IListReceiptMoneyService
    {
        private readonly IUserService _iuserService;

        public ListReceiptMoneyService(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IUnitOfWork unitOfWork,
            IUserService iuserService) : base(logger, optionsAccessor, unitOfWork)
        {
            _iuserService = iuserService;
        }
        public override async Task<ResponseViewModel> Create(ListReceiptMoneyViewModel viewModel)
        {
            if (viewModel.Shipments == null || viewModel.Shipments.Count() == 0)
            {
                return ResponseViewModel.CreateError("Chưa chọn vận đơn");
            }
            if ((viewModel.ListReceiptMoneyTypeId ?? 0) == 0)
            {
                return ResponseViewModel.CreateError("Không tìm thấy loại bảng kê nộp tiền");
            }
            if (viewModel.ListReceiptMoneyTypeId.Value == ListReceiptMoneyTypeHelper.EMPLOYEE || viewModel.IsDirectly == true)
            {
                if (!viewModel.PaidByEmpId.HasValue || !_unitOfWork.RepositoryR<User>().Any(x => x.Id == viewModel.PaidByEmpId.Value))
                {
                    return ResponseViewModel.CreateError("Không tìm thấy thông tin nhân viên nộp tiền");
                }
                if (_unitOfWork.RepositoryR<Shipment>().Any(lsCusPayment => viewModel.Shipments.Any(y53 => lsCusPayment.Id == y53.Id && ((y53.COD > 0 && lsCusPayment.KeepingCODEmpId.HasValue && lsCusPayment.KeepingCODEmpId.Value != viewModel.PaidByEmpId.Value)
                                                       || (y53.TotalPrice > 0 && lsCusPayment.KeepingTotalPriceEmpId.HasValue && lsCusPayment.KeepingTotalPriceEmpId.Value != viewModel.PaidByEmpId.Value)))))
                {
                    var shipmentToReceipMoney = _unitOfWork.RepositoryR<Shipment>().FindBy(lsCusPayment => viewModel.Shipments.Any(y => lsCusPayment.Id == y.Id && ((y.COD > 0 && lsCusPayment.KeepingCODEmpId.HasValue && lsCusPayment.KeepingCODEmpId.Value != viewModel.PaidByEmpId.Value)
                                                               || (y.TotalPrice > 0 && lsCusPayment.KeepingTotalPriceEmpId.HasValue && lsCusPayment.KeepingTotalPriceEmpId.Value != viewModel.PaidByEmpId.Value))));
                    var shipmentNumbers = string.Join(",", shipmentToReceipMoney.Select(x => x.ShipmentNumber));

                    return ResponseViewModel.CreateError($"{shipmentNumbers} hiện không giữ COD hoặc cước phí");
                }
            }
            else
            {
                if (!viewModel.FromHubId.HasValue || !_unitOfWork.RepositoryR<Hub>().Any(x => x.Id == viewModel.FromHubId.Value))
                {
                    return ResponseViewModel.CreateError("Không tìm thấy thông tin bưu cục nộp tiền");
                }
                if (!viewModel.ToHubId.HasValue || !_unitOfWork.RepositoryR<Hub>().Any(x => x.Id == viewModel.ToHubId.Value))
                {
                    return ResponseViewModel.CreateError("Không tìm thấy thông tin bưu cục xác nhận");
                }
                if (_unitOfWork.RepositoryR<Shipment>().Any(lsCusPayment => viewModel.Shipments.Any(y73 => lsCusPayment.Id == y73.Id && ((y73.COD > 0 && lsCusPayment.KeepingCODHubId.HasValue && lsCusPayment.KeepingCODHubId.Value != viewModel.FromHubId.Value)
                                                      || (y73.TotalPrice > 0 && lsCusPayment.KeepingTotalPriceHubId.HasValue && lsCusPayment.KeepingTotalPriceHubId.Value != viewModel.FromHubId.Value)))))
                    using (var context2 = new ApplicationContext())
                    {
                        var shipmentToReceipMoney = _unitOfWork.RepositoryR<Shipment>().FindBy(lsCusPayment => viewModel.Shipments.Any(y76 => lsCusPayment.Id == y76.Id && ((y76.COD > 0 && lsCusPayment.KeepingCODHubId.HasValue && lsCusPayment.KeepingCODHubId.Value != viewModel.FromHubId.Value)
                                                                   || (y76.TotalPrice > 0 && lsCusPayment.KeepingTotalPriceHubId.HasValue && lsCusPayment.KeepingTotalPriceHubId.Value != viewModel.FromHubId.Value))));
                        var shipmentNumbers = string.Join(",", shipmentToReceipMoney.Select(x => x.ShipmentNumber));

                        return ResponseViewModel.CreateError($"{shipmentNumbers} hiện không giữ COD hoặc cước phí");
                    }
            }
            using (TransactionScope tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    viewModel.ListReceiptMoneyStatusId = ListReceiptMoneyStatusHelper.CREATED;
                    viewModel.TotalPrice = viewModel.Shipments.Sum(o => o.TotalPrice);
                    viewModel.TotalCOD = viewModel.Shipments.Sum(o => o.COD);
                    viewModel.GrandTotal = viewModel.TotalPrice + viewModel.TotalCOD;
                    viewModel.TotalShipment = viewModel.Shipments.Count();
                    if (!viewModel.GrandTotalReal.HasValue || viewModel.GrandTotalReal == 0) viewModel.GrandTotalReal = viewModel.GrandTotal;
                    var listReceiptMoney = Mapper.Map<ListReceiptMoney>(viewModel);
                    if (!listReceiptMoney.CreatedBy.HasValue) listReceiptMoney.CreatedBy = viewModel.PaidByEmpId;
                    _unitOfWork.RepositoryCRUD<ListReceiptMoney>().Insert(listReceiptMoney);
                    _unitOfWork.Commit();
                    if (listReceiptMoney.Id <= 0)
                    {
                        tran.Dispose();
                        return ResponseViewModel.CreateError(string.Format("Tạo bảng {0} kê nộp tiền không thành công, vui lòng thử lại.",listReceiptMoney.Id));
                    }
                        listReceiptMoney.Code = $"BKP{RandomUtil.GetCode(listReceiptMoney.Id, 6)}";
                    _unitOfWork.Repository<Proc_AddShipmentToListReceiptMoney>().ExecProcedureSingle(
                        Proc_AddShipmentToListReceiptMoney.GetEntityProc(listReceiptMoney.Id, viewModel.Shipments, false));
                    InsertSchedule(listReceiptMoney);
                    _unitOfWork.Commit();
                    tran.Complete();
                    return ResponseViewModel.CreateSuccess(Mapper.Map<ListReceiptMoneyInfoViewModel>(listReceiptMoney));
                }
                catch (Exception ex)
                {
                    tran.Dispose();
                    return ResponseViewModel.CreateError(ex.Message);
                }
            }
        }

        public override async Task<ResponseViewModel> Update(ListReceiptMoneyViewModel viewModel)
        {
            try
            {
                var checkListReceiptMoney = _unitOfWork.Repository<Proc_CheckExistGetListGoods>()
                    .ExecProcedureSingle(Proc_CheckExistGetListGoods.GetEntityProc(viewModel.Id));
                if (checkListReceiptMoney == null)
                {
                    return ResponseViewModel.CreateError("Không tìm thấy thông tin bảng kê điều chỉnh");
                }
                if (viewModel.Shipments == null || viewModel.Shipments.Count() == 0)
                {
                    return ResponseViewModel.CreateError("Chưa chọn vận đơn");
                }
                if ((viewModel.ListReceiptMoneyTypeId ?? 0) == 0)
                {
                    return ResponseViewModel.CreateError("Không tìm thấy loại bảng kê nộp tiền");
                }
                if (viewModel.ListReceiptMoneyTypeId.Value == ListReceiptMoneyTypeHelper.EMPLOYEE || viewModel.IsDirectly == true)
                {
                    if (!viewModel.PaidByEmpId.HasValue || !_unitOfWork.RepositoryR<User>().Any(x => x.Id == viewModel.PaidByEmpId.Value))
                    {
                        return ResponseViewModel.CreateError("Không tìm thấy thông tin nhân viên nộp tiền");
                    }
                    if (_unitOfWork.RepositoryR<Shipment>().Any(lsCusPayment => viewModel.Shipments.Any(y53 => lsCusPayment.Id == y53.Id && ((y53.COD > 0 && lsCusPayment.KeepingCODEmpId.HasValue && lsCusPayment.KeepingCODEmpId.Value != viewModel.PaidByEmpId.Value && lsCusPayment.ListReceiptMoneyCODId != viewModel.Id)
                                                           || (y53.TotalPrice > 0 && lsCusPayment.KeepingTotalPriceEmpId.HasValue && lsCusPayment.KeepingTotalPriceEmpId.Value != viewModel.PaidByEmpId.Value && lsCusPayment.ListReceiptMoneyTotalPriceId != viewModel.Id)))))
                    {
                        var shipmentToReceipMoney = _unitOfWork.RepositoryR<Shipment>().FindBy(lsCusPayment => viewModel.Shipments.Any(y => lsCusPayment.Id == y.Id && ((y.COD > 0 && lsCusPayment.KeepingCODEmpId.HasValue && lsCusPayment.KeepingCODEmpId.Value != viewModel.PaidByEmpId.Value && lsCusPayment.ListReceiptMoneyCODId != viewModel.Id)
                                                                   || (y.TotalPrice > 0 && lsCusPayment.KeepingTotalPriceEmpId.HasValue && lsCusPayment.KeepingTotalPriceEmpId.Value != viewModel.PaidByEmpId.Value && lsCusPayment.ListReceiptMoneyTotalPriceId != viewModel.Id))));
                        var shipmentNumbers = string.Join(",", shipmentToReceipMoney.Select(x => x.ShipmentNumber));
                        return ResponseViewModel.CreateError($"{shipmentNumbers} hiện không giữ COD hoặc cước phí");
                    }
                }
                else
                {
                    if (!viewModel.FromHubId.HasValue || !_unitOfWork.RepositoryR<Hub>().Any(x => x.Id == viewModel.FromHubId.Value))
                    {
                        return ResponseViewModel.CreateError("Không tìm thấy thông tin bưu cục nộp tiền");
                    }
                    if (!viewModel.ToHubId.HasValue || !_unitOfWork.RepositoryR<Hub>().Any(x => x.Id == viewModel.ToHubId.Value))
                    {
                        return ResponseViewModel.CreateError("Không tìm thấy thông tin bưu cục xác nhận");
                    }
                    //if (_unitOfWork.RepositoryR<Shipment>().Any(lsCusPayment => viewModel.Shipments.Any(y73 => lsCusPayment.Id == y73.Id && ((y73.COD > 0 && lsCusPayment.KeepingCODHubId.HasValue && lsCusPayment.KeepingCODHubId.Value != viewModel.FromHubId.Value)
                    //                                      || (y73.TotalPrice > 0 && lsCusPayment.KeepingTotalPriceHubId.HasValue && lsCusPayment.KeepingTotalPriceHubId.Value != viewModel.FromHubId.Value)))))
                    //{
                    //    var shipmentToReceipMoney = _unitOfWork.RepositoryR<Shipment>().FindBy(lsCusPayment => viewModel.Shipments.Any(y76 => lsCusPayment.Id == y76.Id && ((y76.COD > 0 && lsCusPayment.KeepingCODHubId.HasValue && lsCusPayment.KeepingCODHubId.Value != viewModel.FromHubId.Value)
                    //                                               || (y76.TotalPrice > 0 && lsCusPayment.KeepingTotalPriceHubId.HasValue && lsCusPayment.KeepingTotalPriceHubId.Value != viewModel.FromHubId.Value))));
                    //    var shipmentNumbers = string.Join(",", shipmentToReceipMoney.Select(x => x.ShipmentNumber));

                    //    return ResponseViewModel.CreateError($"{shipmentNumbers} hiện không giữ COD hoặc cước phí");
                    //}
                }
                if (!viewModel.CreatedBy.HasValue) viewModel.CreatedBy = viewModel.PaidByEmpId;
                viewModel.TotalPrice = viewModel.Shipments.Sum(o => o.TotalPrice);
                viewModel.TotalCOD = viewModel.Shipments.Sum(o => o.COD);
                viewModel.GrandTotal = viewModel.TotalPrice + viewModel.TotalCOD;
                viewModel.TotalShipment = viewModel.Shipments.Count();
                viewModel.FirstLockDate = checkListReceiptMoney.FirstLockDate;
                if (!viewModel.GrandTotalReal.HasValue || viewModel.GrandTotalReal == 0) viewModel.GrandTotalReal = viewModel.GrandTotal;
                var listReceiptMoney = Mapper.Map<ListReceiptMoney>(viewModel);
                _unitOfWork.RepositoryCRUD<ListReceiptMoney>().Update(listReceiptMoney);
                _unitOfWork.Repository<Proc_AddShipmentToListReceiptMoney>().ExecProcedureSingle(
                        Proc_AddShipmentToListReceiptMoney.GetEntityProc(listReceiptMoney.Id, viewModel.Shipments, true));
                InsertSchedule(listReceiptMoney);
                _unitOfWork.Commit();
                return ResponseViewModel.CreateSuccess(Mapper.Map<ListReceiptMoneyInfoViewModel>(listReceiptMoney));
            }
            catch (Exception ex)
            {
                return ResponseViewModel.CreateError(ex.Message);
            }
        }
        public async Task<ResponseViewModel> InsertSchedule(ListReceiptMoney listReceiptMoney)
        {
            var task = Task.Run(() =>
           {
               using (var unitOfWork = new Data.Core.UnitOfWork(new Data.ApplicationContext()))
               {
                   var schedules = unitOfWork.RepositoryR<ListReceiptMoneySchedule>().FindBy(x => (x.ListReceiptMoneyId ?? 0) == listReceiptMoney.Id).ToList();
                   if (schedules.Count() == 0 || (schedules.Count() > 0 && schedules.OrderByDescending(x => x.Id).FirstOrDefault().ListReceiptMoneyStatusId != listReceiptMoney.ListReceiptMoneyStatusId))
                   {
                       var schedule = new ListReceiptMoneySchedule()
                       {
                           AttachmentId = listReceiptMoney.AttachmentId,
                           FromHubId = listReceiptMoney.FromHubId,
                           ToHubId = listReceiptMoney.ToHubId,
                           ListReceiptMoneyStatusId = listReceiptMoney.ListReceiptMoneyStatusId,
                           PaidByEmpId = listReceiptMoney.PaidByEmpId,
                           TotalCOD = listReceiptMoney.TotalCOD,
                           TotalPrice = listReceiptMoney.TotalPrice,
                           GrandTotal = listReceiptMoney.GrandTotal,
                           TotalShipment = listReceiptMoney.TotalShipment,
                           ListReceiptMoneyId = listReceiptMoney.Id
                       };
                       unitOfWork.RepositoryCRUD<ListReceiptMoneySchedule>().Insert(schedule);
                   }
               }
           });
            await task;
            return ResponseViewModel.CreateSuccess();
        }
        public async Task<ResponseViewModel> Lock(ListReceiptMoneyViewModel viewModel)
        {
            var listReceiptMoney = _unitOfWork.RepositoryR<ListReceiptMoney>().GetSingle(viewModel.Id, new[] { "ListReceiptMoneyStatus" });
            if (listReceiptMoney.ListReceiptMoneyStatusId == ListReceiptMoneyStatusHelper.CREATED ||
                listReceiptMoney.ListReceiptMoneyStatusId == ListReceiptMoneyStatusHelper.UNLOCKED)
            {
                if (listReceiptMoney.ListReceiptMoneyStatusId == ListReceiptMoneyStatusHelper.CREATED)
                {
                    listReceiptMoney.FirstLockDate = DateTime.Now;
                }
                listReceiptMoney.ListReceiptMoneyStatusId = ListReceiptMoneyStatusHelper.LOCKED;
                listReceiptMoney.LockDate = DateTime.Now;
                listReceiptMoney.CreatedBy = viewModel.CreatedBy;
                await this.Update(listReceiptMoney);
                await this.InsertSchedule(listReceiptMoney);
                await _unitOfWork.CommitAsync();

                if (viewModel.Shipments == null || viewModel.Shipments.Count() == 0)
                {
                    viewModel.Shipments = _unitOfWork.RepositoryR<ListReceiptMoneyShipment>()
                        .FindBy(x => x.ListReceiptMoneyId == viewModel.Id)
                        .AsEnumerable().Select(x => new ShipmentToReceipt()
                        {
                            Id = x.ShipmentId ?? 0,
                            TotalPrice = x.TotalPrice ?? 0,
                            COD = x.COD ?? 0,
                        }).ToList();
                    //foreach (var item in viewModel.Shipments)
                    //{
                    //    var resUpdate = _unitOfWork.Repository<Proc_UpdateRealExportSAP>()
                    //                    .ExecProcedureSingle(Proc_UpdateRealExportSAP.GetEntityProc(item.Id, 5, null));
                    //}
                }

                return ResponseViewModel.CreateSuccess(listReceiptMoney);
            }
            else
            {
                return ResponseViewModel.CreateError($"Bảng kê hiện {listReceiptMoney.ListReceiptMoneyStatus.Name} nên không được phép khóa");
            }
        }
        public async Task<ResponseViewModel> Unlock(ListReceiptMoneyViewModel viewModel)
        {
            var listReceiptMoney = _unitOfWork.RepositoryR<ListReceiptMoney>().GetSingle(viewModel.Id, new[] { "ListReceiptMoneyStatus" });
            if (listReceiptMoney.ListReceiptMoneyStatusId == ListReceiptMoneyStatusHelper.LOCKED || listReceiptMoney.ListReceiptMoneyStatusId == ListReceiptMoneyStatusHelper.CONFIRMED)
            {
                listReceiptMoney.ListReceiptMoneyStatusId = ListReceiptMoneyStatusHelper.UNLOCKED;
                listReceiptMoney.CancelReason = viewModel.CancelReason;
                listReceiptMoney.ReasonListGoodsId = viewModel.ReasonListGoodsId;
                await this.Update(listReceiptMoney);
                await this.InsertSchedule(listReceiptMoney);
                await _unitOfWork.CommitAsync();
                return ResponseViewModel.CreateSuccess(listReceiptMoney);
            }
            else
            {
                return ResponseViewModel.CreateError($"Bảng kê hiện {listReceiptMoney.ListReceiptMoneyStatus.Name} nên không được phép khóa");
            }
        }
        public async Task<ResponseViewModel> Confirm(ListReceiptMoneyViewModel viewModel)
        {
            var listReceiptMoney = _unitOfWork.RepositoryR<ListReceiptMoney>().GetSingle(viewModel.Id, new[] { "ListReceiptMoneyStatus" });
            if (listReceiptMoney.ListReceiptMoneyStatusId == ListReceiptMoneyStatusHelper.LOCKED)
            {
                using (TransactionScope tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        if (viewModel.Shipments == null || viewModel.Shipments.Count() == 0)
                        {
                            viewModel.Shipments = _unitOfWork.RepositoryR<ListReceiptMoneyShipment>()
                                .FindBy(x => x.ListReceiptMoneyId == viewModel.Id)
                                .AsEnumerable().Select(x => new ShipmentToReceipt()
                                {
                                    Id = x.ShipmentId ?? 0,
                                    TotalPrice = x.TotalPrice ?? 0,
                                    COD = x.COD ?? 0,
                                }).ToList();
                            if (viewModel.Shipments.Count() == 0)
                                return ResponseViewModel.CreateError("Chưa chọn vận đơn");
                        }
                        listReceiptMoney.ListReceiptMoneyStatusId = ListReceiptMoneyStatusHelper.CONFIRMED;
                        viewModel.TotalPrice = viewModel.Shipments.Sum(o => o.TotalPrice);
                        viewModel.TotalCOD = viewModel.Shipments.Sum(o => o.COD);
                        viewModel.GrandTotal = viewModel.TotalPrice + viewModel.TotalCOD;
                        viewModel.TotalShipment = viewModel.Shipments.Count();
                        listReceiptMoney.ModifiedWhen = DateTime.Now;
                        listReceiptMoney.ModifiedBy = viewModel.AcceptByUserId;
                        listReceiptMoney.GrandTotalReal = viewModel.GrandTotalReal;
                        listReceiptMoney.FeeBank = viewModel.FeeBank;
                        listReceiptMoney.AccountingAccountId = viewModel.AccountingAccountId;
                        listReceiptMoney.AcceptDate = viewModel.AcceptDate;
                        listReceiptMoney.WarningNote = viewModel.WarningNote;
                        var listReceiptConfirmMoney = new ListReceiptConfirmMoney()
                        {
                            ToHubId = listReceiptMoney.ToHubId,
                            FromHubId = listReceiptMoney.FromHubId,
                            ConfirmByListReceiptMoneyId = listReceiptMoney.Id,
                            ListReceiptMoneyTypeId = listReceiptMoney.ListReceiptMoneyTypeId,
                            TotalShipment = viewModel.TotalShipment,
                            TotalPrice = viewModel.TotalPrice,
                            TotalCOD = viewModel.TotalCOD,
                            GrandTotal = viewModel.GrandTotal,
                        };
                        _unitOfWork.RepositoryCRUD<ListReceiptConfirmMoney>().Insert(listReceiptConfirmMoney);
                        await _unitOfWork.CommitAsync();
                        var shipmentRepository = _unitOfWork.RepositoryCRUD<Shipment>();
                        var listShipmentIds = viewModel.Shipments.Select(s => s.Id);
                        var shipments = shipmentRepository.FindBy(x => listShipmentIds.Contains(x.Id)).AsParallel();
                        shipments.ForAll((shipment) =>
                        {
                            var shipmetModel = viewModel.Shipments.FirstOrDefault(x => shipment.Id == x.Id);
                            if (shipmetModel.COD > 0)
                            {
                                shipment.KeepingCODEmpId = viewModel.AcceptByUserId;
                                shipment.KeepingCODHubId = listReceiptMoney.ToHubId;
                            }
                            if (shipmetModel.TotalPrice > 0)
                            {
                                shipment.KeepingTotalPriceEmpId = viewModel.AcceptByUserId;
                                shipment.KeepingTotalPriceHubId = listReceiptMoney.ToHubId;
                            }
                            //var listReceiptConfirmMoneyShipment = new ListReceiptConfirmMoneyShipment()
                            //{
                            //    ListReceiptConfirmMoneyId = listReceiptConfirmMoney.Id,
                            //    ShipmentId = shipmetModel.Id,
                            //    COD = shipmetModel.COD,
                            //    TotalPrice = shipmetModel.TotalPrice
                            //};
                            //using (var unitOfWork2 = new Data.Core.UnitOfWork(new Data.ApplicationContext()))
                            //{
                            //    var listReceiptConfirmMoneyShipmentRepository = unitOfWork2.RepositoryCRUD<ListReceiptConfirmMoneyShipment>();
                            //    listReceiptConfirmMoneyShipmentRepository.Insert(listReceiptConfirmMoneyShipment);
                            //}
                            //var resUpdate = _unitOfWork.Repository<Proc_UpdateRealExportSAP>()
                            //                .ExecProcedureSingle(Proc_UpdateRealExportSAP.GetEntityProc(shipmetModel.Id, 6, null));
                            //shipmentRepository.Update(shipment);
                        });
                        await _unitOfWork.CommitAsync();
                        tran.Complete();
                        return ResponseViewModel.CreateSuccess(listReceiptMoney);
                    }
                    catch (Exception ex)
                    {
                        tran.Dispose();
                        return ResponseViewModel.CreateError("Xác nhận không thành công, vui lòng thử lại!");
                    }
                }
            }
            else
            {
                return ResponseViewModel.CreateError($"Bảng kê hiện {listReceiptMoney.ListReceiptMoneyStatus.Name} nên không được phép xác nhận");
            }
        }
        public async Task<ResponseViewModel> ReConfirm(ListReceiptMoneyViewModel viewModel)
        {
            var listReceiptMoney = _unitOfWork.RepositoryR<ListReceiptMoney>().GetSingle(viewModel.Id, new[] { "ListReceiptMoneyStatus" });
            if (listReceiptMoney.ListReceiptMoneyStatusId == ListReceiptMoneyStatusHelper.CONFIRMED)
            {
                using (TransactionScope tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        if (viewModel.Shipments == null || viewModel.Shipments.Count() == 0)
                        {
                            viewModel.Shipments = _unitOfWork.RepositoryR<ListReceiptMoneyShipment>()
                                .FindBy(x => x.ListReceiptMoneyId == viewModel.Id)
                                .AsEnumerable().Select(x => new ShipmentToReceipt()
                                {
                                    Id = x.ShipmentId ?? 0,
                                    TotalPrice = x.TotalPrice ?? 0,
                                    COD = x.COD ?? 0,
                                }).ToList();
                            if (viewModel.Shipments.Count() == 0)
                                return ResponseViewModel.CreateError("Chưa chọn vận đơn");
                        }
                        listReceiptMoney.ListReceiptMoneyStatusId = ListReceiptMoneyStatusHelper.CONFIRMED;
                        viewModel.TotalPrice = viewModel.Shipments.Sum(o => o.TotalPrice);
                        viewModel.TotalCOD = viewModel.Shipments.Sum(o => o.COD);
                        viewModel.GrandTotal = viewModel.TotalPrice + viewModel.TotalCOD;
                        viewModel.TotalShipment = viewModel.Shipments.Count();
                        listReceiptMoney.ModifiedWhen = DateTime.Now;
                        listReceiptMoney.ModifiedBy = viewModel.AcceptByUserId;
                        listReceiptMoney.GrandTotalReal = viewModel.GrandTotalReal;
                        listReceiptMoney.FeeBank = viewModel.FeeBank;
                        listReceiptMoney.AccountingAccountId = viewModel.AccountingAccountId;
                        listReceiptMoney.AcceptDate = viewModel.AcceptDate;
                        listReceiptMoney.WarningNote = viewModel.WarningNote;
                        var listReceiptConfirmMoney = _unitOfWork.RepositoryR<ListReceiptConfirmMoney>().GetSingle(f => f.ConfirmByListReceiptMoneyId == listReceiptMoney.Id);
                        //var listReceiptConfirmMoney = new ListReceiptConfirmMoney()
                        //{
                        listReceiptConfirmMoney.ToHubId = listReceiptMoney.ToHubId;
                        listReceiptConfirmMoney.FromHubId = listReceiptMoney.FromHubId;
                        listReceiptConfirmMoney.ConfirmByListReceiptMoneyId = listReceiptMoney.Id;
                        listReceiptConfirmMoney.ListReceiptMoneyTypeId = listReceiptMoney.ListReceiptMoneyTypeId;
                        listReceiptConfirmMoney.TotalShipment = viewModel.TotalShipment;
                        listReceiptConfirmMoney.TotalPrice = viewModel.TotalPrice;
                        listReceiptConfirmMoney.TotalCOD = viewModel.TotalCOD;
                        listReceiptConfirmMoney.GrandTotal = viewModel.GrandTotal;
                        //};
                        _unitOfWork.RepositoryCRUD<ListReceiptConfirmMoney>().InsertAndUpdate(listReceiptConfirmMoney);
                        await _unitOfWork.CommitAsync();
                        //var shipmentRepository = _unitOfWork.RepositoryCRUD<Shipment>();
                        //var listShipmentIds = viewModel.Shipments.Select(s => s.Id);
                        //var shipments = shipmentRepository.FindBy(x => listShipmentIds.Contains(x.Id)).AsParallel();
                        //shipments.ForAll((shipment) =>
                        //{
                        //    var shipmetModel = viewModel.Shipments.FirstOrDefault(x => shipment.Id == x.Id);
                        //    if (shipmetModel.COD > 0)
                        //    {
                        //        shipment.KeepingCODEmpId = viewModel.AcceptByUserId;
                        //        shipment.KeepingCODHubId = listReceiptMoney.ToHubId;
                        //    }
                        //    if (shipmetModel.TotalPrice > 0)
                        //    {
                        //        shipment.KeepingTotalPriceEmpId = viewModel.AcceptByUserId;
                        //        shipment.KeepingTotalPriceHubId = listReceiptMoney.ToHubId;
                        //    }
                        //    //var listReceiptConfirmMoneyShipment = new ListReceiptConfirmMoneyShipment()
                        //    //{
                        //    //    ListReceiptConfirmMoneyId = listReceiptConfirmMoney.Id,
                        //    //    ShipmentId = shipmetModel.Id,
                        //    //    COD = shipmetModel.COD,
                        //    //    TotalPrice = shipmetModel.TotalPrice
                        //    //};
                        //    //using (var unitOfWork2 = new Data.Core.UnitOfWork(new Data.ApplicationContext()))
                        //    //{
                        //    //    var listReceiptConfirmMoneyShipmentRepository = unitOfWork2.RepositoryCRUD<ListReceiptConfirmMoneyShipment>();
                        //    //    listReceiptConfirmMoneyShipmentRepository.Insert(listReceiptConfirmMoneyShipment);
                        //    //}
                        //    //var resUpdate = _unitOfWork.Repository<Proc_UpdateRealExportSAP>()
                        //    //                .ExecProcedureSingle(Proc_UpdateRealExportSAP.GetEntityProc(shipmetModel.Id, 6, null));
                        //    shipmentRepository.Update(shipment);
                        //});
                        //await _unitOfWork.CommitAsync();
                        tran.Complete();
                        return ResponseViewModel.CreateSuccess(listReceiptMoney);
                    }
                    catch (Exception ex)
                    {
                        tran.Dispose();
                        return ResponseViewModel.CreateError("Xác nhận không thành công, vui lòng thử lại!");
                    }
                }
            }
            else
            {
                return ResponseViewModel.CreateError($"Bảng kê hiện {listReceiptMoney.ListReceiptMoneyStatus.Name} nên không được phép xác nhận lại");
            }
        }
        public async Task<ResponseViewModel> Cancel(ListReceiptMoneyViewModel viewModel)
        {
            var listReceiptMoney = _unitOfWork.RepositoryR<ListReceiptMoney>().GetSingle(viewModel.Id, new[] { "ListReceiptMoneyStatus" });
            if (listReceiptMoney.ListReceiptMoneyStatusId == ListReceiptMoneyStatusHelper.CREATED ||
                 listReceiptMoney.ListReceiptMoneyStatusId == ListReceiptMoneyStatusHelper.UNLOCKED)
            {
                try
                {
                    if (viewModel.Shipments == null || viewModel.Shipments.Count() == 0)
                    {
                        viewModel.Shipments = _unitOfWork.RepositoryR<ListReceiptMoneyShipment>()
                            .FindBy(x => x.ListReceiptMoneyId == viewModel.Id)
                            .AsEnumerable().Select(x => new ShipmentToReceipt()
                            {
                                Id = x.ShipmentId ?? 0,
                                TotalPrice = x.TotalPrice ?? 0,
                                COD = x.COD ?? 0,
                            }).ToList();
                        if (viewModel.Shipments.Count() == 0)
                        {
                            listReceiptMoney.ListReceiptMoneyStatusId = ListReceiptMoneyStatusHelper.CANCELLED;
                            listReceiptMoney.IsEnabled = false;
                            _unitOfWork.Commit();
                            return ResponseViewModel.CreateSuccess(listReceiptMoney);
                        }
                    }
                    listReceiptMoney.ListReceiptMoneyStatusId = ListReceiptMoneyStatusHelper.CANCELLED;
                    listReceiptMoney.IsEnabled = false;
                    var shipmentRepository = _unitOfWork.RepositoryCRUD<Shipment>();
                    var listReceiptMoneyShipmentRepository = _unitOfWork.RepositoryCRUD<ListReceiptMoneyShipment>();
                    var listShipmentIds = viewModel.Shipments.Select(s => s.Id);
                    var shipments = shipmentRepository.FindBy(x => listShipmentIds.Contains(x.Id)).AsParallel();
                    var listReceiptMoneyShipments = listReceiptMoneyShipmentRepository.FindBy(x => x.ListReceiptMoneyId == listReceiptMoney.Id).AsParallel();
                    shipments.ForAll((shipment) =>
                    {
                        var shipmetModel = viewModel.Shipments.FirstOrDefault(x => shipment.Id == x.Id);
                        using (var unitOfWork2 = new Data.Core.UnitOfWork(new Data.ApplicationContext()))
                        {
                            var lRSRepository = unitOfWork2.RepositoryCRUD<ListReceiptMoneyShipment>();
                            if (shipment.ListReceiptMoneyCODId == listReceiptMoney.Id)
                            {
                                shipment.KeepingCODEmpId = listReceiptMoney.PaidByEmpId;
                                shipment.KeepingCODHubId = null;
                                var listReceiptMoneyShipmentId = lRSRepository
                                                                .FindBy(x => x.ListReceiptMoneyId != listReceiptMoney.Id && x.COD > 0 && x.ShipmentId == shipment.Id)
                                                                .Max(x => x.ListReceiptMoneyId);
                                shipment.ListReceiptMoneyCODId = listReceiptMoneyShipmentId;
                            }
                            if (shipment.ListReceiptMoneyTotalPriceId == listReceiptMoney.Id)
                            {
                                shipment.KeepingTotalPriceEmpId = listReceiptMoney.PaidByEmpId;
                                shipment.KeepingTotalPriceHubId = null;
                                var listReceiptMoneyShipmentId = lRSRepository
                                                                .FindBy(x => x.ListReceiptMoneyId != listReceiptMoney.Id && x.TotalPrice > 0 && x.ShipmentId == shipment.Id)
                                                                .Max(x => x.ListReceiptMoneyId);
                                shipment.ListReceiptMoneyTotalPriceId = listReceiptMoneyShipmentId;
                            }
                        }
                        shipmentRepository.Update(shipment);
                    });
                    listReceiptMoneyShipments.ForAll((listReceiptMoneyShipment) =>
                    {
                        listReceiptMoneyShipment.IsEnabled = false;
                        listReceiptMoneyShipmentRepository.Update(listReceiptMoneyShipment);
                    });
                    await _unitOfWork.CommitAsync();
                    return ResponseViewModel.CreateSuccess(listReceiptMoney);
                }
                catch (Exception ex)
                {
                    return ResponseViewModel.CreateError(ex.Message);
                }
            }
            else
            {
                return ResponseViewModel.CreateError($"Bảng kê hiện {listReceiptMoney.ListReceiptMoneyStatus.Name} nên không được phép hủy");
            }
        }
        public ResponseViewModel GetByType(int hubId, int type, int? empId, DateTime? fromDate = null, DateTime? toDate = null, int? pageSize = null, int? pageNumber = null, string cols = null)
        {
            try
            {
                //var listHub = _iuserService.GetListHubFromHubId(hubId);
                //Expression<Func<ListReceiptMoney, bool>> predicate = x => x.CreatedWhen.HasValue && listHub.Contains(x.FromHubId.Value) && x.ListReceiptMoneyTypeId == type;
                Expression<Func<ListReceiptMoney, bool>> predicate = x => x.CreatedWhen.HasValue && x.FromHubId == hubId && x.ListReceiptMoneyTypeId == type;
                if (fromDate.HasValue)
                {
                    predicate = predicate.And(x => fromDate.Value.Date <= x.CreatedWhen.Value.Date);
                }
                if (toDate.HasValue)
                {
                    predicate = predicate.And(x => toDate.Value.Date >= x.CreatedWhen.Value.Date);
                }
                if (empId.HasValue) predicate = predicate.And(x => empId == x.PaidByEmpId);
                return FindBy(predicate, pageSize, pageNumber, cols);
            }
            catch (Exception ex)
            {
                return ResponseViewModel.CreateError(ex.Message);
            }
        }

        public ResponseViewModel GetByTypeConfirm(int hubId, int type, int? empId, DateTime? fromDate = null, DateTime? toDate = null, int? pageSize = null, int? pageNumber = null, string cols = null)
        {
            try
            {
                var listHub = _iuserService.GetListHubFromHubId(hubId);
                Expression<Func<ListReceiptMoney, bool>> predicate = x => x.FirstLockDate.HasValue && listHub.Contains(x.FromHubId.Value) && x.ListReceiptMoneyTypeId == type;
                if (fromDate.HasValue)
                {
                    predicate = predicate.And(x => fromDate.Value.Date <= x.FirstLockDate.Value.Date);
                }
                if (toDate.HasValue)
                {
                    predicate = predicate.And(x => toDate.Value.Date >= x.FirstLockDate.Value.Date);
                }
                if (empId.HasValue) predicate = predicate.And(x => empId == x.PaidByEmpId);
                return FindBy(predicate, pageSize, pageNumber, cols);
            }
            catch (Exception ex)
            {
                return ResponseViewModel.CreateError(ex.Message);
            }
        }

        public ResponseViewModel GetToConfirmByType(int hubId, int type, DateTime? fromDate = null, DateTime? toDate = null, string bankAccount = null, int? pageSize = null, int? pageNumber = null, string cols = null)
        {
            try
            {
                Expression<Func<ListReceiptMoney, bool>> predicate = x => x.FirstLockDate.HasValue && x.ToHubId == hubId && x.ListReceiptMoneyTypeId == type;
                if (fromDate.HasValue)
                {
                    predicate = predicate.And(x => fromDate.Value.Date <= x.FirstLockDate.Value.Date);
                }
                if (toDate.HasValue)
                {
                    predicate = predicate.And(x => toDate.Value.Date >= x.FirstLockDate.Value.Date);
                }
                if (!Util.IsNull(bankAccount))
                {
                    predicate = predicate.And(x => x.BankAccount == bankAccount);
                }
                var listReceiptMoneyStatusIds = new int[] {
                    ListReceiptMoneyStatusHelper.CONFIRMED,
                    ListReceiptMoneyStatusHelper.LOCKED,
                    ListReceiptMoneyStatusHelper.UNLOCKED
                };
                predicate = predicate.And(x => listReceiptMoneyStatusIds.Contains(x.ListReceiptMoneyStatusId ?? 0));
                return FindBy(predicate, pageSize, pageNumber, cols);
            }
            catch (Exception ex)
            {
                return ResponseViewModel.CreateError(ex.Message);
            }
        }

        public ResponseViewModel GetListReceiptByShipmentId(int id)
        {
            var shipment = _unitOfWork.RepositoryR<Shipment>().GetSingle(id);
            if (Util.IsNull(shipment))
            {
                return ResponseViewModel.CreateError("Không tìm thấy thông tin vận đơn");
            }
            var listReceiptMoneyShipment = _unitOfWork.RepositoryR<ListReceiptMoneyShipment>().FindBy(f => f.ShipmentId == shipment.Id);
            if (Util.IsNull(listReceiptMoneyShipment) || listReceiptMoneyShipment.Count() == 0)
            {
                return ResponseViewModel.CreateError("Chưa có bảng kê xác nhận thu tiền");
            }
            List<int> listId = listReceiptMoneyShipment.Select(s => s.ListReceiptMoneyId.Value).ToList<int>();
            Expression<Func<ListReceiptMoney, bool>> predicate = x => x.CreatedWhen.HasValue && listId.Contains(x.Id);
            return FindBy(predicate, null, null);
        }

        public ResponseViewModel GetListReceiptByShipmentNumber(string shipmentNumber)
        {
            //var shipment = _unitOfWork.RepositoryR<Shipment>().GetSingle(id);
            var data = _unitOfWork.Repository<Proc_GetByShipmentNumber>()
                .ExecProcedure(Proc_GetByShipmentNumber.GetEntityProc(shipmentNumber));
            if (data.Count() > 0)
            {
                var shipment = data.First();
                if (Util.IsNull(shipment))
                {
                    return ResponseViewModel.CreateError("Không tìm thấy thông tin vận đơn");
                }
                var listReceiptMoneyShipment = _unitOfWork.RepositoryR<ListReceiptMoneyShipment>().FindBy(f => f.ShipmentId == shipment.Id);
                if (Util.IsNull(listReceiptMoneyShipment) || listReceiptMoneyShipment.Count() == 0)
                {
                    return ResponseViewModel.CreateError("Chưa có bảng kê xác nhận thu tiền");
                }
                List<int> listId = listReceiptMoneyShipment.Select(s => s.ListReceiptMoneyId.Value).ToList<int>();
                Expression<Func<ListReceiptMoney, bool>> predicate = x => x.CreatedWhen.HasValue && listId.Contains(x.Id);
                return FindBy(predicate, null, null);
            }
            else
            {
                return ResponseViewModel.CreateError("Không tìm thấy thông tin vận đơn");
            }
        }
    }
}
