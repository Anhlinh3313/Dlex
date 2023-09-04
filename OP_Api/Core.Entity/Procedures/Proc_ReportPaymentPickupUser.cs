using System;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_ReportPaymentPickupUser : IEntityProcView
    {
        public const string ProcName = "Proc_ReportPaymentPickupUser";

        public int Id { get; set; }
        public Int64 RowNum { get; set; }
        public int TotalCount { get; set; }
        public string FromHubCode { get; set; }
        public string PickEmpCode { get; set; }
        public string PickEmpName { get; set; }
        public string ShipmentNumber { get; set; }
        public DateTime? OrderDate { get; set; }
        public string PaymentTypeName { get; set; }
        public double? TotalPrice { get; set; }
        public double? TotalPriceSYS { get; set; }
        public double? TotalPricePay { get; set; }
        public double? RePay { get; set; }
        public double? TotalPaid { get; set; }
        public double? RePayPickup { get; set; }
        public string Note { get; set; }
        public bool? IsReturn { get; set; }
        public string ShipmentNumberChild { get; set; }
        public string ToHubName { get; set; }
        public string DeliveredEmpName { get; set; }
        public DateTime? EndDeliveryTime { get; set; }

        public Proc_ReportPaymentPickupUser()
        {
        }

        public static IEntityProc GetEntityProc(DateTime? fromDate = null, DateTime? toDate = null, int? hubId = null, int? userId = null, int? pageNumber = 1, int? pageSize = 20)
        {

            SqlParameter DateFrom = new SqlParameter("@DateFrom", fromDate);
            if (!fromDate.HasValue)
                DateFrom.Value = DBNull.Value;

            SqlParameter DateTo = new SqlParameter("@DateTo", toDate);
            if (!toDate.HasValue)
                DateTo.Value = DBNull.Value;

            SqlParameter HubId = new SqlParameter("@HubId", hubId);
            if (!hubId.HasValue)
                HubId.Value = DBNull.Value;

            SqlParameter UserId = new SqlParameter("@UserId", userId);
            if (!userId.HasValue)
                UserId.Value = DBNull.Value;

            SqlParameter PageNumber = new SqlParameter("@PageNumber", pageNumber);
            if (!pageNumber.HasValue)
                PageNumber.Value = DBNull.Value;

            SqlParameter PageSize = new SqlParameter("@PageSize", pageSize);
            if (!pageSize.HasValue)
                PageSize.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @DateFrom, @DateTo, @HubId, @UserId, @PageNumber, @PageSize",
                new SqlParameter[] {
                DateFrom,
                DateTo,
                HubId,
                UserId,
                PageNumber,
                PageSize
                }
            );
        }
    }
}
