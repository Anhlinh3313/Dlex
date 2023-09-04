using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Business.Services.Models;
using Core.Data.Abstract;
using Core.Infrastructure.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Core.Business.ViewModels;
using Core.Infrastructure.Utils;
using Core.Entity.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class ChargedRemoteController : BaseController
    {
        public ChargedRemoteController(Microsoft.Extensions.Logging.ILogger<dynamic> logger, IOptions<AppSettings> optionsAccessor, IOptions<JwtIssuerOptions> jwtOptions, IUnitOfWork unitOfWork) : base(logger, optionsAccessor, jwtOptions, unitOfWork)
        {
        }

        [HttpPost("SaveSetup")]
        public async Task<JsonResult> SaveSetup([FromBody] ChargedRemoteViewModel chargedRemote)
        {
            if (chargedRemote == null)
            {
                return JsonUtil.Error("Dữ liệu trống!");
            }
            _unitOfWork.RepositoryCRUD<ChargedRemote>().DeleteWhere(f => f.Id > 0);
            await _unitOfWork.CommitAsync();
            foreach (var item in chargedRemote.DistrictIds)
            {
                var check = _unitOfWork.RepositoryCRUD<ChargedRemote>().FindBy(f => f.DistrictId == item).FirstOrDefault();
                if (check == null)
                {
                    ChargedRemote chargedRemotenNew = new ChargedRemote();
                    chargedRemotenNew.DistrictId = item;
                    _unitOfWork.RepositoryCRUD<ChargedRemote>().Insert(chargedRemotenNew);
                    await _unitOfWork.CommitAsync();
                }
            }
            return JsonUtil.Success("Cập nhật thành công!");
        }

        [HttpGet("ProvinceSelected")]
        public async Task<JsonResult> ProvinceSelected()
        {
            var selected = await _unitOfWork.RepositoryCRUD<ChargedRemote>().GetAllAsync().Select(s => s.DistrictId).ToList<int>();
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
            var selected = await _unitOfWork.RepositoryCRUD<ChargedRemote>().GetAllAsync().Select(s => s.DistrictId).ToList<int>();
            var districtSelected = _unitOfWork.RepositoryR<District>().FindBy(f => selected.Contains(f.Id) && dataFilter.arrayInt1.Contains(f.ProvinceId))
                .Select(s => s.Id).ToList<int>();
            return JsonUtil.Success(districtSelected);
        }
    }
}
