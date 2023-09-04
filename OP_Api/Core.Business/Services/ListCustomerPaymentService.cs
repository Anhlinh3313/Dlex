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
using System.Transactions;

namespace Core.Business.Services
{
    public class ListCustomerPaymentService : GeneralService<ListCustomerPaymentViewModel, ListCustomerPaymentInfoViewModel, ListCustomerPayment>, IListCustomerPaymentService
    {
        private readonly IUserService _iuserService;

        public ListCustomerPaymentService(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IUnitOfWork unitOfWork,
            IUserService iuserService) : base(logger, optionsAccessor, unitOfWork)
        {
            _iuserService = iuserService;
        }
        public async Task<ResponseViewModel> AddShipmentToPayment(ListCustomerPaymentViewModel viewModel)
        {
            try
            {
                var listCustomerPayment = _unitOfWork.RepositoryCRUD<ListCustomerPayment>().GetSingle(viewModel.Id);
                if (Util.IsNull(listCustomerPayment)) ResponseViewModel.CreateError("Không tìm thấy thông tin bảng kê");
                if (viewModel.ShipmentIds.Count() == 0) ResponseViewModel.CreateError("Vui lòng chọn vận đơn");
                var listShipmentId = string.Join(",", viewModel.ShipmentIds);
                var result = _unitOfWork.Repository<Proc_AddShipmentToListPayment>()
                    .ExecProcedureSingle(Proc_AddShipmentToListPayment.GetEntityProc(viewModel.Id, listShipmentId));
                if (result.IsSuccess == false) return ResponseViewModel.CreateError(result.Message);
                //double grandTotal = 0;
                //int totalShipment = 0;
                //listCustomerPayment.GrandTotal = grandTotal;
                //listCustomerPayment.TotalShipment = totalShipment;
                //await this.InsertSchedule(listCustomerPayment);
                //await _unitOfWork.CommitAsync();
                return ResponseViewModel.CreateSuccess(Mapper.Map<ListCustomerPaymentInfoViewModel>(listCustomerPayment));
            }
            catch (Exception ex)
            {
                return ResponseViewModel.CreateError(ex.Message);
            }
        }
        public async Task<ResponseViewModel> CreateNew(ListCustomerPaymentViewModel viewModel)
        {
            try
            {
                if ((viewModel.ListCustomerPaymentTypeId ?? 0) == 0)
                {
                    return ResponseViewModel.CreateError("Không tìm thấy loại bảng kê nộp tiền");
                }
                if (!viewModel.CustomerId.HasValue || !_unitOfWork.RepositoryR<Customer>().Any(x => x.Id == viewModel.CustomerId.Value))
                {
                    return ResponseViewModel.CreateError("Không tìm thấy thông tin khách hàng cần thanh toán");
                }
                if (!viewModel.HubCreatedId.HasValue || !_unitOfWork.RepositoryR<Hub>().Any(x => x.Id == viewModel.HubCreatedId.Value))
                {
                    return ResponseViewModel.CreateError("Không tìm thấy thông tin bưu cục hiện hành");
                }
                var listCustomerPayment = Mapper.Map<ListCustomerPayment>(viewModel);
                _unitOfWork.RepositoryCRUD<ListCustomerPayment>().Insert(listCustomerPayment);
                await _unitOfWork.CommitAsync();
                double grandTotal = 0;
                int totalShipment = 0;
                listCustomerPayment.GrandTotal = grandTotal;
                listCustomerPayment.TotalShipment = totalShipment;
                if (listCustomerPayment.ListCustomerPaymentTypeId == 1)
                {
                    listCustomerPayment.Code = $"RC{listCustomerPayment.Id.ToString("D5")}";
                }
                else if (listCustomerPayment.ListCustomerPaymentTypeId == 2)
                {
                    listCustomerPayment.Code = $"PAY{listCustomerPayment.Id.ToString("D5")}";
                }
                else
                {
                    listCustomerPayment.Code = $"PR{listCustomerPayment.Id.ToString("D5")}";
                }
                await this.InsertSchedule(listCustomerPayment);
                await _unitOfWork.CommitAsync();
                return ResponseViewModel.CreateSuccess(Mapper.Map<ListCustomerPaymentInfoViewModel>(listCustomerPayment));
            }
            catch (Exception ex)
            {
                return ResponseViewModel.CreateError(ex.Message);
            }
        }
        public override async Task<ResponseViewModel> Create(ListCustomerPaymentViewModel viewModel)
        {
            try
            {
                if (viewModel.ShipmentIds == null || viewModel.ShipmentIds.Count() == 0)
                {
                    return ResponseViewModel.CreateError("Chưa chọn vận đơn");
                }
                if ((viewModel.ListCustomerPaymentTypeId ?? 0) == 0)
                {
                    return ResponseViewModel.CreateError("Không tìm thấy loại bảng kê nộp tiền");
                }
                if (!viewModel.CustomerId.HasValue || !_unitOfWork.RepositoryR<Customer>().Any(x => x.Id == viewModel.CustomerId.Value))
                {
                    return ResponseViewModel.CreateError("Không tìm thấy thông tin khách hàng cần thanh toán");
                }
                if (_unitOfWork.RepositoryR<Shipment>().Any(lsCusPayment => viewModel.ShipmentIds.Contains(lsCusPayment.Id)
                                                                && ((viewModel.ListCustomerPaymentTypeId == ListCustomerPaymentTypeHelper.PAYMENT_COD && lsCusPayment.ListCustomerPaymentCODId.HasValue)
                                                                || (viewModel.ListCustomerPaymentTypeId == ListCustomerPaymentTypeHelper.PAYMENT_FEE && lsCusPayment.ListCustomerPaymentTotalPriceId.HasValue))))
                {
                    var shipmentToReceipMoney = _unitOfWork.RepositoryR<Shipment>().FindBy(lsCusPayment => viewModel.ShipmentIds.Contains(lsCusPayment.Id)
                                                                && ((viewModel.ListCustomerPaymentTypeId == ListCustomerPaymentTypeHelper.PAYMENT_COD && lsCusPayment.ListCustomerPaymentCODId.HasValue)
                                                                || (viewModel.ListCustomerPaymentTypeId == ListCustomerPaymentTypeHelper.PAYMENT_FEE && lsCusPayment.ListCustomerPaymentTotalPriceId.HasValue)));
                    var shipmentNumbers = string.Join(",", shipmentToReceipMoney.Select(x => x.ShipmentNumber));
                    return ResponseViewModel.CreateError($"{shipmentNumbers} hiện đã nằm trong bảng kê thanh toán");
                }
                if (!viewModel.HubCreatedId.HasValue || !_unitOfWork.RepositoryR<Hub>().Any(x => x.Id == viewModel.HubCreatedId.Value))
                {
                    return ResponseViewModel.CreateError("Không tìm thấy thông tin bưu cục hiện hành");
                }
                var listCustomerPayment = Mapper.Map<ListCustomerPayment>(viewModel);
                _unitOfWork.RepositoryCRUD<ListCustomerPayment>().Insert(listCustomerPayment);
                await _unitOfWork.CommitAsync();
                double grandTotal = 0;
                int totalShipment = 0;
                int error = 0;
                using (var unitOfWork = new Data.Core.UnitOfWork(new Data.ApplicationContext()))
                {
                    var shipmentRepository = unitOfWork.RepositoryCRUD<Shipment>();
                    var listCustomerPaymentRepository = unitOfWork.RepositoryCRUD<ListCustomerPaymentShipment>();
                    var shipments = shipmentRepository.FindBy(x => viewModel.ShipmentIds.Contains(x.Id)).AsParallel();
                    shipments.ForAll((shipment) =>
                    {
                        double cod = 0;
                        double totalPrice = 0;
                        if (viewModel.ListCustomerPaymentTypeId == ListCustomerPaymentTypeHelper.PAYMENT_COD)
                        {
                            shipment.ListCustomerPaymentCODId = listCustomerPayment.Id;
                            cod = shipment.COD;
                        }
                        else if (viewModel.ListCustomerPaymentTypeId == ListCustomerPaymentTypeHelper.PAYMENT_FEE)
                        {
                            shipment.ListCustomerPaymentTotalPriceId = listCustomerPayment.Id;
                            totalPrice = shipment.TotalPrice;
                        }
                        var ListCustomerPaymentShipment = new ListCustomerPaymentShipment()
                        {
                            ListCustomerPaymentId = listCustomerPayment.Id,
                            ShipmentId = shipment.Id,
                            COD = cod,
                            TotalPrice = totalPrice
                        };
                        grandTotal += (cod + totalPrice);
                        totalShipment++;
                        try
                        {
                            listCustomerPaymentRepository.Insert(ListCustomerPaymentShipment);
                            shipmentRepository.Update(shipment);
                        }
                        catch (Exception exx)
                        {
                            error++;
                            var a = exx.Message;
                        }
                    });
                    unitOfWork.Commit();
                }
                listCustomerPayment.GrandTotal = grandTotal;
                listCustomerPayment.TotalShipment = totalShipment;
                listCustomerPayment.Code = $"BKCP{RandomUtil.GetCode(listCustomerPayment.Id, 6)}";
                await this.InsertSchedule(listCustomerPayment);
                await _unitOfWork.CommitAsync();
                return ResponseViewModel.CreateSuccess(Mapper.Map<ListCustomerPaymentInfoViewModel>(listCustomerPayment));

            }
            catch (Exception ex)
            {
                return ResponseViewModel.CreateError(ex.Message);
            }
        }
        public async Task<ResponseViewModel> InsertSchedule(ListCustomerPayment listCustomerPayment)
        {
            var task = Task.Run(() =>
           {
               using (var unitOfWork = new Data.Core.UnitOfWork(new Data.ApplicationContext()))
               {
                   var schedule = new ListCustomerPaymentSchedule()
                   {
                       AttachmentId = listCustomerPayment.AttachmentId,
                       HubCreatedId = listCustomerPayment.HubCreatedId,
                       CustomerId = listCustomerPayment.CustomerId,
                       GrandTotal = listCustomerPayment.GrandTotal,
                       TotalShipment = listCustomerPayment.TotalShipment,
                       ListCustomerPaymentId = listCustomerPayment.Id,
                       ListCustomerPaymentTypeId = listCustomerPayment.ListCustomerPaymentTypeId,
                       Paid = listCustomerPayment.Paid,
                       Locked = listCustomerPayment.Locked,
                       DateFrom = listCustomerPayment.DateFrom,
                       DateTo = listCustomerPayment.DateTo
                   };
                   unitOfWork.RepositoryCRUD<ListCustomerPaymentSchedule>().Insert(schedule);
               }
           });
            await task;
            return ResponseViewModel.CreateSuccess();
        }
        public async Task<ResponseViewModel> Lock(ListCustomerPaymentViewModel viewModel)
        {
            var listCustomerPayment = _unitOfWork.RepositoryR<ListCustomerPayment>().GetSingle(viewModel.Id, new[] { "ListCustomerPaymentStatus" });
            if (!(listCustomerPayment.Locked ?? false))
            {
                listCustomerPayment.Locked = true;
                listCustomerPayment.AcceptDateFrom = viewModel.AcceptDateFrom;
                listCustomerPayment.AcceptDateTo = viewModel.AcceptDateTo;
                await this.Update(listCustomerPayment);
                await this.InsertSchedule(listCustomerPayment);
                await _unitOfWork.CommitAsync();
                return ResponseViewModel.CreateSuccess(listCustomerPayment);
            }
            else
            {
                return ResponseViewModel.CreateError($"Bảng kê hiện đã khóa");
            }
        }
        public async Task<ResponseViewModel> Unlock(ListCustomerPaymentViewModel viewModel)
        {
            var listCustomerPayment = _unitOfWork.RepositoryR<ListCustomerPayment>().GetSingle(viewModel.Id, new[] { "ListCustomerPaymentStatus" });
            if ((listCustomerPayment.Locked ?? false))
            {
                listCustomerPayment.Locked = false;
                await this.Update(listCustomerPayment);
                await this.InsertSchedule(listCustomerPayment);
                await _unitOfWork.CommitAsync();
                return ResponseViewModel.CreateSuccess(listCustomerPayment);
            }
            else
            {
                return ResponseViewModel.CreateError($"Bảng kê hiện đã khóa");
            }
        }
        public async Task<ResponseViewModel> Cancel(ListCustomerPaymentViewModel viewModel)
        {
            var listCustomerPayment = _unitOfWork.RepositoryR<ListCustomerPayment>().GetSingle(viewModel.Id, new[] { "ListCustomerPaymentStatus" });
            if (!(listCustomerPayment.Paid ?? false))
            {
                if (!(listCustomerPayment.Locked ?? false))
                {
                    using (TransactionScope tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        try
                        {
                            var shipmenRepository = _unitOfWork.RepositoryCRUD<Shipment>();
                            var shipments = shipmenRepository.FindBy(x => x.ListCustomerPaymentCODId.Value == listCustomerPayment.Id
                                                                                         || x.ListCustomerPaymentTotalPriceId.Value == listCustomerPayment.Id).AsParallel();
                            shipments.ForAll((shipment) =>
                            {
                                if (shipment.ListCustomerPaymentCODId == listCustomerPayment.Id)
                                {
                                    shipment.ListCustomerPaymentCODId = null;
                                }
                                if (shipment.ListCustomerPaymentTotalPriceId == listCustomerPayment.Id)
                                {
                                    shipment.ListCustomerPaymentTotalPriceId = null;
                                }
                            });
                            listCustomerPayment.IsEnabled = false;
                            InsertSchedule(listCustomerPayment);
                            _unitOfWork.Commit();
                            tran.Complete();
                            return ResponseViewModel.CreateSuccess(listCustomerPayment);
                        }
                        catch (Exception ex)
                        {
                            tran.Dispose();
                            return ResponseViewModel.CreateError("Hủy bảng kê không thành công");
                        }
                    }
                }
                else
                {
                    return ResponseViewModel.CreateError($"Bảng kê hiện đã khóa nên không được phép hủy");
                }
            }
            else
            {
                return ResponseViewModel.CreateError($"Bảng kê hiện đã thanh toán nên không được phép hủy");
            }

        }
        public async Task<ResponseViewModel> Pay(ListCustomerPaymentViewModel viewModel)
        {
            var listCustomerPayment = _unitOfWork.RepositoryR<ListCustomerPayment>().GetSingle(viewModel.Id, new[] { "ListCustomerPaymentStatus" });
            if ((listCustomerPayment.Locked ?? false))
            {
                if (!(listCustomerPayment.Paid ?? false))
                {
                    try
                    {
                        var shipmenRepository = _unitOfWork.RepositoryCRUD<Shipment>();
                        var shipments = shipmenRepository.FindBy(x => x.ListCustomerPaymentCODId.Value == listCustomerPayment.Id
                                                                                     || x.ListCustomerPaymentTotalPriceId.Value == listCustomerPayment.Id).AsParallel();
                        shipments.ForAll((shipment) =>
                        {
                            if (shipment.ListCustomerPaymentCODId == listCustomerPayment.Id)
                            {
                                shipment.IsPaidCODToCustomer = true;
                            }
                            if (shipment.ListCustomerPaymentTotalPriceId == listCustomerPayment.Id)
                            {
                                shipment.IsPaidPrice = true;
                            }
                        });
                        listCustomerPayment.Paid = true;
                        InsertSchedule(listCustomerPayment);
                        await _unitOfWork.CommitAsync();
                        return ResponseViewModel.CreateSuccess(listCustomerPayment);
                    }
                    catch (Exception ex)
                    {
                        return ResponseViewModel.CreateError(ex.Message);
                    }
                }
                return ResponseViewModel.CreateError($"Bảng kê hiện đã thanh toán");
            }
            return ResponseViewModel.CreateError($"Bảng kê hiện chưa khóa nên không được phép thanh toán");

        }

