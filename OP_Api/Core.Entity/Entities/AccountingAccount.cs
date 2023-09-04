using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class AccountingAccount : EntitySimple
    {
        public AccountingAccount() { }
        public bool IsTransfer { set; get; }
    }
}
