using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class StatusPrintShipment
    {
        public StatusPrintShipment() { }

        public int Id { set; get; }
        public bool IsEnabled { set; get; }
        public string Code { set; get; }
        public string Name { set; get; }
    }
}
