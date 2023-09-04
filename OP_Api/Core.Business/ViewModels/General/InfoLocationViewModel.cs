using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels.General
{
    public class InfoLocationViewModel
    {
        public InfoLocationViewModel()
        {
        }
        public int? ProvinceId { get; set; }
        public int DistrictId { get; set; }
        public int WardId { get; set; }
        public int HubId { get; set; }
        public Hub Hub { get; set; }
    }
}
