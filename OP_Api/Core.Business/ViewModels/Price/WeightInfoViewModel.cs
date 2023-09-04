using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class WeightInfoViewModel : SimpleViewModel
    {
        public WeightInfoViewModel() { }

        public double? WeightFrom { set; get; }
        public double? WeightTo { set; get; }
        public double? WeightPlus { set; get; }
        public int FormulaId { set; get; }
        public int? PricingTypeId { set; get; }

        public int? WeightGroupId { set; get; }
        public int? GoodsGroupId { get; set; }
        public int? StructureId { get; set; }
        public bool IsAuto { get; set; }
        public bool IsWeightCal { get; set; }
        public double? ValueFrom { get; set; }
        public double? ValueTo { get; set; }
        //public int? PricingTypeId { get; set; }

        public FormulaViewModel Formula { set; get; }
        public WeightGroupViewModel WeightGroup { set; get; }
        public StructureViewModel Structure { get; set; }
    }
}
