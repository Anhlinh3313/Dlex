using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Core.Business.ViewModels;
using Core.Entity.Entities;
using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Data.Abstract;
using Core.Infrastructure.Helper;
using Microsoft.Extensions.Options;
using Core.Infrastructure.Utils;
using Core.Entity.Procedures;
using System.Linq.Expressions;
using LinqKit;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class PriceListController : GeneralController<PriceListViewModel, PriceListInfoViewModel, PriceList>
    {
        public PriceListController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IOptions<JwtIssuerOptions> jwtOptions,
            IUnitOfWork unitOfWork,
            IGeneralService<PriceListViewModel, PriceListInfoViewModel, PriceList> iGeneralService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
        }


        public override async Task<JsonResult> Update([FromBody] PriceListViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return JsonUtil.Error(ModelState);
            }
            var data = await _iGeneralService.Update(viewModel);
            if (viewModel.IsPublic)
            {
                var priceListInHub = _unitOfWork.RepositoryR<PriceList>().FindBy(x => x.IsPublic && x.Id != viewModel.Id && x.HubId == viewModel.HubId);
                foreach (var priceList in priceListInHub)
                {
                    priceList.IsPublic = false;
                }
                await _unitOfWork.RepositoryCRUD<PriceList>().CommitAsync();
            }
            return JsonUtil.Create(data);
        }

        [HttpPost("GetListPrice")]
        public JsonResult GetListPrice([FromBody] FilterViewModel ViewModel)
        {
            var companyId = GetCurrentCompanyId();
            var data = _unitOfWork.Repository<Proc_GetListPrice>().ExecProcedure(Proc_GetListPrice.GetEntityProc(ViewModel.PageNumber, ViewModel.PageSize, ViewModel.SearchText, ViewModel.CenterHubId, companyId));
            if (!Util.IsNull(data))
            {
                return JsonUtil.Success(data);
            }
            else
            {
                return JsonUtil.Error("Get data error!!!");
            }
        }

        //[HttpPost("CopyPriceList")]
        //public JsonResult CopyPriceList([FromBody]CopyPriceListViewModel model)
        //{
        //    var data = _unitOfWork.Repository<Proc_CopyPriceList>().ExecProcedure(Proc_CopyPriceList.GetEntityProc(model.PriceListCopyId, model.PriceListNewId));

        //    var result = data.FirstOrDefault() as Proc_CopyPriceList;
        //    if (result.Result)
        //    {
        //        return JsonUtil.Success("Copy bảng giá thành công");
        //    }
        //    else
        //    {
        //        return JsonUtil.Error("Copy bảng giá lỗi");
        //    }
        //}

        //[HttpGet("GetByHubAndCustomer")]
        //public JsonResult GetByHubAndCustomer(int hubId, int customerId)
        //{
        //    var customerPriceList = _unitOfWork.RepositoryR<CustomerPriceList>().FindBy(g => g.CustomerId == customerId).Select(s => s.PriceListId).ToArray();
        //    var hub = _unitOfWork.RepositoryR<Hub>().GetSingle(f => f.Id == hubId);
        //    if (Util.IsNull(hub))
        //    {
        //        return JsonUtil.Error("Không tìm thấy Hub!");
        //    }
        //    DateTime date = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd"));
        //    Expression<Func<PriceList, bool>> predicate = x => (x.PublicDateFrom <= date && (date <= x.PublicDateTo || x.PublicDateTo == null));
        //    var listHubIds = _unitOfWork.RepositoryR<Hub>().FindBy(f => f.Id == hub.Id || f.Id == hub.CenterHubId || f.Id == hub.PoHubId).Select(s => s.Id).ToArray();
        //    var data = _unitOfWork.RepositoryR<PriceList>().FindBy(
        //        predicate.And(f=>(customerPriceList.Contains(f.Id) && listHubIds.Contains(f.HubId))
        //        || (f.IsPublic == true && listHubIds.Contains(f.HubId)))).OrderBy(o => o.NumOrder);
        //    if (data.Count() > 0)
        //    {
        //        return JsonUtil.Success(data);
        //    }
        //    return JsonUtil.Error("Không tìm thấy bảng giá!");
        //}

    }


}
