using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Core.Data.Core.Utils;
using Core.Entity.Entities;

namespace Core.Business.ViewModels.General
{
    public class ProvinceViewModel : SimpleViewModel<ProvinceViewModel, Province>
    {
        public ProvinceViewModel()
        {
        }

        public int CountryId { get; set; }
    }
}
