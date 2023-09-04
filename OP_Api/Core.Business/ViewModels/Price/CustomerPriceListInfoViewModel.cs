using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class CustomerPriceListInfoViewModel : BasicViewModel
    {
        public CustomerPriceListInfoViewModel() { }

        public int CustomerId { set; get; }
        public int PriceListId { set; get; }
        public PriceListInfoViewModel PriceList { set; get; }
    }
}
