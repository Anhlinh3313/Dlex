using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Business.ViewModels;
using Core.Business.ViewModels.Shipments;
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
    public class ShipmentVersionController : GeneralController<CreateUpdateShipmentViewModel, ShipmentVersionInfoViewModel, ShipmentVersion>
    {

        private readonly IGeneralService _iGeneralServiceRaw;
        public ShipmentVersionController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor, IOptions<JwtIssuerOptions> jwtOptions,
            IUnitOfWork unitOfWork,
            IGeneralService<CreateUpdateShipmentViewModel, ShipmentVersionInfoViewModel, ShipmentVersion> iGeneralService,
            IGeneralService iGeneralServiceRaw
            ) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
            //_iShipmentService = iShipmentService;
            _iGeneralServiceRaw = iGeneralServiceRaw;
        }

        public override async Task<JsonResult> Create([FromBody]CreateUpdateShipmentViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return JsonUtil.Error(ModelState);
            }
            //var shipmentVersion = Mapper.Map<ShipmentVersion>(viewModel);
            //_unitOfWork.RepositoryCRUD<ShipmentVersion>().Insert(shipmentVersion);
            //await _unitOfWork.CommitAsync();
            var data = await _iGeneralServiceRaw.Create<ShipmentVersion, CreateUpdateShipmentViewModel>(viewModel);
            if (viewModel.ServiceDVGTIds != null && viewModel.ServiceDVGTIds.Count() > 0)
            {
                foreach (var sDVGTId in viewModel.ServiceDVGTIds)
                {
                    var ssDVGT = new ShipmentServiceDVGTVersion();
                    ssDVGT.ShipmentId = viewModel.Id;
                    ssDVGT.ServiceDVGTId = sDVGTId;
                    _unitOfWork.RepositoryCRUD<ShipmentServiceDVGTVersion>().Insert(ssDVGT);
                }
            }

            return JsonUtil.Create(data);
        }

        [HttpGet("GetByShipmentId")]
        public JsonResult GetByShipmentId(int id, string cols = null)
        {
            return JsonUtil.Create(_iGeneralService.FindBy(x => x.ShipmentId == id, cols: cols));
        }

        [HttpPost("GetByListShipmentId")]
        public JsonResult GetByListShipmentId([FromBody]GetByIdsViewModel viewModel)
        {
            return JsonUtil.Create(_iGeneralService.FindBy(x => viewModel.Ids.Contains(x.ShipmentId), cols: viewModel.Cols));
        }
    }
}
