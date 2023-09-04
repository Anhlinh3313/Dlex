using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class Holiday : EntitySimple
    {
        public Holiday() { }

        public string NotHoliday { set; get; }
        public DateTime Date { get; set; }
        public bool? IsSa { get; set; }
        public bool? IsSu { get; set; }
        public bool? IsFull { get; set; }
    }
}
