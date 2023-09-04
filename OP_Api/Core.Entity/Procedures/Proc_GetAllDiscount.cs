using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetAllDiscount : IEntityProcView
    {
        public const string ProcName = "Proc_GetAllDiscount";
        [Key]
        public int Id { get; set; }
        public DateTime? CreatedWhen { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedWhen { get; set; }
        public int? ModifiedBy { get; set; }
        public string ConcurrencyStamp { get; set; }
        public bool IsEnabled { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int DiscountTypeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public double ValueFrom { get; set; }
        public double ValueTo { get; set; }
        public bool IsPublic { get; set; }
        public double DiscountPercent { get; set; }
        public string DiscountName { get; set; }
        public Int32? TotalCount { get; set; }

        public Proc_GetAllDiscount() { }
        public static IEntityProc GetEntityProc(DateTime? fromDate = null, DateTime? toDate = null,int ? customerId = null, int? pageNumber = null,
            int? pageSize = null)
        {
            SqlParameter DateFrom = new SqlParameter("@DateFrom", fromDate);
            if (!fromDate.HasValue)
                DateFrom.Value = DBNull.Value;

            SqlParameter DateTo = new SqlParameter("@DateTo", toDate);
            if (!toDate.HasValue)
                DateTo.Value = DBNull.Value;

            SqlParameter PageNumber = new SqlParameter("@PageNumber", pageNumber);
            if (!pageNumber.HasValue)
                PageNumber.Value = DBNull.Value;

            SqlParameter CustomerId = new SqlParameter("@CustomerId", customerId);
            if (!customerId.HasValue)
                CustomerId.Value = DBNull.Value;

            SqlParameter PageSize = new SqlParameter("@PageSize", pageSize);
            if (!pageSize.HasValue)
                PageSize.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @CustomerId,@DateFrom,@DateTo,@PageNumber,@PageSize",
                new SqlParameter[] {
                    CustomerId,DateFrom,DateTo,PageNumber,PageSize
                }
            );
        }
    }
}
