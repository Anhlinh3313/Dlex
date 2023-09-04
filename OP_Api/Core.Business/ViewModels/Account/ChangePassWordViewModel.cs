using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Core.Data.Core.Utils;

namespace Core.Business.ViewModels
{
    public class ChangePassWordViewModel 
    {
        public ChangePassWordViewModel()
        {
        }

        public int UserId { get; set; }
		public string CurrentPassWord { get; set; }
		public string NewPassWord { get; set; }
		public string NameClient { get; set; }

    }
}
