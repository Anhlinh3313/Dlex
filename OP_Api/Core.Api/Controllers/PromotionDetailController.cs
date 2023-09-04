using AutoMapper;
using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Business.ViewModels;
using Core.Business.ViewModels.PromotionDetails;
using Core.Data.Abstract;
using Core.Entity.Entities;
using Core.Infrastructure.Helper;
using Core.Infrastructure.Utils;
using LinqKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class PromotionDetailController : BaseController
    {
        private readonly IPromotionService _iPromotionService;

        public PromotionDetailController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<CompanyInformation> companyInformation,
            IOptions<AppSettings> optionsAccessor, IOptions<JwtIssuerOptions> jwtOptions,
            IUnitOfWork unitOfWork,
            IPromotionService promotionService
            ) : base(logger, optionsAccessor, jwtOptions, unitOfWork)
        {
            _iPromotionService = promotionService;
        }

        [HttpGet("GetListPromotionDetailByPromotionId")]
        public JsonResult GetListPromotionDetailByPromotionId(int promotionId)
        {
            var data = _unitOfWork.RepositoryR<PromotionDetail>().FindBy(x => x.PromotionId == promotionId && x.IsEnabled == true);
            return JsonUtil.Success(Mapper.Map<IEnumerable<PromotionDetailInfo>>(data));
        }
    }
}
