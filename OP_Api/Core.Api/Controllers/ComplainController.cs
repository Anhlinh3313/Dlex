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
    public class ComplainController : GeneralController<ComplainViewModel, ComplainInfoViewModel, Complain>
    {
        private readonly IGeneralService<ComplainHandleViewModel, ComplainHandle> _iGeneralServiceHanle;
        public ComplainController
            (Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IOptions<JwtIssuerOptions> jwtOptions,
            IUnitOfWork unitOfWork,
            IGeneralService<ComplainViewModel, ComplainInfoViewModel, Complain> iGeneralService,
            IGeneralService<ComplainHandleViewModel, ComplainHandle> iGeneralServiceHandle
            ) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
            _iGeneralServiceHanle = iGeneralServiceHandle;
        }

        public override async Task<JsonResult> Create([FromBody] ComplainViewModel viewModel)
        {
            if (Util.IsNull(viewModel.ComplainTypeId)) return JsonUtil.Error("Vui lòng chọn yêu cầu khiếu nại");
            var shipment = await _unitOfWork.RepositoryR<Shipment>().GetSingleAsync(viewModel.ShipmentId);
            if (Util.IsNull(shipment)) return JsonUtil.Error("Không tìm thấy thông tin vận đơn");
            var findComplains = _unitOfWork.RepositoryR<Complain>().FindBy(f => f.ShipmentId == shipment.Id);
            if (!Util.IsNull(findComplains))
            {
                if (findComplains.Count() > 0) return JsonUtil.Error("Yêu cầu hỗ trợ/khiếu nại đang được xử lý.");
            }
            //
            var res = _unitOfWork.Repository<Proc_RoleByComplainType>()
                .ExecProcedureSingle(Proc_RoleByComplainType.GetEntityProc(viewModel.ComplainTypeId));
            viewModel.ForwardToHubId = shipment.FromHubId;
            if (!Util.IsNull(res))
            {
                if (res.IsAcceptReturn == true)
                {
                    viewModel.ForwardToHubId = shipment.FromHubId;
                }
            }
            //
            viewModel.ComplainStatusId = ComplainStatusHelper.New;
            return await base.Create(viewModel);
        }

        [HttpGet("GetComplainStatus")]
        public JsonResult GetComplainStatus()
        {
            return JsonUtil.Success(_unitOfWork.RepositoryR<ComplainStatus>().GetAll());
        }

        [HttpPost("GetComplainBy")]
        public JsonResult GetComplainBy([FromBody] ComplainFilterViewModel viewModel)
        {
            var currentuser = GetCurrentUser();
            Expression<Func<Complain, bool>> predicate = x => x.Id > 0;
            if (!Util.IsNull(viewModel))
            {
                if (!Util.IsNull(viewModel.FromDate))
                {
                    viewModel.FromDate = viewModel.FromDate.AddHours(viewModel.FromDate.Hour * -1);
                    predicate = predicate.And(x => x.CreatedWhen >= viewModel.FromDate);
                }
                if (!Util.IsNull(viewModel.ToDate))
                {
                    viewModel.ToDate = viewModel.ToDate.AddHours(24 - viewModel.ToDate.Hour);
                    predicate = predicate.And(x => x.CreatedWhen <= viewModel.ToDate);
                }
                if (!Util.IsNull(viewModel.CenterHubIds) && viewModel.CenterHubIds.Count() > 0)
                {
                    var listHubIds = _unitOfWork.RepositoryR<Hub>()
                        .FindBy(f => f.CenterHubId.HasValue && f.PoHubId.HasValue && viewModel.CenterHubIds.Contains(f.CenterHubId.Value)).Select(s => s.Id).ToList();
                    if (listHubIds.Count() > 0) predicate = predicate.And(f => listHubIds.Contains(f.HandlingHubId.Value) || listHubIds.Contains(f.ForwardToHubId.Value));
                }
                var res = _unitOfWork.Repository<Proc_GetRoleAndHubByUser>()
               .ExecProcedure(Proc_GetRoleAndHubByUser.GetEntityProc(currentuser.Id)).ToList();
                if (!Util.IsNull(res))
                {
                    if (res.Count() > 0)
                    {
                        var listHubIds = res.Select(s => s.HubId).ToList();
                        predicate = predicate.And(x => listHubIds.Contains(x.HandlingHubId.Value) || listHubIds.Contains(x.ForwardToHubId.Value));
                        var listComplainTypeIds = res.Select(s => s.ComplainTypeId).ToList();
                        predicate = predicate.And(x => listComplainTypeIds.Contains(x.ComplainTypeId));
                    }
                }
                if (!Util.IsNull(viewModel.CustomerIds) && viewModel.CustomerIds.Count() > 0) predicate = predicate.And(f => viewModel.CustomerIds.Contains(f.CustomerId));
                if (!Util.IsNull(viewModel.ComplainStatusIds) && viewModel.ComplainStatusIds.Count() > 0) predicate = predicate.And(f => viewModel.ComplainStatusIds.Contains(f.ComplainStatusId));
                if (!Util.IsNull(viewModel.ComplainTypeIds) && viewModel.ComplainTypeIds.Count() > 0) predicate = predicate.And(f => viewModel.ComplainTypeIds.Contains(f.ComplainTypeId));
                if (viewModel.Type == 1) predicate = predicate.And(f => f.HandlingEmpId == currentuser.Id || f.ForwardToEmpId == currentuser.Id);//only handling
                else if (viewModel.Type == 2) predicate = predicate.And(f => f.HandlingEmpId == currentuser.Id || f.ForwardToEmpId == currentuser.Id || !f.ForwardToEmpId.HasValue);//handling & new
            }
            return FindBy(predicate, viewModel.PageSize, viewModel.PageNum, viewModel.cols);
        }
        
        [HttpPost("AddComplainHandle")]
        public async Task<JsonResult> AddComplainHandle([FromBody] ComplainHandleViewModel viewModel)
        {
            var complain = _unitOfWork.RepositoryR<Complain>().GetSingle(viewModel.ComplainId);
            if (Util.IsNull(complain)) return JsonUtil.Error("Không tìm tháy yêu cầu hỗ trợ/khiếu nại để xử lý");
            //
            var currentuser = GetCurrentUser();
            //
            complain.HandlingHubId = currentuser.HubId.Value;
            complain.ComplainStatusId = viewModel.ComplainStatusId;
            _unitOfWork.RepositoryCRUD<Complain>().Update(complain);
            //
            viewModel.HandleHubId = currentuser.HubId.Value;
            viewModel.HandleEmpId = currentuser.Id;
            viewModel.EndDate = DateTime.Now;
            var re = await _iGeneralServiceHanle.Create(viewModel);
            _unitOfWork.Commit();
            //
            return JsonUtil.Create(re);
        }

        [HttpGet("GetComplainHandle")]
        public JsonResult GetComplainHandle(int complainId, int? pageSize = 10, int? pageNum = 1, string cols = null)
        {
            return JsonUtil.Create(_iGeneralServiceHanle.FindBy(f => f.ComplainId == complainId, false, pageSize, pageNum, cols));
        }
    }
}
