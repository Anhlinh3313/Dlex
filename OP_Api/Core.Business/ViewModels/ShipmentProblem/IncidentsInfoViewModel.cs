using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class IncidentsInfoViewModel : EntitySimple
    {
        public IncidentsInfoViewModel() { }
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
        //
        public Reason IncidentsReason { get; set; }
        public Hub IncidentsByHub { get; set; }
        public UserInfoViewModel IncidentsByEmp { get; set; }
        public UserInfoViewModel HandleByEmp { get; set; }
        public UserInfoViewModel CreatedByEmp { get; set; }
    }
}
