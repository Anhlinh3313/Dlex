using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class CloseOpenSealViewModel
    {
        public string SealNumber { get; set; }
        public string TruckScheudleNumber { get; set; }
        public string Location { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }
    }
}
