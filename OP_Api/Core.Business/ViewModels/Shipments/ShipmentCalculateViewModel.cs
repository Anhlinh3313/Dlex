using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class ShipmentCalculateViewModel
    {
        public ShipmentCalculateViewModel() { }
        public bool IsReturn { get; set; }
        public double? PriceReturn { get; set; }
        public double DisCount { get; set; }
        public bool IsAgreementPrice { get; set; }
        public DateTime? OrderDate { get; set; }
        public double DefaultPrice { set; get; }
        public int? PriceListId { set; get; }
        public int? PriceServiceId { set; get; }
        public int SenderId { get; set; }
        public int? FromWardId { get; set; }
        public int? FromDistrictId { get; set; }
        public int? FromHubId { get; set; }
        public int? FromProvinceId { get; set; }
        public int ToDistrictId { get; set; }
        public int? ToWardId { get; set; }
        public double Weight { get; set; }
        public double Distance { get; set; }
        public double CalWeight { get; set; }
        public int ServiceId { get; set; }
        public int PaymentTypeId { get; set; }
        public double COD { get; set; }
        public double Insured { get; set; }
        public double OtherPrice { get; set; }
        public List<int> ServiceDVGTIds { get; set; }
        public List<PriceDVGTViewModel> priceDVGTs { get; set; }
        public int? StructureId { get; set; }
        public int PricingTypeId { get; set; }
        public int TotalItem { get; set; }
        public int TotalBox { get; set; }
        //
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        //
        public List<BoxPriceViewModel> PriceBox { get; set; }
    }
}
