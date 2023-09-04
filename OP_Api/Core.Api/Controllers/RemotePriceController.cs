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
    public class RemotePriceController : GeneralController<RemotePriceViewModel, RemotePrice>
    {
        IGeneralService<RemoteKm> _iGeneralServiceKm;
        IGeneralService<RemotePriceDetail> _iGeneralServiceDetail;
        public RemotePriceController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IOptions<JwtIssuerOptions> jwtOptions,
            IUnitOfWork unitOfWork,
            IGeneralService<RemotePriceViewModel, RemotePrice> iGeneralService,
            IGeneralService<RemoteKm> iGeneralServiceKm,
            IGeneralService<RemotePriceDetail> iGeneralServiceDetail
            ) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
            _iGeneralServiceKm = iGeneralServiceKm;
            _iGeneralServiceDetail = iGeneralServiceDetail;
        }

        [HttpPost("AddRemotePrice")]
        public async Task<JsonResult> AddRemotePrice()
        {
            RemotePrice remotePrice = new RemotePrice();
            var data = await _iGeneralService.Create(remotePrice);
            return JsonUtil.Create(data);
        }

        [HttpPost("AddRemoteKm")]
        public async Task<JsonResult> AddRemoteKm()
        {
            RemoteKm remoteKm = new RemoteKm();
            var data = await _iGeneralServiceKm.Create(remoteKm);
            return JsonUtil.Create(data);
        }

        [HttpGet("GetRemotePrice")]
        public async Task<JsonResult> GetRemotePrice()
        {
            var remotePrices = _iGeneralService.GetAll();
            var remoteKms = _iGeneralServiceKm.GetAll();
            var remotePriceDetials = _iGeneralServiceDetail.GetAll();
            var model = new
            {
                remotePrices = remotePrices,
                remoteKms = remoteKms,
                remotePriceDetials = remotePriceDetials
            };
            return JsonUtil.Success(model);
        }

        [HttpPost("UpdateRemotePrice")]
        public async Task<JsonResult> UpdateRemotePrice([FromBody]UpdateRemotePriceViewModel viewModel)
        {
            if (viewModel.RemotePrices.Count() == 0) return JsonUtil.Error("Vui lòng thêm cột");
            if (viewModel.RemoteKms.Count() == 0) return JsonUtil.Error("Vui lòng thêm dòng");
            foreach (var itemCol in viewModel.RemotePrices)
            {
                var col = _unitOfWork.RepositoryR<RemotePrice>().GetSingle(f => f.Id == itemCol.Id);
                if (!Util.IsNull(col))
                {
                    col.Name = itemCol.Name;
                    col.Code = itemCol.Code;
                    col.FromValue = itemCol.FromValue;
                    col.ToValue = itemCol.ToValue;
                    _unitOfWork.RepositoryCRUD<RemotePrice>().Update(col);
                }
            }
            foreach (var itemCol in viewModel.RemoteKms)
            {
                var col = _unitOfWork.RepositoryR<RemoteKm>().GetSingle(f => f.Id == itemCol.Id);
                if (!Util.IsNull(col))
                {
                    col.FromKm = itemCol.FromKm;
                    col.ToKm = itemCol.ToKm;
                    _unitOfWork.RepositoryCRUD<RemoteKm>().Update(col);
                }
            }
            foreach (var itemCol in viewModel.RemotePriceDetails)
            {
                var col = _unitOfWork.RepositoryR<RemotePriceDetail>().GetSingle(f => f.RemotePriceId == itemCol.RemotePriceId && f.RemoteKmId == itemCol.RemoteKmId);
                if (Util.IsNull(col))
                {
                    RemotePriceDetail remotePriceDetail = new RemotePriceDetail();
                    remotePriceDetail.Price = itemCol.Price;
                    remotePriceDetail.RemoteKmId = itemCol.RemoteKmId;
                    remotePriceDetail.RemotePriceId = itemCol.RemotePriceId;
                    _unitOfWork.RepositoryCRUD<RemotePriceDetail>().Insert(remotePriceDetail);
                }
                else
                {
                    col.Price = itemCol.Price;
                    _unitOfWork.RepositoryCRUD<RemotePriceDetail>().Update(col);
                }
            }
            await _unitOfWork.CommitAsync();
            return JsonUtil.Success("Lưu dữ liệu thành công");
        }
    }
}
