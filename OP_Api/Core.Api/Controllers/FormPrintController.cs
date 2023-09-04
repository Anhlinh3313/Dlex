using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Business.ViewModels;
using Core.Data.Abstract;
using Core.Entity.Entities;
using Core.Entity.Procedures;
using Core.Infrastructure.Helper;
using Core.Infrastructure.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormPrintController : GeneralController<FormPrintViewModel, FormPrint>
    {
        public FormPrintController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IOptions<JwtIssuerOptions> jwtOptions,
            IUnitOfWork unitOfWork,
            IGeneralService<FormPrintViewModel, FormPrint> iGeneralService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
        }

        [HttpGet("GetFormPrintType")]
        public JsonResult GetFormPrintType()
        {
            var datas = _unitOfWork.RepositoryR<FormPrintType>().GetAll();
            return JsonUtil.Success(datas);
        }

        [HttpGet("GetFormPrintByType")]
        public JsonResult GetFormPrintByType(int typeId)
        {
            return base.FindBy(f => f.FormPrintTypeId == typeId);
        }

        [HttpGet("GetFormPrintA5")]
        public JsonResult GetFormPrintA5(int customerId)
        {
            int formPrintId = 0;
            var data = _unitOfWork.RepositoryR<CustomerSettinng>().GetSingle(f => f.CustomerId == customerId);
            if (!Util.IsNull(data))
            {
                if (!Util.IsNull(data.FormPrintId)) formPrintId = data.FormPrintId.Value;
            }
            var dataFormPrint = _unitOfWork.RepositoryR<FormPrint>().FindBy(f => (f.IsPublic == true || f.Id == formPrintId) 
            && f.FormPrintTypeId==FormPrintTypeHelper.FormPrintBill).OrderByDescending(o => o.NumOrder).FirstOrDefault();
            if(Util.IsNull(dataFormPrint))return JsonUtil.Error("Không tim thấy mẫu in Bill.");
            else return JsonUtil.Success(dataFormPrint);
        }

        [HttpGet("GetFormPrintLabel")]
        public JsonResult GetFormPrintLabel()
        {
            var dataFormPrint = _unitOfWork.RepositoryR<FormPrint>().FindBy(f => f.FormPrintTypeId == FormPrintTypeHelper.FormPrintLabel)
                .OrderByDescending(o => o.NumOrder).FirstOrDefault();
            if (Util.IsNull(dataFormPrint)) return JsonUtil.Error("Không tim thấy mẫu in Label.");
            else return JsonUtil.Success(dataFormPrint);
        }

        [HttpGet("GetFormPrintBarCode")]
        public JsonResult GetFormPrintBarCode()
        {
            var dataFormPrint = _unitOfWork.RepositoryR<FormPrint>().FindBy(f => f.FormPrintTypeId == FormPrintTypeHelper.FormPrintBarCode)
                .OrderByDescending(o => o.NumOrder).FirstOrDefault();
            if (Util.IsNull(dataFormPrint)) return JsonUtil.Error("Không tim thấy mẫu in Barcode.");
            else return JsonUtil.Success(dataFormPrint);
        }

        [HttpGet("GetFormPrintPackage")]
        public JsonResult GetFormPrintPackage()
        {
            var dataFormPrint = _unitOfWork.RepositoryR<FormPrint>().FindBy(f => f.FormPrintTypeId == FormPrintTypeHelper.FormPrintPackage)
                .OrderByDescending(o => o.NumOrder).FirstOrDefault();
            if (Util.IsNull(dataFormPrint)) return JsonUtil.Error("Không tim thấy mẫu in Package.");
            else return JsonUtil.Success(dataFormPrint);
        }

        [HttpPost("GetListFormPrint")]
        public JsonResult GetListFormPrint([FromBody] GetListFormPrintViewModel ViewModel)
        {
            var data = _unitOfWork.Repository<Proc_GetListFormPrint>()
                .ExecProcedure(Proc_GetListFormPrint.GetEntityProc(ViewModel.FormPrintId, ViewModel.FormPrintTypeId,
                ViewModel.SearchText,ViewModel.PageNum, ViewModel.PageSize));
            if (!Util.IsNull(data))
            {
                return JsonUtil.Success(data);
            }
            else
            {
                return JsonUtil.Error("Get data error!!!");
            }
        }
    }
}