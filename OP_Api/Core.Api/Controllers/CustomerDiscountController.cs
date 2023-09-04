using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Business.ViewModels;
using Core.Business.ViewModels.Discounts;
using Core.Business.ViewModels.KPIShipments;
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
    public class CustomerDiscountController : GeneralController<CustomerDiscountViewModel, CustomerDiscount>
    {
        public CustomerDiscountController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IOptions<JwtIssuerOptions> jwtOptions,
            IUnitOfWork unitOfWork,
            IGeneralService<CustomerDiscountViewModel, CustomerDiscount> iGeneralService
            ) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
        }
        [HttpGet("GetCustomerDiscount")]
        public JsonResult GetCustomerDiscount(int discountId)
        {
            var listDiscount = _unitOfWork.Repository<Proc_GetCustomerDiscount>()
                               .ExecProcedure(Proc_GetCustomerDiscount.GetEntityProc(discountId)).ToList();
            return JsonUtil.Success(listDiscount);
        }
    }
}
