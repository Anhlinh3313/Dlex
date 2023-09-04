using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class Area : EntitySimple
    {
        public Area() { }

        public int? AreaGroupId { set; get; }
        public bool IsAuto { get; set; }
        public double ExpectedTime { get; set; }
        public AreaGroup AreaGroup { set; get; }
    }
}
