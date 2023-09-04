using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels.Shipments
{
    public class AssignShipmentWarehousingViewModel
    {
        public AssignShipmentWarehousingViewModel() { }

        public string ShipmentNumber { get; set; }
        public string cols { get; set; }
        public string Note { get; set; }
        public int[] ServiceDVGTIds { get; set; }
        public int? StructureId { get; set; }
        public int? ReasonId { get; set; }
        public int TotalBox { get; set; }
        public double Weight { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double CalWeight { get; set; }
        public bool IsCheck { get; set; }
        public bool IsPushCustomer { get; set; }
        public int TypeWarehousing { get; set; }
        public int ListGoodsId { get; set; }
        public int? ToProvinceId { get; set; }
        public int? ToDistrictId { get; set; }
        public int? ToWardId { get; set; }
        public string ShippingAddress { get; set; }
        public int? ServiceId { get; set; }
        public string Content { get; set; }
        public string CusNote { get; set; }
        public int Doc { get; set; }
        public List<Box> Boxes { get; set; }
        public bool IsPackage { get; set; }
        public bool IsScan { get; set; } = false;
        public bool IsAccept { get; set; } = true;


    }
}
