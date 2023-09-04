using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity.Entities
{
    public class ShipmentServiceDVGT : IEntityBase
    {
        public ShipmentServiceDVGT() { }

        public int Id { get; set; }
        public int ShipmentId { set; get; }
        //public int? ServiceDVGTId { get; set; } //old
        public int? ServiceId { get; set; }
        //public ServiceDVGT ServiceDVGT { get; set; }
        public Service Service { get; set; }
        public bool IsEnabled { get; set; }
        public double Price { get; set; }
        public bool IsAgree { get; set; }
        public int? CompanyId { get; set; }
    }
}
