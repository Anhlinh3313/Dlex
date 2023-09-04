using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class ComplainTypeInfoViewModel : SimpleViewModel
    {
        public ComplainTypeInfoViewModel() { }

        public int? RoleId { get; set; }

        public virtual Role Role { get; set; }
    }
}
