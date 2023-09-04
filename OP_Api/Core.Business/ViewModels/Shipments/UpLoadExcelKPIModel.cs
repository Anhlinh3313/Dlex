namespace Core.Entity.Entities
{
    public class UpLoadExcelKPIModel
    {
        public UpLoadExcelKPIModel() { }

        public string HubRoutingCode { get; set; }
        public int? CutOffTimeId { get; set; }
        public string CutOffTimeCode { get; set; }
        public double? KPIFullLading { get; set; }
        public double? KPIExportSAP { get; set; }
        public double? StartTransferTime { get; set; }
        public double? KPITransfer { get; set; }
        public double? KPIStartDeliveryTime { get; set; }
        public double? KPIDelivery { get; set; }
        public double? KPIPaymentMoney { get; set; }
        public double? KPIConfirmPaymentMoney { get; set; }
        public bool? IsAllowOverDayKPIStartDeliv { get; set; }
        public bool? IsAllowOverDayKPIPaymentMoney { get; set; }

    }
}