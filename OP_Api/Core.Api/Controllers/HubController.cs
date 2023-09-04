using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Business.ViewModels;
using Core.Business.ViewModels.General;
using Core.Data.Abstract;
using Core.Entity.Entities;
using Core.Entity.Procedures;
using Core.Infrastructure.Helper;
using Core.Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class HubController: GeneralController<HubViewModel, HubInfoViewModel, Hub>
    {
        public HubController(
           Microsoft.Extensions.Logging.ILogger<dynamic> logger,
           IOptions<AppSettings> optionsAccessor,
           IOptions<JwtIssuerOptions> jwtOptions,
           IUnitOfWork unitOfWork,
           IGeneralService<HubViewModel, HubInfoViewModel, Hub> iGeneralService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
        }
        [HttpGet("GetCenterHub")]
        public JsonResult GetCenterHub(int? pageSize, int? pageNumber, string cols = null)
        {
            return FindBy(x => !x.CenterHubId.HasValue, pageSize, pageNumber, cols);
        }

        [HttpGet("GetPoHub")]
        public JsonResult GetPoHub(int? pageSize, int? pageNumber, string cols = null)
        {
            return FindBy(x => x.CenterHubId.HasValue && !x.PoHubId.HasValue, pageSize, pageNumber, cols);
        }

        [HttpGet("GetStationHub")]
        public JsonResult GetStationHub(int? pageSize, int? pageNumber, string cols = null)
        {
            return FindBy(x => x.CenterHubId.HasValue && x.PoHubId.HasValue, pageSize, pageNumber, cols);
        }

        [HttpGet("GetStationHubByPoId")]
        public JsonResult GetStationHubByPoId(int poId, string cols = null)
        {
            return FindBy(x => x.PoHubId == poId && x.CenterHubId.HasValue && x.PoHubId.HasValue, cols: cols);
        }

        [HttpPost("GetCenterHubs")]
        public JsonResult GetCenterHubs([FromBody]FilterViewModel ViewModel)
        {
            var data = _unitOfWork.Repository<Proc_GetCenterHubs>().ExecProcedure(Proc_GetCenterHubs.GetEntityProc(ViewModel.PageNumber, ViewModel.PageSize, ViewModel.SearchText));
            if (!Util.IsNull(data))
            {
                return JsonUtil.Success(data);
            }
            else
            {
                return JsonUtil.Error("Get data error!!!");
            }
        }

        [HttpPost("GetPoHubs")]
        public JsonResult GetPoHubs([FromBody] FilterViewModel ViewModel)
        {
            var data = _unitOfWork.Repository<Proc_GetPoHubs>().ExecProcedure(Proc_GetPoHubs.GetEntityProc(ViewModel.PageNumber, ViewModel.PageSize, ViewModel.CenterHubId, ViewModel.SearchText));
            if (!Util.IsNull(data))
            {
                return JsonUtil.Success(data);
            }
            else
            {
                return JsonUtil.Error("Get data error!!!");
            }
        }

        [HttpGet("GetListHubFromHubId")]
        public JsonResult GetListHubFromHubId(int? hubId)
        {
            var companyId = GetCurrentCompanyId();
            var data = _unitOfWork.Repository<Proc_GetListHubFromHubId>().ExecProcedure(Proc_GetListHubFromHubId.GetEntityProc(hubId));
            if (!Util.IsNull(data))
            {
                return JsonUtil.Success(data);
            }
            else
            {
                return JsonUtil.Error("Get data error!!!");
            }
        }

        [HttpPost("GetStationHubs")]
        public JsonResult GetStationHubs([FromBody] FilterViewModel ViewModel)
        {
            var data = _unitOfWork.Repository<Proc_GetStationHubs>().ExecProcedure(Proc_GetStationHubs.GetEntityProc(ViewModel.PageNumber, ViewModel.PageSize,
                ViewModel.CenterHubId, ViewModel.POHubId, ViewModel.SearchText));
            if (!Util.IsNull(data))
            {
                return JsonUtil.Success(data);
            }
            else
            {
                return JsonUtil.Error("Get data error!!!");
            }
        }

        [HttpGet("GetPoHubByCenterId")]
        public JsonResult GetPoHubByCenterId(int centerId, string cols = null)
        {
            return FindBy(x => x.CenterHubId == centerId && x.CenterHubId.HasValue && !x.PoHubId.HasValue, cols: cols);
        }

        [HttpGet("UpdateCenterHub")]
        public JsonResult UpdateCenterHub(int centerHubId)
        {
            var data = _unitOfWork.Repository<Proc_UpdateCenterHub>().ExecProcedure(Proc_UpdateCenterHub.GetEntityProc(centerHubId));
            if (!Util.IsNull(data))
            {
                return JsonUtil.Success(data);
            }
            else
            {
                return JsonUtil.Error("Get data error!!!");
            }
        }

        [HttpGet("UpdatePoHub")]
        public JsonResult UpdatePoHub(int poHubId)
        {
            var data = _unitOfWork.Repository<Proc_UpdatePoHub>().ExecProcedure(Proc_UpdatePoHub.GetEntityProc(poHubId));
            if (!Util.IsNull(data))
            {
                return JsonUtil.Success(data);
            }
            else
            {
                return JsonUtil.Error("Get data error!!!");
            }
        }

        [HttpGet("UpdateStationHub")]
        public JsonResult UpdateStationHub(int stationHubId)
        {
            var data = _unitOfWork.Repository<Proc_UpdateStationHub>().ExecProcedure(Proc_UpdateStationHub.GetEntityProc(stationHubId));
            if (!Util.IsNull(data))
            {
                return JsonUtil.Success(data);
            }
            else
            {
                return JsonUtil.Error("Get data error!!!");
            }
        }

        [HttpGet("GetHubByUserId")]
        public JsonResult GetHubByUserId(int userId)
        {
            var data = _unitOfWork.Repository<Proc_GetHubByUserId>().ExecProcedure(Proc_GetHubByUserId.GetEntityProc(userId));
            if (!Util.IsNull(data))
            {
                return JsonUtil.Success(data);
            }
            else
            {
                return JsonUtil.Error("Get data error!!!");
            }
        }

        [HttpGet("GetHubByProvinceDistrictWard")]
        public JsonResult GetHubByProvinceDistrictWard(int provinceId, int districtId, int wardId)
        {
            var data = _unitOfWork.Repository<Proc_GetHubByProvinceDistrictWard>()
                .ExecProcedure(Proc_GetHubByProvinceDistrictWard.GetEntityProc(provinceId, districtId, wardId));
            if (!Util.IsNull(data))
            {
                return JsonUtil.Success(data);
            }
            else
            {
                return JsonUtil.Error("Get data error!!!");
            }
        }


        [AllowAnonymous]
        [HttpGet("GetInfoLocation")]
        public JsonResult GetInfoLocation(int? countryId, string provinceName, string districtName, string wardName)
        {
            int provinceId = 0, districtId = 0, wardId = 0;
            Hub hub = new Hub();
            countryId = (countryId ?? 1);
            //
            var reponsiveProvince = _unitOfWork.RepositoryCRUD<Province>();
            var provinces = reponsiveProvince.FindBy(o => o.CountryId == countryId);
            var dataProvinceId = Business.Core.Helpers.StringHelper.GetBestMatches(provinces, "Id", "Name", provinceName, null,
                                                                Business.Core.Helpers.StringHelper._REPLACES_LOCATION_NAME);
            if (Util.IsInt(dataProvinceId))
            {
                provinceId = (int)dataProvinceId;
            }
            //
            var reponsiveDistrict = _unitOfWork.RepositoryCRUD<District>();
            var districts = reponsiveDistrict.FindBy(o => o.ProvinceId == provinceId);
            var dataDistrictId = Business.Core.Helpers.StringHelper.GetBestMatches(districts, "Id", "Name", districtName, null,
                                                                Business.Core.Helpers.StringHelper._REPLACES_LOCATION_NAME);
            if (Util.IsInt(dataDistrictId))
            {
                districtId = (int)dataDistrictId;
            }
            //
            var reponsiveWard = _unitOfWork.RepositoryCRUD<Ward>();
            var wards = reponsiveWard.FindBy(o => o.DistrictId == districtId);
            var dataWardId = Business.Core.Helpers.StringHelper.GetBestMatches(wards, "Id", "Name", wardName, null,
                                                                Business.Core.Helpers.StringHelper._REPLACES_LOCATION_NAME);
            if (Util.IsInt(dataWardId))
            {
                wardId = (int)dataWardId;
            }

            var hubRoutingWard = _unitOfWork.RepositoryR<HubRoutingWard>().GetSingle(x => x.WardId == wardId, new string[] { "HubRouting" });
            if (hubRoutingWard != null)
            {
                hub = _unitOfWork.RepositoryR<Hub>().GetSingle(hubRoutingWard.HubRouting.HubId);
            }
            else
            {
                var hubRoute = _unitOfWork.RepositoryR<HubRoute>().GetSingle(x => x.WardId == wardId, new string[] { "HubRoute" });
                if (hubRoute != null)
                {
                    hub = _unitOfWork.RepositoryR<Hub>().GetSingle(hubRoute.HubId);
                }
            }
            //
            var infolocation = new InfoLocationViewModel();
            infolocation.ProvinceId = provinceId;
            infolocation.DistrictId = districtId;
            infolocation.WardId = wardId;
            infolocation.HubId = hub.Id;
            infolocation.Hub = hub;


            return JsonUtil.Success(infolocation);
        }
    }
}
