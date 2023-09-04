using System;
namespace Core.Entity.Entities
{
    public class RequestLadingSchedule : EntityBasic
    {
        public RequestLadingSchedule()
        {
        }

        public int ShipmentId { get; set; }
        public int? HubId { get; set; }
        public int UserId { get; set; }
        public int ShipmentStatusId { get; set; }
        public double Lat { get; set; } = 0;
        public double Lng { get; set; } = 0;
        public string Location { get; set; }
        public string Note { get; set; }
        public int? ReasonId { get; set; }
        public int? CreatedByUAType { get; set; }

        public Shipment Shipment { get; set; }
        public Hub Hub { get; set; }
        public User User { get; set; }
        public ShipmentStatus ShipmentStatus { get; set; }
    }
}
