using Core.Entity.Abstract;
using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels.TruckSchedules
{
    public class TruckScheduleDetailViewModel : IEntityBase
    {
        public TruckScheduleDetailViewModel() { }

        public TruckScheduleDetailViewModel(int truckScheduleId,int? hubId, string sealNumber, string location, string note, double? lat, double? lng, int truckScheduleStatusId) {
            TruckScheduleId = truckScheduleId;
            HubId = hubId;
            SealNumber = sealNumber;
            Location = location;
            Note = note;
            Lat = lat;
            Lng = lng;
            TruckScheduleStatusId = truckScheduleStatusId;
        }
        public int Id { set; get; }
        public bool IsEnabled { set; get; }
        public int TruckScheduleId { get; set; }
        public int? HubId { get; set; }
        public string SealNumber { get; set; }
        public string Location { get; set; }
        public string Note { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public int TruckScheduleStatusId { get; set; }
        public int? CompanyId { get; set; }

        public TruckScheduleStatus TruckScheduleStatus { get; set; }
        public Hub Hub { get; set; }
    }
}
