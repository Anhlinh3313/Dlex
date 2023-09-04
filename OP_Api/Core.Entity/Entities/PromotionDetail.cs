using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class PromotionDetail : EntityBasic
    {
        public PromotionDetail() 
        { 
        }
        public int? PromotionId { set; get; }
        public int? PromotionFormulaId { set; get; }
        public double? ValueFrom { set; get; }
        public double? ValueTo { set; get; }
        public double? Value { set; get; }
    }
}
