using Core.Business.ViewModels.PromotionDetails;
using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels.Promotions
{
    public class PromotionViewModel : SimpleViewModel<PromotionViewModel, Promotion>
    {
        public PromotionViewModel() { }
        public bool? PaymentNow { get; set; }
        public int? PromotionTypeId { set; get; }
        public string PromotionNot { set; get; }
        public double? TotalPromotion { set; get; }
        public double? TotalCode { set; get; }
        public DateTime? FromDate { set; get; }
        public DateTime? ToDate { set; get; }
        public bool? IsPublic { get; set; }
        public bool? IsHidden { get; set; }
        public List<PromotionDetailInfoViewModel> PromotionDetails { get; set; }
    }
}
