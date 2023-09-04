using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Core.Data.Core.Utils;
using Core.Entity.Entities;

namespace Core.Business.ViewModels.General
{
    public class WardViewModel : SimpleViewModel<SimpleViewModel, Ward>
    {
        public WardViewModel()
        {
        }
        public bool IsRemote { get; set; }
        public int DistrictId { get; set; }
    }
}
