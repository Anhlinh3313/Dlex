using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Business.ViewModels;
using Core.Business.ViewModels.ChangeCODS;
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
    public class ChangeCODController : GeneralController<ChangeCODViewModel, ChangeCOD>
    {
        private readonly CompanyInformation _icompanyInformation;
        private readonly IShipmentService _iShipmentService;

        public ChangeCODController(Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IOptions<JwtIssuerOptions> jwtOptions, IUnitOfWork unitOfWork,
            IGeneralService<ChangeCODViewModel, ChangeCOD> iGeneralService,
            IShipmentService iShipmentService,
            IOptions<CompanyInformation> companyInformation) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
            _icompanyInformation = companyInformation.Value;
            _iShipmentService = iShipmentService;
        }

        [HttpPost("GetListChangeCOD")]
        public JsonResult GetListChangeCOD([FromBody] ChangeCODFilterViewModel model)
        {
            var res = _unitOfWork.Repository<Proc_GetListChangeCOD>()
               .ExecProcedure(Proc_GetListChangeCOD.GetEntityProc(model.UserId,model.IsAccept, model.DateFrom, model.DateTo, model.PageNumber, model.PageSize)).ToList();

            return JsonUtil.Success(res);
        }

        [HttpPost("Accept")]
        public JsonResult Accept([FromBody] ChangeCOD model)
        {
            if (model.ShipmentId == null)
            {
                return JsonUtil.Error("ShipmentId trống");
            }
            else if (model.IsAccept == true)
            {
                return JsonUtil.Error("Vận đơn đã được xác nhận");
            }
            else
            {
                Shipment shipment = _unitOfWork.RepositoryR<Shipment>().GetSingle(model.ShipmentId);

                Shipment VDCT = new Shipment();
                
                if (model.ChangeCODTypeId == 2)
                {

                    if (model.NewCOD >= shipment.COD)
                    {
                        return JsonUtil.Error("COD thay đổi phải nhỏ hơn COD vận đơn");
                    }
                    else
                    {
                        User user = GetCurrentUser();

                        var resutlt = _unitOfWork.Repository<Proc_GetShipmentNumberAuto>().ExecProcedureSingle(Proc_GetShipmentNumberAuto.GetEntityProc(shipment.Id, shipment.FromProvinceId));
                        var shipmentNumberBasic = _iShipmentService.GetCodeByType(_icompanyInformation.TypeShipmentCode, _icompanyInformation.PrefixShipmentCode, shipment.Id, resutlt.CountNumber, shipment.FromProvinceId);

                        VDCT.ShipmentId = shipment.Id;
                        VDCT.ShipmentNumber = shipmentNumberBasic;
                        VDCT.TypeId = 2;
                        VDCT.COD = 0;
                        VDCT.Insured = shipment.COD - model.NewCOD;
                        VDCT.SenderId = shipment.SenderId;
                        VDCT.SenderName = _icompanyInformation.Name.ToUpper();
                        VDCT.SenderPhone = user.Hub.PhoneNumber;
                        VDCT.PickingAddress = user.Hub.Address;
                        VDCT.FromProvinceId = user.Hub.District.ProvinceId;
                        VDCT.FromDistrictId = user.Hub.DistrictId;
                        VDCT.FromWardId = user.Hub.WardId;
                        VDCT.ReceiverId = shipment.ReceiverId;
                        VDCT.ReceiverName = shipment.ReceiverName;
                        VDCT.ReceiverPhone = shipment.ReceiverPhone;
                        VDCT.ShippingAddress = shipment.ShippingAddress;
                        VDCT.ToProvinceId = shipment.ToProvinceId;
                        VDCT.ToDistrictId = shipment.ToDistrictId;
                        VDCT.ToWardId = shipment.ToWardId;
                        VDCT.OrderDate = DateTime.Now;
                        VDCT.ShipmentStatusId = 15;
                        VDCT.FromHubId = user.HubId;

                        _unitOfWork.RepositoryCRUD<Shipment>().Insert(VDCT);
                        _unitOfWork.Commit();
                    }

                    shipment.COD = model.NewCOD;
                    _unitOfWork.RepositoryCRUD<Shipment>().Update(shipment);

                    Shipment ship = _unitOfWork.RepositoryR<Shipment>().GetAll().OrderByDescending(x => x.Id).FirstOrDefault();

                    model.ShipmentSupportId = ship.Id;
                }
                    model.IsAccept = true;
                    _unitOfWork.RepositoryCRUD<ChangeCOD>().Update(model);
                    _unitOfWork.Commit();

                return JsonUtil.Success(VDCT);
            }
        }

        [HttpGet("Reject")]
        public JsonResult Reject(int id)
        {
            ChangeCOD changeCOD = _unitOfWork.RepositoryR<ChangeCOD>().GetSingle(id);

            if (changeCOD == null)
            {
                return JsonUtil.Error("Không tìm thấy dữ liệu");
            }
            else
            {
                _unitOfWork.RepositoryCRUD<ChangeCOD>().DeleteEmpty(changeCOD.Id);
                _unitOfWork.Commit();

                return JsonUtil.Success();
            }
        }
    }
}