using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels
{
    public class ShipmentListPrintViewModel
    {
        public ShipmentListPrintViewModel() {  }

        public List<int> ShipmentIds { get; set; }
        public string cols { get; set; }
    }
}
