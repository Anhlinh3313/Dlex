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
    public class CustomerPriceListDVGTController : GeneralController<CustomerPriceListDVGTViewModel, CustomerPriceListDVGTInfoViewModel, CustomerPriceListDVGT>
    {
        public CustomerPriceListDVGTController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IOptions<JwtIssuerOptions> jwtOptions,
            IUnitOfWork unitOfWork,
            IGeneralService<CustomerPriceListDVGTViewModel, CustomerPriceListDVGTInfoViewModel, CustomerPriceListDVGT> iGeneralService)
            : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
        }

        [HttpGet("GetByPriceListDVGT")]
        public JsonResult GetByPriceListDVGT(int id)
        {
            return JsonUtil.Create(_iGeneralService.FindBy(f => f.PriceListDVGTId == id,null,null,"Customer"));
        }
    }
}