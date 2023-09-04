using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class ComplainInfoViewModel : SimpleViewModel
    {
        public ComplainInfoViewModel() { }
        public DateTime? CreatedWhen { get; set; }
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
        public virtual User HandlingUser { get; set; }
        public virtual Hub HandlingHub { get; set; }
        public virtual User ForwardToEmp { get; set; }
        public virtual Hub ForwardToHub { get; set; }
    }
}
