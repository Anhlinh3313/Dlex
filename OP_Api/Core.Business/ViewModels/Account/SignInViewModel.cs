using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Core.Data.Core.Utils;

namespace Core.Business.ViewModels
{
    public class SignInViewModel 
    {
        public SignInViewModel()
        {
            
        }
        public string CompanyCode { get; set; }
        public string UserName { get; set; }
		public string PassWord { get; set; }
        public int TypeUserId { get; set; } = 2;
    }
}
