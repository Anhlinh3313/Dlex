using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetListHistoryReceiptMoney : IEntityProcView
    {
        public const string ProcName = "Proc_GetListHistoryReceiptMoney";


        [Key]
        public int Id { get; set; }
        public string ReceiptMoneyCode { get; set; }
        public string DataChanged { get; set; }
        public DateTime CreatedWhen { get; set; }
        public string ModifiedName { get; set; }
        public int? ReceiptMoneyId { get; set; }
        public double? GrandTotal { get; set; }
        public double? FeeBank { get; set; }
        public double? GrandTotalReal { get; set; }
        public string AccountingAccountCode { get; set; }
        public string AccountingAccountName { get; set; }
        public DateTime? AcceptDate { get; set; }
        public string CashFlowCode { get; set; }
        public string CashFlowName { get; set; }
        public string WarningNote { get; set; }
        public double? PriceDiff { get; set; }
        public string TypeTransfer { get; set; }
        public int? TotalShipment { get; set; }
        public string CreatedName { get; set; }
        public Int64 RowNum { get; set; }
        public int TotalCount { get; set; }

        public static IEntityProc GetEntityProc(
            DateTime? fromDate,
            DateTime? toDate,
            string searchText,
            int? pageNumber,
            int? pageSize

            )
        {
            SqlParameter FromDate = new SqlParameter("FromDate", fromDate);
            if (!fromDate.HasValue)
                FromDate.Value = DBNull.Value;
            SqlParameter ToDate = new SqlParameter("ToDate", toDate);
            if (!toDate.HasValue)
                ToDate.Value = DBNull.Value;
            SqlParameter SearchText = new SqlParameter("@SearchText", searchText);
            if (string.IsNullOrEmpty(searchText))
                SearchText.Value = DBNull.Value;
            SqlParameter PageNumber = new SqlParameter("@PageNumber", pageNumber);
            if (!pageNumber.HasValue)
                PageNumber.Value = DBNull.Value;

            SqlParameter PageSize = new SqlParameter("@PageSize", pageSize);
            if (!pageSize.HasValue)
                PageSize.Value = DBNull.Value;
            return new EntityProc(
                $"{ProcName} @FromDate, @ToDate, @SearchText, @PageNumber, @PageSize ",
                new SqlParameter[]
                {
                    FromDate,
                    ToDate,
                    SearchText,
                    PageNumber,
                    PageSize
                }
            );
        }
    }
}
