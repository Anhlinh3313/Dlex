using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class BranchInfoViewModel : SimpleViewModel
    {
        public BranchInfoViewModel() { }

        public int BankId { get; set; }
        public virtual Bank Bank { get; set; }
    }
}
