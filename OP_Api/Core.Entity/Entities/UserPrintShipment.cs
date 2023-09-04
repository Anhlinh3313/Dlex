using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class UserPrintShipment: IEntityBase
    {
        public UserPrintShipment() { }

        public int Id { set; get; }
        public bool IsEnabled { set; get; }
        public DateTime CreatedWhen { set; get; }
        public int StatusPrintId { set; get; }
        public int UserId { set; get; }
        public int ShipmentId { set; get; }
        public int PrintTypeId { set; get; }
        public int? CompanyId { get; set; }
    }
}
