using System;
using System.Collections.Generic;
using Core.Entity.Entities;
using Core.Infrastructure.ViewModels;
using System.Threading.Tasks;
using Core.Business.ViewModels;

namespace Core.Business.Services.Abstract
{
    public interface IListReceiptMoneyService : IGeneralService<ListReceiptMoneyViewModel, ListReceiptMoneyInfoViewModel, ListReceiptMoney>
    {
        Task<ResponseViewModel> Lock(ListReceiptMoneyViewModel viewModel);
        Task<ResponseViewModel> Unlock(ListReceiptMoneyViewModel viewModel);
        Task<ResponseViewModel> Confirm(ListReceiptMoneyViewModel viewModel);
        Task<ResponseViewModel> ReConfirm(ListReceiptMoneyViewModel viewModel);
        Task<ResponseViewModel> Cancel(ListReceiptMoneyViewModel viewModel);
        ResponseViewModel GetByType(int hubId, int type, int? empId, DateTime? fromDate = null, DateTime? toDate = null, int? pageSize = null, int? pageNumber = null, string cols = null);
        ResponseViewModel GetByTypeConfirm(int hubId, int type, int? empId, DateTime? fromDate = null, DateTime? toDate = null, int? pageSize = null, int? pageNumber = null, string cols = null);
        ResponseViewModel GetToConfirmByType(int hubId, int type, DateTime? fromDate = null, DateTime? toDate = null, string bankAccount = null, int? pageSize = null, int? pageNumber = null, string cols = null);
        ResponseViewModel GetListReceiptByShipmentId(int id);
        ResponseViewModel GetListReceiptByShipmentNumber(string shipmentNumber);
    }
}
