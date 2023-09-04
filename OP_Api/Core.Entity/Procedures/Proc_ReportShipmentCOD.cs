using System;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_ReportShipmentCOD : IEntityProcView
    {
        public const string ProcName = "Proc_ReportShipmentCOD";

        public int Id { get; set; }
        public Int64 RowNum { get; set; }
        public int TotalCount { get; set; }
        public string ShipmentNumber { get; set; }
        public DateTime? OrderDate { get; set; }
        public string FromHubName { get; set; }
        public string SenderName { get; set; }
        public string CustomerCode { get; set; }
        public string FromProvinceName { get; set; }
        public string ToProvinceName { get; set; }
        public double COD { get; set; }
        public bool? IsReturn { get; set; }
        public string ShipmentNumberRelation { get; set; }
        public string RealRecipientName { get; set; }
        public string Content { get; set; }
        public string ToHubName { get; set; }
        public string BlockReceiptCODCode { get; set; }
        public string AcceptReceiptCODCode { get; set; }
        public string CreateListPaid { get; set; }
        public string CompletedListPaid { get; set; }
        public DateTime? EndDeliveryTime { get; set; }
        public string DeliveredEmpName { get; set; }

        public Proc_ReportShipmentCOD()
        {
        }

        public static IEntityProc GetEntityProc(DateTime? fromDate = null, DateTime? toDate = null, int? isReturn = null, int? tohubId = null, int? empId = null, int? pageNumber = 1, int? pageSize = 20)
        {

            SqlParameter DateFrom = new SqlParameter("@DateFrom", fromDate);
            if (!fromDate.HasValue)
                DateFrom.Value = DBNull.Value;

            SqlParameter DateTo = new SqlParameter("@DateTo", toDate);
            if (!toDate.HasValue)
                DateTo.Value = DBNull.Value;

            SqlParameter IsReturn = new SqlParameter("@IsReturn", isReturn);
            if (!isReturn.HasValue)
                IsReturn.Value = DBNull.Value;

            SqlParameter TohubId = new SqlParameter("@TohubId", tohubId);
            if (!tohubId.HasValue)
                TohubId.Value = DBNull.Value;

            SqlParameter EmpId = new SqlParameter("@EmpId", empId);
            if (!empId.HasValue)
                EmpId.Value = DBNull.Value;

            SqlParameter PageNumber = new SqlParameter("@PageNum", pageNumber);
            if (!pageNumber.HasValue)
                PageNumber.Value = DBNull.Value;

            SqlParameter PageSize = new SqlParameter("@PageSize", pageSize);
            if (!pageSize.HasValue)
                PageSize.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @DateFrom, @DateTo, @IsReturn, @TohubId, @EmpId, @PageNum, @PageSize",
                new SqlParameter[] {
                DateFrom,
                DateTo,
                IsReturn,
                TohubId,
                EmpId,
                PageNumber,
                PageSize
                }
            );
        }
    }
}

