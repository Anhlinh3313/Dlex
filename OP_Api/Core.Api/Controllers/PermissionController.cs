using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Data.Abstract;
using Core.Entity.Entities;
using Core.Entity.Procedures;
using Core.Infrastructure.Helper;
using Core.Infrastructure.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class PermissionController : BaseController
    {
        private readonly IPermissionService _iPermissionService;

        public PermissionController(
           Microsoft.Extensions.Logging.ILogger<dynamic> logger,
           IOptions<AppSettings> optionsAccessor,
           IOptions<JwtIssuerOptions> jwtOptions,
           IUnitOfWork unitOfWork,
           IPermissionService iPermissionService) : base(logger, optionsAccessor, jwtOptions, unitOfWork)
        {
            _iPermissionService = iPermissionService;
        }

        // api filter chức vụ theo điều phối
        [HttpGet("GetByRoleId")]
        public JsonResult GetByRoleId(int roleId)
        {
            return JsonUtil.Success(_unitOfWork.RepositoryR<RolePage>().FindBy(x => x.RoleId == roleId && x.IsEnabled == true));
        }

        // update phân quyền
        [HttpPost("UpdatePermission")]
        public async Task<JsonResult> UpdatePermission([FromBody] List<RolePage> map)
        {
            foreach (var item in map)
            {
                if (item.Id == 0)
                    _unitOfWork.RepositoryCRUD<RolePage>().Insert(item);
                else
                    _unitOfWork.RepositoryCRUD<RolePage>().Update(item);
            }
            await _unitOfWork.CommitAsync();

            return JsonUtil.Success(map);
        }

        [HttpGet("CheckPermissionDetail")]
        public JsonResult CheckPermissionDetail(string aliasPath, int moduleId)
        {
            var data = _unitOfWork.Repository<Proc_PermissionCheckDetail>()
                          .ExecProcedure(Proc_PermissionCheckDetail.GetEntityProc(GetCurrentUserId(), aliasPath, moduleId));
            return JsonUtil.Success(data);
        }

        [HttpGet("GetByRolePageByPageId")]
        public JsonResult GetByRoleId(int roleId,int pageId)
        {
            return JsonUtil.Success(_unitOfWork.RepositoryR<RolePage>().FindBy(x => x.RoleId == roleId && x.PageId == pageId && x.IsEnabled == true));
        }

        [HttpGet("GetAllPermissionByRoleId")]
        public JsonResult GetPermissionByRoleId(int roleId)
        {
            var data = _unitOfWork.Repository<Proc_GetAllPermissionByRoleId>()
                          .ExecProcedure(Proc_GetAllPermissionByRoleId.GetEntityProc(roleId));
            return JsonUtil.Success(data);
        }
    }
}
