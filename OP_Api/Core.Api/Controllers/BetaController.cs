using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Business.Services.Models;
using Core.Data.Abstract;
using Core.Entity.Procedures;
using Core.Infrastructure.Api;
using Core.Infrastructure.Helper;
using Core.Infrastructure.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BetaController : BaseController
    {
        public BetaController(Microsoft.Extensions.Logging.ILogger<dynamic> logger, IOptions<AppSettings> optionsAccessor, IOptions<JwtIssuerOptions> jwtOptions, IUnitOfWork unitOfWork) : base(logger, optionsAccessor, jwtOptions, unitOfWork)
        {
        }

        [HttpGet("TestGooglePubSub")]
        public async Task<JsonResult> TestGooglePubSub()
        {
            ApiGooglePubSub apiGooglePubSub = new ApiGooglePubSub();
            var res = await apiGooglePubSub.PublishToTopic();
            return JsonUtil.Success();
        }

        [HttpGet("TestGetStatusPush")]
        public JsonResult TestGetStatusPush(int senderId)
        {
            var data = _unitOfWork.Repository<Proc_GetShipmentStatusPush>().ExecProcedure(Proc_GetShipmentStatusPush.GetEntityProc(senderId));
            return JsonUtil.Success(data);
        }
    }
}