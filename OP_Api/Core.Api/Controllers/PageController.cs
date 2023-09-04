using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Business.ViewModels.Pages;
using Core.Data.Abstract;
using Core.Entity.Entities;
using Core.Infrastructure.Helper;
using Core.Infrastructure.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class PageController: GeneralController<PageViewModel, Page>
    {
        public PageController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IOptions<JwtIssuerOptions> jwtOptions,
            IUnitOfWork unitOfWork,
            IGeneralService<PageViewModel, Page> iGeneralService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
        }

        // api filter module theo hệ thống chung
        [HttpGet("GetPageByModuleId")]
        public JsonResult GetPageByModuleId(int id, bool isDisplayAll = false)
        {
            if (isDisplayAll)
                return JsonUtil.Create(_iGeneralService.FindBy(x => x.ModulePageId == id));
            else
                return JsonUtil.Create(_iGeneralService.FindBy(x => x.ModulePageId == id && x.IsEnabled));
        }

        [HttpGet("GetMenuByModuleId")]
        public JsonResult GetMenuByModuleId(int id)
        {
            var currentUser = GetCurrentUserPage();
            var userRole = _unitOfWork.RepositoryR<UserRole>().FindBy(x => x.UserId == currentUser.Id).Select(s => s.RoleId);
            var rolePages = _unitOfWork.RepositoryR<RolePage>().FindBy(x => userRole.Contains(x.RoleId));
            return JsonUtil.Success(_unitOfWork.RepositoryR<Page>().FindBy(x => x.IsEnabled && x.ModulePageId == id && rolePages.Any(rp => rp.PageId == x.Id && rp.IsAccess)).OrderBy(x => x.PageOrder));
        }
    }
}
