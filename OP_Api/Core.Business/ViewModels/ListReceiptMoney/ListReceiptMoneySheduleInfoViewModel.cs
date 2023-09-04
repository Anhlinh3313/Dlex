using Core.Business.ViewModels.General;
using Core.Data.Core.Utils;
using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Core.Business.ViewModels
{
    public class ListReceiptMoneySheduleInfoViewModel : SimpleViewModel
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

        public HubInfoViewModel FromHubName { get; set; }
        public HubInfoViewModel ToHubName { get; set; }
        public UserInfoViewModel PaidByEmp { get; set; }
        public UserInfoViewModel CreatedBy { get; set; }
        public UserInfoViewModel ModifiedBy { get; set; }
        public ListReceiptMoneyStatusInfoViewModel ListReceiptMoneyStatus { get; set; }

        public ListReceiptMoneySheduleInfoViewModel()
        {

        }
    }
}
