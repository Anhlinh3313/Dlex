using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Business.ViewModels;
using Core.Business.ViewModels.Companies;
using Core.Data.Abstract;
using Core.Entity.Entities;
using Core.Entity.Procedures;
using Core.Infrastructure.Helper;
using Core.Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class CustomerController : GeneralController<CustomerViewModel, Customer>
    {
        public CustomerController(
           Microsoft.Extensions.Logging.ILogger<dynamic> logger,
           IOptions<AppSettings> optionsAccessor,
           IOptions<JwtIssuerOptions> jwtOptions,
           IUnitOfWork unitOfWork,
           IGeneralService<CustomerViewModel, Customer> iGeneralService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
        }
        [HttpPost("GetCustomerByFilter")]
        public JsonResult GetCustomerByFilter([FromBody]FilterViewModel viewModel)
        {
            var companyId = GetCurrentCompanyId();
            var data = _unitOfWork.Repository<Proc_GetListCustomer>().ExecProcedure(Proc_GetListCustomer.GetEntityProc(
                    viewModel.CustomerId, viewModel.SearchText, viewModel.ProvinceId, viewModel.IsAccept, viewModel.PageSize, viewModel.PageNumber, companyId
                ));
            return JsonUtil.Success(data);
        }

        [HttpGet("UpdateCustomerbyUserFail")]
        public JsonResult UpdateCustomerbyUserFail(int customerId)
        {
            var data = _unitOfWork.Repository<Proc_UpdateCustomerbyUserFail>().ExecProcedure(Proc_UpdateCustomerbyUserFail.GetEntityProc(customerId));
            if (!Util.IsNull(data))
            {
                return JsonUtil.Success(data);
            }
            else
            {
                return JsonUtil.Error("Get data error!!!");
            }
        }

        [HttpGet("RanDomCodeCustomer")]
        public JsonResult RanDomCodeCustomer(string code)
        {
            var data = _unitOfWork.Repository<Proc_RanDomCodeCustomer>().ExecProcedure(Proc_RanDomCodeCustomer.GetEntityProc(code));
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

