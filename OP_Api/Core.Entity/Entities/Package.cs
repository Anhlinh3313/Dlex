using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity.Entities
{
    public class Package : EntitySimple
    {
        public Package()
        {
        }

        public string Content { get; set; }
        public double Weight { get; set; }
        public double CalWeight { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public int? CreatedHubId { get; set; }
        public string SealNumber { get; set; }
        //
        public int? CurrentHubId { get; set; }
        public int? CurrentUserId { get; set; }
        public int StatusId { get; set; }
        public int TotalShipment { get; set; }
        public int? ToHubId { get; set; }
        public int? ToProvinceId { get; set; }
        public int? OpenHubId { get; set; }
        public int? OpenUserId { get; set; }
        public DateTime? OpenWhen { get; set; }

        //
        [ForeignKey("ToHubId")]
        public virtual Hub ToHub { get; set; }
        [ForeignKey("CreatedHubId")]
        public virtual Hub CreatedHub { get; set; }
        [ForeignKey("OpenHubId")]
        public virtual Hub OpenHub { get; set; }

    }
}
