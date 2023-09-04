
using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Business.ViewModels;
using Core.Data.Abstract;
using Core.Entity.Entities;
using Core.Entity.Procedures;
using Core.Infrastructure.Helper;
using Core.Infrastructure.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]

    public class UserRelationController : GeneralController<UserRelationViewModel, UserRelation>
    {
        public UserRelationController(
           Microsoft.Extensions.Logging.ILogger<dynamic> logger,
           IOptions<AppSettings> optionsAccessor,
           IOptions<JwtIssuerOptions> jwtOptions,
           IUnitOfWork unitOfWork,
           IGeneralService<UserRelationViewModel, UserRelation> iGeneralService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
        }

        [HttpPost("GetUserRelation")]
        public JsonResult GetUserRelation([FromBody] FilterViewModel ViewModel)
        {
            var data = _unitOfWork.Repository<Proc_GetUserRelationByUserId>().ExecProcedure(Proc_GetUserRelationByUserId.GetEntityProc(ViewModel.PageNumber, ViewModel.PageSize, ViewModel.UserId));
            if (!Util.IsNull(data))
            {
                return JsonUtil.Success(data);
            }
            else
            {
                return JsonUtil.Error("Get data error!!!");
            }
        }

        [HttpGet("GetUserByUserRelationId")]
        public JsonResult GetUserByUserRelationId( int? userId)
        {
            var data = _unitOfWork.Repository<Proc_GetUserByUserRelationId>().ExecProcedure(Proc_GetUserByUserRelationId.GetEntityProc(userId));
            if (!Util.IsNull(data))
            {
                return JsonUtil.Success(data);
            }
            else
            {
                return JsonUtil.Error("Get data error!!!");
            }
        }

        [HttpPost("CreateUserRelation")]
        public JsonResult CreateUserRelation([FromBody] UserRelationViewModel ViewModel)
        {
            var dataUserRelation = _unitOfWork.RepositoryR<UserRelation>().FindBy(f => f.UserId == ViewModel.UserId && f.UserRelationId == ViewModel.UserRelationId && f.IsEnabled == true);
            if (dataUserRelation.Count() > 0)
            {
                return JsonUtil.Error("Nhóm nhân viên bị trùng");
            }
            var companyId = GetCurrentCompanyId();
            var data = _unitOfWork.Repository<Proc_CreateUserRelation>().ExecProcedure(Proc_CreateUserRelation.GetEntityProc(ViewModel.Code, ViewModel.Name, ViewModel.UserId, ViewModel.UserRelationId, companyId));
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
