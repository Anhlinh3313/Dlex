using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Data.Abstract;
using Core.Infrastructure.Utils;
using Core.Infrastructure.ViewModels;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.Services
{
    public class ProviderCodeService : BaseService, IProviderCodeService
    {
        public ProviderCodeService
            (
            Microsoft.Extensions.Logging.ILogger<dynamic> logger, 
            IOptions<AppSettings> optionsAccessor, 
            IUnitOfWork unitOfWork
            ) : base(logger, optionsAccessor, unitOfWork)
        {
        }

        ResponseViewModel IProviderCodeService.ValidatorListCode(List<string> listCode)
        {
            //var checkInProvider = _unitOfWork.RepositoryR<ProviderCode>
            return ResponseViewModel.CreateSuccess();
        }
    }
}
