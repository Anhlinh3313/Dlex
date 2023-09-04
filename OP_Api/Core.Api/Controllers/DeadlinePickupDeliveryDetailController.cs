using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Core.Entity.Entities;
using Core.Business.ViewModels;
using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Data.Abstract;
using Core.Infrastructure.Helper;
using Microsoft.Extensions.Options;
using Core.Infrastructure.Utils;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class DeadlinePickupDeliveryDetailController : GeneralController<DeadlinePickupDeliveryDetailViewModel, DeadlinePickupDeliveryDetail>
    {
        public DeadlinePickupDeliveryDetailController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IOptions<JwtIssuerOptions> jwtOptions,
            IUnitOfWork unitOfWork,
            IGeneralService<DeadlinePickupDeliveryDetailViewModel, DeadlinePickupDeliveryDetail> iGeneralService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
        }

        [HttpPost("GetByDeadlinePickupDelivery")]
        public JsonResult GetByDeadlinePickupDelivery([FromBody]DataFilterViewModel dataFilter)
        {
            return base.FindBy(x => x.DeadlinePickupDeliveryId == dataFilter.typeInt1, null, null, dataFilter.typeString1);
        }


        [HttpPost("UploadExcelDeadline")]
        public JsonResult UploadExcelDeadline([FromBody]DeadlinePickupDeliveryDetailUploadExcelViewModel dataExcels)
        {
            if (dataExcels.AreaCodes.Count() == 0)
            {
                return JsonUtil.Error(ValidatorMessage.UploadExcelPrice.AreaCodesNotEmpty);
            }
            else if (dataExcels.ServiceCodes.Count() == 0)
            {
                return JsonUtil.Error(ValidatorMessage.UploadExcelDeadline.ServiceCodesNotEmpty);
            }
            else if (dataExcels.DeadlinePickupDeliveryViewModel == null)
            {
                return JsonUtil.Error(ValidatorMessage.UploadExcelDeadline.DeadlinePickupDeliveryNotEmpty);
            }
            //
            var dataArea = _unitOfWork.RepositoryR<Area>().FindBy(f => f.AreaGroupId == dataExcels.DeadlinePickupDeliveryViewModel.AreaGroupId);
            if (dataArea.Count() == 0)
            {
                return JsonUtil.Error(ValidatorMessage.UploadExcelPrice.AreaGroupNotEmpty);
            }
            foreach (string code in dataExcels.AreaCodes)
            {
                var checkArea = dataArea.FirstOrDefault(f => f.Code == code);
                if (checkArea == null)
                {
                    return JsonUtil.Error(string.Format(ValidatorMessage.UploadExcelPrice.AreaCodeNotExist, code));
                }
            }
            //
            var dataService = _unitOfWork.RepositoryR<Service>().GetAll();
            if (dataService.Count() == 0)
            {
                return JsonUtil.Error(ValidatorMessage.UploadExcelDeadline.SystemNotEmpty);
            }
            foreach (string code in dataExcels.ServiceCodes)
            {
                var checkWeight = dataService.FirstOrDefault(f => f.Code == code);
                if (checkWeight == null)
                {
                    return JsonUtil.Error(string.Format(ValidatorMessage.UploadExcelDeadline.WeightServiceNotExist, code));
                }
            }
            if (dataExcels.DeadlineUploadExcelViewModels.Count() == 0)
            {
                return JsonUtil.Error(ValidatorMessage.UploadExcelDeadline.DeadlineNotEmpty);
            }
            foreach (var data in dataExcels.DeadlineUploadExcelViewModels)
            {
                var areaId = dataArea.FirstOrDefault(f => f.Code == data.AreaCode).Id;
                var serviceId = dataService.FirstOrDefault(f => f.Code == data.ServiceCode).Id;
                var deadline = _unitOfWork.RepositoryR<DeadlinePickupDeliveryDetail>()
                    .GetSingle(s => s.AreaId == areaId && s.ServiceId == serviceId && s.DeadlinePickupDeliveryId == dataExcels.DeadlinePickupDeliveryViewModel.Id);
                if (deadline == null)//insert
                {
                    deadline = new DeadlinePickupDeliveryDetail();
                    deadline.DeadlinePickupDeliveryId = dataExcels.DeadlinePickupDeliveryViewModel.Id;
                    deadline.AreaId = areaId;
                    deadline.ServiceId = serviceId;
                }
                if (data.TimeDelivery.HasValue)
                {
                    deadline.TimeDelivery = data.TimeDelivery.Value;
                }
                if (data.TimePickup.HasValue)
                {
                    deadline.TimePickup = data.TimePickup.Value;
                }
                deadline.IsEnabled = true;
                _unitOfWork.RepositoryCRUD<DeadlinePickupDeliveryDetail>().InsertAndUpdate(deadline);
            }
            _unitOfWork.RepositoryCRUD<DeadlinePickupDeliveryDetail>().Commit();
            return JsonUtil.Success("Thành công!");
        }
    }
}
