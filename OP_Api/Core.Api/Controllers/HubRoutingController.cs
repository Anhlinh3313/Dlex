using AutoMapper;
using Core.Business.Services.Models;
using Core.Business.ViewModels;
using Core.Business.ViewModels.General;
using Core.Data.Abstract;
using Core.Entity.Entities;
using Core.Entity.Procedures;
using Core.Infrastructure.Helper;
using Core.Infrastructure.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class HubRoutingController: BaseController
    {
        public HubRoutingController
            (
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IOptions<JwtIssuerOptions> jwtOptions,
            IUnitOfWork unitOfWork) : base(logger, optionsAccessor, jwtOptions, unitOfWork)
        {
        }
        [HttpGet("GetHubRoutingByPoHubId")]
        public JsonResult GetHubRoutingByPoHubId(int? stationHubId)
        {
            var hubRouting = _unitOfWork.RepositoryR<HubRouting>().GetAll(inc => inc.Hub, inc => inc.User).Where(x => x.Hub.Id == stationHubId || x.HubId == stationHubId).ToList();
            return JsonUtil.Success(Mapper.Map<IEnumerable<HubRoutingInfoViewModel>>(hubRouting));
        }

        [HttpGet("GetDatasFromHub")]
        public JsonResult GetDatasFromStationHubId(int stationHubId, int hubRoutingId, bool isTruckDelivery)
        {
            GetDatasFromHubViewModel data = new GetDatasFromHubViewModel();
            data.Wards = _unitOfWork.Repository<Proc_Hub_RoutingWard_Availability>().ExecProcedure(Proc_Hub_RoutingWard_Availability.GetEntityProc(stationHubId, hubRoutingId, isTruckDelivery));
            if (data.Wards.Count() > 0)
                data.SelectedWardIds = data.Wards.Where(r => r.HubRoutingId != null && r.HubRoutingId == hubRoutingId).Select(r => r.WardId).ToArray();
            return JsonUtil.Success(data);
        }

        [HttpPost("Create")]
        public JsonResult Create([FromBody] HubRoutingCreateUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return JsonUtil.Error(ModelState);
            }

            var listError = new List<KeyValuePair<string, object>>();

            try
            {
                var listHub = _unitOfWork.RepositoryR<HubRouting>().FindBy(f => f.IsTruckDelivery == model.IsTruckDelivery).Select(s => s.Id).ToList();
                var tempDatas = _unitOfWork.RepositoryR<HubRoutingWard>().FindBy(
                    r => model.WardIds.Contains(r.WardId) && listHub.Contains(r.HubRoutingId)
                    , r => r.Ward, r => r.HubRouting);
                //

                foreach (var item in tempDatas)
                {
                    listError.Add(new KeyValuePair<string, object>(item.Ward.Code, $"Phường/xã {item.Ward.Name} đã được phân cho HubRouting {item.HubRouting.Name}"));
                }

                if (listError.Count == 0)
                {
                    HubRouting hubRouting = Mapper.Map<HubRouting>(model);
                    _unitOfWork.RepositoryCRUD<HubRouting>().Insert(hubRouting);
                    _unitOfWork.Commit();

                    var getHubRoutingByCode = _unitOfWork.RepositoryR<HubRouting>().FindBy(f => f.Code == model.Code && f.Name == model.Name);
             
                    int length = model.WardIds.Length;
                    for (int i = 0; i < length; i++)
                    {
                        HubRoutingWard hrw = new HubRoutingWard();
                        hrw.HubRoutingId = hubRouting.Id;
                        hrw.IsEnabled = true;
                        hrw.WardId = model.WardIds[i];
                        _unitOfWork.RepositoryCRUD<HubRoutingWard>().Insert(hrw);
                    }
                    if (!Util.IsNull(model.StreetJoinIds))
                    {
                        int lengthStreet = model.StreetJoinIds.Length;
                        for (int i = 0; i < lengthStreet; i++)
                        {
                            HubRoutingStreetJoin hrt = new HubRoutingStreetJoin();
                            hrt.HubRoutingId = hubRouting.Id;
                            hrt.StreetJoinId = model.StreetJoinIds[i];
                            _unitOfWork.RepositoryCRUD<HubRoutingStreetJoin>().Insert(hrt);
                        }
                    }
                    _unitOfWork.Commit();
                    return JsonUtil.Success(Mapper.Map<HubRoutingInfoViewModel>(hubRouting));
                }
                else
                {
                    return JsonUtil.Error(listError);
                }
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }
        }

        [HttpPost("Update")]
        public JsonResult Update([FromBody] HubRoutingCreateUpdateViewModel model)
        {

            var listError = new List<KeyValuePair<string, object>>();
            try
            {
                HubRouting hubRouting = _unitOfWork.RepositoryR<HubRouting>().GetSingle(model.Id);

                if (hubRouting == null)
                {
                    return JsonUtil.Error("Không tìm thấy dữ liệu cần chỉnh sửa");
                }

                if (hubRouting.Code == model.Code)
                {
                    return JsonUtil.Error("Mã đã tồn tại");
                }

                else
                {
                    var tempDatas = _unitOfWork.RepositoryR<HubRoutingWard>().FindBy(
                   r => model.WardIds.Contains(r.WardId) && r.HubRoutingId != model.Id,
                   r => r.Ward, r => r.HubRouting);
                    //
                    var tempDatasStreet = _unitOfWork.RepositoryR<HubRoutingStreetJoin>().FindBy(
                        f => model.StreetJoinIds.Contains(f.StreetJoinId) && f.HubRoutingId != model.Id,
                        f => f.StreetJoin, f => f.HubRouting);

                    foreach (var item in tempDatas)
                    {
                        listError.Add(new KeyValuePair<string, object>(item.Ward.Code, $"Phường/xã {item.Ward.Name} đã được phân cho HubRouting {item.HubRouting.Name}"));
                    }

                    foreach (var item in tempDatasStreet)
                    {
                        listError.Add(new KeyValuePair<string, object>(item.StreetJoin.Code, $"Tuyến/đường {item.StreetJoin.Name} đã được phân cho HubRouting {item.HubRouting.Name}"));
                    }

                    hubRouting = Mapper.Map(model, hubRouting);

                    _unitOfWork.RepositoryCRUD<HubRoutingWard>().DeleteWhere(r => r.HubRoutingId == model.Id);
                    _unitOfWork.RepositoryCRUD<HubRoutingStreetJoin>().DeleteWhere(f => f.HubRoutingId == model.Id);
                    _unitOfWork.Commit();

                    int length = model.WardIds.Length;
                    for (int i = 0; i < length; i++)
                    {
                        HubRoutingWard hrw = new HubRoutingWard();
                        hrw.HubRoutingId = model.Id;
                        hrw.WardId = model.WardIds[i];
                        _unitOfWork.RepositoryCRUD<HubRoutingWard>().Insert(hrw);
                    }
                    int lengthStreet = model.StreetJoinIds.Length;
                    for (int i = 0; i < lengthStreet; i++)
                    {
                        HubRoutingStreetJoin hrt = new HubRoutingStreetJoin();
                        hrt.HubRoutingId = hubRouting.Id;
                        hrt.StreetJoinId = model.StreetJoinIds[i];
                        _unitOfWork.RepositoryCRUD<HubRoutingStreetJoin>().Insert(hrt);
                    }
                    _unitOfWork.RepositoryCRUD<HubRouting>().Update(hubRouting);
                    _unitOfWork.Commit();

                    return JsonUtil.Success(Mapper.Map<HubRoutingInfoViewModel>(hubRouting));
                }
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }
        }

        [HttpPost("Delete")]
        public JsonResult Create([FromBody] SimpleViewModel model)
        {
            var hubRouting = _unitOfWork.RepositoryR<HubRouting>().GetSingle(model.Id);
            if (Util.IsNull(hubRouting))
            {
                return JsonUtil.Error("Dữ liệu không tồn tại!");
            }
            try
            {
                _unitOfWork.RepositoryCRUD<HubRoutingWard>().DeleteWhere(f => f.HubRoutingId == model.Id);
                _unitOfWork.RepositoryCRUD<HubRoutingStreetJoin>().DeleteWhere(f => f.HubRoutingId == model.Id);
                _unitOfWork.RepositoryCRUD<HubRouting>().Delete(hubRouting);
                return JsonUtil.Success(hubRouting);
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }
        }

        [HttpGet("GetWardByHubId")]
        public JsonResult GetWardByHubId(int? hubId)
        {
            var data = _unitOfWork.Repository<Proc_GetWardByHubId>().ExecProcedure(Proc_GetWardByHubId.GetEntityProc(hubId));
            if (!Util.IsNull(data))
            {
                return JsonUtil.Success(data);
            }
            else
            {
                return JsonUtil.Error("Get data error!!!");
            }
        }

        [HttpGet("GetWardIdsByHubId")]
        public JsonResult GetWardIdsByHubId(int? hubId)
        {
            GetWardIdsByHubIdViewModel data = new GetWardIdsByHubIdViewModel();
            data.WardIds = _unitOfWork.Repository<Proc_GetWardIdsByHubId>().ExecProcedure(Proc_GetWardIdsByHubId.GetEntityProc(hubId));
            if (data.WardIds.Count() > 0)
            {
                data.SelectedWardIds = data.WardIds.Select(r => r.Id).ToArray();
            }    
            return JsonUtil.Success(data);
        }
    }
}
