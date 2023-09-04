using Core.Entity.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace Core.Entity.Procedures
{
    public class Proc_UpdatePromotionCustomer : IEntityProcView
    {
        public const string ProcName = "Proc_UpdatePromotionCustomer";
        [Key]
        public Boolean IsSuccess { get; set; }
        public string Message { get; set; }

        public Proc_UpdatePromotionCustomer() { }
        public static IEntityProc GetEntityProc(int promotionCustomerId)
        {
            SqlParameter PromotionCustomerId = new SqlParameter("@PromotionCustomerId", promotionCustomerId);
            return new EntityProc(
                $"{ProcName} @PromotionCustomerId",
                new SqlParameter[] {
                    PromotionCustomerId
                }
            );
        }
    }
}
