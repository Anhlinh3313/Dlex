using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class KPIShipment : EntitySimple
    {
        public bool IsPublic { get; set; }
        public int Priority { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int ServiceId { get; set; }
    }
}
