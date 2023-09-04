using Core.Entity.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace Core.Entity.Procedures
{
    public class Proc_RanDomCodeCustomer : IEntityProcView
    {
        public const string ProcName = "Proc_RanDomCodeCustomer";
        [Key]
        public string CustomerCode { get; set; }

        public Proc_RanDomCodeCustomer() { }
        public static IEntityProc GetEntityProc(string code)
        {
            SqlParameter Code = new SqlParameter("@Code", code);

            return new EntityProc(
                $"{ProcName} @Code",
                new SqlParameter[] {
                    Code
                }
            );
        }
    }
}
