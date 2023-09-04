using Core.Entity.Abstract;
using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels.PriceListSettings
{
    public class PriceListSettingViewModel : SimpleViewModel<PriceListSettingViewModel, PriceListSetting>
    {
        public PriceListSettingViewModel() { }
        public int? CustomerId { get; set; }
        public int? ServiceId { get; set; }
        public int? PriceListId { get; set; }
        public double? VATSurcharge { get; set; }
        public double? FuelSurcharge { get; set; }
        public double? VSVXSurcharge { get; set; }
        public double? DIM { get; set; }
    }
}
