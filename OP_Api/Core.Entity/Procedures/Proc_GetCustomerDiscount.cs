using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetCustomerDiscount : IEntityProcView
    {
        public const string ProcName = "Proc_GetCustomerDiscount";
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }

        public Proc_GetCustomerDiscount() { }
        public static IEntityProc GetEntityProc(int discountId)
        {
            SqlParameter DiscountId = new SqlParameter("@DiscountId", discountId);

            return new EntityProc(
                $"{ProcName} @DiscountId",
                new SqlParameter[] {
                    DiscountId
                }
            );
        }
    }
}
