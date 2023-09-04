using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class PriceListViewModel : SimpleViewModel
    {
        public PriceListViewModel() { }

        public int HubId { set; get; }
        public DateTime? PublicDateFrom { set; get; }
        public DateTime? PublicDateTo { set; get; }
        public double? VATSurcharge { set; get; }
        public double FuelSurcharge { set; get; }
        public double RemoteSurcharge { set; get; }
        public bool IsPublic { set; get; }
        public int NumOrder { get; set; }
        public int? PriceListDVGTId { get; set; }
        public bool? IsShared { get; set; }

}
}
