using System;
namespace Core.Business.ViewModels
{
    public class ReceiveShipmentViewModel
    {
        public string ShipmentNumber { get; set; }
        //Lading Schedule
        public double CurrentLat { get; set; }
        public double CurrentLng { get; set; }
        public string Location { get; set; }
        public bool IsUpdateBK { get; set; }
        public int[] ServiceDVGTIds { get; set; }
        public int? StructureId { get; set; }
        public int? ReasonId { get; set; }
        public int? ListGoodsId { get; set; }
        public int? ToHubId { get; set; }
        public bool IsCheckSchedule { get; set; }
        public bool IsPackage { get; set; }
        public string Note { get; set; }
        public int? ToUserId { get; set; }
        public bool IsScan { get; set; } = false;
        public bool IsAccept { get; set; } = true;

        public ReceiveShipmentViewModel()
        {
        }
    }
}
