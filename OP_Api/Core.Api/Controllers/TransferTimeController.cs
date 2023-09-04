using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Business.ViewModels.TransferTimes;
using Core.Data.Abstract;
using Core.Entity.Entities;
using Core.Entity.Procedures;
using Core.Infrastructure.Helper;
using Core.Infrastructure.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferTimeController : GeneralController<TransferTimeViewModel, TransferTimeInfoViewModel, TransferTime>
    {
        public TransferTimeController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IOptions<JwtIssuerOptions> jwtOptions,
            IUnitOfWork unitOfWork,
            IGeneralService<TransferTimeViewModel, TransferTimeInfoViewModel, TransferTime> iGeneralService
            ) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
        }

		[HttpGet("CheckExistCodeToCreateUpdateTransferTime")]
		public JsonResult CheckExistCodeToCreateUpdateTransferTime(string code, int? id = null)
		{
			try
			{
				var res = _unitOfWork.Repository<Proc_CheckExistCodeToCreateUpdateTransferTime>()
					   .ExecProcedure(Proc_CheckExistCodeToCreateUpdateTransferTime.GetEntityProc(code, id));


				return JsonUtil.Success(res);
			}
			catch (Exception ex)
			{
				return JsonUtil.Error(ex.Message);
			}
		}


		[HttpGet("GetCutOffTimeNonDuplicate")]
		public JsonResult GetCutOffTimeNonDuplicate(int? cutOffId)
		{
			try
			{
				var res = _unitOfWork.Repository<Proc_GetCutOffTimeNonDuplicate>()
					   .ExecProcedure(Proc_GetCutOffTimeNonDuplicate.GetEntityProc(cutOffId));


				return JsonUtil.Success(res);
			}
			catch (Exception ex)
			{
				return JsonUtil.Error(ex.Message);
			}
		}

		[HttpGet("GetAllTransferTime")]
		public JsonResult GetAllTransferTime()
		{
			try
			{
				var res = _unitOfWork.Repository<Proc_GetAllTransferTime>()
					   .ExecProcedure(Proc_GetAllTransferTime.GetEntityProc());


				return JsonUtil.Success(res);
			}
			catch (Exception ex)
			{
				return JsonUtil.Error(ex.Message);
			}
		}
	}

}