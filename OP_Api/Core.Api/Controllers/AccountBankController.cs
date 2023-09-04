
using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Business.ViewModels;
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
    public class AccountBankController : GeneralController<BankAccountViewModel, BankAccountInfoViewModel, BankAccount>
    {
        public AccountBankController(Microsoft.Extensions.Logging.ILogger<dynamic> logger, IOptions<AppSettings> optionsAccessor, IOptions<JwtIssuerOptions> jwtOptions, IUnitOfWork unitOfWork, IGeneralService<BankAccountViewModel, BankAccountInfoViewModel, BankAccount> iGeneralService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {

        }
                
        [HttpGet("GetBankAll")]
        public JsonResult GetBankAll()
        {
            var result = _unitOfWork.RepositoryR<Bank>().FindBy(f => f.Id > 0);
            return JsonUtil.Success(result);
        }

        [HttpGet("GetBranchBy")]
        public JsonResult GetBranchBy(int bankId)
        {
            var result = _unitOfWork.RepositoryR<Branch>().FindBy(f => f.BankId == bankId);
            return JsonUtil.Success(result);
        }

        [HttpPost("GetListBankAccount")]
        public JsonResult GetListBankAccount([FromBody] FilterViewModel ViewModel)
        {
            var companyId = GetCurrentCompanyId();
            var data = _unitOfWork.Repository<Proc_GetListBankAccount>().ExecProcedure(Proc_GetListBankAccount.GetEntityProc(ViewModel.PageNumber, ViewModel.PageSize, ViewModel.SearchText, companyId));
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
