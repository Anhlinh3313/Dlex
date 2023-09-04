using Core.Data.Core.Utils;
using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Core.Business.ViewModels
{
    public class ListReceiptMoneyViewModel : EntitySimple
    {
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
        public double? PriceCOD { get; set; }
        public double? GrandTotal { get; set; }
        public double? GrandTotalReal { get; set; }
        public int? ListReceiptMoneyStatusId { get; set; }
        public int? ListReceiptMoneyTypeId { get; set; }
        public int? AttachmentId { get; set; }
        public string Note { get; set; }
        public bool IsDirectly { get; set; }
        public int? AccountingAccountId { get; set; }
        public DateTime? AcceptDate { get; set; }
        public DateTime? LockDate { get; set; }
        public DateTime? FirstLockDate { get; set; }
        public string WarningNote { get; set; }
        public bool? Seen { get; set; }
        public int? CashFlowId { get; set; }
        public string CancelReason { get; set; }
        public int? AcceptByUserId { get; set; }
        public List<ShipmentToReceipt> Shipments { get; set; }
        public int? ReasonListGoodsId { get; set; }
        public ListReceiptMoneyViewModel()
        {
        }
    }
}
