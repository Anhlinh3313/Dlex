using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Business.ViewModels.General;
using Core.Entity.Abstract;
using Core.Entity.Entities;
using Core.Entity.Procedures;

namespace Core.Business.ViewModels
{
    public class ParentRequestShipmentTrackingViewModel
    {
        public ParentRequestShipmentTrackingViewModel()
        {
        }

        public int RequestShipmentId { get; set; }
        public List<ChildRequestShipmentTrackingViewModel> ChildRequestShipments { get; set; }
    }
}
