using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class CustomerPriceService : EntitySimple
    {
        public int CustomerId { get; set; }
        public int PriceServiceId { get; set; }
        public double? VATPercent { get; set; }
        public double? FuelPercent { get; set; }
        public double? DIM { get; set; }
        public double? RemoteAreasPricePercent { get; set; }
        public Customer Customer { get; set; }
    }
}
