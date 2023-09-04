using Core.Entity.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace Core.Entity.Procedures
{
    public class Proc_UpdateCustomerbyUserFail : IEntityProcView
    {
        public const string ProcName = "Proc_UpdateCustomerbyUserFail";
        [Key]
        public Boolean IsSuccess { get; set; }
        public string Message { get; set; }

        public Proc_UpdateCustomerbyUserFail() { }
        public static IEntityProc GetEntityProc(int customerId)
        {
            SqlParameter CustomerId = new SqlParameter("@CustomerId", customerId);
            return new EntityProc(
                $"{ProcName} @CustomerId",
                new SqlParameter[] {
                    CustomerId
                }
            );
        }
    }
}
