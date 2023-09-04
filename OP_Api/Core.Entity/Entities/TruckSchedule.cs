using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entity.Entities
{
    public class TruckSchedule : EntitySimple
    {
        public int FromHubId { get; set; }
        public int ToHubId { get; set; }
        public int TruckScheduleStatusId { get; set; }
        public int TruckId { get; set; }
        public string SealNumber { get; set; }
        public DateTime? StartDatetime { get; set; }
        public double? StartKM { get; set; }

        [ForeignKey("TruckScheduleStatusId")]
        public virtual TruckScheduleStatus TruckScheduleStatus { get; set; }

        [ForeignKey("FromHubId")]
        public virtual Hub FromHub { get; set; }

        [ForeignKey("ToHubId")]
        public virtual Hub ToHub { get; set; }

        [ForeignKey("TruckId")]
        public virtual Truck Truck { get; set; }
    }
}
