using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class DeadlinePickupDeliveryDetail : EntitySimple
    {
        public DeadlinePickupDeliveryDetail() { }

        public int DeadlinePickupDeliveryId { get; set; }
        public int ServiceId { get; set; }
        public int AreaId { get; set; }
        public double TimePickup { get; set; }
        public double TimeDelivery { get; set; }
    }
}
