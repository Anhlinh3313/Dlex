using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entity.Entities
{
    public class UploadExcelHistory : EntitySimple
    {
        public UploadExcelHistory()
        {

        }

        public int UserId { get; set; }
        public int? TotalCreated { get; set; }
        public int? TotalUpdated { get; set; }
        
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