        public ResponseViewModel GetByTypeNew(int hubId, int senderId, int? typePaymentId = null)
        {
            try
            {
                var listHub = _iuserService.GetListHubFromHubId(hubId);
                Expression<Func<ListCustomerPayment, bool>> predicate = x => x.CreatedWhen.HasValue && listHub.Contains(x.HubCreatedId.Value) && x.Locked == false
                && (!typePaymentId.HasValue || x.ListCustomerPaymentTypeId == typePaymentId);
                predicate = predicate.And(f => f.CustomerId == senderId);
                return FindBy(predicate);
            }
            catch (Exception ex)
            {
                return ResponseViewModel.CreateError(ex.Message);
            }
        }

        public ResponseViewModel GetByType(int hubId, int? type = null, DateTime? fromDate = null, DateTime? toDate = null, int? pageSize = null, int? pageNumber = null, string cols = null)
        {
            try
            {
                var listHub = _iuserService.GetListHubFromHubId(hubId);
                Expression<Func<ListCustomerPayment, bool>> predicate = x => x.CreatedWhen.HasValue && listHub.Contains(x.HubCreatedId.Value) && (x.ListCustomerPaymentTypeId == type || !type.HasValue);
                if (fromDate.HasValue)
                {
                    predicate = predicate.And(x => fromDate.Value.Date <= x.CreatedWhen.Value.Date);
                }
                if (toDate.HasValue)
                {
                    predicate = predicate.And(x => toDate.Value.Date >= x.CreatedWhen.Value.Date);
                }
                return FindBy(predicate, pageSize, pageNumber, cols);
            }
            catch (Exception ex)
            {
                return ResponseViewModel.CreateError(ex.Message);
            }
        }

