using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class Weight : EntitySimple
    {
        public Weight() { }

        public double WeightFrom { set; get; }
        public double WeightTo { set; get; }
        public double WeightPlus { set; get; }
        public int FormulaId { set; get; }
        public int? PricingTypeId { set; get; }

        public int? WeightGroupId { set; get; }
        public bool IsAuto { get; set; }
        public bool IsWeightCal { get; set; }
        public int? StructureId { get; set; }
        public double? ValueFrom { get; set; }
        public double? ValueTo { get; set; }
        //public int? PricingTypeId { get; set; }

        public Formula Formula { set; get; }
        public WeightGroup WeightGroup { set; get; }
        public Structure Structure { get; set; }
    }
}
