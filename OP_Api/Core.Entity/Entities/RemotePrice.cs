using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class RemotePrice : EntitySimple
    {
        public double FromValue { get; set; }
        public double ToValue { get; set; }
    }
}
