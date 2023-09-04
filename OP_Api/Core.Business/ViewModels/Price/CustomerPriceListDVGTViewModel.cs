using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class CustomerPriceListDVGTViewModel : SimpleViewModel
    {
        public int? CustomerId { get; set; }
        public int? PriceListDVGTId { get; set; }
    }
}
