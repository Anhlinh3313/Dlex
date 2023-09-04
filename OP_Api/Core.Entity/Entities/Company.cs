using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity.Entities
{
    public class Company: EntitySimple
    {
        public Company()
        {
        }

        public string PhoneNumber { get; set; }
        public string Hotline { get; set; }
        public string Address { get; set; }
        public string Website { get; set; }
        public string PrefixShipmentNumber { get; set; }
        public string PrefixRequestNumber { get; set; }
        public double TopUp { get; set; }
        public string LogoUrl { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }

        [ForeignKey("CompanySizeId")]
        public virtual CompanySize CompanySize { get; set; }
    }
}
