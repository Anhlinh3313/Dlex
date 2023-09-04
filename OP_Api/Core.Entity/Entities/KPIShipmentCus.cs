using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class KPIShipmentCus : IEntityBase
    {
        public int Id { get; set; }
        public bool IsEnabled { get; set; }
        public int CusId {get; set; }
        public int KPIShipmentId { get; set; }
        public int? CompanyId { get; set; }
    }
}
