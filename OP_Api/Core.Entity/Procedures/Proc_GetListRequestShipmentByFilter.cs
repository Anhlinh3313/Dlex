using Core.Entity.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace Core.Entity.Procedures
{
    public class Proc_GetListRequestShipmentByFilter : IEntityProcView
    {
        public const string ProcName = "Proc_GetListRequestShipmentByFilter";

        [Key]
        public int Id { get; set; }
        public int? ShipmentStatusId { get; set; }
        public string ShipmentStatusName { get; set; }
        public string ShipmentNumber { get; set; }
        public bool? IsEnabled { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? CreatedWhen { get; set; }
        public DateTime? AssignPickTime { get; set; }
        public int? FromHubId { get; set; }
        public string FromHubCode { get; set; }
        public string FromHubName { get; set; }
        public int? PickUserId { get; set; }
        public string PickUserCode { get; set; }
        public string PickUserFullName { get; set; }
        public int? SenderId { get; set; }
        public string SenderName { get; set; }
        public string SenderPhone { get; set; }
        public string CompanyFrom { get; set; }
        public string PickingAddress { get; set; }
        public int? FromProvinceId { get; set; }
        public string FromProvinceCode { get; set; }
        public string FromProvinceName { get; set; }
        public int? FromHubRoutingId { get; set; }
        public string FromHubRoutingCode { get; set; }
        public string FromHubRoutingName { get; set; }
        public int? ReceiverId { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public string CompanyTo { get; set; }
        public string ShippingAddress { get; set; }
        public int? ToHubId { get; set; }
        public string ToHubCode { get; set; }
        public string ToHubName { get; set; }
        public int? ToProvinceId { get; set; }
        public string ToProvinceCode { get; set; }
        public string ToProvinceName { get; set; }
        public int? ToHubRoutingId { get; set; }
        public string ToHubRoutingCode { get; set; }
        public string ToHubRoutingName { get; set; }
        public int? CountShipmentAccept { get; set; }
        public int? TotalShipment { get; set; }
        public int? TotalShipmentNotFill { get; set; }
        public string CusNote { get; set; }
        public string Note { get; set; }
        public double? Weight { get; set; }
        public int? TotalCount { get; set; }
        public DateTime? StartPickTime { get; set; }
        public string CurrentUserCode { get; set; }
        public string CurrentUserName { get; set; }
        public string ReasonName { get; set; }
        public string FromDistrictCode { get; set; }
        public string FromDistrictName { get; set; }
        public string ToDistrictCode { get; set; }
        public string ToDistrictName { get; set; }
        public int? TotalBoxShipment { get; set; }
        public double? TotalWeightShipment { get; set; }

        public Proc_GetListRequestShipmentByFilter()
        {
        }

        public static IEntityProc GetEntityProc(
            int userId, string listHub = null, string type = null, string typePickup = null, bool? isEnabled = null, string shipmentNumber = null, DateTime? orderDateFrom = null, DateTime? orderDateTo = null,
            int? senderId = null, int? fromHubId = null, int? toHubId = null, int? shipmentStatusId = null, int? pickUserId = null, int? pageNumber = null, int? pageSize = null, bool? isSortDescending = null,
            int? pickupType = null)
        {
            SqlParameter parameter1 = new SqlParameter("@UserId", userId);
            SqlParameter parameter2 = new SqlParameter("@ListHub", listHub);
            if (string.IsNullOrWhiteSpace(listHub)) parameter2.Value = DBNull.Value;
            SqlParameter parameter3 = new SqlParameter("@Type", type);
            if (string.IsNullOrWhiteSpace(type))parameter3.Value = DBNull.Value;
            SqlParameter parameter33 = new SqlParameter("@TypePickup", typePickup);
            if (string.IsNullOrWhiteSpace(typePickup))parameter33.Value = DBNull.Value;
            SqlParameter parameter4 = new SqlParameter("@IsEnabled", isEnabled);
            if (!isEnabled.HasValue)parameter4.Value = DBNull.Value;
            SqlParameter parameter5 = new SqlParameter("@ShipmentNumber", shipmentNumber);
            if (string.IsNullOrWhiteSpace(shipmentNumber))parameter5.Value = DBNull.Value;
            SqlParameter parameter6 = new SqlParameter("@OrderDateFrom", orderDateFrom);
            if (!orderDateFrom.HasValue)parameter6.Value = DBNull.Value;
            SqlParameter parameter7 = new SqlParameter("@OrderDateTo", orderDateTo);
            if (!orderDateTo.HasValue)parameter7.Value = DBNull.Value;
            SqlParameter parameter8 = new SqlParameter("@SenderId", senderId);
            if (!senderId.HasValue)parameter8.Value = DBNull.Value;
            SqlParameter parameter9 = new SqlParameter("@FromHubId", fromHubId);
            if (!fromHubId.HasValue)parameter9.Value = DBNull.Value;
            SqlParameter parameter10 = new SqlParameter("@ToHubId", toHubId);
            if (!toHubId.HasValue)parameter10.Value = DBNull.Value;
            SqlParameter parameter11 = new SqlParameter("@ShipmentStatusId", shipmentStatusId);
            if (!shipmentStatusId.HasValue)parameter11.Value = DBNull.Value;
            SqlParameter parameter12 = new SqlParameter("@PickUserId", pickUserId);
            if (!pickUserId.HasValue)parameter12.Value = DBNull.Value;
            SqlParameter parameter13 = new SqlParameter("@PageNumber", pageNumber);
            if (!pageNumber.HasValue)parameter13.Value = DBNull.Value;
            SqlParameter parameter14 = new SqlParameter("@PageSize", pageSize);
            if (!pageSize.HasValue)parameter14.Value = DBNull.Value;
            SqlParameter parameter15 = new SqlParameter("@IsSortDescending", isSortDescending);
            if (!isSortDescending.HasValue)parameter15.Value = DBNull.Value;
            SqlParameter parameter16 = new SqlParameter("@PickupType", pickupType);
            if (!pickupType.HasValue) parameter16.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @UserId, @ListHub, @Type,@TypePickup, @IsEnabled, @ShipmentNumber, @OrderDateFrom, @OrderDateTo, @SenderId, @FromHubId, @ToHubId, @ShipmentStatusId, @pickUserId, @PageNumber, @PageSize, @IsSortDescending,@PickupType",
                new SqlParameter[] {
                    parameter1,
                    parameter2,
                    parameter3,
                    parameter33,
                    parameter4,
                    parameter5,
                    parameter6,
                    parameter7,
                    parameter8,
                    parameter9,
                    parameter10,
                    parameter11,
                    parameter12,
                    parameter13,
                    parameter14,
                    parameter15,
                    parameter16
                }
            );
        }
    }
}
