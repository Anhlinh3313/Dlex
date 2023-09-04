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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class PriceServiceController : GeneralController<PriceServiceViewModel, PriceServiceInfoViewModel, PriceService>
    {
        public PriceServiceController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IOptions<JwtIssuerOptions> jwtOptions,
            IUnitOfWork unitOfWork,
            IGeneralService<PriceServiceViewModel, PriceServiceInfoViewModel, PriceService> iGeneralService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
        }

        [HttpGet("GetByCode")]
        public JsonResult GetByCode(string code)
        {
            if (code == "%") code = "";
            else code = code.Replace("%", "");  
            return base.FindBy(x => x.Code.ToUpper().Contains(code.ToUpper()), 20, 1);
        }

        [HttpPost("GetListPriceService")]
        public JsonResult GetListPriceService([FromBody] FilterViewModel ViewModel)
        {
            var companyId = GetCurrentCompanyId();
            var data = _unitOfWork.Repository<Proc_GetListPriceService>().ExecProcedure(Proc_GetListPriceService.GetEntityProc(ViewModel.PageNumber, ViewModel.PageSize, ViewModel.SearchText,
                ViewModel.DateFrom, ViewModel.DateTo, ViewModel.ServiceId, ViewModel.ProvinceFromId, ViewModel.ProvinceToId, companyId));
            if (!Util.IsNull(data))
            {
                return JsonUtil.Success(data);
            }
            else
            {
                return JsonUtil.Error("Get data error!!!");
            }
        }

        //[HttpGet("GetByPriceList")]
        //public JsonResult GetByPriceList(int priceListId)
        //{
        //    return base.FindBy(x => x.PriceListId == priceListId);
        //}

        //[HttpGet("GetDIM")]
        //public JsonResult GetDIM(int priceListId, int serviceId, int? PriceServiceId = 0, int? senderId = 0)
        //{
        //    var data = _unitOfWork.RepositoryR<PriceService>().FindBy(f => f.ServiceId == serviceId && f.PriceListId == priceListId).FirstOrDefault();
        //    return JsonUtil.Success(data);
        //}

        //[HttpGet("GetByPriceListService")]
        //public JsonResult GetByPriceListService(int priceListId, int serviceId)
        //{
        //    return base.FindBy(x => x.PriceListId == priceListId && x.ServiceId == serviceId);
        //}

        //[HttpGet("CopyPriceService")]
        //public JsonResult CopyPriceService(int priceServiceId, string newCode)
        //{
        //    var data = _unitOfWork.Repository<Proc_CopyPriceService>().ExecProcedureSingle(Proc_CopyPriceService.GetEntityProc(priceServiceId, newCode));
        //    return JsonUtil.Success(data);
        //}
    }
}
