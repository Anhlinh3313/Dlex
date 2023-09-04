using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class AddDelayViewModel
    {
        public AddDelayViewModel() { }

        public string ShipmentNumber { get; set; }
        public string ListGoodsCode { get; set; }
        public int DelayReasonId { get; set; }
        public string DelayNote { get; set; }
        public double DelayTime { get; set; }
    }
}
