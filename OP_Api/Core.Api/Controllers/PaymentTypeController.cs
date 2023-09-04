using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Core.Business.ViewModels;
using Core.Entity.Entities;
using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Data.Abstract;
using Core.Infrastructure.Helper;
using Microsoft.Extensions.Options;
using Core.Infrastructure.Utils;
using Core.Entity.Procedures;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class PaymentTypeController : GeneralController<PaymentTypeViewModel, PaymentType>
    {
        // GET: api/values
        public PaymentTypeController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger, 
            IOptions<AppSettings> optionsAccessor, 
            IOptions<JwtIssuerOptions> jwtOptions, 
            IUnitOfWork unitOfWork, 
            IGeneralService<PaymentTypeViewModel, PaymentType> iGeneralService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
        }

        [HttpGet("GetByCode")]
        public async Task<JsonResult> GetByCode(string code)
        {
            var data = await _unitOfWork.RepositoryR<PaymentType>().GetSingleAsync(f => f.Code == code);
            if (data == null)
            {
                return JsonUtil.Error("Mã hình thức thanh toán không hợp lệ!");
            }
            else
            {
                return JsonUtil.Success(data);
            }
        }

        public override JsonResult GetAll(int? pageSize = null, int? pageNumber = null, string cols = null)
        {
            var data = _unitOfWork.RepositoryR<PaymentType>().GetAll();
            if (data.Count() > 0)
            {
                List<PaymentType> payments =  data.OrderBy(x => x.SortOrder != null ? x.SortOrder : int.MaxValue).ToList();
                return JsonUtil.Success(payments);
            }
            return JsonUtil.Success();
        }

        [HttpPost("GetListPaymentType")]
        public JsonResult GetListPaymentType([FromBody] FilterViewModel ViewModel)
        {
            var companyId = GetCurrentCompanyId();
            var data = _unitOfWork.Repository<Proc_GetListPaymentType>().ExecProcedure(Proc_GetListPaymentType.GetEntityProc(ViewModel.PageNumber, ViewModel.PageSize, ViewModel.SearchText, companyId));
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
