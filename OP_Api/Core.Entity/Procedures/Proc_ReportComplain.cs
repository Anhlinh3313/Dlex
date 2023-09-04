using System;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_ReportComplain : IEntityProcView
    {
        public const string ProcName = "Proc_ReportComplain";

        public int Id { get; set; }
        public int? TotalCount { get; set; }
        public Int64? RowNum { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public DateTime? OrderDate { get; set; }
        public string ShipmentNumber { get; set; }
        public string FromProvinceName { get; set; }
        public string ToProvinceName { get; set; }
        public string ShippingAddress { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public double? Weight { get; set; }
        public double? CalWeight { get; set; }
        public double? COD { get; set; }
        public double? DefaultPrice { get; set; }
        public double? TotalDVGT { get; set; }
        public double? PriceCOD { get; set; }
        public double? TotalPriceAll { get; set; }
        public string ComplainTypeName { get; set; }
        public string ComplainContent { get; set; }
        public string ComplainStatusName { get; set; }
        public bool IsCompensation { get; set; }
        public string HandleEmpFullName { get; set; }
        public DateTime? EndDate { get; set; }
        public double? CompensationValue { get; set; }



        public Proc_ReportComplain()
        {
        }

        public static IEntityProc GetEntityProc(DateTime? dateFrom = null, DateTime? dateTo = null, int? pageNumber = null, int? pageSize = null, int? salerId = null, int? handleEmpId = null, bool? isCompensation = null)
        {
            SqlParameter DateFrom = new SqlParameter("@DateFrom", dateFrom);
            if (!dateFrom.HasValue)
                DateFrom.Value = DBNull.Value;

            SqlParameter DateTo = new SqlParameter("@DateTo", dateTo);
            if (!dateTo.HasValue)
                DateTo.Value = DBNull.Value;

            SqlParameter PageNumber = new SqlParameter("@PageNumber", pageNumber);
            if (!pageNumber.HasValue)
                PageNumber.Value = DBNull.Value;

            SqlParameter PageSize = new SqlParameter("@PageSize", pageSize);
            if (!pageSize.HasValue)
                PageSize.Value = DBNull.Value;

            SqlParameter SalerId = new SqlParameter("@SalerId", salerId);
            if (!salerId.HasValue)
                SalerId.Value = DBNull.Value;

            SqlParameter HandleEmpId = new SqlParameter("@HandleEmpId", handleEmpId);
            if (!handleEmpId.HasValue)
                HandleEmpId.Value = DBNull.Value;

            SqlParameter IsCompensation = new SqlParameter("@IsCompensation", isCompensation);
            if (!isCompensation.HasValue)
                IsCompensation.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @DateFrom, @DateTo, @PageNumber, @PageSize, @SalerId, @HandleEmpId, @IsCompensation",
                new SqlParameter[] {
                DateFrom,
                DateTo,
                PageNumber,
                PageSize,
                SalerId,
                HandleEmpId,
                IsCompensation
                }
            );
        }
    }
}