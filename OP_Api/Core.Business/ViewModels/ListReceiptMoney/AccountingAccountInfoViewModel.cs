using Core.Data.Core.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Core.Business.ViewModels
{
    public class AccountingAccountInfoViewModel : SimpleViewModel
    {
        public AccountingAccountInfoViewModel() { }
        public bool IsTransfer { get; set; }
    }
}
