using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class StreetJoin : IEntityBase
    {
        public StreetJoin() { }

        public int Id { get; set; }
        public int? ProvinceId { get; set; }
        public int? DistrictId { get; set; }
        public int WardId { get; set; }
        public int StreetId { get; set; }
        public bool IsEnabled { get; set; }
        public int? CompanyId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

    }
}
