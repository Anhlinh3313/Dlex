using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class Promotion: EntitySimple
    {
        public Promotion() { }
        public bool PaymentNow { get; set; }
        public int? PromotionTypeId { set; get; }
        public string PromotionNot { set; get; }
        public double? TotalPromotion { set; get; }
        public double? TotalCode { set; get; }
        public DateTime? FromDate { set; get; }
        public DateTime? ToDate { set; get; }
        public bool? IsPublic { get; set; }
        public bool? IsHidden { get; set; }
    }
}
