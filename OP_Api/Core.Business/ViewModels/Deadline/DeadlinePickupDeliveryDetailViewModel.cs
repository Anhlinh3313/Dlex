using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class DeadlinePickupDeliveryDetailViewModel : SimpleViewModel
    {
        public DeadlinePickupDeliveryDetailViewModel() { }

        public int DeadlinePickupDeliveryId { get; set; }
        public int ServiceId { get; set; }
        public int AreaId { get; set; }
        public double TimePickup { get; set; }
        public double TimeDelivery { get; set; }
    }
}
