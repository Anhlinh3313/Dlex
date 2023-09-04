using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_AddDelay : IEntityProcView
    {
        public const string ProcName = "Proc_AddDelay";
        [Key]
        public int IsSuccess { get; set; }
        public int DataCount { get; set; }
        public string Message { get; set; }

        public Proc_AddDelay() { }
        public static IEntityProc GetEntityProc(string shipmentNumber, string listGoodsCode, int delayReasonId, string delayNote, double delayTime, int userId)
        {
            SqlParameter sqlParameter1 = new SqlParameter("@ShipmentNumber", shipmentNumber);
            if (string.IsNullOrWhiteSpace(shipmentNumber)) sqlParameter1.Value = DBNull.Value;
            SqlParameter sqlParameter2 = new SqlParameter("@ListGoodsCode", listGoodsCode);
            if (string.IsNullOrWhiteSpace(listGoodsCode)) sqlParameter2.Value = DBNull.Value;
            SqlParameter sqlParameter3 = new SqlParameter("@DelayReasonId", delayReasonId);
            SqlParameter sqlParameter4 = new SqlParameter("@DelayNote", delayNote);
            if (string.IsNullOrWhiteSpace(delayNote)) sqlParameter4.Value = DBNull.Value;
            SqlParameter sqlParameter5 = new SqlParameter("@DelayTime", delayTime);
            SqlParameter sqlParameter6 = new SqlParameter("@UserId", userId);

            return new EntityProc(
                $"{ProcName} @ShipmentNumber,@ListGoodsCode,@DelayReasonId,@DelayNote,@DelayTime,@UserId",
                new SqlParameter[] {
                    sqlParameter1,
                    sqlParameter2,
                    sqlParameter3,
                    sqlParameter4,
                    sqlParameter5,
                    sqlParameter6
                }
            );
        }
    }
}
