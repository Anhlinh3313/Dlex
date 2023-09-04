using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Core.Entity.Entities;

namespace Core.Business.ViewModels
{
    public class GroupBoxViewModel
    {
        public GroupBoxViewModel(int shipmentId, IEnumerable<dynamic> boxes)
        {
            ShipmentId = shipmentId;
            Boxes = boxes;
        }

        public int ShipmentId { get; set; }
        public virtual IEnumerable<dynamic> Boxes { get; set; } = new Collection<dynamic>();
    }
}
