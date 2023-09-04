using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entity.Entities
{
    public class Compensation : EntitySimple
    {
        public Compensation() { }

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
        //
        [ForeignKey("HandleEmpId")]
        public User HandleEmp { get; set; }
        [ForeignKey("CompensationHubId")]
        public Hub CompensatoinHub { get; set; }
        [ForeignKey("CompensationEmpId")]
        public User CompensationEmp { get; set; }
        public CompensationType CompensationType { get; set; }
        public FeeType FeeType { get; set; }
        [ForeignKey("CreatedByEmpId")]
        public User CreatedByEmp { get; set; }
    }
}
