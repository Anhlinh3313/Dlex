using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class DeadlineShipmentResultViewModel
    {
        public DeadlineShipmentResultViewModel() { }

        public DateTime FormatDatetime { get; set; }
        public string FormatDateTT { get; set; }
        public string FormatDateHH { get; set; }
        public int FormatDay { get; set; }
        public int FormatTime { get; set; }
        public double FormatTimeSys { get; set; }
    }
}
