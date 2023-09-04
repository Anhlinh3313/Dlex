using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Core.Entity.Entities;
using Core.Business.Core.Utils;
using Core.Business.ViewModels;
using Core.Infrastructure.Utils;
using Core.Business.Services.Models;
using Core.Data.Abstract;
using Core.Infrastructure.Helper;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Core.Infrastructure.Api;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class PriceController : BaseController
    {
        private readonly CompanyInformation _icompanyInformation;
        public PriceController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<CompanyInformation> companyInformation,
            IOptions<AppSettings> optionsAccessor, IOptions<JwtIssuerOptions> jwtOptions,
            IUnitOfWork unitOfWork) : base(logger, optionsAccessor, jwtOptions, unitOfWork
        )
        {
            _icompanyInformation = companyInformation.Value;
        }

        // GET: api/values
        [HttpPost("Calculate")]
        public JsonResult Calculate([FromBody]ShipmentCalculateViewModel shipment)
        {
            var result = PriceUtil.Calculate(shipment, _icompanyInformation.Name, true);
            return JsonUtil.Create(result);
        }

        [AllowAnonymous]
        [HttpPost("GetListService")]
        public JsonResult GetListService([FromBody] ShipmentCalculateViewModel shipmentCalculateViewModel)
        {
            List<ServiceInfoViewModel> services = new List<ServiceInfoViewModel>();
            var listService = _unitOfWork.RepositoryR<Service>().GetAll().OrderBy(o=>o.NUMBER_L_W_H_MULTIP);
            foreach (var service in listService)
            {
                shipmentCalculateViewModel.ServiceDVGTIds = null;
                shipmentCalculateViewModel.ServiceId = service.Id;
                var result = PriceUtil.Calculate(shipmentCalculateViewModel, _icompanyInformation.Name, true);
                if (result.IsSuccess)
                {
                    ServiceInfoViewModel serviceInfo = new ServiceInfoViewModel();
                    serviceInfo.Id = service.Id;
                    serviceInfo.Name = service.Name;
                    serviceInfo.Code = service.Code;
                    PriceViewModel price = result.Data as PriceViewModel;
                    if (price.TotalPrice > 0)
                    {
                        DeadlineShipmentViewModel deadlineShipment = new DeadlineShipmentViewModel();
                        deadlineShipment.WardFromId = shipmentCalculateViewModel.FromWardId;
                        deadlineShipment.ServiceId = shipmentCalculateViewModel.ServiceId;
                        deadlineShipment.DistrictToId = shipmentCalculateViewModel.ToDistrictId;
                        var deadlineResult = DeadlineUtil.Deadline(deadlineShipment);
                        if (deadlineResult.IsSuccess)
                        {
                            DeadlineShipmentResultViewModel deadline = deadlineResult.Data as DeadlineShipmentResultViewModel;
                            serviceInfo.ExpectedDeliveryTime = deadline.FormatDatetime;
                        }
                        //
                        serviceInfo.Price = price.TotalPrice;
                        services.Add(serviceInfo);
                    }
                }
            }
            return JsonUtil.Success(services);
        }

        [HttpPost("GetDistance")]
        public JsonResult GetDistance([FromBody]GoogleDistnaceModel viewModel)
        {
            ApiGoogle apiGoogle = new ApiGoogle();
            var aResult = apiGoogle.GetDistance(_icompanyInformation.ApiKey, viewModel.AddressFrom, viewModel.AddressTo, viewModel.LatFrom, viewModel.LngFrom, viewModel.LatTo, viewModel.LngTo);
            var result = aResult.Result;
            return JsonUtil.Success(result);
        }
    }
}
