using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class PriceListSetting : EntitySimple
    {
        public PriceListSetting() { }
        public int? CustomerId { get; set; }
        public int? ServiceId { get; set; }
        public int? PriceListId { get; set; }
        public double? VATSurcharge { get; set; }
        public double? FuelSurcharge { get; set; }
        public double? VSVXSurcharge { get; set; }
        public double? DIM { get; set; }
}
}
