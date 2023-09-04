using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Business.ViewModels;
using Core.Data.Abstract;
using Core.Entity.Entities;
using Core.Infrastructure.Helper;
using Core.Infrastructure.Utils;
using Core.Infrastructure.ViewModels;
using LinqKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class UploadExcelHistoryController : GeneralController<UploadExcelHistoryViewModel, UploadExcelHistoryInfoViewModel, UploadExcelHistory>
    {
        public UploadExcelHistoryController(Microsoft.Extensions.Logging.ILogger<dynamic> logger, IOptions<AppSettings> optionsAccessor, IOptions<JwtIssuerOptions> jwtOptions, IUnitOfWork unitOfWork, IGeneralService<UploadExcelHistoryViewModel, UploadExcelHistoryInfoViewModel, UploadExcelHistory> iGeneralService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
        }

        [AllowAnonymous]
        [HttpGet("GetHistory")]
        public JsonResult GetHistory(DateTime? fromDate, DateTime? toDate, string cols = null)
        {
            Expression<Func<UploadExcelHistory, bool>> predicate = x => x.Id > 0;
            try
            {
                if (!Util.IsNull(fromDate))
                {
                    predicate = predicate.And(x => x.CreatedWhen >= fromDate);
                }
                if (!Util.IsNull(toDate))
                {
                    predicate = predicate.And(x => x.CreatedWhen <= toDate);
                }
                var data = FindBy(predicate, null, null, cols);
                return data;
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }
        }

    }
}
