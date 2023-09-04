using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels.KPIShipmentCuss
{
    public class KPIShipmentCusViewModel
    {
        public int Id { get; set; }
        public bool IsEnabled { get; set; }
        public int CusId { get; set; }
        public int KPIShipmentId { get; set; }
    }
}
