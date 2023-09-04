using Core.Data.Core.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Core.Business.ViewModels
{
    public class ListCustomerPaymentViewModel : SimpleViewModel
    {
        public int? CustomerId { get; set; }
        public double? GrandTotal { get; set; }
        public double? AdjustPrice { get; set; }
        public int? TotalShipment { get; set; }
        public int? ListCustomerPaymentTypeId { get; set; }
        public int? AttachmentId { get; set; }
        public List<int> ShipmentIds { get; set; }
        public int? HubCreatedId { get; set; }
        public string Note { get; set; }
        public bool Paid { get; set; }
        public bool Locked { get; set; }
        public double BeforeGrandTotal { get; set; }
        public double DiscountPercent { get; set; }
        public double GrandTotalPrice { get; set; }
        public double GrandTotalCOD { get; set; }
        public DateTime? AcceptDateFrom { get; set; }
        public DateTime? AcceptDateTo { get; set; }
        public ListCustomerPaymentViewModel()
        {
        }
    }
}
