using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetHubByProvinceDistrictWard : IEntityProcView
    {
        public const string ProcName = "Proc_GetHubByProvinceDistrictWard";
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public Proc_GetHubByProvinceDistrictWard() { }
        public static IEntityProc GetEntityProc(int provinceId, int districtId, int wardId)
        {
            SqlParameter ProvinceId = new SqlParameter("@ProvinceId", provinceId);

            SqlParameter DistrictId = new SqlParameter("@DistrictId", districtId);

            SqlParameter WardId = new SqlParameter("@WardId", wardId);

            return new EntityProc(
                $"{ProcName} @ProvinceId, @DistrictId, @WardId",
                new SqlParameter[] {
                    ProvinceId,
                    DistrictId,
                    WardId
                }
            );
        }
    }
}
