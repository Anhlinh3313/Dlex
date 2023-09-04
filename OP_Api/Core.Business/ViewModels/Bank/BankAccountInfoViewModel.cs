using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class BankAccountInfoViewModel : SimpleViewModel
    {
        public BankAccountInfoViewModel() { }
        public int BranchId { get; set; }
        public int? AccountingAccountId { get; set; }
        public virtual Branch Branch { get; set; }
        public virtual AccountingAccount AccountingAccount { get; set; }
    }
}
