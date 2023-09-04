using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetTimeKPI : IEntityProcView
    {
        public const string ProcName = "Proc_GetTimeKPI";
        [Key]
        public int TargetDeliveryTime { get; set; }
        public int TargetPaymentCOD { get; set; }

        public Proc_GetTimeKPI() { }
        public static IEntityProc GetEntityProc(int? districtId, int? wardId, int? cusId, int serviceId)
        {
            SqlParameter WardId = new SqlParameter("@WardId", wardId);
            if (!wardId.HasValue) WardId.Value = DBNull.Value;

            SqlParameter CusId = new SqlParameter("@CusId", cusId);
            if (!cusId.HasValue) CusId.Value = DBNull.Value;

            SqlParameter ServiceId = new SqlParameter("@ServiceId", serviceId);

            SqlParameter DistrictId = new SqlParameter("@DistrictId", districtId);
            if (!districtId.HasValue) DistrictId.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @DistrictId, @WardId,@CusId,@ServiceId",
                new SqlParameter[] {
                    DistrictId,
                    WardId,
                    CusId,
                    ServiceId
                }
            );
        }
    }
}
