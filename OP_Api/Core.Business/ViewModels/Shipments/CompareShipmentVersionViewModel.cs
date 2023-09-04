using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels.Shipments
{
    public class CompareShipmentVersionViewModel
    {
        public int? ShipmentId { get; set; } = null;
        public int? ShipmentVersionId { get; set; } = null;
    }
}
