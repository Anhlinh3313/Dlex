using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetListHistoryReceiptMoneyDetail : IEntityProcView
    {
        public const string ProcName = "Proc_GetListHistoryReceiptMoneyDetail";


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
        public Int64 RowNum { get; set; }

        public static IEntityProc GetEntityProc(
            int? listReceiptMoneyId,
            int? id

            )
        {
            SqlParameter ListReceiptMoneyId = new SqlParameter("@ListReceiptMoneyId", listReceiptMoneyId);
            if (!listReceiptMoneyId.HasValue)
                ListReceiptMoneyId.Value = DBNull.Value;

            SqlParameter Id = new SqlParameter("@Id", id);
            if (!id.HasValue)
                Id.Value = DBNull.Value;
            return new EntityProc(
                $"{ProcName} @ListReceiptMoneyId, @Id ",
                new SqlParameter[]
                {
                    ListReceiptMoneyId,
                    Id
                }
            );
        }
    }
}
