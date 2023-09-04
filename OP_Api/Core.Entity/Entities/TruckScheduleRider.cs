using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class TruckScheduleRider : IEntityBase
    {
        public TruckScheduleRider() { }
        public int Id { get; set; }
        public int TruckScheduleId { get; set; }
        public int RiderId { get; set; }
        public TruckSchedule TruckSchedule { get; set; }
        public User Rider { get; set; }
        public bool IsEnabled { get; set; }
        public int? CompanyId { get; set; }
    }
}
