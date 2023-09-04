using Core.Entity.Entities;
using Microsoft.AspNetCore.Mvc;
using Core.Business.ViewModels.Roles;
using Core.Business.Services.Models;
using Microsoft.Extensions.Options;
using Core.Infrastructure.Helper;
using Core.Data.Abstract;
using Core.Business.Services.Abstract;
using Core.Business.ViewModels;
using Core.Infrastructure.Utils;
using Core.Entity.Procedures;
using Core.Business.ViewModels.General;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class ProvinceController : GeneralController<ProvinceViewModel, Province>
    {
        public ProvinceController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IOptions<JwtIssuerOptions> jwtOptions,
            IUnitOfWork unitOfWork,
            IGeneralService<ProvinceViewModel, Province> iGeneralService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
        }

        [HttpPost("GetProvinces")]
        public JsonResult GetRoles([FromBody]FilterViewModel ViewModel)
        {
            var companyId = GetCurrentCompanyId();
            var data = _unitOfWork.Repository<Proc_GetProvinces>().ExecProcedure(Proc_GetProvinces.GetEntityProc(
                    ViewModel.CountryId, ViewModel.PageNumber, ViewModel.PageSize, ViewModel.SearchText, companyId
                ));
            if (!Util.IsNull(data))
            {
                return JsonUtil.Success(data);
            }
            else
            {
                return JsonUtil.Error("Get data error!!!");
            }
        }

        [HttpGet("UpdateProvince")]
        public JsonResult UpdateProvince(int provinceId)
        {
            var data = _unitOfWork.Repository<Proc_UpdateProvince>().ExecProcedure(Proc_UpdateProvince.GetEntityProc(provinceId));
            if (!Util.IsNull(data))
            {
                return JsonUtil.Success(data);
            }
            else
            {
                return JsonUtil.Error("Get data error!!!");
            }
        }

        [AllowAnonymous]
        [HttpGet("GetProvinceByName")]
        public JsonResult GetProvinceByName(string name, int? countryId = 1)
        {
            countryId = (countryId ?? 1);
            var reponsive = _unitOfWork.RepositoryCRUD<Province>();
            var provinces = reponsive.FindBy(o => o.CountryId == countryId);
            var resultId = Business.Core.Helpers.StringHelper.GetBestMatches(provinces, "Id", "Name", name, null,
                                                                Business.Core.Helpers.StringHelper._REPLACES_LOCATION_NAME);
            return JsonUtil.Create(_iGeneralService.Get((int)resultId));
        }
    }
}
