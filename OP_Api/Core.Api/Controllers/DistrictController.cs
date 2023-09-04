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
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class DistrictController : GeneralController<DistrictViewModel, District>
    {
        public DistrictController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IOptions<JwtIssuerOptions> jwtOptions,
            IUnitOfWork unitOfWork,
            IGeneralService<DistrictViewModel, District> iGeneralService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
        }

        [HttpPost("GetDistricts")]
        public JsonResult GetRoles([FromBody]FilterViewModel ViewModel)
        {
            var companyId = GetCurrentCompanyId();
            var data = _unitOfWork.Repository<Proc_GetDistricts>().ExecProcedure(Proc_GetDistricts.GetEntityProc(
                    ViewModel.ProvinceId, ViewModel.PageNumber, ViewModel.PageSize, ViewModel.SearchText, ViewModel.IsRemote, companyId
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


        [HttpPost("GetDistrictByProvinceIds")]
        public JsonResult GetDistrictByProvinceIds([FromBody] IdViewModel model)
        {
            return base.FindBy(x => model.Ids.Contains(x.ProvinceId), cols: model.Cols);
        }

        [HttpGet("UpdateDistrict")]
        public JsonResult UpdateProvince(int districtId)
        {
            var data = _unitOfWork.Repository<Proc_UpdateDistrict>().ExecProcedure(Proc_UpdateDistrict.GetEntityProc(districtId));
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
        [HttpGet("GetDistrictByName")]
        public JsonResult GetDistrictByName(string name, int provinceId)
        {
            var reponsive = _unitOfWork.RepositoryCRUD<District>();
            var provinces = reponsive.FindBy(o => o.ProvinceId == provinceId);
            var resultId = Business.Core.Helpers.StringHelper.GetBestMatches(provinces, "Id", "Name", name, null,
                                                                Business.Core.Helpers.StringHelper._REPLACES_LOCATION_NAME);
            return JsonUtil.Create(_iGeneralService.Get((int)resultId));
        }

    }
}
