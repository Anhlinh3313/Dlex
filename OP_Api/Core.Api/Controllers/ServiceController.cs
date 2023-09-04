using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Core.Business.ViewModels;
using Core.Entity.Entities;
using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Data.Abstract;
using Core.Infrastructure.Helper;
using Microsoft.Extensions.Options;
using Core.Business.Core.Utils;
using Core.Infrastructure.Utils;
using System.Data;
using Core.Entity.Procedures;
using System.Linq.Expressions;
using LinqKit;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class ServiceController : GeneralController<ServiceViewModel, Service>
    {
        public ServiceController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IOptions<JwtIssuerOptions> jwtOptions,
            IUnitOfWork unitOfWork,
            IGeneralService<ServiceViewModel, Service> iGeneralService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
        }

        [HttpGet("GetByCode")]
        public async Task<JsonResult> GetByCode(string code)
        {
            var data = await _unitOfWork.RepositoryR<Service>().GetSingleAsync(f => f.Code == code);
            if (data == null)
            {
                return JsonUtil.Error("Mã dịch vụ không hợp lệ!");
            }
            else
            {
                return JsonUtil.Success(data);
            }
        }

        [HttpGet("GetListService")]
        public JsonResult GetListService()
        {
            var data = _iGeneralService.FindBy(f => f.IsSub == false && f.IsReturn == false);
            return JsonUtil.Create(data);
        }

        [HttpGet("GetListServiceSub")]
        public JsonResult GetListServiceSub()
        {
            var data = _iGeneralService.FindBy(f => f.IsSub == true && f.IsReturn == false);
            return JsonUtil.Create(data);
        }

        [HttpPost("GetListService")]
        public JsonResult GetListService([FromBody] FilterViewModel ViewModel)
        {
            var companyId = GetCurrentCompanyId();
            var data = _unitOfWork.Repository<Proc_GetListService>().ExecProcedure(Proc_GetListService.GetEntityProc(ViewModel.PageNumber, ViewModel.PageSize, ViewModel.SearchText, companyId));
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
