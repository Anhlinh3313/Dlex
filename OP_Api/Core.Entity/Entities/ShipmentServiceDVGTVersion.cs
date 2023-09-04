using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity.Entities
{
    public class ShipmentServiceDVGTVersion : IEntityBase
    {
        public ShipmentServiceDVGTVersion() { }

        public int Id { get; set; }
        public int ShipmentId { set; get; }
        public int ServiceDVGTId { get; set; }
        public ServiceDVGT ServiceDVGT { get; set; }
        public bool IsEnabled { get; set; }
        public int? CompanyId { get; set; }
    }
}
