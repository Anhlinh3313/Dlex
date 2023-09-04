using Core.Business.ViewModels.General;
using Core.Data.Core.Utils;
using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Core.Business.ViewModels
{
    public class ListCustomerPaymentInfoViewModel : SimpleViewModel
    {
        public DateTime? CreatedWhen { get; set; }
        public DateTime? ModifiedWhen { get; set; }
        public int? CustomerId { get; set; }
        public int? TotalShipment { get; set; }
        public double? GrandTotal { get; set; }
        public double? AdjustPrice { get; set; }
        public int? ListCustomerPaymentTypeId { get; set; }
        public int? AttachmentId { get; set; }
        public int? HubCreatedId { get; set; }
        public string Note { get; set; }
        public bool Locked { get; set; }
        public bool Paid { get; set; }
        public double BeforeGrandTotal { get; set; }
        public double DiscountPercent { get; set; }
        public double GrandTotalPrice { get; set; }
        public double GrandTotalCOD { get; set; }
        public string StatusName { get { return (this.Paid ? "Đã thanh toán" : (this.Locked ? "Đã khóa" : "Chưa khóa")); } }
        public HubViewModel HubCreated { get; set; }
        public CustomerViewModel Customer { get; set; }
        public UserInfoViewModel UserCreated { get; set; }
        public UserInfoViewModel UserModified { get; set; }
        public ListCustomerPaymentTypeInfoViewModel ListCustomerPaymentType { get; set; }
        public DateTime? AcceptDateFrom { get; set; }
        public DateTime? AcceptDateTo { get; set; }

        public ListCustomerPaymentInfoViewModel()
        {

        }
    }
}
