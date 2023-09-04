using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Business.ViewModels;
using Core.Business.ViewModels.PromotionFormulas;
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
    public class PromotionFormulaController: GeneralController<PromotionFormulaViewModel, PromotionFormula>
    {
        public PromotionFormulaController(
           Microsoft.Extensions.Logging.ILogger<dynamic> logger,
           IOptions<AppSettings> optionsAccessor,
           IOptions<JwtIssuerOptions> jwtOptions,
           IUnitOfWork unitOfWork,
           IGeneralService<PromotionFormulaViewModel, PromotionFormula> iGeneralService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
        }

        [HttpPost("GetListPromotionFormula")]
        public JsonResult GetListPromotionFormula([FromBody] FilterViewModel ViewModel)
        {
            var companyId = GetCurrentCompanyId();
            var data = _unitOfWork.Repository<Proc_GetListPromotionFormula>().ExecProcedure(Proc_GetListPromotionFormula.GetEntityProc(ViewModel.PageNumber, ViewModel.PageSize, ViewModel.SearchText, companyId));
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
