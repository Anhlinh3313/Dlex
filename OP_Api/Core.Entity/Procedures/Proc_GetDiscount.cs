using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetDiscount : IEntityProcView
    {
        public const string ProcName = "Proc_GetDiscount";
        [Key]
        public Int64 Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double DiscountPercent { get; set; }
        public double ValueFrom { get; set; }
        public double ValueTo { get; set; }
        public int DiscountTypeId { get; set; }
        public double? SumTotalPrice { get; set; }
        public double? SumTotalWeight { get; set; }
        public int? SumTotalCount { get; set; }

        public Proc_GetDiscount() { }
        public static IEntityProc GetEntityProc(int categoryPaymentId, int customnerId, bool isSuccess, DateTime fromDate, DateTime toDate, int? listPaymentId = null)
        {
            SqlParameter CategoryPaymentId = new SqlParameter("@CategoryPaymentId", categoryPaymentId);
            SqlParameter CustomnerId = new SqlParameter("@CustomnerId", customnerId);
            SqlParameter FromDate = new SqlParameter("@FromDate", fromDate);
            SqlParameter ToDate = new SqlParameter("@ToDate", toDate);
            SqlParameter IsSuccess = new SqlParameter("@IsSuccess", isSuccess);
            SqlParameter ListPaymentId = new SqlParameter("@ListPaymentId", listPaymentId);
            if (!listPaymentId.HasValue) ListPaymentId.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @CategoryPaymentId,@CustomnerId,@FromDate,@ToDate,@IsSuccess,@ListPaymentId",
                new SqlParameter[] {
                    CategoryPaymentId,CustomnerId,FromDate,ToDate,IsSuccess,ListPaymentId
                }
            );
        }
    }
}