        public ResponseViewModel GetListCustomerPaymentByShipmentNumberAndType(string shipmentNumber, int type)
        {
            var data = _unitOfWork.Repository<Proc_GetByShipmentNumber>()
                .ExecProcedure(Proc_GetByShipmentNumber.GetEntityProc(shipmentNumber));
            if (data.Count() > 0)
            {
                var shipment = data.First();
                if (Util.IsNull(shipment))
                {
                    return ResponseViewModel.CreateError("Không tìm thấy thông tin vận đơn");
                }
                var listCustomerPaymentShipment = _unitOfWork.RepositoryR<ListCustomerPaymentShipment>().FindBy(f => f.ShipmentId == shipment.Id);
                if (Util.IsNull(listCustomerPaymentShipment) || listCustomerPaymentShipment.Count() == 0)
                {
                    if (type == ListCustomerPaymentTypeHelper.PAYMENT_FEE)
                    {
                        return ResponseViewModel.CreateError("Chưa có bảng kê thanh toán cước phí");
                    }
                    if (type == ListCustomerPaymentTypeHelper.PAYMENT_COD)
                    {
                        return ResponseViewModel.CreateError("Chưa có bảng kê thanh toán thu hộ");
                    }
                }
                List<int> listId = listCustomerPaymentShipment.Select(s => s.ListCustomerPaymentId.Value).ToList<int>();
                Expression<Func<ListCustomerPayment, bool>> predicate = x => x.CreatedWhen.HasValue && listId.Contains(x.Id) && x.ListCustomerPaymentTypeId == type;
                return FindBy(predicate, null, null);
            }
            else
            {
                return ResponseViewModel.CreateError("Không tìm thấy thông tin vận đơn");
            }
        }
    }
}
