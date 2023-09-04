using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class DeadlinePickupDeliveryInfoViewModel: SimpleViewModel
    {
        public DeadlinePickupDeliveryInfoViewModel() { }

        public int HubId { get; set; }
        public int AreaGroupId { get; set; }

        public Hub Hub { get; set; }
        public AreaGroup AreaGroup { get; set; }
    }
}
