using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class AreaGroupViewModel : SimpleViewModel<AreaGroupViewModel, AreaGroup>
    {
        public AreaGroupViewModel() { }
        public int HubId { set; get; }
        public bool IsAuto { get; set; }
    }
}
