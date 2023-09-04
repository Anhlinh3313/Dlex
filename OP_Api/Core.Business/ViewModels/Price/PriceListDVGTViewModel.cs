using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class PriceListDVGTViewModel : SimpleViewModel
    {
        public PriceListDVGTViewModel() { }
        
        public int? ServiceId { get; set; }
        public bool IsPublic { get; set; }
        public int NumOrder { get; set; }
        public double VATPercent { get; set; }
    }
}
