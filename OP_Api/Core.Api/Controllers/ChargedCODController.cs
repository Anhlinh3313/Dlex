using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Core.Business.ViewModels;
using Core.Entity.Entities;
using Core.Infrastructure.Utils;
using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Data.Abstract;
using Core.Infrastructure.Helper;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class ChargedCODController : BaseController
    {
        public ChargedCODController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IOptions<JwtIssuerOptions> jwtOptions,
            IUnitOfWork unitOfWork,
            IGeneralService iGeneralService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
        }

        [HttpGet("ProvinceSelected")]
        public async Task<JsonResult> ProvinceSelected()
        {
            var selected = await _unitOfWork.RepositoryCRUD<ChargedCOD>().GetAllAsync().Select(s => s.DistrictId).ToList<int>();
            var provinceSelected = _unitOfWork.RepositoryR<District>().FindBy(f => selected.Contains(f.Id)).GroupBy(g => g.ProvinceId)
                .Select(g => g.First()).Select(s => s.ProvinceId).ToList<int>();
            return JsonUtil.Success(provinceSelected);
        }

        [HttpPost("DistrictAllowSelect")]
        public async Task<JsonResult> DistrictAllowSelect([FromBody] DataFilterViewModel dataFilter)
        {
            var districtAllowSelected = await _unitOfWork.RepositoryCRUD<District>().FindByAsync(f => dataFilter.arrayInt1.Contains(f.ProvinceId)).ToList<District>();
            return JsonUtil.Success(districtAllowSelected);
        }

        [HttpPost("DistrictSelected")]
        public async Task<JsonResult> DistrictSelected([FromBody] DataFilterViewModel dataFilter)
        {
            var selected = await _unitOfWork.RepositoryCRUD<ChargedCOD>().GetAllAsync().Select(s => s.DistrictId).ToList<int>();
            var districtSelected = _unitOfWork.RepositoryR<District>().FindBy(f => selected.Contains(f.Id) && dataFilter.arrayInt1.Contains(f.ProvinceId))
                .Select(s => s.Id).ToList<int>();
            return JsonUtil.Success(districtSelected);
        }

        [HttpPost("SaveSetup")]
        public async Task<JsonResult> SaveSetup([FromBody] ChargedCODViewModel chargedCOD)
        {
            if (chargedCOD == null)
            {
                return JsonUtil.Error("Dữ liệu trống!");
            }
            _unitOfWork.RepositoryCRUD<ChargedCOD>().DeleteWhere(f => f.Id > 0);
            await _unitOfWork.CommitAsync();
            foreach (var item in chargedCOD.DistrictIds)
            {
                var check = _unitOfWork.RepositoryCRUD<ChargedCOD>().FindBy(f => f.DistrictId == item).FirstOrDefault();
                if (check == null)
                {
                    ChargedCOD chargedCODnew = new ChargedCOD();
                    chargedCODnew.DistrictId = item;
                    _unitOfWork.RepositoryCRUD<ChargedCOD>().Insert(chargedCODnew);
                    await _unitOfWork.CommitAsync();
                }
            }
            return JsonUtil.Success("Cập nhật thành công!");
        }
    }
}
