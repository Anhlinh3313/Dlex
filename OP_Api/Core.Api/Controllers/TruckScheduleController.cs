using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Business.ViewModels;
using Core.Business.ViewModels.TruckSchedules;
using Core.Data.Abstract;
using Core.Entity.Entities;
using Core.Infrastructure.Helper;
using Core.Infrastructure.Utils;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class TruckScheduleController : GeneralController<TruckScheduleViewModel, TruckScheduleInfoViewModel, TruckSchedule>
    {
        private readonly IGeneralService<TruckScheduleDetailViewModel, TruckScheduleDetail> _iTruckScheduleDetailService;
        private readonly ITruckScheduleService _iTruckScheduleService;

        public TruckScheduleController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IOptions<JwtIssuerOptions> jwtOptions,
            IUnitOfWork unitOfWork,
            IGeneralService<TruckScheduleDetailViewModel, TruckScheduleDetail> iTruckScheduleDetailService,
            IGeneralService<TruckScheduleViewModel, TruckScheduleInfoViewModel, TruckSchedule> iGeneralService,
            ITruckScheduleService iTruckScheduleService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
            _iTruckScheduleDetailService = iTruckScheduleDetailService;
            _iTruckScheduleService = iTruckScheduleService;
        }

        [HttpGet]
        [Route("GetListByTruckNumber")]
        public JsonResult GetByTruckNumber(string truckNumber, string cols)
        {
            var truck = _unitOfWork.RepositoryR<Truck>().FindBy(x => x.TruckNumber == truckNumber).FirstOrDefault();
            if (truck != null)
            {
                var truckSchedules = ((List<TruckScheduleInfoViewModel>)_iGeneralService.FindBy(x => x.TruckId == truck.Id && (x.TruckScheduleStatusId == 1 || x.TruckScheduleStatusId == 3), cols: cols).Data);
                return JsonUtil.Success(truckSchedules);
            }
            else
            {
                return JsonUtil.Error("Không tìm thấy xe, vui lòng kiểm tra lại");
            }
        }

        [HttpGet]
        [Route("GetTruckScheduleDetail")]
        public JsonResult GetTruckScheduleDetail(int truckScheduleId, string cols)
        {
            var truckSchedules = _iTruckScheduleDetailService.FindBy(x => x.TruckScheduleId == truckScheduleId, cols: cols);
            if (truckSchedules != null)
            {
                return JsonUtil.Success(truckSchedules);
            }
            else
            {
                return JsonUtil.Error("Không tìm thấy xe, vui lòng kiểm tra lại");
            }
        }

        [HttpPost]
        [Route("SearchTruckScheduleByTruckNumber")]
        public JsonResult SearchTruckScheduleByTruckNumber([FromBody] TruckScheduleFilterViewModel model)
        {
            if (!Util.IsNull(model.TruckNumber))
            {
                var truck = _unitOfWork.RepositoryR<Truck>().FindBy(x => x.TruckNumber == model.TruckNumber).FirstOrDefault();
                if (truck != null)
                {
                    var truckSchedules = ((List<TruckScheduleInfoViewModel>)_iGeneralService.FindBy(x => x.TruckId == truck.Id && (x.TruckScheduleStatusId == 1 || x.TruckScheduleStatusId == 3), cols: model.Cols).Data);
                    if (!Util.IsNull(model.FromDate))
                    {
                        truckSchedules = truckSchedules.Where(x => x.StartDatetime >= model.FromDate).ToList();
                    }
                    if (!Util.IsNull(model.ToDate))
                    {
                        truckSchedules = truckSchedules.Where(x => x.StartDatetime <= model.ToDate).ToList();
                    }
                    return JsonUtil.Success(truckSchedules);
                }
                else
                    return JsonUtil.Error("Không tìm thấy xe, vui lòng kiểm tra lại");
            }
            else
                return JsonUtil.Error("Vui lòng nhập biển số xe");
        }

        [HttpPost]
        [Route("SearchTruckSchedule")]
        public JsonResult SearchTruckSchedule([FromBody] TruckScheduleFilterViewModel model)
        {
            return JsonUtil.Success(_iTruckScheduleService.Search(model));
        }

        [HttpGet]
        [Route("GetByCode")]
        public JsonResult GetByCode(string code, string cols)
        {
            var truckSchedule = ((List<TruckScheduleInfoViewModel>)_iGeneralService.FindBy(x => x.Code == code, cols: cols).Data).FirstOrDefault();
            if (Util.IsNull(truckSchedule))
            {
                return JsonUtil.Error("Không tìm thấy chuyến xe, vui lòng kiểm tra lại");
            }
            else if (truckSchedule.TruckScheduleStatusId != 1 && truckSchedule.TruckScheduleStatusId != 3)
            {
                return JsonUtil.Error("Chuyến xe đã được đóng Seal trước đó, vui lòng kiểm tra lại");
            }
            return JsonUtil.Success(truckSchedule);
        }

        public override async Task<JsonResult> Create([FromBody] TruckScheduleViewModel viewModel)
        {
            var dataCreate = await _iGeneralService.Create(viewModel);
            if (dataCreate.IsSuccess)
            {
                var truckSchedule = dataCreate.Data;
                var randomCode = RandomUtil.GetCode(truckSchedule.Id, 6);
                string truckScheduleCode = $"MCX{randomCode}";
                truckSchedule.Name = truckSchedule.Code = truckScheduleCode;
                var dataUpdate = await _iGeneralService.Update(truckSchedule);
                //
                if (!Util.IsNull(viewModel.RiderIds))
                {
                    var users = _unitOfWork.RepositoryR<User>().FindBy(f => viewModel.RiderIds.Contains(f.Id));
                    if (!Util.IsNull(users))
                    {
                        foreach (var user in users)
                        {
                            TruckScheduleRider TSR = new TruckScheduleRider();
                            TSR.RiderId = user.Id;
                            TSR.TruckScheduleId = truckSchedule.Id;
                            TSR.IsEnabled = true;
                            _unitOfWork.RepositoryCRUD<TruckScheduleRider>().Insert(TSR);
                        }
                        _unitOfWork.Commit();
                    }
                }
                //
                var currentUser = GetCurrentUser();
                var hub = _unitOfWork.RepositoryR<Hub>().GetSingle(currentUser.HubId.Value);
                var truckScheduleDetail = new TruckScheduleDetailViewModel(truckSchedule.Id, hub.Id, truckSchedule.SealNumber, hub.Name, null, hub.Lat, hub.Lng, TruckScheduleStatusHelper.NewCreate);
                await _iTruckScheduleDetailService.Create(truckScheduleDetail);
            }
            return JsonUtil.Create(dataCreate);
        }

        public override async Task<JsonResult> Update([FromBody] TruckScheduleViewModel viewModel)
        {
            var dataUpdate = await _iGeneralService.Update(viewModel);
            if (dataUpdate.IsSuccess)
            {
                var truckSchedule = dataUpdate.Data;
                int id = truckSchedule.Id;
                //
                _unitOfWork.RepositoryCRUD<TruckScheduleRider>().DeleteEmptyWhere(f => f.TruckScheduleId == id);
                if (!Util.IsNull(viewModel.RiderIds))
                {
                    var users = _unitOfWork.RepositoryR<User>().FindBy(f => viewModel.RiderIds.Contains(f.Id));
                    if (!Util.IsNull(users))
                    {
                        foreach (var user in users)
                        {
                            TruckScheduleRider TSR = new TruckScheduleRider();
                            TSR.RiderId = user.Id;
                            TSR.TruckScheduleId = id;
                            TSR.IsEnabled = true;
                            _unitOfWork.RepositoryCRUD<TruckScheduleRider>().Insert(TSR);
                        }
                        _unitOfWork.Commit();
                    }
                }
                //
            }
            return JsonUtil.Create(dataUpdate);
        }

        [HttpPost]
        [Route("CloseSeal")]
        public async Task<JsonResult> CloseSeal([FromBody]CloseOpenSealViewModel viewModel)
        {
            if (Util.IsNull(viewModel.SealNumber))
            {
                return JsonUtil.Error("Vui lòng nhập số Seal để đóng Seal!");
            }
            var truckSchedule = _unitOfWork.RepositoryR<TruckSchedule>().FindBy(f => f.Code == viewModel.TruckScheudleNumber).FirstOrDefault();
            if (Util.IsNull(truckSchedule))
            {
                return JsonUtil.Error("Không tìm thấy chuyến xe, vui lòng kiểm tra lại");
            }
            List<int> listAllowClose = new List<int>();
            listAllowClose.Add(TruckScheduleStatusHelper.NewCreate);
            listAllowClose.Add(TruckScheduleStatusHelper.OpenSeal);
            if (!listAllowClose.Contains(truckSchedule.TruckScheduleStatusId))
            {
                return JsonUtil.Error("Trạng thái của chuyến không cho phép đóng Seal");
            }
            var checkDatas = _unitOfWork.RepositoryR<TruckSchedule>().FindBy(f => f.SealNumber == viewModel.SealNumber).AsParallel().Select(s => s.Id);
            if (checkDatas.Count() > 0)
            {
                return JsonUtil.Error("Số Seal đang được sử dụng, vui lòng sử dụng số Seal khác");
            }
            truckSchedule.SealNumber = viewModel.SealNumber;
            truckSchedule.TruckScheduleStatusId = TruckScheduleStatusHelper.CloseSeal;
            var data = await _iGeneralService.Update(truckSchedule);
            // 
            var currentUser = GetCurrentUser();
            var hub = _unitOfWork.RepositoryR<Hub>().GetSingle(currentUser.HubId.Value);
            var truckScheduleDetail = new TruckScheduleDetailViewModel(truckSchedule.Id, hub.Id, truckSchedule.SealNumber, hub.Name, null, hub.Lat, hub.Lng, TruckScheduleStatusHelper.CloseSeal);
            if (!Util.IsNull(viewModel.Location) && !Util.IsNull(viewModel.Lat) && !Util.IsNull(viewModel.Lng))
            {
                truckScheduleDetail.Location = viewModel.Location;
                truckScheduleDetail.Lat = viewModel.Lat;
                truckScheduleDetail.Lng = viewModel.Lng;
            }
            await _iTruckScheduleDetailService.Create(truckScheduleDetail);
            return JsonUtil.Create(data);
        }

        [HttpPost("OpenSeal")]
        public async Task<JsonResult> OpenSeal([FromBody]CloseOpenSealViewModel viewModel)
        {
            if (Util.IsNull(viewModel.SealNumber))
            {
                return JsonUtil.Error("Vui lòng nhập số Seal để mở Seal!");
            }
            var truckSchedule = _unitOfWork.RepositoryR<TruckSchedule>().FindBy(f => f.SealNumber == viewModel.SealNumber).FirstOrDefault();
            if (Util.IsNull(truckSchedule))
            {
                return JsonUtil.Error("Số Seal không hợp lệ, vui lòng kiểm tra lại");
            }
            List<int> listAllowOpen = new List<int>();
            listAllowOpen.Add(TruckScheduleStatusHelper.CloseSeal);
            if (!listAllowOpen.Contains(truckSchedule.TruckScheduleStatusId))
            {
                return JsonUtil.Error("Trạng thái của chuyến không cho phép mở Seal");
            }
            //var checkData = _unitOfWork.RepositoryR<TruckSchedule>().FindBy(f => f.SealNumber == viewModel.TruckScheudleNumber).FirstOrDefault();
            //if (Util.IsNull(checkData))
            //{
            //    return JsonUtil.Error("Số Seal không hợp lệ, vui lòng sử dụng số Seal khác");
            //}
            truckSchedule.SealNumber = viewModel.SealNumber;
            truckSchedule.TruckScheduleStatusId = TruckScheduleStatusHelper.OpenSeal;
            var data = await _iGeneralService.Update(truckSchedule);
            // 
            var currentUser = GetCurrentUser();
            var hub = _unitOfWork.RepositoryR<Hub>().GetSingle(currentUser.HubId.Value);
            var truckScheduleDetail = new TruckScheduleDetailViewModel(truckSchedule.Id, hub.Id, truckSchedule.SealNumber, hub.Name, null, hub.Lat, hub.Lng, TruckScheduleStatusHelper.OpenSeal);
            if (!Util.IsNull(viewModel.Location) && !Util.IsNull(viewModel.Lat) && !Util.IsNull(viewModel.Lng))
            {
                truckScheduleDetail.Location = viewModel.Location;
                truckScheduleDetail.Lat = viewModel.Lat;
                truckScheduleDetail.Lng = viewModel.Lng;
            }
            await _iTruckScheduleDetailService.Create(truckScheduleDetail);
            return JsonUtil.Create(data);
        }
    }
}