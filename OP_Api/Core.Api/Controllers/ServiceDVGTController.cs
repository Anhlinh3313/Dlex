using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Core.Business.ViewModels;
using Core.Entity.Entities;
using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Data.Abstract;
using Core.Infrastructure.Helper;
using Microsoft.Extensions.Options;
using Core.Infrastructure.Utils;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class ServiceDVGTController : GeneralController<ServiceDVGTViewModel, ServiceDVGT>
    {
        public ServiceDVGTController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger, 
            IOptions<AppSettings> optionsAccessor, 
            IOptions<JwtIssuerOptions> jwtOptions, 
            IUnitOfWork unitOfWork, 
            IGeneralService<ServiceDVGTViewModel, ServiceDVGT> iGeneralService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
        }

        [HttpGet("GetByShipmentId")]
        public JsonResult GetByShipmentId(int id)
        {
            int?[] serviceDVGTIds = _unitOfWork.RepositoryR<ShipmentServiceDVGT>().FindBy(x => x.ShipmentId == id).Select(x => x.ServiceId).ToArray();
            return JsonUtil.Create(_iGeneralService.FindBy(x => serviceDVGTIds.Contains(x.Id)));
        }

        [HttpGet("GetPriceDVGTByShipmentId")]
        public JsonResult GetPriceDVGTByShipmentId(int id)
        {
            var data = _unitOfWork.RepositoryR<ShipmentServiceDVGT>().FindBy(x => x.ShipmentId == id, new string[] { "Service" });
            return JsonUtil.Success(data);
        }

        [HttpGet("GetByShipmentVersionId")]
        public JsonResult GetByShipmentVersionId(int id)
        {
            int[] serviceDVGTIds = _unitOfWork.RepositoryR<ShipmentServiceDVGTVersion>().FindBy(x => x.ShipmentId == id).Select(x => x.ServiceDVGTId).ToArray();
            return JsonUtil.Create(_iGeneralService.FindBy(x => serviceDVGTIds.Contains(x.Id)));
        }
    }
}
