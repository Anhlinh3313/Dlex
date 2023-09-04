using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;


namespace Core.Entity.Procedures
{
    public class Proc_ReportListReceiptAccepted : IEntityProcView
    {
        public const string ProcName = "Proc_ReportListReceiptAccepted";
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime? CreatedWhen { get; set; }
        public string UserCreatedName { get; set; }
        public DateTime? FirstLockDate { get; set; }
        public string TypeTransfer { get; set; }
        public string AccountingAccountCode { get; set; }
        public double? GrandTotal { get; set; }
        public double? GrandTotalReal { get; set; }
        public double? FeeBank { get; set; }
        public double? Imparity { get; set; }
        public DateTime? AcceptDate { get; set; }
        public string UserAcceptName { get; set; }
        public DateTime? ModifiedWhen { get; set; }
        public double? SumGrandTotal { get; set; }
        public double? SumGrandTotalReal { get; set; }
        public double? SumImparity { get; set; }
        public double? SumFeeBank { get; set; }
        public int TotalCount { get; set; }

        public Proc_ReportListReceiptAccepted() { }
        public static IEntityProc GetEntityProc(DateTime? fromDate, DateTime? toDate, int? hubId, int? accountingAcountId, int? pageNum, int? pageSize)
        {
            SqlParameter DateFrom = new SqlParameter("@DateFrom", fromDate);
            if (!fromDate.HasValue) DateFrom.Value = DBNull.Value;

            SqlParameter DateTo = new SqlParameter("@DateTo", toDate);
            if (!toDate.HasValue) DateTo.Value = DBNull.Value;

            SqlParameter HubId = new SqlParameter("@HubId", hubId);
            if (!hubId.HasValue) HubId.Value = DBNull.Value;

            SqlParameter AccountingAcountId = new SqlParameter("@AccountingAcountId", accountingAcountId);
            if (!accountingAcountId.HasValue) AccountingAcountId.Value = DBNull.Value;

            SqlParameter PageNumber = new SqlParameter("@PageNumber", pageNum);
            if (!pageNum.HasValue) PageNumber.Value = 1;

            SqlParameter PageSize = new SqlParameter("@PageSize", pageSize);
            if (!pageSize.HasValue) PageSize.Value = 20;

            return new EntityProc(
                $"{ProcName} @DateFrom,@DateTo,@HubId,@AccountingAcountId,@PageNumber,@PageSize",
                new SqlParameter[] {
                    DateFrom,DateTo,HubId,AccountingAcountId,PageNumber,PageSize
                }
            );
        }
    }
}
