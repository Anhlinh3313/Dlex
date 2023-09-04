using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Business.Services.Models;
using Core.Data;
using Core.Data.Abstract;
using Core.Data.Core;
using Core.Entity.Procedures;
using Core.Infrastructure.Api;
using Core.Infrastructure.Helper;
using Core.Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationCenterController : BaseController
    {
        private ApplicationContextRRP _contextRRP;
        public NotificationCenterController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger, 
            IOptions<AppSettings> optionsAccessor, 
            IOptions<JwtIssuerOptions> jwtOptions, 
            IUnitOfWork unitOfWork,
            ApplicationContextRRP contextRRP) : base(logger, optionsAccessor, jwtOptions, unitOfWork)
        {
            _contextRRP = contextRRP;
        }

        [Authorize]
        [HttpGet("GetNotificationMenu")]
        public JsonResult GetNotificationMenu()
        {
            int userId = this.GetCurrentUserId();
            var unitOfWordRRP = new UnitOfWorkRRP(_contextRRP);
            var notifi = unitOfWordRRP.Repository<Proc_GetNotificationMenu>().
                ExecProcedureSingle(Proc_GetNotificationMenu.GetEntityProc(userId));
            return JsonUtil.Success(notifi);
        }
    }
}