using Core.Data.Core.Utils;
using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Core.Business.ViewModels
{
    public class TPLViewModel : SimpleViewModel<TPLViewModel,TPL>
    {
        public TPLViewModel() { }

        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsInternational { get; set; }
        public int? PriceListId { get; set; }
    }
}
