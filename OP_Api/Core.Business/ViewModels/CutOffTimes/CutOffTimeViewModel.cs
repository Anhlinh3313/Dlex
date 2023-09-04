using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels.CutOffTimes
{
    public class CutOffTimeViewModel : EntitySimple
    {
        public CutOffTimeViewModel() { }
        public bool? Monday { get; set; }
        public bool? Tuesday { get; set; }
        public bool? Wednesday { get; set; }
        public bool? Thursday { get; set; }
        public bool? Friday { get; set; }
        public bool? Saturday { get; set; }
        public bool? Sunday { get; set; }
        public DateTime? CutTime1st { get; set; }
        public DateTime? CutTime2nd { get; set; }
        public DateTime? CutTime3rd { get; set; }
		public string DaysOfWeek { get; set; }
	}
}
