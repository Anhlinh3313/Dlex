using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_ReportHandleEmployee : IEntityProcView
    {
        public const string ProcName = "Proc_ReportHandleEmployee";

        [Key]
        public int Id { get; set; }
        public string ShipmentNumber { get; set; }
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
        public string ShippingAddress { get; set; }
        public double? Insured { get; set; }
        public double? COD { get; set; }
        public string ShipmentStatusName { get; set; }
        public string UserCode { get; set; }
        public string UserFullName { get; set; }
        public string RealRecipientName { get; set; }
        public DateTime? EndDeliveryTime { get; set; }
        public double? MustReceiveCOD { get; set; }
        public double? MustReceivePrice { get; set; }
        public double? KeepingCOD { get; set; }
        public double? KeepingPrice { get; set; }
        public double? AwaitAcceptCOD { get; set; }
        public double? AwaitAcceptPrice { get; set; }
        public DateTime? LastDatetimeUpdate { get; set; }
        public int? TimeCompare { get; set; }
        public double? TotalMustReceiveCOD { get; set; }
        public double? TotalMustReceivePrice { get; set; }
        public double? TotalKeepingCOD { get; set; }
        public double? TotalKeepingPrice { get; set; }
        public double? TotalAwaitAcceptCOD { get; set; }
        public double? TotalAwaitAcceptPrice { get; set; }
        public int TotalCount { get; set; }

        public Proc_ReportHandleEmployee()
        {
        }

        public static IEntityProc GetEntityProc(int? hubId = null, int? userId = null, bool? isGroupEmp = null, int? groupStatusId = null,
            int? timeCompare = null, DateTime? dateFrom = null, DateTime? dateTo = null, int? pageNumber = null, int? pageSize = null)
        {

            SqlParameter HubId = new SqlParameter("@HubId", hubId);
            if (!hubId.HasValue)
                HubId.Value = DBNull.Value;

            SqlParameter UserId = new SqlParameter("@UserId", userId);
            if (!userId.HasValue)
                UserId.Value = DBNull.Value;

            SqlParameter IsGroupEmp = new SqlParameter("@IsGroupEmp", isGroupEmp);
            if (!isGroupEmp.HasValue)
                IsGroupEmp.Value = false;

            SqlParameter GroupStatusId = new SqlParameter("@GroupStatusId", groupStatusId);
            if (!groupStatusId.HasValue)
                GroupStatusId.Value = DBNull.Value;

            SqlParameter TimeCompare = new SqlParameter("@TimeCompare", timeCompare);
            if (!timeCompare.HasValue)
                TimeCompare.Value = DBNull.Value;

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
                $"{ProcName} @HubId,@UserId,@IsGroupEmp,@GroupStatusId,@TimeCompare,@DateFrom,@DateTo,@PageNumber,@PageSize",
                new SqlParameter[] {
                HubId,
                UserId,
                IsGroupEmp,
                GroupStatusId,
                TimeCompare,
                DateFrom,
                DateTo,
                PageNumber,
                PageSize
                }
            );
        }
    }
}
