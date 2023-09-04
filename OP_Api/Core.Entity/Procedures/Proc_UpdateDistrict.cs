using Core.Entity.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace Core.Entity.Procedures
{
    public class Proc_UpdateDistrict : IEntityProcView
    {
        public const string ProcName = "Proc_UpdateDistrict";
        [Key]
        public Boolean IsSuccess { get; set; }
        public string Message { get; set; }

        public Proc_UpdateDistrict() { }
        public static IEntityProc GetEntityProc(int districtId)
        {
            SqlParameter DistrictId = new SqlParameter("@DistrictId", districtId);
            return new EntityProc(
                $"{ProcName} @DistrictId",
                new SqlParameter[] {
                    DistrictId
                }
            );
        }
    }
}
