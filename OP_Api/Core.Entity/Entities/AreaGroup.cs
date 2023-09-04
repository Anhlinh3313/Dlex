using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class AreaGroup : EntitySimple
    {
        public AreaGroup() { }

        public int HubId { set; get; }
        public bool IsAuto { get; set; }
        public Hub Hub { get; set; }
    }
}
