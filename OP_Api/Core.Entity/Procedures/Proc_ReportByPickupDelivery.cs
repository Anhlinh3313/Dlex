using System;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_ReportByPickupDelivery : IEntityProcView
    {
        public const string ProcName = "Proc_ReportByPickupDelivery";

        public int Id { get; set; }
        public string Code { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public int? HubId { get; set; }
        public string HubName { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        public double? TotalCODMustCollect { get; set; }
        public double? TotalZCOD { get; set; }
        public double? TotalSubmitCOD { get; set; }
        public double? TotalAcceptedCOD { get; set; }
        public double? TotalAwaitSubmitCOD { get; set; }
        public int TotalPickup { get; set; }
        public int TotalPickupFail { get; set; }
        public int TotalShipment { get; set; }
        public int TotalAwaitAccept { get; set; }
        public int TotalDelivering { get; set; }
        public int TotalDeliveredOne { get; set; }
        public int TotalDeliveredTwo { get; set; }
        public int TotalDeliveredThree { get; set; }
        public int TotalDeliveryFail { get; set; }
        public int TotalReturn { get; set; }
        public double? TotalPricePickup { get; set; }
        public double? TotalPriceDelivery { get; set; }

        public Proc_ReportByPickupDelivery()
        {
        }

        public static IEntityProc GetEntityProc(int? userId = null, DateTime? dateFrom = null, DateTime? dateTo = null, int? hubId = null, string toProvinceIds = null)
        {
            SqlParameter parameter1 = new SqlParameter(
            "@UserId", userId);
            if (!userId.HasValue)
                parameter1.Value = DBNull.Value;
            SqlParameter parameter2 = new SqlParameter(
            "@DateFrom", dateFrom);
            if (!dateFrom.HasValue)
                parameter2.Value = DBNull.Value;
            SqlParameter parameter3 = new SqlParameter(
            "@DateTo", dateTo);
            if (!dateTo.HasValue)
                parameter3.Value = DBNull.Value;

            SqlParameter HubId = new SqlParameter(
           "@HubId", hubId);
            if (!hubId.HasValue)
                HubId.Value = DBNull.Value;

            SqlParameter ToProvinceIds = new SqlParameter("@ToProvinceIds", toProvinceIds);
            if (string.IsNullOrWhiteSpace(toProvinceIds))
                ToProvinceIds.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @UserId, @DateFrom, @DateTo, @HubId, @ToProvinceIds",
                new SqlParameter[] {
                parameter1,
                parameter2,
                parameter3,
                HubId,
                ToProvinceIds
                }
            );
        }
    }
}
