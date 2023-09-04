using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_LogSendEmail : IEntityProcView
    {
        public const string ProcName = "Proc_LogSendEmail";
        [Key]
        public bool IsSuccess { get; set; }

        public Proc_LogSendEmail() { }
        public static IEntityProc GetEntityProc(int userId, string emailAddress, int? shipmentId, int? ladingScheduleId, int? complainId,
            int? incidentsId, bool isDelivered, bool isReturn, bool isSuccess, string message)
        {
            SqlParameter sqlParameter1 = new SqlParameter("@UserId", userId);
            SqlParameter sqlParameter2 = new SqlParameter("@EmailAddress", emailAddress);
            SqlParameter sqlParameter3 = new SqlParameter("@ShipmentId", shipmentId);
            if (!shipmentId.HasValue) sqlParameter3.Value = DBNull.Value;
             SqlParameter sqlParameter4 = new SqlParameter("@LadingScheduleId", ladingScheduleId);
            if (!ladingScheduleId.HasValue) sqlParameter4.Value = DBNull.Value;
            SqlParameter sqlParameter5 = new SqlParameter("@ComplainId", complainId);
            if (!complainId.HasValue) sqlParameter5.Value = DBNull.Value;
            SqlParameter sqlParameter6 = new SqlParameter("@IncidentsId", incidentsId);
            if (!incidentsId.HasValue) sqlParameter6.Value = DBNull.Value;
            SqlParameter sqlParameter7 = new SqlParameter("@IsDelivered", isDelivered);
            SqlParameter sqlParameter8 = new SqlParameter("@IsReturn", isReturn);
            SqlParameter sqlParameter9 = new SqlParameter("@IsSuccess", isSuccess);
            SqlParameter sqlParameter10 = new SqlParameter("@Message", message);
            return new EntityProc(
                $"{ProcName} @UserId,@EmailAddress,@ShipmentId,@LadingScheduleId,@ComplainId,@IncidentsId,@IsDelivered,@IsReturn,@IsSuccess,@Message",
                new SqlParameter[] {
                    sqlParameter1,
                    sqlParameter2,
                    sqlParameter3,
                    sqlParameter4,
                    sqlParameter5,
                    sqlParameter6,
                    sqlParameter7,
                    sqlParameter8,
                    sqlParameter9,
                    sqlParameter10
                }
            );
        }
    }
}
