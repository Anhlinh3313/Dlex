using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class TruckScheduleImage : IEntityBase
    {
        public TruckScheduleImage() { }

        public int Id { get; set; }
        public DateTime? CreatedWhen { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedWhen { get; set; }
        public int? ModifiedBy { get; set; }
        public bool IsEnabled { get; set; } = true;
        public string ImagePath { get; set; }
        public int TruckScheduleDetailId { get; set; }
        public int? CompanyId { get; set; }
    }
}
