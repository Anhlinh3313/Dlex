using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Business.ViewModels.CutOffTimes;
using Core.Data.Abstract;
using Core.Entity.Entities;
using Core.Entity.Procedures;
using Core.Infrastructure.Helper;
using Core.Infrastructure.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CutOffTimeController : GeneralController<CutOffTimeViewModel, CutOffTimeInfoViewModel, CutOffTime>
    {

        public CutOffTimeController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger, 
            IOptions<AppSettings> optionsAccessor, 
            IOptions<JwtIssuerOptions> jwtOptions, 
            IUnitOfWork unitOfWork, 
            IGeneralService<CutOffTimeViewModel, CutOffTimeInfoViewModel, CutOffTime> iGeneralService
            ) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
        }

        [HttpGet("GetCutOffTime")]
        public JsonResult GetCutOffTime()
        {
            try
            { 
                var data = _unitOfWork.Repository<Proc_GetCutOffTime>()
                    .ExecProcedure(Proc_GetCutOffTime.GetEntityProc());
                return JsonUtil.Success(data);
            }catch(Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }
        }

		[HttpGet("CheckExistToCreateUpdateCutOffTime")]
		public JsonResult CheckExistToCreateUpdateCutOffTime(string code, string listDaysOfWeek, int? id = null)
		{
			try
			{
				var res = _unitOfWork.Repository<Proc_CheckExistToCreateUpdateCutOffTime>()
					   .ExecProcedure(Proc_CheckExistToCreateUpdateCutOffTime.GetEntityProc(code, listDaysOfWeek, id));


				return JsonUtil.Success(res);
			}
			catch (Exception ex)
			{
				return JsonUtil.Error(ex.Message);
			}
		}
	}
}