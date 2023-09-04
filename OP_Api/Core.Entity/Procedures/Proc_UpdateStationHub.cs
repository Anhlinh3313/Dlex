using Core.Entity.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace Core.Entity.Procedures
{
    public class Proc_UpdateStationHub : IEntityProcView
    {
        public const string ProcName = "Proc_UpdateStationHub";
        [Key]
        public Boolean IsSuccess { get; set; }
        public string Message { get; set; }

        public Proc_UpdateStationHub() { }
        public static IEntityProc GetEntityProc(int stationHubId)
        {
            SqlParameter StationHubId = new SqlParameter("@StationHubId", stationHubId);
            return new EntityProc(
                $"{ProcName} @StationHubId",
                new SqlParameter[] {
                    StationHubId
                }
            );
        }
    }
}
