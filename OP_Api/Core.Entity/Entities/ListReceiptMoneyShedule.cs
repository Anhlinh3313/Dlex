using System;
namespace Core.Entity.Entities
{
    public class ListReceiptMoneySchedule : EntitySimple
    {
        public int? FromHubId { get; set; }
        public int? ToHubId { get; set; }
        public int? PaidByEmpId { get; set; }
        public int? TotalShipment { get; set; }
        public double? TotalCOD { get; set; }
        public double? TotalPrice { get; set; }
        public double? GrandTotal { get; set; }
        public int? ListReceiptMoneyStatusId { get; set; }
        public int? AttachmentId { get; set; }
        public int? ListReceiptMoneyId { get; set; }

        public virtual ListReceiptMoney ListReceiptMoney { get; set; } 
        public ListReceiptMoneySchedule()
        {
        }
       
    }
}
