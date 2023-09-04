using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entity.Entities
{
    public class ComplainHandle : EntitySimple
    {
        public ComplainHandle() { }

        public int ComplainId { get; set; }
        public int ComplainStatusId { get; set; }
        public string HandleContent { get; set; }
        public string HandleImagePath { get; set; }
        public int? HandleEmpId { get; set; }
        public int? HandleHubId { get; set; }
        //
        [ForeignKey("ComplainStatusId")]
        public virtual ComplainStatus ComplainStatus { get; set; }
        [ForeignKey("HandleEmpId")]
        public virtual User HandleEmp { get; set; }
        [ForeignKey("HandleHubId")]
        public virtual Hub HandleHub { get; set; }
    }
}
