using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Business.ViewModels;
using Core.Data.Abstract;
using Core.Entity.Entities;
using Core.Infrastructure.Helper;
using Core.Infrastructure.Utils;
using Core.Infrastructure.ViewModels;
using LinqKit;
using Microsoft.Extensions.Options;

namespace Core.Business.Services
{
    public class ListGoodsService : GeneralService<ListGoodsCreateUpdateViewModel, ListGoodsViewModel, ListGoods>, IListGoodsService
    {
        public ListGoodsService(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IUnitOfWork unitOfWork) : base(logger, optionsAccessor, unitOfWork)
        {
        }

        public async Task<ListGoodsInfoViewModel> UpdateCode(ListGoods listGoods)
        {
            string bk = $"BK{RandomUtil.GetCode(listGoods.Id, 6)}";
            listGoods.Code = bk;
            listGoods.Name = bk;
            _unitOfWork.RepositoryCRUD<ListGoods>().Update(listGoods);
            await _unitOfWork.CommitAsync();

            var model = Mapper.Map<ListGoodsInfoViewModel>(listGoods);
            model.CreatedHub = _unitOfWork.RepositoryR<Hub>().GetSingle(listGoods.CreatedByHub);

            return model;
        }

        public ResponseViewModel PostByType(int type, int? pageSize = null, int? pageNumber = null, string cols = null, ListGoodsFilterViewModel filterViewModel = null)
        {
            try
            {
                Expression<Func<ListGoods, bool>> predicate = x => x.Id > 0;
                if (!Util.IsNull(filterViewModel))
                {
                    if (!Util.IsNull(filterViewModel.OrderDateFrom))
                    {
                        predicate = predicate.And(x => x.CreatedWhen >= filterViewModel.OrderDateFrom);
                    }
                    if (!Util.IsNull(filterViewModel.OrderDateTo))
                    {
                        predicate = predicate.And(x => x.CreatedWhen <= filterViewModel.OrderDateTo);
                    }
                    if (!Util.IsNull(filterViewModel.type))
                    {
                        if (filterViewModel.type == ListGoodsTypeHelper.BK_NKTTNL)
                        {
                            predicate = predicate.And(x => x.ListGoodsTypeId == ListGoodsTypeHelper.BK_NKTT && x.TotalReceivedError > 0 && x.Note.Contains("Vận đơn lỗi"));
                        }
                        else
                        {
                            predicate = predicate.And(x => x.ListGoodsTypeId == filterViewModel.type);
                        }
                    }
                }
                var data = FindBy(predicate, pageSize, pageNumber, cols);
                return data;
            }
            catch (Exception ex)
            {
                return ResponseViewModel.CreateError(ex.Message);
            }
        }

    }
}
