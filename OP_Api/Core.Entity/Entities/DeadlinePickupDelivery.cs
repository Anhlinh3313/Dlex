using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entity.Entities
{
    public class DeadlinePickupDelivery: EntitySimple
    {
        public DeadlinePickupDelivery() { }

        public int HubId { get; set; }
        public Hub Hub { get; set; }
        public int AreaGroupId { get; set; }
        public AreaGroup AreaGroup { get; set; }
    }
}
