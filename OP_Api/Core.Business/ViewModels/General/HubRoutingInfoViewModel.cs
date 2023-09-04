using Core.Entity.Entities;

namespace Core.Business.ViewModels.General
{
    public class HubRoutingInfoViewModel : SimpleViewModel
    {
        public HubRoutingInfoViewModel()
        {
        }

        public int HubId { get; set; }
        public bool? IsTruckDelivery { get; set; }
        public string CodeConnect { get; set; }
        public int? UserId { get; set; }
        public double? RadiusServe { get; set; }
        public virtual HubInfoViewModel Hub { get; set; }
        public virtual UserInfoViewModel User { get; set; }
    }
}
