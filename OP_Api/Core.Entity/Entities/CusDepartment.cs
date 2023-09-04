using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class CusDepartment : EntitySimple
    {
        public int CustomerId { get; set; }
        public string Address { get; set; }
        public string AddressNote { get; set; }
        public string PhoneNumber { get; set; }
        public string RepresentativeName { get; set; }
        public int? ProvinceId { get; set; }
        public int? DistrictId { get; set; }
        public int? WardId { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }

        public Province Province { get; set; }
        public District District { get; set; }
        public Ward Ward { get; set; }
    }
}
