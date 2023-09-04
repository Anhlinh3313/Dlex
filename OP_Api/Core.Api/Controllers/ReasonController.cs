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
using Core.Entity.Procedures;
using Core.Infrastructure.Helper;
using Core.Infrastructure.Utils;
using LinqKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class ReasonController : GeneralController<ReasonViewModel, Reason>
    {
        public ReasonController(Microsoft.Extensions.Logging.ILogger<dynamic> logger, IOptions<AppSettings> optionsAccessor, IOptions<JwtIssuerOptions> jwtOptions, IUnitOfWork unitOfWork, IGeneralService<ReasonViewModel, Reason> iGeneralService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
        }

        [HttpGet("GetByType")]
        public JsonResult GetByType(string type)
        {
            Expression<Func<Reason, bool>> predicate = x => x.Id > 0;

            switch (type.ToLower())
            {
                case ReasonHelper.DeliverCancel:
                    {
                        predicate = predicate.And(x => x.DeliverCancel);
                        break;
                    }
                case ReasonHelper.DeliverFail:
                    {
                        predicate = predicate.And(x => x.DeliverFail);
                        break;
                    }
                case ReasonHelper.PickCancel:
                    {
                        predicate = predicate.And(x => x.PickCancel);
                        break;
                    }
                case ReasonHelper.PickFail:
                    {
                        predicate = predicate.And(x => x.PickFail);
                        break;
                    }
                case ReasonHelper.PickReject:
                    {
                        predicate = predicate.And(x => x.PickReject);
                        break;
                    }
                case ReasonHelper.ReturnCancel:
                    {
                        predicate = predicate.And(x => x.ReturnCancel);
                        break;
                    }
                case ReasonHelper.ReturnFail:
                    {
                        predicate = predicate.And(x => x.ReturnFail);
                        break;
                    }
                case ReasonHelper.Delay:
                    {
                        predicate = predicate.And(x => x.IsDelay);
                        break;
                    }
                case ReasonHelper.Incidents:
                    {
                        predicate = predicate.And(x => x.IsIncidents);
                        break;
                    }
                case ReasonHelper.UnlockListGood:
                    {
                        predicate = predicate.And(x => x.IsUnlockListGood);
                        break;
                    }
            }

            return base.FindBy(predicate);
        }

        [HttpPost("GetListReason")]
        public JsonResult GetListReason([FromBody] FilterViewModel ViewModel)
        {
            var companyId = GetCurrentCompanyId();
            var data = _unitOfWork.Repository<Proc_GetListReason>().ExecProcedure(Proc_GetListReason.GetEntityProc(ViewModel.PageNumber, ViewModel.PageSize, ViewModel.SearchText, companyId));
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
