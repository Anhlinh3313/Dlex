using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Api.Library;
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
    public class DiscountController : GeneralController<DiscountViewModel, Discount>
    {
        public DiscountController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IOptions<JwtIssuerOptions> jwtOptions,
            IUnitOfWork unitOfWork,
            IGeneralService<DiscountViewModel, Discount> iGeneralService
            ) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
        }

        [HttpGet("GetDiscountByInfoPayment")]
        public JsonResult GetDiscountByInfoPayment(int categoryPaymentId, int senderId, bool isSuccess, DateTime formDate, DateTime toDate, int? listPaymentId = null)
        {
            var listDiscount = _unitOfWork.Repository<Proc_GetDiscount>()
                        .ExecProcedure(Proc_GetDiscount.GetEntityProc(categoryPaymentId,senderId, isSuccess, formDate, toDate, listPaymentId)).ToList();
            return JsonUtil.Success(listDiscount);
        }

        [HttpGet("GetAllDiscount")]
        public JsonResult GetAllDiscount(DateTime? fromDate = null, DateTime? toDate = null, int? customerId = null, int? pageNumber = null, int? pageSize = null)
        {
            var listDiscount = _unitOfWork.Repository<Proc_GetAllDiscount>()
                               .ExecProcedure(Proc_GetAllDiscount.GetEntityProc(fromDate,toDate,customerId, pageNumber, pageSize)).ToList();
            return JsonUtil.Success(listDiscount);
        }
    }
}
