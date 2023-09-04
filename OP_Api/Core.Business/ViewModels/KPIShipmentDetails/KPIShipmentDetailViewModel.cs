using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Business.ViewModels.SetUpKPIDeliverys
{
    public class KPIShipmentDetailViewModel : EntitySimple
    {
        public int WardId { get; set; }
        public int DistrictId { get; set; }
        public string Vehicle { get; set; }
        public int TargetDeliveryTime { get; set; }
        public int KPIShipmentId { get; set; }
        public int? TargetPaymentCOD { get; set; }
    }
}
