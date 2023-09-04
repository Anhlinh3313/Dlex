using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Core.Data.Core.Utils;
using Core.Entity.Abstract;

namespace Core.Business.ViewModels
{
    public class PickupFailsViewModel
    {
        public PickupFailsViewModel()
        {
        }

        public List<int> Ids { get; set; }
        public int? EmpId { get; set; }
        public string ShipmentNumber { get; set; }
        public string ReferencesCode { get; set; }
        public int ShipmentStatusId { get; set; }
        public int? ReasonId { get; set; }
        public double? CurrentLat { get; set; }
        public double? CurrentLng { get; set; }
        public string Location { get; set; }
        public string Note { get; set; }
        public string TPLCode { get; set; }
        public string TPLNumber { get; set; }
        public int? CurrentHubId { get; set; }
        public int? CurrentEmpId { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsPushCustomer { get; set; }
        public bool IsPackage { get; set; }
    }
}
