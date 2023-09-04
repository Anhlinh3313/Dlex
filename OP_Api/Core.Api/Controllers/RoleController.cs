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

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class RoleController : GeneralController<RoleViewModel, Role>
    {
        public RoleController(Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IOptions<JwtIssuerOptions> jwtOptions,
            IUnitOfWork unitOfWork,
            IGeneralService<RoleViewModel, Role> iGeneralService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
        }

        [HttpPost("GetRoles")]
        public JsonResult GetRoles([FromBody]FilterViewModel ViewModel)
        {
            var companyId = GetCurrentCompanyId();
            var data = _unitOfWork.Repository<Proc_GetRoles>().ExecProcedure(Proc_GetRoles.GetEntityProc(ViewModel.PageNumber, ViewModel.PageSize, ViewModel.SearchText, companyId));
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
