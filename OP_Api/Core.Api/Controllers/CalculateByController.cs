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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class CalculateByController : GeneralController<CalculateByViewModel, CalculateBy>
    {
        public CalculateByController
            (
            Microsoft.Extensions.Logging.ILogger<dynamic> logger, 
            IOptions<AppSettings> optionsAccessor, 
            IOptions<JwtIssuerOptions> jwtOptions, 
            IUnitOfWork unitOfWork, 
            IGeneralService<CalculateByViewModel, CalculateBy> iGeneralService
            ) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
        }
    }
}
