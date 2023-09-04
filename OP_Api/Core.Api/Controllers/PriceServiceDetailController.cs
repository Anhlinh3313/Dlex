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
using Core.Entity.Procedures;
using Core.Business.ViewModels.PriceServiceDetails;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class PriceServiceDetailController : GeneralController<PriceServiceDetailViewModel, PriceServiceDetailInfoViewModel, PriceServiceDetail>
    {
        // GET: api/values
        public PriceServiceDetailController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IOptions<JwtIssuerOptions> jwtOptions,
            IUnitOfWork unitOfWork,
            IGeneralService<PriceServiceDetailViewModel, PriceServiceDetailInfoViewModel, PriceServiceDetail> iGeneralService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
        }

        [HttpPost("GetByPriceService")]
        public JsonResult GetByPriceService([FromBody]DataFilterViewModel dataFilter)
        {
            return base.FindBy(x => x.PriceServiceId == dataFilter.typeInt1, null, null, dataFilter.typeString1);
        }

        [HttpGet("GetByPriceServiceId")]
        public JsonResult GetByPriceServiceId(int? areaGroupId, int? priceServiceId)
        {
            var priceServiceDetails = _unitOfWork.RepositoryCRUD<PriceServiceDetail>().FindBy(x => x.PriceServiceId == priceServiceId).ToList();
            //if (priceServiceDetails.Count() == 0) return JsonUtil.Error("Không tìm thấy giá chi tiết!");
            var areas = _unitOfWork.Repository<Proc_GetAreaByPriceService>()
                      .ExecProcedure(Proc_GetAreaByPriceService.GetEntityProc(priceServiceId)).ToList();

            //var freeDistrictSelect = _unitOfWork.Repository<Proc_DistrictFreeSelectPriceServiceDetail>()
            //          .ExecProcedure(Proc_DistrictFreeSelectPriceServiceDetail.GetEntityProc(areaGroupId, priceServiceId)).ToList();

            var districtSelected = _unitOfWork.Repository<Proc_DistrictSelectedPriceServiceDetail>()
                      .ExecProcedure(Proc_DistrictSelectedPriceServiceDetail.GetEntityProc(areaGroupId, priceServiceId)).ToList();

            //var freeProvinceSelect = _unitOfWork.Repository<Proc_ProvinceFreeSelectPriceServiceDetail>()
            //          .ExecProcedure(Proc_ProvinceFreeSelectPriceServiceDetail.GetEntityProc(areaGroupId, priceServiceId)).ToList();

            //var provinceSelected = _unitOfWork.Repository<Proc_ProvinceSelectedPriceServiceDetail>()
            //          .ExecProcedure(Proc_ProvinceSelectedPriceServiceDetail.GetEntityProc(areaGroupId, priceServiceId)).ToList();
            
            var fromProvinces = _unitOfWork.Repository<Proc_FromProvincePriceServiceSelected>()
                .ExecProcedure(Proc_FromProvincePriceServiceSelected.GetEntityProc(priceServiceId)).ToList();

            var fromDistricts = _unitOfWork.Repository<Proc_FromDistrictPriceServiceSelected>()
               .ExecProcedure(Proc_FromDistrictPriceServiceSelected.GetEntityProc(priceServiceId)).ToList();
            //var fromProvincesAllow = _unitOfWork.RepositoryR<Province>().FindBy(f => !fromProvinces.Select(s => s.Id).Contains(f.Id)).ToList();
            var model = new
            {
                priceServiceDetails = priceServiceDetails,
                areas = areas,
                //freeDistrictSelect = freeDistrictSelect,
                districtSelected = districtSelected,
                //freeProvinceSelect = freeProvinceSelect,
                //provinceSelected = provinceSelected,
                fromProvinces = fromProvinces,
                fromDistricts = fromDistricts

            };

            return JsonUtil.Success(model);
        }

        [HttpPost("UploadExcelPriceService")]
        public JsonResult UploadExcelPriceService([FromBody]PriceServiceDetailExcelViewModel dataExcels)
        {
            if (dataExcels.AreaCodes.Count() == 0)
            {
                return JsonUtil.Error(ValidatorMessage.UploadExcelPrice.AreaCodesNotEmpty);
            }
            else if (dataExcels.WeightCodes.Count() == 0)
            {
                return JsonUtil.Error(ValidatorMessage.UploadExcelPrice.WeightCodesNotEmpty);
            }
            else if (dataExcels.PriceServiceViewModel == null)
            {
                return JsonUtil.Error(ValidatorMessage.UploadExcelPrice.PriceServiceNotEmpty);
            }
            //
            var dataArea = _unitOfWork.RepositoryR<Area>().FindBy(f => f.AreaGroupId == dataExcels.PriceServiceViewModel.AreaGroupId);
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
            var dataWeight = _unitOfWork.RepositoryR<Weight>().FindBy(f => f.WeightGroupId == dataExcels.PriceServiceViewModel.WeightGroupId);
            if (dataWeight.Count() == 0)
            {
                return JsonUtil.Error(ValidatorMessage.UploadExcelPrice.WeightGroupNotEmpty);
            }
            foreach (string code in dataExcels.WeightCodes)
            {
                var checkWeight = dataWeight.FirstOrDefault(f => f.Code == code);
                if (checkWeight == null)
                {
                    return JsonUtil.Error(string.Format(ValidatorMessage.UploadExcelPrice.WeightCodeNotExist, code));
                }
            }
            if (dataExcels.PriceUploadExcelViewModel.Count() == 0)
            {
                return JsonUtil.Error(ValidatorMessage.UploadExcelPrice.PriceServiceDetailNotEmpty);
            }
            foreach (var data in dataExcels.PriceUploadExcelViewModel)
            {
                var areaId = dataArea.FirstOrDefault(f => f.Code == data.AreaCode).Id;
                var weightId = dataWeight.FirstOrDefault(f => f.Code == data.WeightCode).Id;
                var price = _unitOfWork.RepositoryR<PriceServiceDetail>()
                    .GetSingle(s => s.AreaId == areaId && s.WeightId == weightId && s.PriceServiceId == dataExcels.PriceServiceViewModel.Id);
                if (price == null)//insert
                {
                    price = new PriceServiceDetail();
                    price.PriceServiceId = dataExcels.PriceServiceViewModel.Id;
                    price.AreaId = areaId;
                    price.WeightId = weightId;
                }
                price.IsEnabled = true;
                price.Price = data.Price;
                _unitOfWork.RepositoryCRUD<PriceServiceDetail>().InsertAndUpdate(price);
            }
            _unitOfWork.RepositoryCRUD<PriceServiceDetail>().Commit();
            return JsonUtil.Success("Thành công!");
        }

        [HttpPost("UpdateFromProvince")]
        public async Task<JsonResult> UpdateFromProvince([FromBody] FromProvinceServiceViewModel viewModel)
        {
            if (Util.IsNull(viewModel)) return JsonUtil.Error("Dữ liệu trống");
            _unitOfWork.RepositoryCRUD<FromProvinceService>().DeleteEmptyWhere(f => f.PriceServiceId == viewModel.PriceServiceId);

            if (viewModel.ProvinceIds.Count() > 0)
            {
                foreach (var item in viewModel.ProvinceIds)
                {
                    FromProvinceService data = new FromProvinceService();
                    data.PriceServiceId = viewModel.PriceServiceId;
                    data.ProvinceId = item;
                    _unitOfWork.RepositoryCRUD<FromProvinceService>().Insert(data);
                }
            }
            _unitOfWork.Commit();
            return JsonUtil.Success("Cập nhật dữ liệu thành công");
        }

        [HttpPost("UpdatePriceServices")]
        public async Task<JsonResult> UpdatePriceServices([FromBody] List<PriceServiceDetailViewModel> models)
        {
            var listCreate = models.FindAll(f => f.Id == 0);

            var listUpdate = models.FindAll(f => f.Id != 0);
            var data = await _iGeneralService.Update(listUpdate);
            foreach (var item in listCreate)
            {
                var any = _unitOfWork.RepositoryR<PriceServiceDetail>()
                    .GetSingle(f => f.PriceServiceId == item.PriceServiceId && f.AreaId == item.AreaId && f.WeightId == item.WeightId);
                if (Util.IsNull(any))
                {
                    var dataCreate = await _iGeneralService.Create(item);
                }
                else
                {
                    if(Util.IsNull(any.Id)|| any.Id == 0)
                    {
                        var dataCreate = await _iGeneralService.Create(item);
                    }
                    else
                    {
                        item.Id = any.Id;
                        var dataUpdate = await _iGeneralService.Update(item);
                    }
                }
            }
            return JsonUtil.Success(data);
        }
        [HttpPost("GetByPriceServiceDetail")]
        public JsonResult GetByPriceServiceDetail([FromBody] FilterPriceServiceDetailViewModel viewModel)
        {
            var priceServiceDetails = _unitOfWork.RepositoryCRUD<PriceServiceDetail>().FindBy(x => x.PriceServiceId == viewModel.PriceServiceId &&
            (viewModel.WeightIds.Contains(x.WeightId) || (viewModel.WeightIds.Count() == 0))).ToList();

            return JsonUtil.Success(priceServiceDetails);
        }
    }
}
