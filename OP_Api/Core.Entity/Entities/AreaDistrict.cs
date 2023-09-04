using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class AreaDistrict : IEntityBase
    {
        public AreaDistrict() {
        }
        public int Id { get; set; }
        public int AreaId { get; set; }
        public int DistrictId { get; set; }
        public Area Area { get; set; }
        public District District { get; set; }
        public bool IsEnabled { get; set; }
        public int? CompanyId { get; set; }

    }
}
