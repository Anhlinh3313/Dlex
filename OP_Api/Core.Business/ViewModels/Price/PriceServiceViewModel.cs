using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class PriceServiceViewModel : SimpleViewModel<PriceServiceViewModel, PriceService>
    {
        public PriceServiceViewModel() { }

        public bool IsKeepWeight { get; set; }
        public int ServiceId { set; get; }
        public int? WeightGroupId { get; set; }
        public int? AreaGroupId { get; set; }
        public int? PriceListId { get; set; }
        public bool IsAuto { get; set; }
        public double VATPercent { get; set; }
        public double FuelPercent { get; set; }
        public double DIM { get; set; }
        public double RemoteAreasPricePercent { get; set; }
        public bool IsTwoWay { get; set; }
        public DateTime? PublicDateFrom { set; get; }
        public DateTime? PublicDateTo { set; get; }
        public bool? IsPublic { get; set; }
        public int? StructureId { get; set; }
        public int? PricingTypeId { get; set; }
        public int NumOrder { get; set; }
    }
}
