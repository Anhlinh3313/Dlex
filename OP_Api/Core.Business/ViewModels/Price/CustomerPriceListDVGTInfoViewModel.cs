using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class CustomerPriceListDVGTInfoViewModel : SimpleViewModel
    {
        public CustomerPriceListDVGTInfoViewModel() { }
        public int? CustomerId { get; set; }
        public int? PriceListDVGTId { get; set; }
        public Customer Customer { get; set; }
    }
}
