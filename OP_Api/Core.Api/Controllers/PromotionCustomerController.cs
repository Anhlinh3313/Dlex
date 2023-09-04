using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Business.ViewModels;
using Core.Business.ViewModels.PromotionCustomers;
using Core.Data.Abstract;
using Core.Entity.Entities;
using Core.Entity.Procedures;
using Core.Infrastructure.Helper;
using Core.Infrastructure.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class PromotionCustomerController : GeneralController<PromotionCustomerViewModel, PromotionCustomer>
    {
        public PromotionCustomerController(
           Microsoft.Extensions.Logging.ILogger<dynamic> logger,
           IOptions<AppSettings> optionsAccessor,
           IOptions<JwtIssuerOptions> jwtOptions,
           IUnitOfWork unitOfWork,
           IGeneralService<PromotionCustomerViewModel, PromotionCustomer> iGeneralService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
        }

        [HttpPost("GetListPromotionCustomer")]
        public JsonResult GetListPromotionCustomer([FromBody] FilterViewModel ViewModel)
        {
            var companyId = GetCurrentCompanyId();
            var data = _unitOfWork.Repository<Proc_GetListPromotionCustomer>().ExecProcedure(Proc_GetListPromotionCustomer.GetEntityProc(ViewModel.PageNumber, ViewModel.PageSize, ViewModel.SearchText,
                ViewModel.PromotionId, ViewModel.CustomerId, companyId));
            if (!Util.IsNull(data))
            {
                return JsonUtil.Success(data);
            }
            else
            {
                return JsonUtil.Error("Get data error!!!");
            }
        }

        [HttpGet("UpdatePromotionCustomer")]
        public JsonResult UpdatePromotionCustomer(int promotionCustomerId)
        {
            var data = _unitOfWork.Repository<Proc_UpdatePromotionCustomer>().ExecProcedure(Proc_UpdatePromotionCustomer.GetEntityProc(promotionCustomerId));
            if (!Util.IsNull(data))
            {
                return JsonUtil.Success(data);
            }
            else
            {
                return JsonUtil.Error("Get data error!!!");
            }
        }

        [HttpGet("CreatePromotionCustomer")]
        public JsonResult CreatePromotionCustomer(int? customerId = null, int? promotionId = null)
        {
            var userId = GetCurrentUserId();
            var companyId = GetCurrentCompanyId();
            var data = _unitOfWork.Repository<Proc_CreatePromotionCustomer>().ExecProcedure(Proc_CreatePromotionCustomer.GetEntityProc(customerId, promotionId, userId, companyId));
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
