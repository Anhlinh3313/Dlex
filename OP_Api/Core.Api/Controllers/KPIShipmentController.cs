using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Business.ViewModels;
using Core.Business.ViewModels.KPIShipments;
using Core.Data.Abstract;
using Core.Entity.Entities;
using Core.Infrastructure.Helper;
using Core.Infrastructure.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class KPIShipmentController : GeneralController<KPIShipmentViewModel, KPIShipment>
    {
        private readonly IKPIShipmentCusService _IKPIShipmentCusService;
        public KPIShipmentController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IOptions<JwtIssuerOptions> jwtOptions,
            IUnitOfWork unitOfWork,
            IKPIShipmentCusService IKPIShipmentCusService,
            IGeneralService<KPIShipmentViewModel, KPIShipment> iGeneralService
            ) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
            _IKPIShipmentCusService = IKPIShipmentCusService;
        }

        [HttpPost("CreateKPIShipmentCus")]
        public JsonResult CreateKPIShipmentCus([FromBody] KPIShipmentCus request)
        {
            try
            {
                return JsonUtil.Success(_IKPIShipmentCusService.CreateKPIShipmentCus(request));
            }
            catch(Exception ex)
            {
               return JsonUtil.Error(ex.Message);
            }
        }

        [HttpPost("EditKPIShipmentCus")]
        public JsonResult EditKPIShipmentCus([FromBody]KPIShipmentCus request)
        {
            try
            {
                return JsonUtil.Success(_IKPIShipmentCusService.UpdateKPIShipmentCus(request));
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }
        }

        [HttpGet("GetKPIShipmentCusByKPIShipemt")]
        public JsonResult GetKPIShipmentCusByKPIShipemt(int request)
        {
            try
            {
                return JsonUtil.Success(_IKPIShipmentCusService.GetKPIShipmentCusByKPIShipment(request));
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }
        }

    }
}
