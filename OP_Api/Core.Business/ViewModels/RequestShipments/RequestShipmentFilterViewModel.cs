using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Core.Business.ViewModels.General;
using Core.Entity.Entities;

namespace Core.Business.ViewModels
{
    public class RequestShipmentFilterViewModel
    {
        public RequestShipmentFilterViewModel()
        {
        }

        //public int UserId { get; set; }
        //public string ListHub { get; set; }
        public int? PickUserId { get; set; }
        public string Type { get; set; }
        public string TypePickup { get; set; }
        public bool? IsEnabled { get; set; }
        public string ShipmentNumber { get; set; }
        public DateTime? OrderDateFrom { get; set; }
        public DateTime? OrderDateTo { get; set; }
        public int? SenderId { get; set; }
        public int? FromHubId { get; set; }
        public int? ToHubId { get; set; }
        public int? ShipmentStatusId { get; set; }
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public bool? IsSortDescending { get; set; }
        public int? PickupType { get; set; }
    }
}
