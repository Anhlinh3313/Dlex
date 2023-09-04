using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entity.Entities
{
    public class ProvideCode : EntitySimple
    {
        public ProvideCode() { }

        public int ProvideCodeStatusId { get; set; }
        public int? ProvideHubId { get; set; }
        public int? ProvideUserId { get; set; }
        public int? ProvideCustomerId { get; set; }

        [ForeignKey("ProvideCodeStatusId")]
        public virtual ProvideCodeStatus ProvideCodeStatus { get; set; }

        [ForeignKey("ProvideHubId")]
        public virtual Hub Hub { get; set; }

        [ForeignKey("ProvideUserId")]
        public virtual User User { get; set; }

        [ForeignKey("ProvideCustomerId")]
        public virtual Customer Customer { get; set; }
    }
}
