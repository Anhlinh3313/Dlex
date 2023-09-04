using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class PriceDVGTViewModel
    {
        public PriceDVGTViewModel() { }

        public string Code { get; set; }
        public string Name { get; set; }
        public string VSEOracleCode { get; set; }
        public int? ServiceId { get; set; }
        public double TotalPrice { get; set; }
        public bool IsAgree { get; set; }
        public string PriceServiceCode { get; set; }
    }
}
