using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetListGoodsSendToHub : IEntityProcView
    {
        public const string ProcName = "Proc_GetListGoodsSendToHub";

        public int Id { get; set; }
        public string Code { get; set; }
        public int? FromHubId { get; set; }
        public int? ToHubId { get; set; }
        public Proc_GetListGoodsSendToHub()
        {

        }

        public static IEntityProc GetEntityProc(int? hubId = null, int? typeId = null)
        {
            SqlParameter parameter1 = new SqlParameter(
           "@HubId", hubId);
            if (!hubId.HasValue)
                parameter1.Value = DBNull.Value;

            SqlParameter parameter2 = new SqlParameter(
           "@TypeId", typeId);
            if (!typeId.HasValue)
                parameter2.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @HubId, @TypeId",
                new SqlParameter[] {
                parameter1,
                parameter2
                }
            );
        }
    }
}
