using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_ReportTransfer :  IEntityProcView
    {
        public const string ProcName = "Proc_ReportTransfer";

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int TotalListGoods { get; set; }
        public int TotalListGoodsSend { get; set; }
        public int TotalListGoodsSendWaiting { get; set; }
        public int TotalListGoodsSendTransfering { get; set; }
        public int TotalListGoodsSendComplete { get; set; }
        public int TotalListGoodsSendCancel { get; set; }
        public int TotalListGoodsSendTo { get; set; }
        public int TotalListGoodsSendToWaiting { get; set; }
        public int TotalListGoodsSendToTransfering { get; set; }
        public int TotalListGoodsSendToComplete { get; set; }
        public int TotalListGoodsSendToCancel { get; set; }
        public int TotalListGoodsReceived { get; set; }

        public Proc_ReportTransfer() { }

        public static IEntityProc GetEntityProc(bool? isAllowChild = false, int? hubId = null, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            SqlParameter parameter1 = new SqlParameter(
           "@IsAllChild", isAllowChild);
            if (!isAllowChild.HasValue)
                parameter1.Value = DBNull.Value;
            SqlParameter parameter2 = new SqlParameter(
            "@HubId", hubId);
            if (!hubId.HasValue)
                parameter2.Value = DBNull.Value;
            SqlParameter parameter3 = new SqlParameter(
            "@DateFrom", dateFrom);
            if (!dateFrom.HasValue)
                parameter3.Value = DBNull.Value;
            SqlParameter parameter4 = new SqlParameter(
            "@DateTo", dateTo);
            if (!dateTo.HasValue)
                parameter4.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @IsAllChild, @HubId, @DateFrom, @DateTo",
                new SqlParameter[] {
                parameter1,
                parameter2,
                parameter3,
                parameter4
                }
            );
        }
    }
}
