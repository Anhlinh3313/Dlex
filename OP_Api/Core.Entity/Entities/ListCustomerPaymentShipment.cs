using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity.Entities
{
    public class ListCustomerPaymentShipment : EntitySimple
    {
        public int? ListCustomerPaymentId { get; set; }
        public int? ShipmentId { get; set; }
        public double? COD { get; set; }
        public double? TotalPrice { get; set; }
        [ForeignKey("ListCustomerPaymentId")]
        public virtual ListCustomerPayment ListCustomerPayment { get; set; }
        [ForeignKey("ShipmentId")]
        public virtual Shipment Shipment { get; set; }
        public ListCustomerPaymentShipment()
        {
        }
       
    }
}
