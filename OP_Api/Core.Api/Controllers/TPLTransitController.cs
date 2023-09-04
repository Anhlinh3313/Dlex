﻿using System;
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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class TPLTransitController : GeneralController<TPLTransitViewModel, TPLTransitInfoViewModel, TPLTransit>
    {
        public IGeneralService _igeneralRawService { get; }
        public TPLTransitController(
            Microsoft.Extensions.Logging.ILogger<dynamic> logger, 
            IOptions<AppSettings> optionsAccessor, 
            IOptions<JwtIssuerOptions> jwtOptions, 
            IUnitOfWork unitOfWork,
            IGeneralService<TPLTransitViewModel, TPLTransitInfoViewModel, TPLTransit> iGeneralService,
            IGeneralService igeneralRawService) : base(logger, optionsAccessor, jwtOptions, unitOfWork, iGeneralService)
        {
            _igeneralRawService = igeneralRawService;
        }
    }
}
