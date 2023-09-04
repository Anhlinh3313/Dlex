using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entity.Entities
{
    public class TruckScheduleDetail : EntitySimple
    {
        public TruckScheduleDetail() { }

        public int TruckScheduleId { get; set; }
        public string SealNumber { get; set; }
        public string Location { get; set; }
        public string Note { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public int TruckScheduleStatusId { get; set; }
        public int HubId { get; set; }

        [ForeignKey("TruckScheduleStatusId")]
        public virtual TruckScheduleStatus TruckScheduleStatus { get; set; }

        [ForeignKey("HubId")]
        public virtual Hub Hub { get; set; }
    }
}
