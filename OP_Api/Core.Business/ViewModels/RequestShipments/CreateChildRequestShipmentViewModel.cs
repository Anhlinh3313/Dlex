using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Core.Data.Core.Utils;
using Core.Entity.Abstract;

namespace Core.Business.ViewModels
{
    public class CreateChildRequestShipmentViewModel
    {
        public CreateChildRequestShipmentViewModel()
        {
        }
        
        public string ShipmentNumber { get; set; }
        public int TotalBox { get; set; }
        public double? Weight { get; set; }
        public double? CalWeight { get; set; }
        public int? TotalChildShipment { get; set; }
        public string Note { get; set; }
    }
}
