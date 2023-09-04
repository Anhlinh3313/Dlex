using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels.TruckSchedules
{
    public class TruckScheduleViewModel : SimpleViewModel<TruckScheduleViewModel, TruckSchedule>
    {
        public TruckScheduleViewModel() { }

        public int FromHubId { get; set; }
        public int? ToHubId { get; set; }
        public int? TruckScheduleStatusId { get; set; }
        public int? TruckId { get; set; }
        public string SealNumber { get; set; }
        public DateTime? StartDatetime { get; set; }
        public double? StartKM { get; set; }
        public int[] RiderIds { get; set; }
    }
}
