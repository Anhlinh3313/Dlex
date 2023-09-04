using System;
using Core.Business.ViewModels.General;
using Core.Entity.Entities;

namespace Core.Business.ViewModels
{
    public class LadingScheduleViewModel
    {
        public LadingScheduleViewModel()
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
        //
        public int? ToHubId { get; set; }
        public bool IsDelay { get; set; }
        public int? DelayReasonId { get; set; }
        public string DelayNote { get; set; }
        public double? DelayTime { get; set; }

        public ShipmentInfoViewModel Shipment { get; set; }
        public HubViewModel Hub { get; set; }
        public UserInfoViewModel User { get; set; }
        public ShipmentStatusViewModel ShipmentStatus { get; set; }
    }
}
