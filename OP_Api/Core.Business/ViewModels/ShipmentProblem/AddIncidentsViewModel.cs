using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class AddIncidentsViewModel : EntitySimple
    {
        public AddIncidentsViewModel() { }

        public string ShipmentNumber { get; set; }
        public int ShipmentId { get; set; }
        public int LadingScheduleId { get; set; }
        public int IncidentsReasonId { get; set; }
        public string IncidentsContent { get; set; }
        public int? IncidentsByHubId { get; set; }
        public int? IncidentsByEmpId { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsCompensation { get; set; }
        public string HandleContent { get; set; }
        public int? HandleByEmpId { get; set; }
        public int CreatedByEmpId { get; set; }
    }
}
