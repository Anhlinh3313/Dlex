using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Core.Data.Core.Utils;

namespace Core.Business.ViewModels.General
{
    public class HubInfoViewModel : SimpleViewModel
    {
        public HubInfoViewModel()
        {
        }

        public int ProvinceId { get; set; }
        public int DistrictId { get; set; }
        public int WardId { get; set; }
        public int? CenterHubId { get; set; }
        public int? PoHubId { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string AddressDisplay { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public bool? HasAirPort { get; set; }
        public double? RadiusServe { get; set; }
        public DistrictViewModel District { get; set; }
        public WardViewModel Ward { get; set; }
        public HubViewModel CenterHub { get; set; }
        public HubViewModel PoHub { get; set; }
    }
}
