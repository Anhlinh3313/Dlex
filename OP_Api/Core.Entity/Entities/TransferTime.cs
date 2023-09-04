using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class TransferTime : EntitySimple
    {
        public TransferTime() { }
        public int? CutOffId { get; set; }
        public double? ExportTime1st { get; set; }
        public double? ExportTime2nd { get; set; }
        public double? ExportTime3rd { get; set; }
        public DateTime? StartTime1st { get; set; }
        public DateTime? StartTime2nd { get; set; }
        public DateTime? StartTime3rd { get; set; }
    }
}
