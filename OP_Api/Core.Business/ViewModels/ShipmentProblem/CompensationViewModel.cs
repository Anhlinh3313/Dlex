using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class CompensationViewModel : EntitySimple
    {
        public CompensationViewModel() { }
        public int ShipmentId { get; set; }
        public int? ComplainId { get; set; }
        public int? IncidentsId { get; set; }
        public string CompensationContent { get; set; }
        public string DocAliasPath { get; set; }
        public double CompensationValue { get; set; }
        public int? HandleEmpId { get; set; }
        public int? CompensationHubId { get; set; }
        public int? CompensationEmpId { get; set; }
        public double CompensationValueEmp { get; set; }
        public int? CompensationtypeId { get; set; }
        public int? FeeTypeId { get; set; }
        public bool IsCompleted { get; set; }
        public int? CreatedByEmpId { get; set; }
    }
}
