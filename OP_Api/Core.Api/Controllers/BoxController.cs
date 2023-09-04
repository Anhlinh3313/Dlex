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
    public class BoxController : GeneralController<BoxViewModel, BoxInfoViewModel, Box>
    {
        public BoxController(Microsoft.Extensions.Logging.ILogger<dynamic> logger, IOptions<AppSettings> optionsAccessor, IOptions<JwtIssuerOptions> jwtOptions, IUnitOfWork unitOfWork, IGeneralService<BoxViewModel, BoxInfoViewModel, Box> iGeneralService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
        }

        public override async Task<JsonResult> Create([FromBody]BoxViewModel viewModel)
        {
            var shipment = _unitOfWork.RepositoryR<Shipment>().GetSingle(viewModel.ShipmentId);
            shipment.TotalBox += 1;

            viewModel.Code = $"BOX{shipment.ShipmentNumber}{RandomUtil.GetCode(shipment.TotalBox, 2)}";
            viewModel.Name = viewModel.Code;

            _unitOfWork.RepositoryCRUD<Shipment>().Update(shipment);
            await _unitOfWork.CommitAsync();

            return JsonUtil.Create(await _iGeneralService.Create(viewModel));
        }

        [HttpGet("GetByShipmentId")]
        public JsonResult GetByShipmentId(int id)
        {
            return JsonUtil.Create(_iGeneralService.FindBy(x => x.ShipmentId == id));
        }

        [HttpPost("GetListByShipmentIds")]
        public JsonResult GetListByShipmentIds([FromBody]GetByIdsViewModel viewModel)
        {
            var ids = viewModel.Ids;
            if (ids != null && ids.Length > 0)
            {
                List<GroupBoxViewModel> groupBox = new List<GroupBoxViewModel>();
                for (int i = 0; i < ids.Length; i++)
                {
                    Console.WriteLine(i);
                    var boxes = _iGeneralService.FindBy(box => box.ShipmentId == ids[i]);
                    var arrBoxes = boxes.Data;
                    groupBox.Add(new GroupBoxViewModel(
                        ids[i],
                        arrBoxes
                    ));
                }
                return JsonUtil.Success(groupBox);
            }
            return JsonUtil.Success();
        }
    }
}
