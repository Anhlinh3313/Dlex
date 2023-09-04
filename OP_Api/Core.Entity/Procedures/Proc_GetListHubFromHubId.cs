using Core.Entity.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace Core.Entity.Procedures
{
    public class Proc_GetListHubFromHubId : IEntityProcView
    {
        public const string ProcName = "Proc_GetListHubFromHubId";
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? POHubId { get; set; }
        public int? CenterHubId { get; set; }

        public Proc_GetListHubFromHubId() { }
        public static IEntityProc GetEntityProc(int? hubId = null)
        {
            SqlParameter HubId = new SqlParameter("@Id", hubId);
            if (!hubId.HasValue)
                HubId.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @Id ",
                new SqlParameter[] {
                    HubId
                }
            );
        }
    }
}
