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
using Core.Business.ViewModels.SetUpKPIDeliverys;
using Core.Infrastructure.Utils;
using Core.Business.ViewModels.KPIShipmentDetails;
using System.Data;
using Core.Api.Library;
using MoreLinq;
using Core.Entity.Procedures;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class KPIShipmentDetailController : GeneralController<KPIShipmentDetailViewModel, KPIShipmentDetail>
    {
        private readonly IShipmentService _iShipmentService;
        private readonly IKPIShipmentDetailService _iKPIShipmentDetailService;
        public KPIShipmentDetailController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IOptions<JwtIssuerOptions> jwtOptions,
            IUnitOfWork unitOfWork,
            IShipmentService iShipmentService,
            IKPIShipmentDetailService iKPIShipmentDetailService,
            IGeneralService<KPIShipmentDetailViewModel, KPIShipmentDetail> iGeneralService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
            _iShipmentService = iShipmentService;
            _iKPIShipmentDetailService = iKPIShipmentDetailService;
        }

        [HttpGet("CalucalteTimeTargetDelivery")]
        public JsonResult CalculateTimeForShipment(int? districtId, int? wardId, int? cusId, int serviceId, DateTime dateTimeStart)
        {
            var res = _iKPIShipmentDetailService.CalculateTimeForShipment(cusId, districtId, wardId, serviceId, dateTimeStart);
            return JsonUtil.Success(res);
        }

        [HttpPost("UpLoadKPIShipmentDetail")]
        public JsonResult UpLoadKPIShipmentDetail([FromBody]List<KPIShipmentDetail> request)
        {
            try
            {
                return JsonUtil.Success(_iKPIShipmentDetailService.InsertKPIShipmentDetail(request));
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }
        }


        [HttpPost("UpdateKPIShipmentDetail")]
        public JsonResult UpdateKPIShipmentDetail([FromBody]List<KPIShipmentDetail> request)
        {
            try
            {
                var result = _iKPIShipmentDetailService.UpdateKPIShipmentDetail(request);
                return JsonUtil.Success(result);
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }
        }

        [HttpGet("GetKPIShipmentDetail")]
        public JsonResult GetKPIShipmentDetail(int kPIshipemntId, int? pageNumber, int? pageSize)
        {
            if (kPIshipemntId == 0)
            {
                return JsonUtil.Success();
            }
            try
            {
                return JsonUtil.Success(_iKPIShipmentDetailService.GetKPIShipmentDetail(kPIshipemntId, pageSize, pageNumber));
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }
        }

        [HttpPost("GetKPIShipmentDetailExport")]
        public dynamic GetKPIShipmentDetailExport([FromBody]GetKPIDetailExportViewModel viewModel)
        {
            if (viewModel.KPIShipmentId == 0)
            {
                return JsonUtil.Success();
            }
            try
            {
                var result = _iKPIShipmentDetailService.GetKPIShipmentDetailExport(viewModel.KPIShipmentId, viewModel.PageSize, viewModel.PageNumber, viewModel.CustomExportFile);
                if (result.Count == 0)
                {
                    return null;
                }
                DataTable dtt = new DataTable();
                dtt = result.ToList().ToDataTable();
                var bytearray = ExportExcelPartern.ExportExcel(viewModel.CustomExportFile, dtt);
                return File(bytearray, "application/xlsx", viewModel.CustomExportFile.FileNameReport + ".xlsx");
            }
            catch (Exception ex)
            {
                return JsonUtil.Error(ex.Message);
            }
        }
    }
}
