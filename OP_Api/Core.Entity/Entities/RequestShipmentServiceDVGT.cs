using System;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Entity.Abstract;

namespace Core.Entity.Entities
{
    public class RequestShipmentServiceDVGT : IEntityBase
    {
        public RequestShipmentServiceDVGT() { }

        public int Id { get; set; }
        public int RequestShipmentId { set; get; }
        public int ServiceDVGTId { get; set; }
        [ForeignKey("ServiceDVGTId")]
        public ServiceDVGT ServiceDVGT { get; set; }
        public bool IsEnabled { get; set; }
        public int? CompanyId { get; set; }
    }
}
