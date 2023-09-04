using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Core.Data.Core.Utils;
using Core.Entity.Abstract;
using Core.Entity.Entities;

namespace Core.Business.ViewModels
{
    public class CompletePickupsViewModel
    {
        public CompletePickupsViewModel()
        {
        }
        public List<int> ShipmentIds { get; set; }
        public List<int> RequestShipmentIds { get; set; }
        public double? CurrentLat { get; set; }
        public double? CurrentLng { get; set; }
        public int ShipmentStatusId { get; set; }
        
    }
}
