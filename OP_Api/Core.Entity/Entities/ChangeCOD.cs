using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class ChangeCOD : EntityBasic
    {
        public ChangeCOD() { }

        public int ShipmentId { get; set; }
        public int ChangeCODTypeId { get; set; }
        public double OldCOD { get; set; }
        public double NewCOD { get; set; }
        public int? ShipmentSupportId { get; set; }
        public bool IsAccept { get; set; }
        public string Note { get; set; }
    }
}
