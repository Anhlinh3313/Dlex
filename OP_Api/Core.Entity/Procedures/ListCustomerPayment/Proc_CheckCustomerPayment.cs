using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;


namespace Core.Entity.Procedures
{
    public class Proc_CheckCustomerPayment : IEntityProcView
    {
        public const string ProcName = "Proc_CheckCustomerPayment";
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public double? SumPayCOD { get; set; }
        public double? SumPayPrice { get; set; }
        public int? SumCount { get; set; }
        public int? TotalCount { get; set; }

        public Proc_CheckCustomerPayment() { }
        public static IEntityProc GetEntityProc(int? categoryPaymentId, DateTime? dateFrom, DateTime? dateTo, int? senderId, 
            bool? isSuccess, int? pageNumber, int? pageSize)
        {
            SqlParameter CategoryPaymentId = new SqlParameter("@CategoryPaymentId", categoryPaymentId);
            if (!categoryPaymentId.HasValue) CategoryPaymentId.Value = DBNull.Value;

             SqlParameter DateFrom = new SqlParameter("@DateFrom", dateFrom);
            if (!dateFrom.HasValue) DateFrom.Value = DBNull.Value;

            SqlParameter DateTo = new SqlParameter("@DateTo", dateTo);
            if (!dateTo.HasValue) DateTo.Value = DBNull.Value;

            SqlParameter SenderId = new SqlParameter("@SenderId", senderId);
            if (!senderId.HasValue) SenderId.Value = DBNull.Value;

            SqlParameter IsSuccess = new SqlParameter("@IsSuccess", isSuccess);
            if (!isSuccess.HasValue) IsSuccess.Value = DBNull.Value;

            SqlParameter PageNumber = new SqlParameter("@PageNumber", pageNumber);
            if (!pageNumber.HasValue) PageNumber.Value = 1;

            SqlParameter PageSize = new SqlParameter("@PageSize", pageSize);
            if (!pageSize.HasValue) PageSize.Value = 20;

            return new EntityProc(
                $"{ProcName} @CategoryPaymentId, @DateFrom, @DateTo, @SenderId, @IsSuccess, @PageNumber, @PageSize",
                new SqlParameter[] {
                    CategoryPaymentId, DateFrom, DateTo, SenderId, IsSuccess, PageNumber, PageSize
                }
            );
        }
    }
}
