using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class PriceListDVGT : EntitySimple
    {
        public PriceListDVGT() { }
        public int? ServiceId { get; set; }
        public bool IsPublic { get; set; }
        public int NumOrder { get; set; }
        public double VATPercent { get; set; }
        public Service Service { get; set; }
    }
}
