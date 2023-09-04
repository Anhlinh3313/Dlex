using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetListChangeCOD : IEntityProcView
    {

        public const string ProcName = "Proc_GetListChangeCOD";

        [Key]
        public int Id { get; set; }
        public DateTime? CreatedWhen { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedWhen { get; set; }
        public int? ModifiedBy { get; set; }
        public bool IsEnabled { get; set; }
        public string ConcurrencyStamp { get; set; }
        public int? ShipmentId { get; set; }
        public int? ChangeCODTypeId { get; set; }
        public double? OldCOD { get; set; }
        public double? NewCOD { get; set; }
        public int? ShipmentSupportId { get; set; }
        public bool? IsAccept { get; set; }
        public string Note { get; set; }
        public string ChangeCODTypeName { get; set; }
        public string ShipmentNumber { get; set; }
        public string ShipmentSupportNumber { get; set; }
        public int TotalCount { get; set; }

        public static IEntityProc GetEntityProc(int? userId = null,bool? isAccept= null, DateTime? dateFrom = null, DateTime? dateTo = null, int? pageNumber = 1, int? pageSize = 20)
        {
            SqlParameter UserId = new SqlParameter("@UserId", userId);
            if (!userId.HasValue)
                UserId.Value = DBNull.Value;

            SqlParameter IsAccept = new SqlParameter("@IsAccept", isAccept);
            if (!isAccept.HasValue)
                IsAccept.Value = DBNull.Value;

            SqlParameter DateFrom = new SqlParameter("@DateFrom", dateFrom);
            if (!dateFrom.HasValue)
                DateFrom.Value = DBNull.Value;

            SqlParameter DateTo = new SqlParameter("@DateTo", dateTo);
            if (!dateTo.HasValue)
                DateTo.Value = DBNull.Value;

            SqlParameter PageNumber = new SqlParameter("@PageNumber", pageNumber);
            if (!pageNumber.HasValue)
                PageNumber.Value = DBNull.Value;

            SqlParameter PageSize = new SqlParameter("@PageSize", pageSize);
            if (!pageSize.HasValue)
                PageSize.Value = DBNull.Value;

            return new EntityProc(
               $"{ProcName} @userId,@isAccept, @dateFrom, @dateTo, @PageNumber, @PageSize",
               new SqlParameter[] {
                UserId,
                IsAccept,
                DateFrom,
                DateTo,
                PageNumber,
                PageSize
               }
           );
        }
    }
}
