using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class ShipmentType : IEntityBase
    {
        public ShipmentType() { }

        public int Id { set; get; }
        public bool IsEnabled { set; get; }
        public string Code { set; get; }
        public string Name { set; get; }
        public int? CompanyId { get; set; }
    }
}
