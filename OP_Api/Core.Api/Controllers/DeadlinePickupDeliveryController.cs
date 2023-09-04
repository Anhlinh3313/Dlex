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
using Core.Business.Core.Utils;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class DeadlinePickupDeliveryController : GeneralController<DeadlinePickupDeliveryViewModel, DeadlinePickupDeliveryInfoViewModel, DeadlinePickupDelivery>
    {
        public DeadlinePickupDeliveryController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IOptions<JwtIssuerOptions> jwtOptions,
            IUnitOfWork unitOfWork,
            IGeneralService<DeadlinePickupDeliveryViewModel, DeadlinePickupDeliveryInfoViewModel, DeadlinePickupDelivery> iGeneralService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
        }

        [HttpPost("DeadlineDelivery")]
        public JsonResult DeadlineDelivery([FromBody]DeadlineShipmentViewModel viewModel)
        {
            var result = DeadlineUtil.Deadline(viewModel);
            return JsonUtil.Create(result);
        }

        public override Task<JsonResult> Delete([FromBody] BasicViewModel viewModel)
        {
            _unitOfWork.RepositoryCRUD<DeadlinePickupDeliveryDetail>().DeleteWhere(f => f.DeadlinePickupDeliveryId == viewModel.Id);
            _unitOfWork.CommitAsync();
            return base.Delete(viewModel);
        }
    }
}
