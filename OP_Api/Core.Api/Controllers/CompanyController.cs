using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Business.ViewModels.Companies;
using Core.Data.Abstract;
using Core.Entity.Entities;
using Core.Infrastructure.Helper;
using Core.Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class CompanyController : GeneralController<CompaniesViewModel, Company>
    {
        public CompanyController(
           Microsoft.Extensions.Logging.ILogger<dynamic> logger,
           IOptions<AppSettings> optionsAccessor,
           IOptions<JwtIssuerOptions> jwtOptions,
           IUnitOfWork unitOfWork,
           IGeneralService<CompaniesViewModel, Company> iGeneralService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
        }
        //get danh sách công ty
        [AllowAnonymous]
        [HttpGet("GetCompany")]
        public JsonResult GetCompany()
        {
            var allCompany = _unitOfWork.RepositoryR<Company>().GetAll();
            return JsonUtil.Success(allCompany);
        }
    }
}
