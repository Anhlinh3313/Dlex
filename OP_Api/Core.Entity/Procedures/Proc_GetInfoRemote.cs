using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetInfoRemote : IEntityProcView
    {
        public const string ProcName = "Proc_GetInfoRemote";
        [Key]
        public Boolean IsRemote { get; set; }
        public double HubRadiusServe { get; set; }
        public double HubRoutingRadiusServe { get; set; }
        public double KmNumber { get; set; }
        public int? RemoteServiceId { get; set; }

        public Proc_GetInfoRemote() { }
        public static IEntityProc GetEntityProc(int? districtId = null, int? wardId = null)
        {
            SqlParameter DistrictId = new SqlParameter("@ToDistrictId", districtId);
            if (!districtId.HasValue) DistrictId.Value = DBNull.Value;
            SqlParameter WardId = new SqlParameter("@ToWardId", wardId);
            if (!wardId.HasValue) WardId.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @ToDistrictId,@ToWardId",
                new SqlParameter[] {
                    DistrictId,
                    WardId
                }
            );
        }
    }
}
