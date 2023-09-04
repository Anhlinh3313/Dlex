using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class IncomingPaymentViewModel : SimpleViewModel
    {
        public string BankAccount { get; set; }
        public DateTime PaymentDate { get; set; }
        public double GrandTotalReal { get; set; }
        public string CashFlowCode { get; set; }
        //
        public int ShipmentId { get; set; }
        public string CustomerCode { get; set; }
        public double? DocTotal { get; set; }
        public double? Total { get; set; }
        public DateTime? DocDate { get; set; }
        public string PaymentType { get; set; }
        public DateTime? TransferDate { get; set; }
        public string DocumentNo { get; set; }
        public string CreatedByFullName { get; set; }
        public string CreatedByCode { get; set; }
        public string ListReceiveMoneyCode { get; set; }
        public bool IsPushIncomming { get; set; }
        public string ShipmentNumber { get; set; }
        public int Flag { get; set; }
    }
}
