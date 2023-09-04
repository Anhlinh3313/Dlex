using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels.PromotionDetails
{
    public class PromotionDetailInfo
    {
        public PromotionDetailInfo()
        {
        }
        public int? PromotionId { get; set; }
        public int? PromotionFormulaId { get; set; }
        public double? ValueFrom { get; set; }
        public double? ValueTo { get; set; }
        public double? Value { get; set; }
        public string ConcurrencyStamp { get; set; }
        public List<PromotionDetailServiceDVGT> PromotionDetailServiceDVGTs { get; set; }
    }
}
