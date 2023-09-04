using System;
using System.Collections.Generic;
using Core.Entity.Entities;
using Core.Infrastructure.ViewModels;
using System.Threading.Tasks;
using Core.Business.ViewModels;

namespace Core.Business.Services.Abstract
{
    public interface IListCustomerPaymentService : IGeneralService<ListCustomerPaymentViewModel, ListCustomerPaymentInfoViewModel, ListCustomerPayment>
    {
        Task<ResponseViewModel> AddShipmentToPayment(ListCustomerPaymentViewModel viewModel);
        Task<ResponseViewModel> CreateNew(ListCustomerPaymentViewModel viewModel);
        Task<ResponseViewModel> Lock(ListCustomerPaymentViewModel viewModel);
        Task<ResponseViewModel> Unlock(ListCustomerPaymentViewModel viewModel);
        Task<ResponseViewModel> Pay(ListCustomerPaymentViewModel viewModel);
        Task<ResponseViewModel> Cancel(ListCustomerPaymentViewModel viewModel);
        ResponseViewModel GetByType(int hubId, int? type = null, DateTime? fromDate = null, DateTime? toDate = null, int? pageSize = null, int? pageNumber = null, string cols = null);
        ResponseViewModel GetByTypeNew(int hubId, int senderId, int? typePaymentId = null);
        ResponseViewModel GetListCustomerPaymentByShipmentNumberAndType(string shipmentNumber, int type);
    }
}
