using Core.Entity.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace Core.Entity.Procedures
{
    public class Proc_CreatePromotionCustomer : IEntityProcView
    {
        public const string ProcName = "Proc_CreatePromotionCustomer";
        [Key]
        public Boolean IsSuccess { get; set; }
        public string Message { get; set; }

        public Proc_CreatePromotionCustomer() { }
        public static IEntityProc GetEntityProc(int? customerId = null, int? promotionId = null, int? userId = null, int? companyId = null)
        {
            SqlParameter CustomerId = new SqlParameter("@CustomerId", customerId);
            SqlParameter PromotionId = new SqlParameter("@PromotionId", promotionId);
            SqlParameter UserId = new SqlParameter("@UserId", userId);
            SqlParameter CompanyId = new SqlParameter("@CompanyId", companyId);

            return new EntityProc(
                $"{ProcName} @CustomerId, @PromotionId, @UserId, @CompanyId",
                new SqlParameter[] {
                    CustomerId,
                    PromotionId,
                    UserId,
                    CompanyId
                }
            );
        }
    }
}
