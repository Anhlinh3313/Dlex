using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Core.Data.Core.Utils;
using Core.Entity.Abstract;
using Core.Entity.Entities;

namespace Core.Business.ViewModels
{
    public class CompletePickupViewModel
    {
        public CompletePickupViewModel()
        {
        }

        //public int Id { get; set; }
        public List<int> ShipmentIds { get; set; }
        public int RequestShipmentId { get; set; }
        public double? CurrentLat { get; set; }
        public double? CurrentLng { get; set; }
        public int ShipmentStatusId { get; set; }
        
    }
}
