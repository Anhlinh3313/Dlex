using Core.Entity.Abstract;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace Core.Entity.Procedures
{
    public class Proc_CheckCompany : IEntityProcView
    {
        public const string ProcName = "Proc_CheckCompany";

        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string CompanyName { get; set; }

        public Proc_CheckCompany() { }
        public static IEntityProc GetEntityProc(string companyCode)
        {
            SqlParameter sqlParameter1 = new SqlParameter("@CompanyCode", companyCode);
            return new EntityProc(
                $"{ProcName} @CompanyCode",
                new SqlParameter[] {
                    sqlParameter1
                }
            );
        }
    }
}
