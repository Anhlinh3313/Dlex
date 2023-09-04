using Core.Entity.Abstract;

namespace Core.Entity.Entities
{
    public class PromotionDetailServiceDVGT: IEntityBase
    {
        public PromotionDetailServiceDVGT()
        {
        }
        public int Id { get; set; }
        public bool IsEnabled { get; set; }
        public int PromotionDetailId { get; set; }
        public int? ServiceId { get; set; }
        public int? CompanyId { get; set; }
    }

}
