using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels.TruckSchedules
{
    public class TruckScheduleInfoViewModel : SimpleViewModel
    {
        public TruckScheduleInfoViewModel()
        {

        }
        
        public string SealNumber { get; set; }
        public int FromHubId { get; set; }
        public int ToHubId { get; set; }
        public int TruckScheduleStatusId { get; set; }
        public int TruckId { get; set; }
        public DateTime? StartDatetime { get; set; }
        public double? StartKM { get; set; }
        public double TotalWeight { get; set; }
        public int TotalBox { get; set; }
        public User[] Riders { get; set; }

        public TruckScheduleStatus TruckScheduleStatus { get; set; }
        public Hub FromHub { get; set; }
        public Hub ToHub { get; set; }
        public Truck Truck { get; set; }
    }
}
