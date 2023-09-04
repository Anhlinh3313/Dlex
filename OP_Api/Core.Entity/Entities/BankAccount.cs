using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class BankAccount : EntitySimple
    {
        public BankAccount() { }
        public int? BranchId { get; set; }
        public int? AccountingAccountId { get; set; }
        public virtual AccountingAccount AccountingAccount { get; set; }
        public virtual Branch Branch { get; set; }
    }
}
