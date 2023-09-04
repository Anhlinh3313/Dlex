using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class ReportRequestPickupAndShipmentDeliveryViewModel
    {

        public ReportRequestPickupAndShipmentDeliveryViewModel(int totalRequestPickup, int totalRequestPickuped, int totalShipmentDelivered, int totalShipmentKeeping)
        {
            TotalRequestPickup = totalRequestPickup;
            TotalRequestPickuped = totalRequestPickuped;
            TotalShipmentDelivered = totalShipmentDelivered;
            TotalShipmentKeeping = totalShipmentKeeping;
        }

        public int TotalRequestPickup { get; set; }
        public int TotalRequestPickuped { get; set; }
        public int TotalShipmentDelivered { get; set; }
        public int TotalShipmentKeeping { get; set; }
    }
}