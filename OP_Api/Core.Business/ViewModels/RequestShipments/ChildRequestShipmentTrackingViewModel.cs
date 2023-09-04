using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Core.Data.Core.Utils;
using Core.Entity.Abstract;
using Core.Entity.Entities;

namespace Core.Business.ViewModels
{
    public class ChildRequestShipmentTrackingViewModel: CreateChildRequestShipmentViewModel
    {
        public ChildRequestShipmentTrackingViewModel()
        {
        }

        public int? CusDepartmentId { get; set; }
        public string CusDepartmentName { get; set; }
        public string ShipmentNumber { get; set; }
        public List<Shipment> Shipments { get; set; }
    }
}
