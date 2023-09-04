using Core.Entity.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace Core.Entity.Procedures
{
    public class Proc_UpdateProvince : IEntityProcView
    {
        public const string ProcName = "Proc_UpdateProvince";
        [Key]
        public Boolean IsSuccess { get; set; }
        public string Message { get; set; }

        public Proc_UpdateProvince() { }
        public static IEntityProc GetEntityProc(int provinceId)
        {
            SqlParameter ProvinceId = new SqlParameter("@ProvinceId", provinceId);
            return new EntityProc(
                $"{ProcName} @ProvinceId",
                new SqlParameter[] {
                    ProvinceId
                }
            );
        }
    }
}
