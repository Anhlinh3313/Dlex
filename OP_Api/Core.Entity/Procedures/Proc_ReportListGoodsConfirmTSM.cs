using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;


namespace Core.Entity.Procedures
{
    public class Proc_ReportListGoodsConfirmTSM : IEntityProcView
    {
        public const string ProcName = "Proc_ReportListGoodsConfirmTSM";

        [Key]
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public string Code { get; set; }
        public string CreatedByName { get; set; }
        public double? GrandTotal { get; set; }
        public double? GrandTotalReal { get; set; }
        public double? FeeBank { get; set; }
        public string AccountingAccount { get; set; }
        public DateTime? AcceptDate { get; set; }
        public DateTime? ModifiedWhen { get; set; }
        public double? MoneyDifference { get; set; }
        public string UserAcceptName { get; set; }
        public int? TotalShipment { get; set; }
        public string PaymentTypeName { get; set; }
        public DateTime? FirstLockDate { get; set; }
        public DateTime? LockDate { get; set; }
        public Int64 RowNum { get; set; }
        public int? TotalCount { get; set; }

        public Proc_ReportListGoodsConfirmTSM()
        {
        }

        public static IEntityProc GetEntityProc(
            int? fromHubId = null,
            int? toHubId = null,
            int? currentUserId = null,
            int? accountingAccountId = null,
            DateTime? dateFrom = null,
            DateTime? dateTo = null,
            int? pageNumber = 1,
            int? pageSize = 20,
            string searchText = ""
        )
        {
            SqlParameter FromHubId = new SqlParameter("@FromHubId", fromHubId);
            if (!fromHubId.HasValue)
                FromHubId.Value = DBNull.Value;

            SqlParameter ToHubId = new SqlParameter("@ToHubId", toHubId);
            if (!toHubId.HasValue)
                ToHubId.Value = DBNull.Value;

            SqlParameter CurrentUserId = new SqlParameter("@CurrentUserId", currentUserId);
            if (!currentUserId.HasValue)
                CurrentUserId.Value = DBNull.Value;

            SqlParameter AccountingAccountId = new SqlParameter("@AccountingAccountId", accountingAccountId);
            if (!accountingAccountId.HasValue)
                AccountingAccountId.Value = DBNull.Value;

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

            SqlParameter SearchText = new SqlParameter("@SearchText", searchText);
            if (string.IsNullOrWhiteSpace(searchText))
                SearchText.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @FromHubId, @ToHubId, @CurrentUserId, @AccountingAccountId, @DateFrom, @DateTo, @PageNumber, @PageSize, @SearchText",
                new SqlParameter[] {
                    FromHubId,
                    ToHubId, 
                    CurrentUserId, 
                    AccountingAccountId,
                    DateFrom, 
                    DateTo, 
                    PageNumber, 
                    PageSize, 
                    SearchText
                }
            );
        }
    }
}