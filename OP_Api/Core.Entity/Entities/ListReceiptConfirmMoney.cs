using System;
namespace Core.Entity.Entities
{
    public class ListReceiptConfirmMoney : EntitySimple
    {
        public int? FromHubId { get; set; }
        public int? ToHubId { get; set; }
        public int? TotalShipment { get; set; }
        public double? TotalCOD { get; set; }
        public double? TotalPrice { get; set; }
        public double? GrandTotal { get; set; }
        public int? AttachmentId { get; set; }
        public int? ListReceiptMoneyId { get; set; }
        public int? ConfirmByListReceiptMoneyId { get; set; }
        public int? ListReceiptMoneyTypeId { get; set; }

        public virtual ListReceiptMoney ListReceiptMoney { get; set; } 
        public virtual ListReceiptMoney ConfirmByListReceiptMoney { get; set; }
        public virtual ListReceiptMoneyType ListReceiptMoneyType { get; set; }
        public ListReceiptConfirmMoney()
        {
        }
       
    }
}
