using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Business.ViewModels
{
    public class ChargedCODViewModel : IEntityBase
    {
        public ChargedCODViewModel() { }

        public int Id { get; set; }
        public int[] DistrictIds { get; set; }
        public bool IsEnabled { get; set; }
        public int? CompanyId { get; set; }

    }
}
