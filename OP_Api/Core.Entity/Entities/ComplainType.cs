using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class ComplainType : EntitySimple
    {
        public ComplainType() { }
        public int? RoleId { get; set; }
        public bool IsPublish { get; set; }

        public virtual Role Role { get; set; }
    }
}
