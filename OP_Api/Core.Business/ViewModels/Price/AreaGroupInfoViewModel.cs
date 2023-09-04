using Core.Business.ViewModels.General;
using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class AreaGroupInfoViewModel : SimpleViewModel
    {
        public AreaGroupInfoViewModel() { }

        public int HubId { get; set; }
        public bool IsAuto { get; set; }
        public HubViewModel Hub { get; set; }
    }
}
