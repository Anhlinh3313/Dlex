using System;
using System.Threading.Tasks;
using Core.Business.ViewModels;
using Core.Entity.Entities;
using Core.Infrastructure.ViewModels;

namespace Core.Business.Services.Abstract
{
    public interface IListGoodsService
    {
        Task<ListGoodsInfoViewModel> UpdateCode(ListGoods listGoods);
        ResponseViewModel PostByType(int type, int? pageSize = null, int? pageNumber = null, string cols = null, ListGoodsFilterViewModel filterViewModel = null);
    }
}
