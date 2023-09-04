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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class PriceListDVGTController : GeneralController<PriceListDVGTViewModel, PriceListDVGT>
    {
        public PriceListDVGTController
            (
            Microsoft.Extensions.Logging.ILogger<dynamic> logger, 
            IOptions<AppSettings> optionsAccessor, 
            IOptions<JwtIssuerOptions> jwtOptions, 
            IUnitOfWork unitOfWork, 
            IGeneralService<PriceListDVGTViewModel, PriceListDVGT> iGeneralService
            ) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
        }

        public override async Task<JsonResult> Create([FromBody] PriceListDVGTViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return JsonUtil.Error(ModelState);
            }
            var data = await _iGeneralService.Create(viewModel);
            if (viewModel.IsPublic)
            {
                var priceListInHub = _unitOfWork.RepositoryR<PriceListDVGT>().FindBy(x => x.IsPublic && x.Id != viewModel.Id);
                foreach (var priceList in priceListInHub)
                {
                    priceList.IsPublic = false;
                }
                await _unitOfWork.RepositoryCRUD<PriceListDVGT>().CommitAsync();
            }
            return JsonUtil.Create(data);
        }

        [HttpGet("GetByCode")]
        public JsonResult GetByCode(string code)
        {
            return base.FindBy(x => x.Code.ToUpper().Contains(code.ToUpper()), 20, 1);
        }
        //public override async Task<JsonResult> Update([FromBody] PriceListDVGTViewModel viewModel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return JsonUtil.Error(ModelState);
        //    }
        //    var data = await _iGeneralService.Update(viewModel);
        //    //if (viewModel.IsPublic)
        //    //{
        //    //    var priceListInHub = _unitOfWork.RepositoryR<PriceListDVGT>().FindBy(x => x.IsPublic && x.Id != viewModel.Id);
        //    //    foreach (var priceList in priceListInHub)
        //    //    {
        //    //        priceList.IsPublic = false;
        //    //    }
        //    //    await _unitOfWork.RepositoryCRUD<PriceListDVGT>().CommitAsync();
        //    //}
        //    return JsonUtil.Create(data);
        //}
    }
}
