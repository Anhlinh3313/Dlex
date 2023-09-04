using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;
namespace Core.Entity.Procedures
{
    public class Proc_ReportRevenueCustomer : IEntityProcView
    {
        public const string ProcName = "Proc_ReportRevenueCustomer";


        [Key]
        public int Id { get; set; }
        public string ShipmentNumber { get; set; }
        public string ReShipmentNumber { get; set; }
        public DateTime? OrderDate { get; set; }
        public string SenderCode { get; set; }
        public string SenderName { get; set; }
        public string AddressNoteFrom { get; set; }
        public string PickingAddress { get; set; }
        public string FromWardName { get; set; }
        public string FromDistrictName { get; set; }
        public string FromProvinceName { get; set; }
        public string FromHubRoutingName { get; set; }
        public string ReceiverCode { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public string AddressNoteTo { get; set; }
        public string ShippingAddress { get; set; }
        public string ToWardName { get; set; }
        public string ToProvinceName { get; set; }
        public string ToHubName { get; set; }
        public string ToHubRoutingName { get; set; }
        public string ConnectCode { get; set; }
        public double? COD { get; set; }
        public double? Insured { get; set; }
        public double? Weight { get; set; }
        public double? CalWeight { get; set; }
        public double? CusWeight { get; set; }
        public string ServiceName { get; set; }
        public string ServiceDVGTCodes { get; set; }
        public int TotalBox { get; set; }
        public int TotalItem { get; set; }
        public string StructureName { get; set; }
        public double? DefaultPrice { get; set; }
        public double? RemoteAreasPrice { get; set; }
        public double? PriceReturn { get; set; }
        public double? TotalDVGT { get; set; }
        public double? TotalPrice { get; set; }
        public double? TotalPriceSYS { get; set; }
        public string PaymentTypeName { get; set; }
        public double? PriceCOD { get; set; }
        public string PaymentCODTypeName { get; set; }
        public string Content { get; set; }
        public string CusNote { get; set; }
        public string Note { get; set; }
        public bool? IsReturn { get; set; }
        public string ShipmentStatusName { get; set; }
        public string RealRecipientName { get; set; }
        public DateTime? EndDeliveryTime { get; set; }
        public string DeliveryNote { get; set; }
        public bool? ISPrioritize { get; set; }
        public bool? IsIncidents { get; set; }
        public int? SenderId { get; set; }
        public string ReasonName { get; set; }
        public string ToDistrictName { get; set; }
        public int? KmNumberTW { get; set; }
        public int? KmNumberTD { get; set; }
        public string CompanyNameFrom { get; set; }
        public double VATPrice { get; set; }
        public double FuelPrice { get; set; }
        public string PackageCode { get; set; }
        public int? PackageId { get; set; }
        public string UserCode { get; set; }
        public string FullName { get; set; }
        public DateTime? EndReturnTime { get; set; }
        public DateTime? EndPickTime { get; set; }
        public int KmNumber { get; set; }
        public string ReceiverCode2 { get; set; }
        public string ReceiptCODCode { get; set; }
        public string ReceiptPriceCode { get; set; }
        public int? TimeCompare { get; set; }
        public double? TotalPricePay { get; set; }
        public double? Distance { get; set; }
        public string ReturnReason { get; set; }
        public DateTime? DeliveryWhenOne { get; set; }
        public string DeliveryReasonOne { get; set; }
        public DateTime? DeliveryWhenTwo { get; set; }
        public string DeliveryReasonTwo { get; set; }
        public DateTime? DeliveryWhenThree { get; set; }
        public string DeliveryReasonThree { get; set; }
        public DateTime? ReceiptCODCreatedWhen { get; set; }
        public int? CountImageDelivery { get; set; }
        public string AllTimeCompareString { get; set; }
        public string TimeCompareString { get; set; }
        public DateTime? InOutDate { get; set; }
        public DateTime? OrderDate2 { get; set; }
        public DateTime? DeliveryAppointmentTime { get; set; }
        public Int64? RowNum { get; set; }
        public int? TotalCount { get; set; }
        public int? TotalShipmentInPackage { get; set; }
        public int? TotalPackage { get; set; }
        public int? TotalBoxs { get; set; }
        public double? SumInsured { get; set; }
        public double? SumCOD { get; set; }
        public double? SumPriceCOD { get; set; }
        public double? SumTotalPrice { get; set; }
        public double? SumTotalPricePay { get; set; }
        public double? SumWeight { get; set; }
        //public string TimeCompareEndPickupTime { get; set; }
        //public string TimeCompareEndDeliveryTime { get; set; }
        //public DateTime? DeadlineDeliveryTime { get; set; }
        //public DateTime? TimeCompareReceiptMoney { get; set; }

        public Proc_ReportRevenueCustomer()
        {
        }

        public static IEntityProc GetEntityProc(
            DateTime? fromDate = null,
            DateTime? toDate = null,
            int? senderId = null,
            int? pageNumber = null,
            int? pageSize = null)
        {

            SqlParameter DateFrom = new SqlParameter("@DateFrom", fromDate);
            if (!fromDate.HasValue)
                DateFrom.Value = DBNull.Value;

            SqlParameter DateTo = new SqlParameter("@DateTo", toDate);
            if (!toDate.HasValue)
                DateTo.Value = DBNull.Value;

            SqlParameter SenderId = new SqlParameter("@SenderId", senderId);
            if (!senderId.HasValue)
                SenderId.Value = DBNull.Value;

            SqlParameter PageNumber = new SqlParameter("@PageNumber", pageNumber);
            if (!pageNumber.HasValue)
                PageNumber.Value = DBNull.Value;

            SqlParameter PageSize = new SqlParameter("@PageSize", pageSize);
            if (!pageSize.HasValue)
                PageSize.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @DateFrom, @DateTo, @SenderId,@PageNumber, @PageSize",
                new SqlParameter[] {
                DateFrom, DateTo, SenderId, PageNumber, PageSize },
                ProcName
            );
        }


    }
}
