using System;
namespace Core.Entity.Entities
{
    public class ListReceiptConfirmMoneyShipment : EntitySimple
    {
        public int? ListReceiptConfirmMoneyId { get; set; }
        public int? ShipmentId { get; set; }
        public double? COD { get; set; }
        public double? TotalPrice { get; set; }

        public virtual ListReceiptConfirmMoney ListReceiptConfirmMoney { get; set; } 
        public virtual Shipment Shipment { get; set; }
        public ListReceiptConfirmMoneyShipment()
        {
        }
       
    }
}
