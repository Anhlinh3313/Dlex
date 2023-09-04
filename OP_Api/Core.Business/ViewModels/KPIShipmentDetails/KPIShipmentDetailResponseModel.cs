using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels.KPIShipmentDetails
{
    public class KPIShipmentDetailResponseModel
    {
        public int Time { get; set; }
        public int RealTime { get; set; }
        public DateTime DeadlineDelivery { get; set; }
        public double TimeCOD { get; set; }
    }
}
