using System;
using Core.Entity.Abstract;

namespace Core.Business.ViewModels
{
    public class CreateUpdateLadingScheduleViewModel : IEntityBasic
    {
        public CreateUpdateLadingScheduleViewModel()
        {
        }

        public CreateUpdateLadingScheduleViewModel(int shipmentId, int? hubId, int? userId, int shipmentStatusId, double? lat, double? lng, string location, string note, int id, int? reasonId = null, DateTime? createdWhen = null, DateTime? modifiedWhen = null, int? toUserId = null)
        {
            ShipmentId = shipmentId;
            Id = id;
            HubId = hubId;
            UserId = userId;
            ShipmentStatusId = shipmentStatusId;
            Lat = lat;
            Lng = lng;
            Location = location;
            Note = note;
            ReasonId = reasonId;
            if (createdWhen.HasValue) CreatedWhen = createdWhen;
            else CreatedWhen = DateTime.Now;
            if (modifiedWhen.HasValue) ModifiedWhen = modifiedWhen;
            else ModifiedWhen = DateTime.Now;
            ToUserId = toUserId;
        }

        public CreateUpdateLadingScheduleViewModel(int shipmentId, int? hubId, int? toHubId, int? userId, int shipmentStatusId, double? lat, double? lng, string location, string note, int id, int? reasonId = null,DateTime? createdWhen = null, DateTime? modifiedWhen = null, int? toUserId = null)
        {
            ShipmentId = shipmentId;
            Id = id;
            HubId = hubId;
            ToHubId = toHubId;
            UserId = userId;
            ShipmentStatusId = shipmentStatusId;
            Lat = lat;
            Lng = lng;
            Location = location;
            Note = note;
            ReasonId = reasonId;
            if (createdWhen.HasValue) CreatedWhen = createdWhen;
            else CreatedWhen = DateTime.Now;
            if (modifiedWhen.HasValue) ModifiedWhen = modifiedWhen;
            else ModifiedWhen = DateTime.Now;
            ToUserId = toUserId;
        }

        public int ShipmentId { get; set; }
        public int? HubId { get; set; }
        public int? UserId { get; set; }
        public int ShipmentStatusId { get; set; }
        public double? Lat { get; set; } = 0;
        public double? Lng { get; set; } = 0;
        public string Location { get; set; }
        public string Note { get; set; }
        public int Id { get; set; }
        public int[] ServiceDVGTIds { get; set; }
        public int? StructureId { get; set; }
        public int? ReasonId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime? CreatedWhen { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedWhen { get; set; }
        public int? ModifiedBy { get; set; }
        public string ConcurrencyStamp { get; set; }
        //
        public int? ToHubId { get; set; }
        public bool IsDelay { get; set; }
        public int? DelayReasonId { get; set; }
        public string DelayNote { get; set; }
        public double? DelayTime { get; set; }
        public int? ToUserId { get; set; }
        public int? CompanyId { get; set; }
    }
}
