using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class DeadlineShipmentViewModel
    {
        public DeadlineShipmentViewModel() { }

        public DateTime? TimeStart { get; set; }
        public int? FromHudId { get; set; }
        public int? DistrictFromId { get; set; }
        public int? WardFromId { get; set; }

        public int ServiceId { get; set; }
        public int? DistrictToId { get; set; }
        public int? WardToId { get; set; }
    }
}
