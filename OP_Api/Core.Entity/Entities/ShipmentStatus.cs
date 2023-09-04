using System;
namespace Core.Entity.Entities
{
    public class ShipmentStatus : EntitySimple
    {
        public ShipmentStatus()
        {
        }

        public string Description { get; set; }
        public bool? IsPick { get; set; }
        public bool? IsPack { get; set; }
        public bool? IsDeliver { get; set; }
        public bool? IsTransfer { get; set; }
        public bool? IsReturnTransfer { get; set; }
        public bool? IsReturn { get; set; }
    }
}
