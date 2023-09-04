using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class ChargedCOD : IEntityBase
    {
        public ChargedCOD() { }

        public int Id { get; set; }
        public int DistrictId { get; set; }
        public District District { set; get; }
        public bool IsEnabled { get; set; }
        public int? CompanyId { get; set; }
    }
}
