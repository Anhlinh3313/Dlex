using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Business.ViewModels;
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
    public class ShipmentStatusController : GeneralController<ShipmentStatusViewModel, ShipmentStatus>
    {
        public ShipmentStatusController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger, 
            IOptions<AppSettings> optionsAccessor, 
            IOptions<JwtIssuerOptions> jwtOptions, 
            IUnitOfWork unitOfWork, 
            IGeneralService<ShipmentStatusViewModel, ShipmentStatus> iGeneralService) :
            base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
        }

        [HttpPost("GetByIds")]
        public JsonResult GetByIds([FromBody]int[] ids) 
        {
            if(ids != null && ids.Length > 0)
            {
                return FindBy(x => ids.Contains(x.Id));
            }

            return JsonUtil.Success();
        }

        [HttpGet("GetByType")]
        public JsonResult GetByType(string type)
        {
            var data = new Object();
            int[] arrStatusTransfer = { 7, 8, 22, 24, 32, 33, 34, 35, 36, 37, 40, 44, 46, 48, 50 };
            int[] arrStatusDeliver = { 10, 11, 12, 13, 14, 30, 39, 45, 48 };
            int[] arrStatusPickup = { 1, 2, 3, 4, 5, 29, 41, 42, 43 };
            int[] arrStatusReturn = { 26, 27, 28, 31, 38, 47, 51 };
            switch (type)
            {
                case "transfer":
                    var shipTransfer = _unitOfWork.RepositoryR<ShipmentStatus>().FindBy(status => arrStatusTransfer.Contains(status.Id));
                    data = shipTransfer;
                    break;
                case "delivery":
                    var shipDelivery = _unitOfWork.RepositoryR<ShipmentStatus>().FindBy(status => arrStatusDeliver.Contains(status.Id));
                    data = shipDelivery;
                    break;
                case "pickup":
                    var shipPickup = _unitOfWork.RepositoryR<ShipmentStatus>().FindBy(status => arrStatusPickup.Contains(status.Id));
                    data = shipPickup;
                    break;
                case "return":
                    var shipReturn = _unitOfWork.RepositoryR<ShipmentStatus>().FindBy(status => arrStatusReturn.Contains(status.Id));
                    data = shipReturn;
                    break;
            }
            return JsonUtil.Success(data);
        }
    }
}
