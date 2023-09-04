using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entity.Entities
{
    public class Incidents : EntitySimple
    { 
        public Incidents() { }
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

        public Reason IncidentsReason { get; set; }
        [ForeignKey("IncidentsByHubId")]
        public Hub IncidentsByHub { get; set; }
        [ForeignKey("IncidentsByEmpId")]
        public User IncidentsByEmp { get; set; }
        [ForeignKey("HandleByEmpId")]
        public User HandleByEmp { get; set; }
        [ForeignKey("CreatedByEmpId")]
        public User CreatedByEmp { get; set; }
    }
}
