using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class TransportTypeController : GeneralController<TransportTypeViewModel, TransportType>
    {
        private readonly IGeneralService _iGeneralServiceRaw;
        public TransportTypeController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IOptions<JwtIssuerOptions> jwtOptions,
            IGeneralService iGeneralServiceRaw,
            IUnitOfWork unitOfWork, IGeneralService<TransportTypeViewModel, TransportType> iGeneralService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
            _iGeneralServiceRaw = iGeneralServiceRaw;
        }

        [HttpGet("GetTPLTransportType")]
        public JsonResult GetTPLTransportType()
        {
            return JsonUtil.Success(_unitOfWork.RepositoryR<TPLTransportType>().GetAll(x => x.TPL, x => x.TransportType));
        }

        public override async Task<JsonResult> Create([FromBody]TransportTypeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return JsonUtil.Error(ModelState);
            }
            var data = await _iGeneralServiceRaw.Create<TransportType, TransportTypeViewModel>(viewModel);
            if (data.IsSuccess)
            {
                var transportType = data.Data as TransportType;
                if (viewModel.TPLIds != null && viewModel.TPLIds.Count() > 0)
                {
                    foreach (var tplId in viewModel.TPLIds)
                    {
                        var tplTransportType = new TPLTransportType();
                        tplTransportType.TransportTypeId = transportType.Id;
                        tplTransportType.TPLId = tplId;
                        _unitOfWork.RepositoryCRUD<TPLTransportType>().Insert(tplTransportType);
                    }
                }
            }
            if (viewModel.TPLIds != null && viewModel.TPLIds.Count() > 0)
            {
                await _unitOfWork.CommitAsync();
            }
            return JsonUtil.Create(data);
        }

        public override async Task<JsonResult> Update([FromBody]TransportTypeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return JsonUtil.Error(ModelState);
            }
            var data = await _iGeneralServiceRaw.Update<TransportType, TransportTypeViewModel>(viewModel);
            if (data.IsSuccess)
            {
                _unitOfWork.RepositoryCRUD<TPLTransportType>().DeleteEmptyWhere(x => x.TransportTypeId == viewModel.Id);
                if (viewModel.TPLIds != null && viewModel.TPLIds.Count() > 0)
                {
                    foreach (var tplId in viewModel.TPLIds)
                    {
                        var tplTransportType = new TPLTransportType();
                        tplTransportType.TransportTypeId = viewModel.Id;
                        tplTransportType.TPLId = tplId;
                        _unitOfWork.RepositoryCRUD<TPLTransportType>().Insert(tplTransportType);
                    }
                }
                await _unitOfWork.RepositoryCRUD<TPLTransportType>().CommitAsync();
            }

            return JsonUtil.Create(data);
        }

        [HttpPost("GetListTransportType")]
        public JsonResult GetListTransportType([FromBody] FilterViewModel ViewModel)
        {
            var companyId = GetCurrentCompanyId();
            var data = _unitOfWork.Repository<Proc_GetListTransportType>().ExecProcedure(Proc_GetListTransportType.GetEntityProc(ViewModel.PageNumber, ViewModel.PageSize, ViewModel.SearchText, companyId));
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
