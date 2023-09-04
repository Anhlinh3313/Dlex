using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Business.ViewModels;
using Core.Data.Abstract;
using Core.Entity.Entities;
using Core.Infrastructure.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Core.Infrastructure.Utils;
using Core.Entity.Procedures;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class StructureController : GeneralController<StructureViewModel, Structure>
    {
        // GET: api/values
        public StructureController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IOptions<JwtIssuerOptions> jwtOptions,
            IUnitOfWork unitOfWork,
            IGeneralService<StructureViewModel, Structure> iGeneralService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
        }

        [HttpGet("GetByCode")]
        public async Task<JsonResult> GetByCode(string code)
        {
            var data = await _unitOfWork.RepositoryR<Structure>().GetSingleAsync(f=>f.Code==code);
            if (data == null)
            {
                return JsonUtil.Error("Mã cơ cấu không hợp lệ!");
            }
            else
            {
                return JsonUtil.Success(data);
            }
        }

        [HttpPost("GetListStructure")]
        public JsonResult GetListStructure([FromBody] FilterViewModel ViewModel)
        {
            var companyId = GetCurrentCompanyId();
            var data = _unitOfWork.Repository<Proc_GetListStructure>().ExecProcedure(Proc_GetListStructure.GetEntityProc(ViewModel.PageNumber, ViewModel.PageSize, ViewModel.SearchText, companyId));
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
