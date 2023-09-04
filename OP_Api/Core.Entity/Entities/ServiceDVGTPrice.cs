using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class ServiceDVGTPrice : EntitySimple
    {
        public ServiceDVGTPrice() { }

        public int? ServiceDVGTId { get; set; }
        public int? FormulaId { get; set; }
        public double ValueFrom { get; set; }
        public double ValueTo { set; get; }
        public double Price { get; set; }
        public int? PriceListDVGTId { get; set; }
        public double? ValuePlus { get; set; }
        public int? CalculateById { get; set; }

        public Formula Formula { set; get; }
        public ServiceDVGT ServiceDVGT { get; set; }
        public PriceListDVGT PriceListDVGT { get; set; }
        public CalculateBy CalculateBy { get; set; }
    }
}
