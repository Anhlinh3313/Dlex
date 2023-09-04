using Core.Data.Core.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Core.Business.ViewModels
{
    public class ListReceiptMoneyShipmentInfoViewModel : SimpleViewModel
    {
        public int? ListReceiptMoneyId { get; set; }
        public int? ShipmentId { get; set; }
        public double? COD { get; set; }
        public double? TotalPrice { get; set; }
        public ShipmentInfoViewModel Shipment { get; set; }
        public ListReceiptMoneyInfoViewModel ListReceiptMoney { get; set; }
        public ListReceiptMoneyShipmentInfoViewModel()
        {
        }
    }
}
