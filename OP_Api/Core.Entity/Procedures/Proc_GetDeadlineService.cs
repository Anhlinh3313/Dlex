using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetDeadlineService : IEntityProcView
    {
        public const string ProcName = "Proc_GetDeadlineService";

        [Key]
        public double DeadlineDelivery { get; set; }
        public static IEntityProc GetEntityProc(int serviceId, int fromHubId, int toDistrictId)
        {
            SqlParameter sqlParameter1 = new SqlParameter("@serviceId", serviceId);
            SqlParameter sqlParameter2 = new SqlParameter("@fromHubId", fromHubId);
            SqlParameter sqlParameter3 = new SqlParameter("@toDistrictId", toDistrictId);
            return new EntityProc(
                $"{ProcName} @serviceId,@fromHubId,@toDistrictId",
                new SqlParameter[] {
                    sqlParameter1,sqlParameter2,sqlParameter3
                }
            );
        }
    }
}
