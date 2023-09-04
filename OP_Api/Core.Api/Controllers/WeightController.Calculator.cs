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
	public partial class WeightController
    {      
		[HttpGet("CalculateWeight")]
		public JsonResult CalculateWeight(double length, double width, double height, int? number_l_w_h_multip = null, int? number_l_w_h_dim = null)
        {
			number_l_w_h_multip = number_l_w_h_multip ?? 1;
			number_l_w_h_dim = number_l_w_h_dim ?? 6000;
			return JsonUtil.Success((length * width * height * number_l_w_h_multip) / number_l_w_h_dim);
        }
    }
}
