using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class AreaInfoViewModel : SimpleViewModel
    {
        public AreaInfoViewModel() { }

        public int? AreaGroupId { set; get; }
        public bool IsAuto { get; set; }
        public double ExpectedTime { get; set; }
        public AreaGroupViewModel AreaGroup { set; get; }
    }
}
