using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Business.ViewModels;
using Core.Entity.Entities;
using Core.Entity.Procedures;
using Core.Infrastructure.Utils;
using LinqKit;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Api.Controllers
{
    public partial class ShipmentController
    {
        [HttpPost("AddDelay")]
        public JsonResult AddDelay([FromBody]AddDelayViewModel viewModel)
        {
            var currentUser = GetCurrentUser();
            var res = _unitOfWork.Repository<Proc_AddDelay>()
                .ExecProcedureSingle(Proc_AddDelay.GetEntityProc(viewModel.ShipmentNumber, viewModel.ListGoodsCode, viewModel.DelayReasonId,
                viewModel.DelayNote, viewModel.DelayTime, currentUser.Id));
            return JsonUtil.Success(string.Format("Đã báo delay thành công cho ({0})", res.DataCount));
        }

        [HttpGet("GetListShipmentDelay")]
        public JsonResult GetListShipmentDelay(DateTime? fromDate, DateTime? toDate, int? customerId, int? serviceId,int? reasonDelayId, int? pageSize, int? pageNum)
        {
            var res = _unitOfWork.Repository<Proc_GetListShipmentDelay>()
                .ExecProcedure(Proc_GetListShipmentDelay.GetEntityProc(fromDate,toDate,customerId,serviceId,reasonDelayId,pageSize,pageNum));
            return JsonUtil.Success(res);
        }

        [HttpPost("AddIncidents")]
        public async Task<JsonResult> AddIncidents([FromBody]AddIncidentsViewModel viewModel)
        {
            if (Util.IsNull(viewModel.ShipmentNumber)) return JsonUtil.Error("Vui lòng nhập mã vận đơn");
            var res = _unitOfWork.Repository<Proc_GetLadingScheduleCurrent>()
                .ExecProcedureSingle(Proc_GetLadingScheduleCurrent.GetEntityProc(viewModel.ShipmentNumber));
            if (!res.IsSuccess) return JsonUtil.Error(res.Message);
            viewModel.ShipmentId = res.ShipmentId.Value;
            viewModel.LadingScheduleId = res.LadingScheduleId.Value;
            var currentUser = GetCurrentUser();
            viewModel.CreatedByEmpId = currentUser.Id;
            viewModel.Code = viewModel.ShipmentNumber;
            viewModel.Name = viewModel.Code;
            var result = await _iGeneralServiceRaw.Create<Incidents, AddIncidentsViewModel>(viewModel);
            if (result.IsSuccess == true && res.ShipmentId.HasValue)
            {
                var ship = _unitOfWork.RepositoryCRUD<Shipment>().GetSingle(res.ShipmentId.Value);
                ship.IsIncidents = true;
                _unitOfWork.Commit();
            }
            return JsonUtil.Create(result);
        }

        [HttpPost("HandleIncidents")]
        public async Task<JsonResult> HandleIncidents([FromBody]AddIncidentsViewModel viewModel)
        {
            var currentUser = GetCurrentUser();
            viewModel.HandleByEmpId = currentUser.Id;
            return JsonUtil.Create(await _iGeneralServiceRaw.Update<Incidents, AddIncidentsViewModel>(viewModel));
        }

        [HttpGet("GetListIncidents")]
        public JsonResult GetListIncidents(int? empId, bool? isCompleted, string shipmentNumber, DateTime? fromDate, DateTime? toDate, int? pageSize, int? pageNum, string cols)
        {
            var currentUser = GetCurrentUser(); var currentuser = GetCurrentUser();
            Expression<Func<Incidents, bool>> predicate = x => x.Id > 0;
            if (!Util.IsNull(fromDate))
            {
                fromDate = fromDate.Value.AddHours(fromDate.Value.Hour * -1);
                predicate = predicate.And(x => x.CreatedWhen >= fromDate);
            }
            if (!Util.IsNull(toDate))
            {
                toDate = toDate.Value.AddHours(24 - toDate.Value.Hour);
                predicate = predicate.And(x => x.CreatedWhen <= toDate);
            }
            if (!Util.IsNull(empId))
            {
                predicate = predicate.And(x => x.CreatedByEmpId == empId);
            }
            if (!Util.IsNull(shipmentNumber))
            {
                predicate = predicate.And(x => x.Code == shipmentNumber);
            }
            if (!Util.IsNull(isCompleted))
            {
                predicate = predicate.And(x => x.IsCompleted == isCompleted);
            }
            return JsonUtil.Create(_iGeneralServiceRaw.FindBy<Incidents, IncidentsInfoViewModel>(predicate, pageSize, pageNum, cols));
        }

        [HttpGet("GetIncidentsById")]
        public JsonResult GetIncidentsById(int incidentsId, string cols)
        {
            var currentUser = GetCurrentUser(); var currentuser = GetCurrentUser();
            Expression<Func<Incidents, bool>> predicate = x => x.Id == incidentsId;
            return JsonUtil.Create(_iGeneralServiceRaw.GetSingle<Incidents, IncidentsInfoViewModel>(predicate, cols));
        }

        [HttpGet("GetFeetype")]
        public JsonResult GetFeeType()
        {
            return JsonUtil.Create(_iGeneralServiceRaw.FindBy<FeeType>(f => f.Id > 0));
        }

        [HttpGet("CompensationType")]
        public JsonResult CompensationType()
        {
            return JsonUtil.Create(_iGeneralServiceRaw.FindBy<CompensationType>(f => f.Id > 0));
        }

        [HttpPost("AddCompensation")]
        public async Task<JsonResult> AddCompensation([FromBody]CompensationViewModel viewModel)
        {
            if (!Util.IsNull(viewModel.ComplainId))
            {
                var checkComplain = _unitOfWork.RepositoryR<Complain>().GetSingle(f => f.Id == viewModel.ComplainId);
                if (Util.IsNull(checkComplain)) return JsonUtil.Error("Không tìm thấy hỗ trợ / khiếu nại.");
            }
            if (!Util.IsNull(viewModel.IncidentsId))
            {
                var checkIncidents = _unitOfWork.RepositoryR<Incidents>().GetSingle(f => f.Id == viewModel.IncidentsId);
                if (Util.IsNull(checkIncidents)) return JsonUtil.Error("Không tìm thấy sự cố của vận đơn.");
            }
            var currentUser = GetCurrentUser();
            viewModel.CreatedByEmpId = currentUser.Id;
            viewModel.IsCompleted = false;
            viewModel.CompensationValue = 0;
            viewModel.CompensationValueEmp = 0;
            return JsonUtil.Create(await _iGeneralServiceRaw.Create<Compensation, CompensationViewModel>(viewModel));
        }

        [HttpGet("GetListCompensation")]
        public JsonResult GetListCompensation(bool? isCompleted, string shipmentNumber, DateTime? fromDate, DateTime? toDate, int? pageSize, int? pageNum, string cols)
        {
            var currentUser = GetCurrentUser(); var currentuser = GetCurrentUser();
            Expression<Func<Compensation, bool>> predicate = x => x.Id > 0;
            if (!Util.IsNull(fromDate))
            {
                fromDate = fromDate.Value.AddHours(fromDate.Value.Hour * -1);
                predicate = predicate.And(x => x.CreatedWhen >= fromDate);
            }
            if (!Util.IsNull(toDate))
            {
                toDate = toDate.Value.AddHours(24 - toDate.Value.Hour);
                predicate = predicate.And(x => x.CreatedWhen <= toDate);
            }
            if (!Util.IsNull(isCompleted))
            {
                predicate = predicate.And(x => x.IsCompleted == isCompleted);
            }
            if (!Util.IsNull(shipmentNumber))
            {
                predicate = predicate.And(x => x.Code == shipmentNumber);
            }
            return JsonUtil.Create(_iGeneralServiceRaw.FindBy<Compensation, CompensationInfoViewModel>(predicate, pageSize, pageNum, cols));
        }

        [HttpPost("HandleCompensation")]
        public async Task<JsonResult> HandleCompensation([FromBody]CompensationViewModel viewModel)
        {
            var currentuser = GetCurrentUser();
            viewModel.HandleEmpId = currentuser.Id;
            return JsonUtil.Create(await _iGeneralServiceRaw.Update<Compensation, CompensationViewModel>(viewModel));
        }
    }
}
