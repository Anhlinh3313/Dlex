using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Core.Data.Core.Utils;

namespace Core.Business.ViewModels.General
{
    public class WardInfoViewModel : SimpleViewModel
    {
        public WardInfoViewModel()
        {
        }

        public int DistrictId { get; set; }
        public DistrictInfoViewModel District { get; set; }
    }
}
