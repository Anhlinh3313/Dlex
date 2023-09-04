using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetRequestShipmentCurrentEmp : IEntityProcView
    {
        public const string ProcName = "Proc_GetRequestShipmentCurrentEmp";

        [Key]
        public int Id { get; set; }
        public DateTime? CreatedWhen { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedWhen { get; set; }
        public int? ModifiedBy { get; set; }
        public string ConcurrencyStamp { get; set; }
        public bool IsEnabled { get; set; } = true;
        public DateTime? OrderDate { get; set; }
        public string ShipmentNumber { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; }
        public string SenderPhone { get; set; }
        public string PickingAddress { get; set; }
        public int? FromHubId { get; set; }
        public int? FromHubRoutingId { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public string ShippingAddress { get; set; }
        public string RealRecipientName { get; set; }
        public int? ToHubId { get; set; }
        public int? ToHubRoutingId { get; set; }
        public int? SellerId { get; set; }
        public string SellerCode { get; set; }
        public string ShopCode { get; set; }
        public string SellerName { get; set; }
        public string SellerPhone { get; set; }
        public string SellerAddress { get; set; }
        public int? ServiceId { get; set; }
        public int? PaymentTypeId { get; set; }
        public int? ThirdPLId { get; set; }
        public int? PickUserId { get; set; }
        public int? DeliverUserId { get; set; }
        public int? TransferUserId { get; set; }
        public int? TransferReturnUserId { get; set; }
        public int? ReturnUserId { get; set; }
        public int? TransferNote { get; set; }
        public int ShipmentStatusId { get; set; }
        public int? NumPick { get; set; }
        public int? NumDeliver { get; set; }
        public int? NumReturn { get; set; }
        public int? NumTransfer { get; set; }
        public int? NumTransferReturn { get; set; }
        public string CurrentWarehouseName { get; set; }
        public string PickWarehouseName { get; set; }
        public string DeliveryWarehouseName { get; set; }
        public double? Weight { get; set; }
        public double? CalWeight { get; set; }
        public string Content { get; set; }
        public string Note { get; set; }
        public DateTime? PaidDate { get; set; }
        public DateTime? StartPickTime { get; set; }
        public DateTime? EndPickTime { get; set; }
        public DateTime? StartReturnTime { get; set; }
        public DateTime? EndReturnTime { get; set; }
        public string ReturnInfo { get; set; }
        public DateTime? StartDeliveryTime { get; set; }
        public DateTime? EndDeliveryTime { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? ExpectedDeliveryTime { get; set; }
        public DateTime? StartTransferTime { get; set; }
        public DateTime? EndTransferTime { get; set; }
        public DateTime? DMTransferCODToHubTime { get; set; }
        public DateTime? HubTransferCODToAccountantTime { get; set; }
        public DateTime? AccountantReceivedCODTime { get; set; }
        public string CusNote { get; set; }
        public string PickupNote { get; set; }
        public string DeliveryNote { get; set; }
        public string ReturnNote { get; set; }
        public string TransferReturnNote { get; set; }
        public DateTime? StartTransferReturnTime { get; set; }
        public DateTime? EndTransferReturnTime { get; set; }
        public DateTime? PickupAppointmentTime { get; set; }
        public DateTime? DeliveryAppointmentTime { get; set; }
        public string DeliveryImagePath { get; set; }
        public string ReturnImagePath { get; set; }
        public string ChangingDeliveryAddress { get; set; }
        public string ChangingDeliveryAddressTime { get; set; }
        public string ChangingDeliveryAddressNote { get; set; }
        public int? OldDeliveryWardId { get; set; }
        public int? OldDeliveryHubId { get; set; }
        public int? OldDeliveryHubRoutingId { get; set; }
        public int? CurrentHubId { get; set; }
        public int? CreatedHubId { get; set; }
        public double? LatFrom { get; set; }
        public double? LngFrom { get; set; }
        public double? LatTo { get; set; }
        public double? LngTo { get; set; }
        public int? CurrentEmpId { get; set; }
        public bool IsCompleted { get; set; }
        public double? COD { get; set; }
        public double? Insured { get; set; }
        public double? DefaultPrice { get; set; }
        public double? RemoteAreasPrice { get; set; }
        public double? FuelPrice { get; set; }
        public double? TotalDVGT { get; set; }
        public double? OtherPrice { get; set; }
        public double? VATPrice { get; set; }
        public double? TotalPrice { get; set; }
        public int? PriceListId { get; set; }
        public string CompanyFrom { get; set; }
        public string CompanyTo { get; set; }
        public string AddressNoteFrom { get; set; }
        public string AddressNoteTo { get; set; }
        public int? FromWardId { get; set; }
        public int? FromDistrictId { get; set; }
        public int? FromProvinceId { get; set; }
        public int? ToWardId { get; set; }
        public int? ToDistrictId { get; set; }
        public int? ToProvinceId { get; set; }
        public int? StructureId { get; set; }
        public int TotalBox { get; set; }
        public DateTime? FirstPickupTime { get; set; }
        public DateTime? AssignPickTime { get; set; }
        public bool IsPaidPrice { get; set; }
        public int CountShipment { get; set; }
        public int CountShipmentAccept { get; set; }
        public double WeightAccept { get; set; }
        public double CalWeightAccept { get; set; }
        public int TotalBoxAccept { get; set; }
        public int? CusDepartmentId { get; set; }
        public int? ToStreetId { get; set; }
        public int? FromStreetId { get; set; }
        public string STRServiceDVGTIds { get; set; }
        public int? RequestShipmentId { get; set; }
        public int? TotalChildRequestShipment { get; set; }
        public int? TotalChildShipment { get; set; }
        public string SenderCode { get; set; }
        public int? CustomerTypeId { get; set; }
        public string FromProvinceName { get; set; }
        public string FromDistrictName { get; set; }
        public string FromWardName { get; set; }
        public string ToProvinceName { get; set; }
        public string ToDistrictName { get; set; }
        public string ToWardName { get; set; }
        public string ServiceName { get; set; }
        public string StructureName { get; set; }
        public string PaymentTypeName { get; set; }
        public string FromHubName { get; set; }
        public string ToHubName { get; set; }
        public string ShipmentStatusName { get; set; }
        public int TotalShipment { get; set; }
        //public int? TotalBoxShipment { get; set; }
        //public int? TotalWeightShipment { get; set; }

        public int TotalCount { get; set; }

        public Proc_GetRequestShipmentCurrentEmp() { }

        public static IEntityProc GetEntityProc(int? userId = null, string statusIds = null, string searchText = null, int ? pageNumber = 1, int? pageSize = 20)
        {
            SqlParameter UserId = new SqlParameter("@UserId", userId);
            if (!userId.HasValue) UserId.Value = DBNull.Value;
            SqlParameter StatusIds = new SqlParameter("@StatusIds", statusIds);
            if (string.IsNullOrWhiteSpace(statusIds)) StatusIds.Value = DBNull.Value;
            SqlParameter SearchText = new SqlParameter("@SearchText", searchText);
            if (string.IsNullOrWhiteSpace(searchText)) SearchText.Value = DBNull.Value;
            SqlParameter PageNumber = new SqlParameter("@PageNUmber", pageNumber);
            SqlParameter PageSize = new SqlParameter("@PageSize", pageSize);

            return new EntityProc(
                $"{ProcName} @UserId,@StatusIds,@SearchText,@PageNUmber,@PageSize",
                new SqlParameter[] {
                    UserId,
                    StatusIds,
                    SearchText,
                    PageNumber,
                    PageSize
                }
            );
        }
    }
}
