using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels.KPIShipments
{
    public class KPIShipmentViewModel : EntitySimple
    {
        public bool IsPublic { get; set; }
        public int Priority { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int ServiceId { get; set; }
    }
}
