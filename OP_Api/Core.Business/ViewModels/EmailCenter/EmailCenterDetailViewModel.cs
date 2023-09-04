using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class EmailCenterDetailViewModel
    {
        public EmailCenterDetailViewModel() { }
        public int ShipmentId { get; set; }
        public int? LadingScheduleId { get; set; }
        public int? ComplainId { get; set; }
        public int? IncidentsId { get; set; }
        public bool IsDeliverd { get; set; }
        public bool IsReturn { get; set; }
    }
}
