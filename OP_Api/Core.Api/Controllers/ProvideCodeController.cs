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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class ProvideCodeController : GeneralController<ProvideCodeViewModel, ProvideCodeInfoViewModel, ProvideCode>
    {
        public ProvideCodeController
            (
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IOptions<JwtIssuerOptions> jwtOptions,
            IUnitOfWork unitOfWork,
            IGeneralService<ProvideCodeViewModel, ProvideCodeInfoViewModel, ProvideCode> iGeneralService
            ) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
        }

        [HttpPost("Provide")]
        public async Task<JsonResult> Provide([FromBody]ProvideCodeCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return JsonUtil.Error(ModelState);
            }
            if (viewModel.NumberStart < viewModel.NumberEnd)
            {
                List<string> autogenousCodes = new List<string>();
                for (int i = viewModel.NumberStart; i <= viewModel.NumberEnd; i++)
                {
                    autogenousCodes.Add($"{viewModel.Prefix + RandomUtil.GetCode(i, viewModel.Length)}");
                }
                var checkShipments = _unitOfWork.RepositoryCRUD<Shipment>().FindBy(provideCode => autogenousCodes.Contains(provideCode.ShipmentNumber));
                if (checkShipments.Count() > 0)
                {
                    return JsonUtil.Error(ValidatorMessage.ProvideCode.ProvideCodeExistInShipment, checkShipments);
                }
                var datas = _unitOfWork.RepositoryR<ProvideCode>().FindBy(f => autogenousCodes.Contains(f.Code));
                if (datas.Count() == 0)
                {
                    List<ProvideCode> listSuccess = new List<ProvideCode>();
                    foreach (string code in autogenousCodes)
                    {
                        ProvideCode provideCode = new ProvideCode();
                        provideCode.Code = code;
                        provideCode.IsEnabled = true;
                        provideCode.ProvideCodeStatusId = ProvideCodeStatusHelper.NewProvideCode;
                        provideCode.ProvideCustomerId = viewModel.ProvideCustomerId;
                        provideCode.ProvideHubId = viewModel.ProvideHubId;
                        provideCode.ProvideUserId = viewModel.ProvideUserId;
                        _unitOfWork.RepositoryCRUD<ProvideCode>().Insert(provideCode);
                        listSuccess.Add(provideCode);
                    }
                    if (listSuccess.Count() == autogenousCodes.Count())
                    {
                        _unitOfWork.RepositoryCRUD<ProvideCode>().Commit();
                        return JsonUtil.Success(listSuccess);
                    }
                    else
                    {
                        _unitOfWork.RepositoryCRUD<ProvideCode>().Dispose();
                        return JsonUtil.Error(ValidatorMessage.ProvideCode.CreateError);
                    }
                }
                else if (datas.Count() > 0)
                {
                    if (datas.Count() < autogenousCodes.Count())
                    {
                        return JsonUtil.Error(ValidatorMessage.ProvideCode.ProvideCodeExist, datas);
                    }
                    else if (datas.Count() == autogenousCodes.Count())
                    {
                        var currentUserId = GetCurrentUserId();
                        var checkUsed = datas.Where(f => f.ProvideCodeStatusId == ProvideCodeStatusHelper.UsedCode
                        || f.ProvideCodeStatusId == ProvideCodeStatusHelper.CancelCode
                        || (f.ProvideUserId != null && f.ProvideUserId.Value != currentUserId)
                        || f.ProvideCodeStatusId == ProvideCodeStatusHelper.NewProvideCode
                        || f.ProvideCodeStatusId == ProvideCodeStatusHelper.ErrorCode);
                        if (checkUsed.Count() > 0)
                        {
                            return JsonUtil.Error(ValidatorMessage.ProvideCode.ProvideCodeUsed, checkUsed);
                        }
                        else
                        {
                            foreach (var data in datas)
                            {

                                data.ProvideCodeStatusId = ProvideCodeStatusHelper.NewProvideCode;
                                data.ProvideCustomerId = viewModel.ProvideCustomerId;
                                data.ProvideHubId = viewModel.ProvideHubId;
                                data.ProvideUserId = viewModel.ProvideUserId;
                                _unitOfWork.RepositoryCRUD<ProvideCode>().Update(data);
                            }
                            _unitOfWork.RepositoryCRUD<ProvideCode>().Commit();
                            return JsonUtil.Success(datas);
                        }
                    }
                    else
                    {
                        return JsonUtil.Error(ValidatorMessage.ProvideCode.DataError, datas);
                    }

                }
                else
                {
                    return JsonUtil.Error(ValidatorMessage.ProvideCode.DataError, datas);
                }
            }
            else if (viewModel.NumberStart == viewModel.NumberEnd)
            {
                if (viewModel.Count <= 0)
                {
                    return JsonUtil.Error(ValidatorMessage.ProvideCode.ProvideCountError);
                }
                else
                {
                    List<ProvideCode> listDatas = new List<ProvideCode>();
                    for (int i = 0; i < viewModel.Count; i++)
                    {
                        ProvideCode provideCode = new ProvideCode();
                        provideCode.ProvideCodeStatusId = ProvideCodeStatusHelper.NewProvideCode;
                        provideCode.ProvideCustomerId = viewModel.ProvideCustomerId;
                        provideCode.ProvideHubId = viewModel.ProvideHubId;
                        provideCode.ProvideUserId = viewModel.ProvideUserId;
                        _unitOfWork.RepositoryCRUD<ProvideCode>().Insert(provideCode);
                        _unitOfWork.RepositoryCRUD<ProvideCode>().Commit();
                        provideCode.Code = $"{viewModel.Prefix + RandomUtil.GetCode(provideCode.Id, viewModel.Length)}";
                        var checkListCoesInShipment = _unitOfWork.RepositoryR<Shipment>().FindBy(proCode => proCode.ShipmentNumber == provideCode.Code);
                        if (checkListCoesInShipment.Count() > 0)
                        {
                            provideCode.ProvideCodeStatusId = ProvideCodeStatusHelper.ErrorCode;
                            provideCode.IsEnabled = false;
                        }
                        var checkListCodesInProvide = _unitOfWork.RepositoryR<ProvideCode>().FindBy(f => f.Code == provideCode.Code && f.Id != provideCode.Id);
                        if (checkListCodesInProvide.Count() > 0)
                        {
                            provideCode.ProvideCodeStatusId = ProvideCodeStatusHelper.ErrorCode;
                            provideCode.IsEnabled = false;
                        }
                        _unitOfWork.RepositoryCRUD<ProvideCode>().Update(provideCode);
                        _unitOfWork.RepositoryCRUD<ProvideCode>().Commit();
                        listDatas.Add(provideCode);
                    }
                    return JsonUtil.Success(listDatas);
                }
            }
            else
            {
                return JsonUtil.Error(ValidatorMessage.ProvideCode.NumberStartAdnEndInValid);
            }
        }

        [HttpGet("Trackings")]
        public async Task<JsonResult> Trackings(string code, string cols)
        {
            List<string> listCol = new List<string>();
            if (cols != null)
            {
                foreach (string col in cols.Split(","))
                {
                    listCol.Add(col);
                }
            }
            var data = _unitOfWork.RepositoryR<ProvideCode>().FindBy(x => x.Code == code, listCol.ToArray());
            return JsonUtil.Success(data);
        }
    }
}
