using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class CusDepartmentViewModel : EntitySimple
    {
        public CusDepartmentViewModel() { }

        public int CustomerId { get; set; }
    }
}
