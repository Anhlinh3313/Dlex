using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class Branch : EntitySimple
    {
        public Branch() { }
        public int BankId { get; set; }
        public virtual Bank Bank { get; set; }
    }
}
