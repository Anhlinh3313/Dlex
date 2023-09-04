using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class Truck : EntitySimple
    {
        public Truck() { }

        public int PayLoad { get; set; }
        public string TruckNumber { get; set; }
        public int LoadLimit { get; set; }
        public int? TruckRentalId { get; set; }
        public string TruckRentalName { get; set; }
        public int TruckOwnershipId { get; set; }
        public string TruckOwnershipName { get; set; }
        public int TruckTypeId { get; set; }
        public string TruckTypeName { get; set; }
    }
}
