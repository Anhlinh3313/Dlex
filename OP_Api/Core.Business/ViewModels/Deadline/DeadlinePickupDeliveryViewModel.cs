using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class DeadlinePickupDeliveryViewModel : SimpleViewModel
    {
        public int HubId { get; set; }
        public int AreaGroupId { get; set; }
    }
}
