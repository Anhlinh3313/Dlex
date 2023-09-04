using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Core.Business.ViewModels.Shipments;
using Core.Data.Core.Utils;
using Core.Entity.Abstract;

namespace Core.Business.ViewModels
{
    public class ListRequestShipmentAndShipment
    {
        public ListRequestShipmentAndShipment()
        {
        }

        public int? CusDepartmentId { get; set; }
        public CreateChildRequestShipmentViewModel RequestShipment { get; set; }
        public List<string> ShipmentNumbers { get; set; }
    }
}
