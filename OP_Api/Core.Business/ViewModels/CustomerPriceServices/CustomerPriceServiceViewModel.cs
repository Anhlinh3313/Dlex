using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class CustomerPriceServiceViewModel: SimpleViewModel<CustomerPriceServiceViewModel, CustomerPriceService>
    {
        public CustomerPriceServiceViewModel() { }

        public int CustomerId { get; set; }
        public int PriceServiceId { get; set; }
        public double? VATPercent { get; set; }
        public double? FuelPercent { get; set; }
        public double? DIM { get; set; }
        public double? RemoteAreasPricePercent { get; set; }
    }
}
