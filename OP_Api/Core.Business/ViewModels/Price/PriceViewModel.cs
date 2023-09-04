using Core.Entity.Procedures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class PriceViewModel
    {
        public bool IsAgreementPrice { get; set; }
        public double DefaultPrice { set; get; }
        public double TotalDVGT { get; set; }
        public double RemoteAreasPrice { get; set; }
        public double FuelPrice { get; set; }
        public double OtherPrice { get; set; }
        public double VATPrice { get; set; }
        public double TotalPrice { get; set; }
        public double PriceReturn { get; set; }
        public double PriceCOD { get; set; }
        public double TotalPriceSYS { get; set; }
        public DateTime PublicDateFrom { set; get; }
        public DateTime PublicDateTo { set; get; }
        public DateTime? DeadlineDelivery { get; set; }
        public double DeadlineDeliveryTime { get; set; }
        public int NumOrder { get; set; }
        //
        public double DefaultPriceP { get; set; }
        public double DefaultPriceS { get; set; }
        //
        public double VATPercent { get; set; }
        public double FuelPercent { get; set; }
        public double RemotePercent { get; set; }
        public double DIM { get; set; }
        public double Weight { get; set; }
        public double CalWeight { get; set; }
        public double LastWeight { get; set; }
        //
        public List<PriceDVGTViewModel> PriceDVGTs { get; set; }
        public Proc_PriceService PriceService { get; set; }
    }
}
