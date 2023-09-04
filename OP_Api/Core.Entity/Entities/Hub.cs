using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Entity.Abstract;

namespace Core.Entity.Entities
{
    public class Hub : EntitySimple
    {
        public Hub()
        {
        }

        public int DistrictId { get; set; }
        public int WardId { get; set; }
        public int ProvinceId { get; set; }
        public int? CenterHubId { get; set; }
        public int? PoHubId { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string AddressDisplay { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public bool? HasAirPort { get; set; }
        public double? RadiusServe { get; set; }
        public virtual District District { get; set; }
        public virtual Ward Ward { get; set; }
        public virtual Hub CenterHub { get; set; }
        public virtual Hub PoHub { get; set; }
        public virtual List<User> Users { get; set; }
        public virtual List<Hub> PoHubs { get; set; }
    }
}
