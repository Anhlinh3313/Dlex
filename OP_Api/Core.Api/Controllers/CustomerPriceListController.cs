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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class CustomerPriceListController : BaseController
    {
        private readonly IGeneralService _iGeneralService;

        public CustomerPriceListController(Microsoft.Extensions.Logging.ILogger<dynamic> logger, IOptions<AppSettings> optionsAccessor, IOptions<JwtIssuerOptions> jwtOptions, IUnitOfWork unitOfWork, IGeneralService iGeneralService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
            _iGeneralService = iGeneralService;
        }

        [HttpPost("Create")]
        public virtual async Task<JsonResult> Create([FromBody] CustomerPriceListViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return JsonUtil.Error(ModelState);
            }
            _unitOfWork.RepositoryCRUD<CustomerPriceList>().DeleteEmptyWhere(x => x.CustomerId == viewModel.CustomerId && x.PriceListId == viewModel.PriceListId);
            return JsonUtil.Create(await _iGeneralService.Create<CustomerPriceList, CustomerPriceListInfoViewModel, CustomerPriceListViewModel>(viewModel));
        }
        [HttpPost("Update")]
        public virtual async Task<JsonResult> Update([FromBody] CustomerPriceListViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return JsonUtil.Error(ModelState);
            }
            _unitOfWork.RepositoryCRUD<CustomerPriceList>().DeleteEmptyWhere(x => (x.CustomerId == viewModel.CustomerId && x.PriceListId == viewModel.PriceListId) || x.Id == viewModel.Id);
            return JsonUtil.Create(await _iGeneralService.Create<CustomerPriceList, CustomerPriceListInfoViewModel, CustomerPriceListViewModel>(viewModel));
        }
        [HttpGet("GetPriceListByCustomerId")]
        public virtual JsonResult GetPriceListByCustomerId(int customerId, string cols)
        {
            var arr = new string[0];

            if (!string.IsNullOrEmpty(cols))
            {
                arr = cols.Split(',');
            }
            var result = _iGeneralService.FindBy<CustomerPriceList, CustomerPriceListInfoViewModel>(x => x.CustomerId == customerId, includeProperties: arr);
            return JsonUtil.Success(result);
        }
    }
}
