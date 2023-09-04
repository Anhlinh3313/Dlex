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
	public partial class WeightController : GeneralController<WeightViewModel, WeightInfoViewModel, Weight>
    {
        public WeightController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger, 
            IOptions<AppSettings> optionsAccessor, 
            IOptions<JwtIssuerOptions> jwtOptions, 
            IUnitOfWork unitOfWork,
            IGeneralService<WeightViewModel, WeightInfoViewModel, Weight> iGeneralService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
        }

        [HttpGet("GetByWeightGroup")]
        public JsonResult GetByWeightGroup(int weightGroupId, int? priceServiceId = 0)
        {
            var listPriceDetail = _unitOfWork.RepositoryR<PriceServiceDetail>().FindBy(f => f.PriceServiceId == priceServiceId)
                .GroupBy(g=>g.WeightId)
                .Select(group => group.FirstOrDefault().WeightId).Select(s=>s).ToList();
            var data = base.FindBy(x => x.WeightGroupId == weightGroupId || listPriceDetail.Contains(x.Id));
            return data;
        }

        [HttpPost("UpdateWeights")]
        public async Task<JsonResult> UpdateWeights([FromBody] List<WeightViewModel> weights)
        {
            var data = await _iGeneralService.Update(weights);
            return JsonUtil.Success(data);
        }

        [HttpPost("DeleteWeight")]
        public JsonResult DeleteWeight([FromBody] PriceServiceDetailViewModel model)
        {
            //
            if(_unitOfWork.RepositoryR<PriceServiceDetail>().Any(f=>f.WeightId == model.Id && f.PriceServiceId == model.PriceServiceId))
            {
                _unitOfWork.RepositoryCRUD<PriceServiceDetail>().DeleteWhere(f => f.WeightId == model.Id && f.PriceServiceId == model.PriceServiceId);
                _unitOfWork.RepositoryCRUD<Weight>().Delete(model.Id);
                _unitOfWork.Commit();
                //
                return JsonUtil.Success();
            }
            else
            {
                return JsonUtil.Error("Xóa bảng giá lỗi, vui lòng Ctrl + F5 và thử lại!");
            }
        }
    }
}
