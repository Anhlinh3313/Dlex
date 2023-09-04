using Core.Business.ViewModels.General;
using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class PriceListInfoViewModel : SimpleViewModel
    {
        public PriceListInfoViewModel() { }

        public int HubId { set; get; }
        public HubViewModel Hub { set; get; }
        public DateTime? PublicDateFrom { set; get; }
        public DateTime? PublicDateTo { set; get; }
        public double? VATSurcharge { set; get; }
        public double FuelSurcharge { set; get; }
        public double RemoteSurcharge { set; get; }
        public int NumOrder { get; set; }
        public bool IsPublic { set; get; }
        public int? PriceListDVGTId { get; set; }
        public bool? IsShared { get; set; }

    public PriceListDVGTViewModel PriceListDVGT { get; set; }
    }
}
