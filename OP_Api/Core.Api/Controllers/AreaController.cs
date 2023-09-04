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
using System.Linq.Expressions;
using Core.Entity.Procedures;
using Core.Data;
using Core.Data.Core;
using System.Threading;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class AreaController : GeneralController<AreaViewModel, AreaInfoViewModel, Area>
    {
        public IGeneralService _igeneralRawService { get; }



        public AreaController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor, IOptions<JwtIssuerOptions> jwtOptions,
            IUnitOfWork unitOfWork,
            IGeneralService<AreaViewModel, AreaInfoViewModel, Area> iGeneralService,
            IGeneralService igeneralRawService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
            _igeneralRawService = igeneralRawService;
        }



        [HttpGet("GetAreaByAreaGroupId")]
        public JsonResult GetAreaByAreaGroupId(int areaGroupId)
        {
            return base.FindBy(x => x.AreaGroupId == areaGroupId);
        }

        public override Task<JsonResult> Delete([FromBody] BasicViewModel viewModel)
        {
            _unitOfWork.RepositoryCRUD<AreaDistrict>().DeleteWhere(f => f.AreaId == viewModel.Id);
            return base.Delete(viewModel);
        }

        public override async Task<JsonResult> Create([FromBody] AreaViewModel viewModel)
        {
            //if (!ModelState.IsValid)
            //{
            //    return JsonUtil.Error(ModelState);
            //}
            var result = await _iGeneralService.Create(viewModel);
            if (result.IsSuccess)
            {
                _unitOfWork.RepositoryCRUD<AreaDistrict>().DeleteWhere(r => r.AreaId == viewModel.Id);// xóa quận / huyện đã chọn
                List<AreaDistrict> listAreaDistrict = new List<AreaDistrict>();
                var area = result.Data as AreaInfoViewModel;
                if (!Util.IsNull(viewModel.DistrictIds))
                {
                    foreach (int item in viewModel.DistrictIds)
                    {
                        AreaDistrict areaDistrict = new AreaDistrict();
                        areaDistrict.AreaId = area.Id;
                        areaDistrict.DistrictId = item;
                        listAreaDistrict.Add(areaDistrict);
                    }
                    if (listAreaDistrict != null)
                    {
                        await _igeneralRawService.Create<AreaDistrict>(listAreaDistrict);
                    }
                }
            }
            //
            return JsonUtil.Create(result);
        }

        public override async Task<JsonResult> Update([FromBody] AreaViewModel viewModel)
        {
            //if (ModelState.IsValid)
            //{
            //    return JsonUtil.Error(ModelState);
            //}
            var result = await _iGeneralService.Update(viewModel);
            if (result.IsSuccess)
            {
                _unitOfWork.RepositoryCRUD<AreaDistrict>().DeleteWhere(r => r.AreaId == viewModel.Id);// xóa quận / huyện đã chọn
                List<AreaDistrict> listAreaDistrict = new List<AreaDistrict>();
                var area = result.Data as AreaInfoViewModel;
                foreach (int item in viewModel.DistrictIds)
                {
                    AreaDistrict areaDistrict = new AreaDistrict();
                    areaDistrict.AreaId = area.Id;
                    areaDistrict.DistrictId = item;
                    listAreaDistrict.Add(areaDistrict);
                }
                if (listAreaDistrict != null)
                {
                    await _igeneralRawService.Create<AreaDistrict>(listAreaDistrict);
                }
            }
            return JsonUtil.Create(result);
        }

        [HttpPost("UpdateAreaDistricts")]
        public JsonResult UpdateAreaDistricts([FromBody] AreaViewModel area)
        {
            var data =  _unitOfWork.Repository<Proc_UpdateAreaDistricts>().ExecProcedureSingle(Proc_UpdateAreaDistricts.GetEntityProc(area.Id, area.MultiSelectDistrict, area.MultiSelectFromProvince));
            return JsonUtil.Success(data);
        }

        [HttpPost("UpdateAreas")]
        public async Task<JsonResult> UpdateAreas([FromBody] List<AreaViewModel> areas)
        {
            var data = await _iGeneralService.Update(areas);
            //int i = 0;
            //while (areas.Count() > 0)
            //{
            //    AreaViewModel area = areas[i];
            //    await _unitOfWork.Repository<Proc_UpdateAreaDistricts>().ExecProcedureCMDAsync(Proc_UpdateAreaDistricts.GetEntityProc(area.Id, area.MultiSelectDistrict, area.MultiSelectFromProvince));
            //    i++;
            //    if (i == areas.Count()) break;
            //}
            //_unitOfWork.Commit();
            return JsonUtil.Success(data);
        }

        [HttpPost("DeleteArea")]
        public JsonResult DeleteArea([FromBody] PriceServiceDetailViewModel model)
        {
            //
            if(_unitOfWork.RepositoryR<PriceServiceDetail>().Any(f => f.AreaId == model.Id && f.PriceServiceId == model.PriceServiceId))
            {
                _unitOfWork.RepositoryCRUD<PriceServiceDetail>().DeleteWhere(f => f.AreaId == model.Id && f.PriceServiceId == model.PriceServiceId);//
                _unitOfWork.RepositoryCRUD<AreaDistrict>().DeleteWhere(r => r.AreaId == model.Id);// xóa quận / huyện đã chọn
                _unitOfWork.RepositoryCRUD<FromProvinceArea>().DeleteWhere(f => f.AreaId == model.Id);
                _unitOfWork.RepositoryCRUD<Area>().Delete(model.Id);
                _unitOfWork.Commit();
                //
                return JsonUtil.Success();
            }
            else
            {
                return JsonUtil.Error("Xóa cột lỗi, vui lòng lưu Ctrl + F5 và thử lại.");
            }
        }
        //
        [HttpGet("GetDistrictByArea")]
        public JsonResult GetDistrictByArea(int areaId)
        {
            var listDistrict = _unitOfWork.RepositoryR<AreaDistrict>().FindBy(f => f.AreaId == areaId).Select(s => s.DistrictId).ToArray();
            var dataDistrict = _unitOfWork.RepositoryR<District>().FindBy(f => listDistrict.Contains(f.Id));
            return JsonUtil.Success(dataDistrict);
        }
        [HttpGet("GetProvinceByArea")]
        public JsonResult GetProvinceByArea(int areaId)
        {
            var listDistrict = _unitOfWork.RepositoryR<AreaDistrict>().FindBy(f => f.AreaId == areaId).Select(s => s.DistrictId).ToArray();
            var dataDistrict = _unitOfWork.RepositoryR<District>().FindBy(f => listDistrict.Contains(f.Id));
            var listProvince = dataDistrict.GroupBy(g => g.ProvinceId).Select(g => g.First()).Select(s => s.ProvinceId).ToArray();
            var dataProvince = _unitOfWork.RepositoryR<Province>().FindBy(f => listProvince.Contains(f.Id));
            return JsonUtil.Success(dataProvince);
        }
        //
        [HttpPost("GetDistrictAllowSelect")]
        public JsonResult GetDistrictAllowSelect([FromBody] DataFilterViewModel dataFilter)
        {
            var listDistrictInProvince = _unitOfWork.RepositoryR<District>().FindBy(f => dataFilter.arrayInt1.Contains(f.ProvinceId)).Select(s => s.Id).ToArray();
            var selectedDistrict = _unitOfWork.RepositoryR<AreaDistrict>()
                .FindBy(f => listDistrictInProvince.Contains(f.DistrictId) && f.AreaId == dataFilter.typeInt2).Select(s => s.DistrictId).ToArray();
            //
            var selectedAreas = _unitOfWork.RepositoryR<Area>()
                .FindBy(f => f.AreaGroupId == dataFilter.typeInt1).Select(s => s.Id).ToArray();
            var selectedDistricts = _unitOfWork.RepositoryR<AreaDistrict>()
                .FindBy(f => selectedAreas.Contains(f.AreaId)).Select(s => s.DistrictId).ToArray();
            var allowDistricts = _unitOfWork.RepositoryR<AreaDistrict>()
                .FindBy(f => listDistrictInProvince.Contains(f.DistrictId) && (selectedDistrict.Contains(f.DistrictId) || !selectedDistricts.Contains(f.DistrictId))).Select(s => s.DistrictId).ToArray();
            var resultDistricts = _unitOfWork.RepositoryR<District>()
                .FindBy(f => listDistrictInProvince.Contains(f.Id) && (!selectedDistricts.Contains(f.Id) || allowDistricts.Contains(f.Id)));
            return JsonUtil.Success(resultDistricts);
        }
        //
        [HttpPost("GetDistrictSelected")]
        public JsonResult GetDistrictSelected([FromBody] DataFilterViewModel dataFilter)
        {
            var listDistrictInProvince = _unitOfWork.RepositoryR<District>().FindBy(f => dataFilter.arrayInt1.Contains(f.ProvinceId)).Select(s => s.Id).ToArray();
            var selectedDistrict = _unitOfWork.RepositoryR<AreaDistrict>()
                .FindBy(f => listDistrictInProvince.Contains(f.DistrictId) && f.AreaId == dataFilter.typeInt2).Select(s => s.DistrictId).ToArray();
            //
            var resultDistricts = _unitOfWork.RepositoryR<District>()
                .FindBy(f => listDistrictInProvince.Contains(f.Id) && (selectedDistrict.Contains(f.Id)));
            return JsonUtil.Success(resultDistricts);
        }
        //
        [HttpGet("GetProvinceAllowSelectByArea")]
        public JsonResult GetProvinceAllowSelectByArea(int areaGroupId = 0, int areaId = 0)
        {
            //
            var selectedAreas = _unitOfWork.RepositoryR<Area>()
                .FindBy(f => f.AreaGroupId == areaGroupId).Select(s => s.Id).ToArray();
            var thisSelectedDistricts = _unitOfWork.RepositoryR<AreaDistrict>()
                .FindBy(f => f.AreaId == areaId).Select(s => s.DistrictId).ToArray();
            var otherSelectedDistricts = _unitOfWork.RepositoryR<AreaDistrict>()
                .FindBy(f => selectedAreas.Contains(f.AreaId) && f.AreaId != areaId).Select(s => s.DistrictId).ToArray();
            var freeSelectedDistricts = _unitOfWork.RepositoryR<District>()
                .FindBy(f => !thisSelectedDistricts.Contains(f.Id) && !otherSelectedDistricts.Contains(f.Id)).Select(s => s.Id).ToArray();
            var allowProvinces = _unitOfWork.RepositoryR<District>()
                .FindBy(f => !otherSelectedDistricts.Contains(f.Id) &&
                (freeSelectedDistricts.Contains(f.Id) || thisSelectedDistricts.Contains(f.Id))).GroupBy(g => g.ProvinceId).Select(g => g.First()).Select(s => s.ProvinceId).ToArray();
            var resultProvinces = _unitOfWork.RepositoryR<Province>()
                .FindBy(f => allowProvinces.Contains(f.Id));
            return JsonUtil.Success(resultProvinces);
        }
        //
        [HttpGet("GetProvinceSelectedByArea")]
        public JsonResult GetProvinceSelectedByArea(int areaGroupId = 0, int areaId = 0)
        {
            //var selectedArea = _unitOfWork.RepositoryR<AreaDistrict>()
            //    .FindBy(s => s.AreaId == areaId).Select(s => s.DistrictId).ToArray();
            //
            var selectedAreas = _unitOfWork.RepositoryR<Area>()
                .FindBy(f => f.AreaGroupId == areaGroupId && f.Id == areaId).Select(s => s.Id).ToArray();
            var selectedDistricts = _unitOfWork.RepositoryR<AreaDistrict>()
                .FindBy(f => selectedAreas.Contains(f.AreaId)).Select(s => s.DistrictId).ToArray();
            var allowProvinces = _unitOfWork.RepositoryR<District>()
                .FindBy(f => selectedDistricts.Contains(f.Id)).GroupBy(g => g.ProvinceId).Select(g => g.First()).Select(s => s.ProvinceId).ToArray();
            var resultProvinces = _unitOfWork.RepositoryR<Province>()
                .FindBy(f => allowProvinces.Contains(f.Id));
            return JsonUtil.Success(resultProvinces);
        }
    }
}
