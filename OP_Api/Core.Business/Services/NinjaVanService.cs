using Core.Business.Services.Abstract;
using Core.Business.Services.Models;
using Core.Business.ViewModels.NinjaVanConnections;
using Core.Data.Abstract;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Core.Business.Services
{
    public class NinjaVanService : BaseService, INinjaVanService
    {
        public NinjaVanService(Microsoft.Extensions.Logging.ILogger<dynamic> logger,
            IOptions<AppSettings> optionsAccessor,
            IUnitOfWork unitOfWork
            )
            : base(logger, optionsAccessor, unitOfWork)
        {

        }
       
    }
}
