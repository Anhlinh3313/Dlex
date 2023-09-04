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
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class WardController : GeneralController<WardViewModel, Ward>
    {
        public WardController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IOptions<JwtIssuerOptions> jwtOptions,
            IUnitOfWork unitOfWork,
            IGeneralService<WardViewModel, Ward> iGeneralService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
        }

        [HttpPost("GetWards")]
        public JsonResult GetWards([FromBody] FilterViewModel ViewModel)
        {
            var companyId = GetCurrentCompanyId();
            var data = _unitOfWork.Repository<Proc_GetWards>().ExecProcedure(Proc_GetWards.GetEntityProc(
                    ViewModel.ProvinceId, ViewModel.Districtid, ViewModel.PageNumber, ViewModel.PageSize, ViewModel.SearchText, ViewModel.IsRemote, companyId
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

        [HttpPost("GetWardByDistrictIds")]
        public JsonResult GetWardByDistrictIds([FromBody] GetWardByDistrictIdsViewModel model)
        {
            return base.FindBy(x => model.Ids.Contains(x.DistrictId), cols: model.Cols);
        }

        [AllowAnonymous]
        [HttpGet("GetWardByName")]
        public JsonResult GetWardByName(string name, int districtId)
        {
            var reponsive = _unitOfWork.RepositoryCRUD<Ward>();
            var provinces = reponsive.FindBy(o => o.DistrictId == districtId);
            var resultId = Business.Core.Helpers.StringHelper.GetBestMatches(provinces, "Id", "Name", name, null,
                                                                Business.Core.Helpers.StringHelper._REPLACES_LOCATION_NAME);
            return JsonUtil.Create(_iGeneralService.Get((int)resultId));
        }

        [HttpGet("UpdateWard")]
        public JsonResult UpdateWard(int wardId)
        {
            var data = _unitOfWork.Repository<Proc_UpdateWard>().ExecProcedure(Proc_UpdateWard.GetEntityProc(wardId));
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
