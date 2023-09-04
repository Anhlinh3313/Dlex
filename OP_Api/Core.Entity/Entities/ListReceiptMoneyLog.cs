using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class ListReceiptMoneyLog : EntityBasic
    {
        public int StatusId { get; set; }
        public int ListReceiptMoneyId { get; set; }
        public string Reason { get; set; }
        public string DataChanged { get; set; }
        public int? FromHubId { get; set; }
        public int? ToHubId { get; set; }
        public int? PaidByEmpId { get; set; }
        public int? TotalShipment { get; set; }
        public double? TotalCOD { get; set; }
        public double? TotalPrice { get; set; }
        public double? GrandTotal { get; set; }
        public int? ListReceiptMoneyStatusId { get; set; }
        public int? AttachmentId { get; set; }
        public int? ListReceiptMoneyTypeId { get; set; }
        public string Note { get; set; }
        public string ImagePathDOC { get; set; }
        public string BankAccount { get; set; }
        public bool? IsTransfer { get; set; }
        public double? GrandTotalReal { get; set; }
        public string CancelReason { get; set; }
        public DateTime? FirstLockDate { get; set; }
        public int? ReasonListGoodsId { get; set; }
        public string WarningNote { get; set; }
        public int? AccountingAccountId { get; set; }
        public int? CashFlowId { get; set; }
        public double? FeeBank { get; set; }
        public DateTime? AcceptDate { get; set; }
        //public DateTime? LockDate { get; set; }
        //public bool? Seen { get; set; }
    }
}
