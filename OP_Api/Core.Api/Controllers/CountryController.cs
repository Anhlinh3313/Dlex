using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Business.ViewModels;
using Core.Business.ViewModels.Companies;
using Core.Business.ViewModels.General;
using Core.Data.Abstract;
using Core.Entity.Entities;
using Core.Entity.Procedures;
using Core.Infrastructure.Helper;
using Core.Infrastructure.Utils;
using LinqKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Linq.Expressions;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class CountryController : GeneralController<CountryViewModel, Country>
    {
        public CountryController(Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IOptions<JwtIssuerOptions> jwtOptions,
            IUnitOfWork unitOfWork,
            IGeneralService<CountryViewModel, Country> iGeneralService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
        }

        [HttpPost("Search")]
        public JsonResult Search([FromBody] SearchViewModel model)
        {
            Expression<Func<Country, bool>> predicate = x => x.IsEnabled;
            if (!Util.IsNull(model.SearchText))
            {
                predicate = predicate.And(x => x.Code.Contains(model.SearchText.Trim()) ||
                x.Name.Contains(model.SearchText.Trim()));
            }
            
            return base.FindBy(predicate, model.PageSize, model.PageNumber, model.Cols);
        }

        [HttpGet("UpdateCountry")]
        public JsonResult UpdateCountry(int countryId)
        {
            var data = _unitOfWork.Repository<Proc_UpdateCountry>().ExecProcedure(Proc_UpdateCountry.GetEntityProc(countryId));
            if (!Util.IsNull(data))
            {
                return JsonUtil.Success(data);
            }
            else
            {
                return JsonUtil.Error("Get data error!!!");
            }
        }

        [HttpPost("GetListCountrys")]
        public JsonResult GetListCountrys([FromBody] FilterViewModel ViewModel)
        {
            var companyId = GetCurrentCompanyId();
            var data = _unitOfWork.Repository<Proc_GetListCountrys>().ExecProcedure(Proc_GetListCountrys.GetEntityProc(ViewModel.PageNumber, ViewModel.PageSize, ViewModel.SearchText, companyId));
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
