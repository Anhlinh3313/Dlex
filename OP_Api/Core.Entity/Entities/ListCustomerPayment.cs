using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity.Entities
{
    public class ListCustomerPayment : EntitySimple
    {
        public int? CustomerId { get; set; }
        public int? TotalShipment { get; set; }
        public double? GrandTotal { get; set; }
        public double? AdjustPrice { get; set; }
        public int? AttachmentId { get; set; }
        public int? ListCustomerPaymentTypeId { get; set; }
        public bool? Paid { get; set; }
        public bool? Locked { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string Note { get; set; }
        public int? HubCreatedId { get; set; }
        public double BeforeGrandTotal { get; set; }
        public double DiscountPercent { get; set; }
        public double GrandTotalPrice { get; set; }
        public double GrandTotalCOD { get; set; }
        
        //
        [ForeignKey("CreatedBy")]
        public virtual User UserCreated { get; set; }
        [ForeignKey("ModifiedBy")]
        public virtual User UserModified { get; set; }
        [ForeignKey("ListCustomerPaymentTypeId")]
        public virtual ListCustomerPaymentType ListCustomerPaymentType { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
        public DateTime? AcceptDateFrom { get; set; }
        public DateTime? AcceptDateTo { get; set; }
        public ListCustomerPayment()
        {
        }
       
    }
}
