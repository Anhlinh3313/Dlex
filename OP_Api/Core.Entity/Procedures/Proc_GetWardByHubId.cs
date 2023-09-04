using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_GetWardByHubId : IEntityProcView
    {
        public const string ProcName = "Proc_GetWardByHubId";

        [Key]
        public int Id { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string WardName { get; set; }

        public Proc_GetWardByHubId()
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
