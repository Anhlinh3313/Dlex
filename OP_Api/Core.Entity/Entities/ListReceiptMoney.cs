using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity.Entities
{
    public class ListReceiptMoney : EntitySimple
    {
        //public DateTime? LockDate { get; set; }
        public double FeeBank { get; set; }
        public bool IsTransfer { get; set; }
        public string BankAccount { get; set; }
        public string ImagePathDOC { get; set; }
        public int? FromHubId { get; set; }
        public int? ToHubId { get; set; }
        public int? PaidByEmpId { get; set; }
        public int? TotalShipment { get; set; }
        public double? TotalCOD { get; set; }
        public double? TotalPrice { get; set; }
        public double? GrandTotal { get; set; }
        public double? GrandTotalReal { get; set; }
        public int? ListReceiptMoneyStatusId { get; set; }
        public int? AttachmentId { get; set; }
        public int? ListReceiptMoneyTypeId { get; set; }
        public string Note { get; set; }
        public int? AccountingAccountId { get; set; }
        public DateTime? AcceptDate { get; set; }
        public DateTime? LockDate { get; set; }
        public DateTime? FirstLockDate { get; set; }
        public string WarningNote { get; set; }
        public bool? Seen { get; set; }
        public int? CashFlowId { get; set; }
        public string CancelReason { get; set; }
        public int? ReasonListGoodsId { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User UserCreated { get; set; }
        [ForeignKey("ModifiedBy")]
        public virtual User UserModified { get; set; }
        [ForeignKey("ListReceiptMoneyStatusId")]
        public virtual ListReceiptMoneyStatus ListReceiptMoneyStatus { get; set; }
        [ForeignKey("ListReceiptMoneyTypeId")]
        public virtual ListReceiptMoneyType ListReceiptMoneyType { get; set; }
        [ForeignKey("PaidByEmpId")]
        public virtual User PaidByEmp { get; set; }
        [ForeignKey("FromHubId")]
        public virtual Hub FromHub { get; set; }
        [ForeignKey("ToHubId")]
        public virtual Hub ToHub { get; set; }
        [ForeignKey("AccountingAccountId")]
        public virtual AccountingAccount AccountingAccount { get; set; }
        [ForeignKey("CashFlowId")]
        public virtual CashFlow CashFlow { get; set; }
        [ForeignKey("ReasonListGoodsId")]
        public virtual Reason Reason { get; set; }

        public ListReceiptMoney()
        {
        }
       
    }
}
