using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class HolidayController : GeneralController<HolidayViewModel, Holiday>
    {
        public HolidayController(Microsoft.Extensions.Logging.ILogger<dynamic> logger, IOptions<AppSettings> optionsAccessor, IOptions<JwtIssuerOptions> jwtOptions, IUnitOfWork unitOfWork, IGeneralService<HolidayViewModel, Holiday> iGeneralService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
        }

        [HttpGet("GetHolidayByYear")]
        public JsonResult GetHolidayByYear(int year, int? pageNumber = null, int? pageSize = null)
        {
			try
			{
				var companyId = GetCurrentCompanyId();
				var res = _unitOfWork.Repository<Proc_GetHolidayByYear>()
					   .ExecProcedure(Proc_GetHolidayByYear.GetEntityProc(year, pageNumber, pageSize, companyId));

				
				return JsonUtil.Success(res); 
			}
			catch(Exception ex)
			{
				return JsonUtil.Error(ex.Message);
			}
        }

		//[HttpPost("CreateHoliday")]
		//public JsonResult CreateHoliday([FromBody] HolidayViewModel holiday)
		//{
		//	try
		//	{	
		//		var res = _unitOfWork.Repository<Proc_CreateHoliday>()
		//			   .ExecProcedure(Proc_CreateHoliday.GetEntityProc(GetCurrentUserId(), holiday.Code, holiday.Name, holiday.Date, holiday.IsSa,
		//			   holiday.IsSu, holiday.IsFull));


		//		return JsonUtil.Success(res);
		//	}
		//	catch (Exception ex)
		//	{
		//		return JsonUtil.Error(ex.Message);
		//	}
		//}

		//[HttpPost("UpdateHolidayById")]
		//public JsonResult UpdateHolidayById([FromBody] HolidayViewModel holiday)
		//{
		//	try
		//	{
		//		var res = _unitOfWork.Repository<Proc_UpdateHolidayById>()
		//			   .ExecProcedure(Proc_UpdateHolidayById.GetEntityProc(holiday.Id, holiday.Code, holiday.Name, holiday.Date, holiday.IsSa,
		//			   holiday.IsSu, holiday.IsFull));


		//		return JsonUtil.Success(res);
		//	}
		//	catch (Exception ex)
		//	{
		//		return JsonUtil.Error(ex.Message);
		//	}
		//}

		//[HttpPost("DeleteHolidayById")]
		//public JsonResult DeleteHolidayById(int id)
		//{
		//	try
		//	{
		//		var res = _unitOfWork.Repository<Proc_DeleteHolidayById>()
		//			   .ExecProcedure(Proc_DeleteHolidayById.GetEntityProc(id));


		//		return JsonUtil.Success(res);
		//	}
		//	catch (Exception ex)
		//	{
		//		return JsonUtil.Error(ex.Message);
		//	}
		//}

		//[HttpPost("CheckExistToCreateUpdateHoliday")]
		//public JsonResult CheckExistToCreateUpdateHoliday([FromBody] HolidayViewModel holiday)
		//{
		//	try
		//	{
		//		var res = _unitOfWork.Repository<Proc_CheckExistToCreateUpdateHoliday>()
		//			   .ExecProcedure(Proc_CheckExistToCreateUpdateHoliday.GetEntityProc(holiday.Id, holiday.Code, holiday.Date, holiday.IsSa, holiday.IsSu));


		//		return JsonUtil.Success(res);
		//	}
		//	catch (Exception ex)
		//	{
		//		return JsonUtil.Error(ex.Message);
		//	}
		//}
	}
}
