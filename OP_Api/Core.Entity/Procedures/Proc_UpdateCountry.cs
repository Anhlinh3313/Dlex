using Core.Entity.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace Core.Entity.Procedures
{
    public class Proc_UpdateCountry : IEntityProcView
    {
        public const string ProcName = "Proc_UpdateCountry";
        [Key]
        public Boolean IsSuccess { get; set; }
        public string Message { get; set; }

        public Proc_UpdateCountry() { }
        public static IEntityProc GetEntityProc(int countryId)
        {
            SqlParameter CountryId = new SqlParameter("@CountryId", countryId);
            return new EntityProc(
                $"{ProcName} @CountryId",
                new SqlParameter[] {
                    CountryId
                }
            );
        }
    }
}
