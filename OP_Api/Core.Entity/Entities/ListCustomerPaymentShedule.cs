using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity.Entities
{
    public class ListCustomerPaymentSchedule : EntitySimple
    {
        public int? CustomerId { get; set; }
        public int? TotalShipment { get; set; }
        public double? GrandTotal { get; set; }
        public int? AttachmentId { get; set; }
        public int? ListCustomerPaymentTypeId { get; set; }
        public bool? Paid { get; set; }
        public bool? Locked { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int? ListCustomerPaymentId { get; set; }
        public string Note { get; set; }
        public int? HubCreatedId { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User UserCreated { get; set; }
        [ForeignKey("ModifiedBy")]
        public virtual User UserModified { get; set; }
        [ForeignKey("ListCustomerPaymentTypeId")]
        public virtual ListCustomerPaymentType ListCustomerPaymentType { get; set; }
        [ForeignKey("ListCustomerPaymentId")]
        public virtual ListCustomerPayment ListCustomerPayment { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
        public ListCustomerPaymentSchedule()
        {
        }
       
    }
}
