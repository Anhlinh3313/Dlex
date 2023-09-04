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
using Core.Infrastructure.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerPriceServiceController : GeneralController<CustomerPriceServiceViewModel, CustomerPriceServiceInfoViewModel, CustomerPriceService>
    {
        public CustomerPriceServiceController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IOptions<JwtIssuerOptions> jwtOptions,
            IUnitOfWork unitOfWork,
            IGeneralService<CustomerPriceServiceViewModel, CustomerPriceServiceInfoViewModel, CustomerPriceService> iGeneralService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
        }

        [HttpGet("GetByPriceServiceId")]
        public JsonResult GetByPriceServiceId(int priceServiceId)
        {
            var rs = FindBy(x => x.PriceServiceId == priceServiceId,null,null,"Customer");
            return rs;
        }
    }
}