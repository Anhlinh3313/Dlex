using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetListWarehousing : IEntityProcView
    {
        public const string ProcName = "Proc_GetListWarehousing";

        [Key]
        public int Id { get; set; }
        public string ShipmentNumber { get; set; }
        public string SOENTRY { get; set; }
        public DateTime? OrderDate { get; set; }
        public string ShipmentStatusName { get; set; }
        public string SenderCode { get; set; }
        public string SenderName { get; set; }
        public string SenderPhone { get; set; }
        public string PickingAddress { get; set; }
        public string ToHubName { get; set; }
        public string ToDistrictName { get; set; }
        public string ToProvinceName { get; set; }
        public string ShippingAddress { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public double Weight { get; set; }
        public double CalWeight { get; set; }
        public int TotalBox { get; set; }
        public double COD { get; set; }
        public double Insured { get; set; }
        public string ServiceName { get; set; }
        public bool? IsIncidents { get; set; }

        public bool? IsPrioritize { get; set; }
        public string ListGoodsCode { get; set; }
        public int? ToHubId { get; set; }
        public double TotalWeight { get; set; }
        public double TotalCOD { get; set; }
        public DateTime? InOutDate { get; set; }
        public string PackageCode { get; set; }
        public int? PackageId { get; set; }
        public string ToHubRoutingName { get; set; }
        public int? TotalCount { get; set; }
        public int? TotalShipmentInPackage { get; set; }
        public int? TotalPackage { get; set; }
        public int? TotalBoxs { get; set; }

        public static IEntityProc GetEntityProc(
            int? warehousingType = null,
            int? currentHubId = null,
            int? userId = null,
            int? hubId = null,
            string listGoodsList = null,
            int? toHubId = null,
            int? toUserId = null,
            int? serviceId = null,
            bool? isPrioritize = null,
            bool? isIncidents = null,
            bool? isAllShipment = null,
            int? pageNumber = 1,
            int? pageSize = 20,
            int? senderId = null,
            DateTime? dateFrom = null,
            DateTime? dateTo = null,
            bool? isHideInPackage = null,
            bool? isNullHubRouting = null,
            int? userEmpId = null
            )
        {
            SqlParameter WarehousingType = new SqlParameter("@WarehousingType", warehousingType);
            if (!warehousingType.HasValue) WarehousingType.Value = DBNull.Value;
            SqlParameter CurrentHubId = new SqlParameter("@CurrentHubId", currentHubId);
            if (!currentHubId.HasValue) CurrentHubId.Value = DBNull.Value;
            SqlParameter UserId = new SqlParameter("@UserId", userId);
            if (!userId.HasValue) UserId.Value = DBNull.Value;
            SqlParameter HubId = new SqlParameter("@HubId", hubId);
            if (!hubId.HasValue) HubId.Value = DBNull.Value;
            SqlParameter ServiceId = new SqlParameter("@ServiceId", serviceId);
            if (!serviceId.HasValue) ServiceId.Value = DBNull.Value;
            SqlParameter IsPrioritize = new SqlParameter("@IsPrioritize", isPrioritize);
            if (!isPrioritize.HasValue) IsPrioritize.Value = DBNull.Value;
            SqlParameter IsIncidents = new SqlParameter("@IsIncidents", isIncidents);
            if (!isIncidents.HasValue) IsIncidents.Value = DBNull.Value;
            SqlParameter IsAllShipment = new SqlParameter("@IsAllShipment", isAllShipment);
            if (!isAllShipment.HasValue) IsAllShipment.Value = DBNull.Value;
            SqlParameter PageNumber = new SqlParameter("@PageNUmber", pageNumber);
            SqlParameter PageSize = new SqlParameter("@PageSize", pageSize);
            SqlParameter ListGoodsList = new SqlParameter("@ListGoodsList", listGoodsList);
            if (string.IsNullOrWhiteSpace(listGoodsList)) ListGoodsList.Value = DBNull.Value;
            SqlParameter ToHubId = new SqlParameter("@ToHubId", toHubId);
            if (!toHubId.HasValue) ToHubId.Value = DBNull.Value;
            SqlParameter ToUserId = new SqlParameter("@ToUserId", toUserId);
            if (!toUserId.HasValue) ToUserId.Value = DBNull.Value;
            SqlParameter SenderId = new SqlParameter("@SenderId", senderId);
            if (!senderId.HasValue) SenderId.Value = DBNull.Value;
            SqlParameter DateFrom = new SqlParameter("@DateFrom", dateFrom);
            if (!dateFrom.HasValue) DateFrom.Value = DBNull.Value;
            SqlParameter DateTo = new SqlParameter("@DateTo", dateTo);
            if (!dateTo.HasValue) DateTo.Value = DBNull.Value;
            SqlParameter IsHideInPackage = new SqlParameter("@IsHideInPackage", isHideInPackage);
            if (!isHideInPackage.HasValue) IsHideInPackage.Value = DBNull.Value;
            SqlParameter IsNullHubRouting = new SqlParameter("@IsNullHubRouting", isNullHubRouting);
            if (!isNullHubRouting.HasValue) IsNullHubRouting.Value = DBNull.Value;
            SqlParameter UserEmpId = new SqlParameter("@UserEmpId", userEmpId);
            if (!userEmpId.HasValue) UserEmpId.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @WarehousingType,@CurrentHubId,@UserId,@HubId,@ServiceId,@IsPrioritize,@IsIncidents,@IsAllShipment,@PageNUmber,@PageSize,@ListGoodsList,@ToHubId,@ToUserId,@SenderId,@DateFrom,@DateTo,@IsHideInPackage,@IsNullHubRouting,@UserEmpId",
                new SqlParameter[] {
                    WarehousingType,
                    CurrentHubId,
                    UserId,
                    HubId,
                    ServiceId,
                    IsPrioritize,
                    IsIncidents,
                    IsAllShipment,
                    PageNumber,
                    PageSize,
                    ListGoodsList,
                    ToHubId,
                    ToUserId,
                    SenderId,
                    DateFrom,
                    DateTo,
                    IsHideInPackage,
                    IsNullHubRouting,
                    UserEmpId
                }
            );
        }
    }
}
