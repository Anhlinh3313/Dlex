using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class KPIShipmentDetail : EntitySimple
    {
        public int WardId { get; set; }
        public int? DistrictId { get; set; }
        public string Vehicle { get; set; }
        public int TargetDeliveryTime { get; set; }
        public int KPIShipmentId { get; set; }
        public int? TargetPaymentCOD { get; set; }
    }
}
