using System;
namespace Core.Entity.Entities
{
    public class ListReceiptMoneyShipment : EntitySimple
    {
        public int? ListReceiptMoneyId { get; set; }
        public int? ShipmentId { get; set; }
        public double? COD { get; set; }
        public double? TotalPrice { get; set; }
        public virtual ListReceiptMoney ListReceiptMoney { get; set; } 
        public virtual Shipment Shipment { get; set; }
        public ListReceiptMoneyShipment()
        {
        }
       
    }
}
