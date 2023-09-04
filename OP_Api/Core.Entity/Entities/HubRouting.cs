using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity.Entities
{
    [Table("Core_HubRouting")]
    public class HubRouting : EntitySimple
    {
        public HubRouting()
        {
        }

        public int HubId { get; set; }
        public int? UserId { get; set; }
        public bool? IsTruckDelivery { get; set; }
        public string CodeConnect { get; set; }
        public double? RadiusServe { get; set; }
        public virtual Hub Hub { get; set; }
        public virtual User User { get; set; }

    }
}
