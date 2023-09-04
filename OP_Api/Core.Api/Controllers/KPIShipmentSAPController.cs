using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Business.ViewModels;
using Core.Data.Abstract;
using Core.Entity.Entities;
using Core.Entity.Procedures;
using Core.Infrastructure.Helper;
using Core.Infrastructure.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KPIShipmentSAPController : BaseController
    {
        private readonly IGeneralService _iGeneralService;
        public KPIShipmentSAPController(
           Microsoft.Extensions.Logging.ILogger<dynamic> logger,
           IOptions<AppSettings> optionsAccessor,
           IOptions<JwtIssuerOptions> jwtOptions,
           IUnitOfWork unitOfWork,
           IGeneralService iGeneralService
           ) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
            _iGeneralService = iGeneralService;
        }

        [HttpGet("GetHubRoutingExport")]
        public async Task<JsonResult> GetHubRoutingExport(int? codeConnect = null, int? code = null, int? userId = null, int? centerHubId = null, int? poHubId = null, int? stationHubId = null)
        {
            var res = _unitOfWork.Repository<Proc_GetHubRoutingExport>()
               .ExecProcedure(Proc_GetHubRoutingExport.GetEntityProc(codeConnect, code, userId, centerHubId, poHubId, stationHubId)).ToList();

            return JsonUtil.Success(res);
        }
        [HttpPost("CheckUpLoadExcel")]
        public async Task<JsonResult> CheckUpLoadExcel([FromBody]List<UpLoadExcelKPIModel> viewModels)
        {
            string HubCodes = "";
            string COTCodes = "";
            foreach(var item in viewModels)
            {
                HubCodes = HubCodes + item.HubRoutingCode +",";
                COTCodes = COTCodes + item.CutOffTimeCode + ",";
            }
            try { 
                var res = _unitOfWork.Repository<Proc_CheckKPIUpLoad>()
                .ExecProcedureSingle(Proc_CheckKPIUpLoad.GetEntityProc(HubCodes, COTCodes));
                if (res.Result == true)
                {
                    return JsonUtil.Success(res);
                } 
                else
                {
                    return JsonUtil.Error(res.Message);
                }
            }catch(Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }
        }
        [HttpPost("UpLoadExcelKPI")]
        public async Task<JsonResult> UpLoadExcelKPI([FromBody]List<UpLoadExcelKPIModel> viewModels)
        {
            
            try
            {
                UpLoadExcelKPIByProc sqlUserList = new UpLoadExcelKPIByProc();
                sqlUserList.AddRange(viewModels); 
                var res = _unitOfWork.Repository<Proc_UpLoadExcelKPI>()
                        .ExecProcedureSingle(Proc_UpLoadExcelKPI.GetEntityProc(sqlUserList));
                if (res.Result == true)
                {
                    return JsonUtil.Success();
                }
                else
                {
                    return JsonUtil.Error("Đã có lỗi xảy ra !!!");
                }
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }
        }
        [HttpPost("UpdatelKPIShipmentSAP")]
        public async Task<JsonResult> UpdatelKPIShipmentSAP([FromBody]UpLoadExcelKPIModel viewModel)
        {

            try
            {
                List<UpLoadExcelKPIModel> viewModels = new List<UpLoadExcelKPIModel>();
                viewModels.Add(viewModel);
                UpdateKPIModelByProc sqlUserList = new UpdateKPIModelByProc();
                sqlUserList.AddRange(viewModels);
                var res = _unitOfWork.Repository<Proc_UpdateKPIShipmentSAP>()
                        .ExecProcedureSingle(Proc_UpdateKPIShipmentSAP.GetEntityProc(sqlUserList));
                if (res.Result == true)
                {
                    return JsonUtil.Success();
                }
                else
                {
                    return JsonUtil.Error("Đã có lỗi xảy ra !!!");
                }
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }
        }

        [HttpGet("GetByWhere")]
        public JsonResult GetByWhere(string value, int? id)
        {
            var datas = _unitOfWork.Repository<Proc_SearchEntityByValue>()
                .ExecProcedure(Proc_SearchEntityByValue.GetEntityProc("Core_HubRouting", value, id));
            return JsonUtil.Success(datas);
        }

        [HttpGet("GetConnectCode")]
        public JsonResult GetConnectCode(string value)
        {
            var datas = _unitOfWork.Repository<Proc_GetConnectCode>()
                .ExecProcedure(Proc_GetConnectCode.GetEntityProc(value));
            return JsonUtil.Success(datas);
        }
    }
}