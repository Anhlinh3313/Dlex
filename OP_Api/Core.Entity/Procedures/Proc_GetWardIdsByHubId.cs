using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_GetWardIdsByHubId : IEntityProcView
    {
        public const string ProcName = "Proc_GetWardIdsByHubId";

        [Key]
        public int Id { get; set; }

        public Proc_GetWardIdsByHubId()
        {
        }

        public static IEntityProc GetEntityProc(int? hubId = null)
        {
            SqlParameter HubId = new SqlParameter("@HubId", hubId);
            if (!hubId.HasValue)
                HubId.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @HubId",
                new SqlParameter[] {
                    HubId
                }
           );
        }
    }
}
