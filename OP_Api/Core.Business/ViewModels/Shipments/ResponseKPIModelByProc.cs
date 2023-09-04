using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity.Entities
{
    public class ResponseKPIModelByProc
    {
        public ResponseKPIModelByProc() { }

        public DateTime? COT { get; set; }
        public DateTime? KPIFullLading { get; set; }
        public DateTime? KPIFullLadingDay { get; set; }
        public DateTime? KPIExportSAP { get; set; }
        public DateTime? StartTransferTime { get; set; }
        public DateTime? KPITransfer { get; set; }
        public DateTime? StartDeliveryTime { get; set; }
        public DateTime? KPIDelivery { get; set; }
        public DateTime? KPIPaymentMoney { get; set; }
        public DateTime? KPIConfirmPaymentMoney { get; set; }

    }
}
