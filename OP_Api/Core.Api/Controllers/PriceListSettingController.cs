using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Business.ViewModels;
using Core.Business.ViewModels.PriceListSettings;
using Core.Data.Abstract;
using Core.Entity.Entities;
using Core.Entity.Procedures;
using Core.Infrastructure.Helper;
using Core.Infrastructure.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class PriceListSettingController : GeneralController<PriceListSettingViewModel, PriceListSetting>
    {
        public PriceListSettingController(
           Microsoft.Extensions.Logging.ILogger<dynamic> logger,
           IOptions<AppSettings> optionsAccessor,
           IOptions<JwtIssuerOptions> jwtOptions,
           IUnitOfWork unitOfWork,
           IGeneralService<PriceListSettingViewModel, PriceListSetting> iGeneralService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
        }

        [HttpPost("GetListPriceListSetting")]
        public JsonResult GetListPriceListSetting([FromBody] FilterViewModel ViewModel)
        {
            var companyId = GetCurrentCompanyId();
            var data = _unitOfWork.Repository<Proc_GetListPriceListSetting>().ExecProcedure(Proc_GetListPriceListSetting.GetEntityProc(ViewModel.PageNumber, ViewModel.PageSize, companyId, 
                ViewModel.CustomerId, ViewModel.ServiceId, ViewModel.PriceListId));
            if (!Util.IsNull(data))
            {
                return JsonUtil.Success(data);
            }
            else
            {
                return JsonUtil.Error("Get data error!!!");
            }
        }
    }
}
