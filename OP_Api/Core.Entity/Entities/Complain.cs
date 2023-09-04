using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entity.Entities
{
    public class Complain : EntitySimple
    {
        public Complain() { }
        public int ShipmentId { get; set; }
        public int CustomerId { get; set; }
        public int ComplainTypeId { get; set; }
        public int ComplainStatusId { get; set; }
        public string ComplainContent { get; set; }
        public string ComplainImagePath { get; set; }
        public int? HandlingEmpId { get; set; }
        public int? HandlingHubId { get; set; }
        public int? ForwardToEmpId { get; set; }
        public int? ForwardToHubId { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCompensation { get; set; }
        public virtual Shipment Shipment { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ComplainType ComplainType { get; set; }
        public virtual ComplainStatus ComplainStatus { get; set; }

        [ForeignKey("HandlingEmpId")]
        public virtual User HandlingUser { get; set; }
        [ForeignKey("HandlingHubId")]
        public virtual Hub HandlingHub { get; set; }
    }
}
