using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Business.ViewModels
{
    public class ListGoodsCreateUpdateViewModel : SimpleViewModel
    {
        public ListGoodsCreateUpdateViewModel()
        {
        }

        public int TotalBox { get; set; }
        public int? TruckId { get; set; }
        public double FeeRent { get; set; }
        public int? ReceiveHubId { get; set; }
        public int ShipmentStatusId { get; set; }
        public double CurrentLat { get; set; }
        public double CurrentLng { get; set; }
        public string Location { get; set; }
        public int? TPLId { get; set; }
        public string TPLNumber { get; set; }
        public List<int> ShipmentIds { get; set; }
        public string SealNumber { get; set; }
        public int ListGoodsTypeId { get; set; }
        public int NumPrint { get; set; }
        public int CreatedByHub { get; set; }
        public int? ListGoodsStatusId { get; set; }
        public int? FromHubId { get; set; }
        public int? ToHubId { get; set; }
        public DateTime? CancelTime { get; set; }
        public string CancelNote { get; set; }
        public string Note { get; set; }
        //Transit
        public int? EmpId { get; set; }
        public int? TransportTypeId { get; set; }
        public DateTime? StartExpectedTime { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndExpectedTime { get; set; }
        public DateTime? EndTime { get; set; }
        public double RealWeight { get; set; }
        public string TruckNumber { get; set; }
        public string MAWB { get; set; }
        public int? TruckScheduleId { get; set; }
        //
        public int TotalReceived { get; set; }
        public int TotalNotReceive { get; set; }
        public int TotalReceivedOther { get; set; }
        public int TotalReceivedError { get; set; }
        public int TotalShipment { get; set; }
    }
}
