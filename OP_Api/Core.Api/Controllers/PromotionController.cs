using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Business.ViewModels;
using Core.Business.ViewModels.Promotions;
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
    public class PromotionController : BaseController
    {
        private readonly IPromotionService _iPromotionService;

        public PromotionController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<CompanyInformation> companyInformation,
            IOptions<AppSettings> optionsAccessor, IOptions<JwtIssuerOptions> jwtOptions,
            IUnitOfWork unitOfWork,
            IPromotionService promotionService
            ) : base(logger, optionsAccessor, jwtOptions, unitOfWork
        )
        {
            _iPromotionService = promotionService;
        }

        [HttpPost("GetListPromotion")]
        public JsonResult GetListPromotion([FromBody] FilterViewModel ViewModel)
        {
            var companyId = GetCurrentCompanyId();
            var data = _unitOfWork.Repository<Proc_GetListPromotion>().ExecProcedure(Proc_GetListPromotion.GetEntityProc(ViewModel.PageNumber, ViewModel.PageSize, ViewModel.SearchText, ViewModel.DateFrom,
                ViewModel.DateTo, ViewModel.PromotionTypeId, ViewModel.IsPublic, ViewModel.IsHidden, companyId));
            if (!Util.IsNull(data))
            {
                return JsonUtil.Success(data);
            }
            else
            {
                return JsonUtil.Error("Get data error!!!"); 
            }
        }

        [HttpGet("GetAllPromotionType")]
        public JsonResult GetAllPromotionType()
        {
            var data = _unitOfWork.RepositoryR<PromotionType>().GetAll();
            return JsonUtil.Success(data);
        }

        [HttpGet("GetById")]
        public JsonResult GetById(int id)
        {
            var data = _unitOfWork.RepositoryR<Promotion>().GetSingle(id);

            return JsonUtil.Success(data);
        }

        [HttpPost("Create")]
        public JsonResult Create([FromBody] PromotionViewModel viewModel)
        {
            var res = _iPromotionService.Create(viewModel);

            return JsonUtil.Create(res);
        }

        [HttpPost("Update")]
        public JsonResult Update([FromBody] PromotionViewModel viewModel)
        {
            var res = _iPromotionService.Update(viewModel);

            return JsonUtil.Create(res);
        }

        [HttpGet("Delete")]
        public JsonResult Delete(int promotionId)
        {
            var res = _iPromotionService.Delete(promotionId);

            return JsonUtil.Create(res);
        }


    }
}
