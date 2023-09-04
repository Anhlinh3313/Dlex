using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class Discount : EntitySimple
    {
        public Discount() { }
        public int DiscountTypeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public double ValueFrom { get; set; }
        public double ValueTo { get; set; }
        public bool IsPublic { get; set; }
        public double DiscountPercent { get; set; }
    }
}
