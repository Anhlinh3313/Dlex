using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Business.Core.Utils;
using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Business.ViewModels;
using Core.Data.Abstract;
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
    public class EmailCenterController : BaseController
    {
        private readonly IGeneralService _iGeneralService;
        private readonly CompanyInformation _icompanyInformation;
        public EmailCenterController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IOptions<JwtIssuerOptions> jwtOptions,
            IGeneralService iGeneralService,
            IOptions<CompanyInformation> companyInformation,
            IUnitOfWork unitOfWork) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
            _iGeneralService = iGeneralService;
            _icompanyInformation = companyInformation.Value;
        }

        [HttpPost("SendEmailShipment")]
        public JsonResult SendEmailShipment([FromBody] EmailCenterViewModel viewModel)
        {
            var userId = GetCurrentUserId();
            var infoFrom = viewModel.NameFrom;
            if (Util.IsNull(infoFrom)) infoFrom = _icompanyInformation.Name.ToUpper();//TÊN CTY GỬI ĐI
            if (viewModel.InfoShipments.Count() == 0) return JsonUtil.Error("Vui lòng chỉ định vận đơn cầ gửi.");
            var listShipmentId = string.Join(",", viewModel.InfoShipments.Select(s => s.ShipmentId).ToList());
            var results = _unitOfWork.Repository<Proc_GetEmailAddress>()
                .ExecProcedure(Proc_GetEmailAddress.GetEntityProc(listShipmentId)).ToList();
            if (results.Count() == 0) return JsonUtil.Error("Không tìm thấy thông tin vận đơn.");
            while (results.Select(s => s.IsSend == false).Count() > 0)
            {
                var infoShipment = results.Where(f => f.IsSend == false).FirstOrDefault();
                if (Util.IsNull(infoShipment)) break;
                var shipmentNumber = infoShipment.ShipmentNumber;
                var nameTo = infoShipment.ObjectName;
                if (Util.IsNull(nameTo)) nameTo = "QUÝ KHÁCH";
                var infoShipments = results.Where(f => f.IsSend == false && f.EmailAddress == infoShipment.EmailAddress).ToList();
                if (infoShipments.Count() > 1)
                {
                    shipmentNumber = string.Join(", ", infoShipments.Select(s => s.ShipmentNumber).ToList());
                }
                var body = string.Format(viewModel.Body, nameTo, shipmentNumber);
                var resSendMail = MailUtil.SendEmail(infoShipment.EmailAddress, viewModel.Title, body, infoFrom);
                foreach (var item in infoShipments)
                {
                    var findShip = viewModel.InfoShipments.Where(s => s.ShipmentId == item.ShipmentId).FirstOrDefault();
                    _unitOfWork.Repository<Proc_LogSendEmail>()
                .ExecProcedureSingle(Proc_LogSendEmail.GetEntityProc(userId, item.EmailAddress, findShip.ShipmentId,
                findShip.LadingScheduleId, findShip.ComplainId, findShip.IncidentsId, findShip.IsDeliverd, findShip.IsReturn,
                resSendMail == "TRUE", resSendMail));
                    item.IsSend = true;
                }
            }
            return JsonUtil.Success(string.Format("Gửi email thành công ({0} vận đơn)", results.Count()));
        }
    }
}