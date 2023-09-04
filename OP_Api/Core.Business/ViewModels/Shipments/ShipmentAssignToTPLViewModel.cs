using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Business.ViewModels.Shipments
{
    public class ShipmentAssignToTPLViewModel
    {
        public ShipmentAssignToTPLViewModel() { }

        public int TPLId { get; set; }
        public DateTime? TPLCreatedWhen { get; set; }
        public int ShipmentId { get; set; }
        public string ShipmentNumber { get; set; }
        public string TPLNumber { get; set; }
        public double TPLPrice { get; set; }
        public double TPLPriceReal { get; set; }
    }
}
