using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Business.ViewModels;
using Core.Business.ViewModels.Companies;
using Core.Data.Abstract;
using Core.Entity.Entities;
using Core.Entity.Procedures;
using Core.Infrastructure.Helper;
using Core.Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class CustomerInfoLogController : GeneralController<CustomerInfoLogViewModel, CustomerInfoLog>
    {
        public CustomerInfoLogController(
           Microsoft.Extensions.Logging.ILogger<dynamic> logger,
           IOptions<AppSettings> optionsAccessor,
           IOptions<JwtIssuerOptions> jwtOptions,
           IUnitOfWork unitOfWork,
           IGeneralService<CustomerInfoLogViewModel, CustomerInfoLog> iGeneralService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
        }

        [HttpGet("SearchByPhoneNumber")]
        public JsonResult SearchByPhoneNumber(string text, int? senderId = null, string cols = null)
        {
            var companyId = GetCurrentCompanyId();
            if (Util.IsNull(senderId)) senderId = this.GetCurrentUserId();
            return JsonUtil.Create(_iGeneralService.FindBy(x => x.SenderId == senderId && x.PhoneNumber.Contains(text) && x.CompanyId == companyId, 10, 1, cols));
        }

        [HttpPost("GetListCustomerInfoLog")]
        public JsonResult GetListCustomerInfoLog([FromBody]FilterViewModel viewModel)
        {
            var companyId = GetCurrentCompanyId();
            var data = _unitOfWork.Repository<Proc_GetListCustomerInfoLog>().ExecProcedure(
                Proc_GetListCustomerInfoLog.GetEntityProc(viewModel.SenderId, viewModel.ProvinceId, viewModel.PageNumber, viewModel.PageSize, viewModel.SearchText, companyId));
            return JsonUtil.Success(data);
        }
        [HttpPost("CreateOrUpdateImportExcel")]
        public JsonResult CreateOrUpdateImportExcel([FromBody]List<CustomerInfoLogViewModel> viewModels)
        {
            var companyId = GetCurrentCompanyId();
            if (Util.IsNull(viewModels) || viewModels.Count() == 0)
            {
                return JsonUtil.Error("Thông tin lỗi, không thể update", viewModels);
            }
            List<CustomerInfoLogViewModel> listError = new List<CustomerInfoLogViewModel>();
            foreach (var viewModel in viewModels)
            {
                var result = _unitOfWork.Repository<Proc_CreateOrUpdateCustomerInfoLog>().ExecProcedureSingle(
                    Proc_CreateOrUpdateCustomerInfoLog.GetEntityProc(viewModel.Code, viewModel.Name, viewModel.PhoneNumber,
                    viewModel.CompanyName, viewModel.Address, viewModel.AddressNote, viewModel.ProvinceId,
                    viewModel.DistrictId, viewModel.WardId, viewModel.Lat, viewModel.Lng, viewModel.SenderId, companyId));
                if (result.IsSuccess == false)
                {
                    viewModel.Note = result.Message;
                    listError.Add(viewModel);
                }
            }
            if (listError.Count() == 0) return JsonUtil.Success(null, "Upload dữ liệu thành công.");
            else return JsonUtil.Error(string.Format("Upload dữ liệu {0}/{1} dòng.", listError.Count(), viewModels.Count()), listError);
        }
    }
}
