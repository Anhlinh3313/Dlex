using System;
using Core.Entity.Abstract;

namespace Core.Entity.Entities
{
    public class ShipmentPackage : IEntityBase
    {
        public ShipmentPackage()
        {
        }

        public int Id { get; set; }
        public int ShipmentId { get; set; }
        public int PackageId { get; set; }
        public bool IsEnabled { get; set; }
        public int? CompanyId { get; set; }
        //
        public bool IsInput { get; set; }
        public int? InputUserId { get; set; }
        public DateTime? InputWhen { get; set; }
        public bool IsOutput { get; set; }
        public int? OutputUserId { get; set; }
        public DateTime? OutputWhen { get; set; }
    }
}
