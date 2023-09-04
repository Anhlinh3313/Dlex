using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class AccountingAccountViewModel : EntitySimple
    {
        public AccountingAccountViewModel() { }
        public bool IsTransfer { set; get; }
    }
}
