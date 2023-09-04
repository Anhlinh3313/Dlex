using Core.Business.Services.Models;
using Core.Business.ViewModels.General;
using Core.Data.Abstract;
using Core.Entity.Entities;
using Core.Infrastructure.Helper;
using Core.Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class HubRouteController : BaseController
    {
        public HubRouteController(Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IOptions<JwtIssuerOptions> jwtOptions,
            IUnitOfWork unitOfWork) : base(logger, optionsAccessor, jwtOptions, unitOfWork)
        {
        }

        [HttpGet("GetDatasFromHub")]
        public JsonResult GetDatasFromHub(int hubId)
        {
            if (hubId != 0)
            {
                DataFromHubViewModel dataFromHub = new DataFromHubViewModel();

                //Get All data from hubRoute
                var hubs = _unitOfWork.RepositoryCRUD<HubRoute>().FindBy(r => r.HubId == hubId);

                if (hubs != null && hubs.Any())
                {
                    //Get Wards
                    int[] wardIds = hubs.Select(r => r.WardId).Distinct().ToArray();
                    dataFromHub.Wards = _unitOfWork.RepositoryR<Ward>().FindBy(r => wardIds.Contains(r.Id));
                    //Get DistrictIds selected
                    dataFromHub.SelectedDistrictIds = dataFromHub.Wards.Select(r => r.DistrictId).Distinct().ToArray();

                    //Get District selected objects
                    var selectedDistricts = _unitOfWork.RepositoryR<District>().FindBy(r => dataFromHub.SelectedDistrictIds.Contains(r.Id));

                    //Get ProvinceCityIds
                    dataFromHub.SelectedProvinceCiyIds = selectedDistricts.Select(r => r.ProvinceId).Distinct().ToArray();
                    //Get ProvinceCities
                    dataFromHub.Districts = _unitOfWork.RepositoryR<District>().FindBy(r => dataFromHub.SelectedProvinceCiyIds.Contains(r.ProvinceId));

                    return JsonUtil.Success(dataFromHub);
                }
            }

            return JsonUtil.Success();
        }

        [AllowAnonymous]
        [HttpPost("GetHubRouteByWardIds")]
        public JsonResult GetHubRouteByWardIds([FromBody] GetHobRouteByWardIdsViewModel model)
        {
            var hubs = _unitOfWork.RepositoryCRUD<HubRoute>().GetAll().ToList().Join(_unitOfWork.RepositoryCRUD<Hub>().GetAll().ToList(), hubR => hubR.HubId, hub => hub.Id, (hubR, hub) => new { hubR.Id, hubR.HubId, hubR.WardId, hub.Name }).Where(r => model.Ids.Contains(r.WardId) && r.HubId != model.HubId);

            return JsonUtil.Success(hubs);
        }

        [HttpPost("SaveChangeHubRoute")]
        public JsonResult SaveChangeHubRoute([FromBody] HubRouteSaveChangeViewModel model)
        {
            if (model.WardIds != null && model.HubId != 0)
            {
                int length = model.WardIds.Length;

                try
                {
                    //Xóa phân vùng đang có
                    _unitOfWork.RepositoryCRUD<HubRoute>().DeleteWhere(r => r.HubId == model.HubId);

                    for (int i = 0; i < length; i++)
                    {
                        _unitOfWork.RepositoryCRUD<HubRoute>().Insert(new HubRoute { HubId = model.HubId, WardId = model.WardIds[i] });
                    }
                    _unitOfWork.Commit();
                }
                catch (Exception ex)
                {
                    return Json(ex.Message);
                }
            }

            return JsonUtil.Success();
        }
    }
}
