using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Core.Data.Core.Utils;
using Core.Entity.Entities;

namespace Core.Business.ViewModels.General
{
    public class ProvinceInfoViewModel : SimpleViewModel
    {
        public ProvinceInfoViewModel()
        {
        }

        public int CountryId { get; set; }
        public CountryViewModel Country { get; set; }
    }
}
