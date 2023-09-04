using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class TPLTransit : EntitySimple
    {
        public TPLTransit() { }

        public int TPLId { get; set; }
        public string TPLNumber { get; set; }
        public int ShipmentId { get; set; }
        public int FromHubId { get; set; }
        public int ToHubId { get; set; }
        public int StatusId { get; set; }
    }
}
