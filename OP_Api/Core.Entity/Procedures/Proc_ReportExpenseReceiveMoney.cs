using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_ReportExpenseReceiveMoney : IEntityProcView
    {
        public const string ProcName = "Proc_ReportExpenseReceiveMoney";

        [Key]
        public int Id { get; set; }
        public DateTime AcceptDate { get; set; }
        public string AccountingAccountCode { get; set; }
        public string AccountingAccountName { get; set; }
        public string Note { get; set; }
        public double? MoneyDifference { get; set; }
        public double? FeeBank { get; set; }
        public double? GrandTotal { get; set; }
        public double? GrandTotalReal { get; set; }
        public string FullName { get; set; }
        public string Code { get; set; }
        public Int64? RowNum { get; set; }
        public double SumMoneyDifference { get; set; }
        public double SumMoneyFeeBank { get; set; }
        public double SumGrandTotal { get; set; }
        public double SumGrandTotalReal { get; set; }
        public int? TotalCount { get; set; }

        public Proc_ReportExpenseReceiveMoney()
        {
        }

        public static IEntityProc GetEntityProc(
            DateTime? dateFrom = null,
            DateTime? dateTo = null, 
            int? hubId = null,
            int? userId = null,
            int? accountingAccountId = null,
            int? pageNumber = null,
            int? pageSize = null
        )
        {
            SqlParameter DateFrom = new SqlParameter("@DateFrom", dateFrom);
            if (!dateFrom.HasValue)
                DateFrom.Value = DBNull.Value;

            SqlParameter DateTo = new SqlParameter("@DateTo", dateTo);
            if (!dateTo.HasValue)
                DateTo.Value = DBNull.Value;

            SqlParameter HubId = new SqlParameter("@HubId", hubId);
            if (!hubId.HasValue)
                HubId.Value = DBNull.Value;

            SqlParameter UserId = new SqlParameter("@UserId", userId);
            if (!userId.HasValue)
                UserId.Value = DBNull.Value;

            SqlParameter AccountingAccountId = new SqlParameter("@AccountingAccountId", accountingAccountId);
            if (!accountingAccountId.HasValue)
                AccountingAccountId.Value = DBNull.Value;

            SqlParameter PageNumber = new SqlParameter("@PageNumber", pageNumber);
            if (!pageNumber.HasValue)
                PageNumber.Value = DBNull.Value;

            SqlParameter PageSize = new SqlParameter("@PageSize", pageSize);
            if (!pageSize.HasValue)
                PageSize.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @DateFrom,@DateTo,@HubId,@AccountingAccountId,@UserId,@PageNumber,@PageSize",
                new SqlParameter[] {
                    DateFrom,
                    DateTo,
                    HubId,
                    UserId,
                    AccountingAccountId,
                    PageNumber,
                    PageSize,
                }
            );
        }
    }
}
