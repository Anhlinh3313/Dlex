using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetShipmentCurrentEmp : IEntityProcView
    {
        public const string ProcName = "Proc_GetShipmentCurrentEmp";

        [Key]
        public int Id { get; set; }
        public DateTime? CreatedWhen { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedWhen { get; set; }
        public int? ModifiedBy { get; set; }
        public string ConcurrencyStamp { get; set; }
        public bool IsEnabled { get; set; } = true;
        public bool IsIncidents { get; set; }
        public bool IsDelay { get; set; }
        public double DefaultPriceS { get; set; }
        public double DefaultPriceP { get; set; }
        public bool IsPrioritize { get; set; }
        public int? InputUserId { get; set; }
        public double DisCount { get; set; }
        public bool IsPaymentChild { get; set; }
        public bool IsCreatedChild { get; set; }
        public bool IsBox { get; set; }
        public int? PaymentCODTypeId { get; set; }
        public int? ReceiverId { get; set; }
        public int? BusinessUserId { get; set; }
        public double? PriceReturn { get; set; }
        public double? PriceCOD { get; set; }
        public double? TotalPriceSYS { get; set; }
        public int? CountPushVSE { get; set; }
        public bool IsPushCustomer { get; set; }
        public int? UploadExcelHistoryId { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShipmentNumber { get; set; }
        public int? SenderId { get; set; }
        public string SenderName { get; set; }
        public string SenderPhone { get; set; }
        public string PickingAddress { get; set; }
        public int? FromHubId { get; set; }
        public int? FromHubRoutingId { get; set; }
        public int? FromWardId { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public string ShippingAddress { get; set; }
        public string RealRecipientName { get; set; }
        public int? ToHubId { get; set; }
        public int? ToHubRoutingId { get; set; }
        public int? ToWardId { get; set; }
        public string ShopCode { get; set; }
        public int? ServiceId { get; set; }
        public int? PaymentTypeId { get; set; }
        public int? PickUserId { get; set; }
        public int? DeliverUserId { get; set; }
        public int? ReturnUserId { get; set; }
        public int ShipmentStatusId { get; set; }
        public int NumDeliver { get; set; }
        public double Weight { get; set; }
        public double? CalWeight { get; set; }
        public double CusWeight { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public string Content { get; set; }
        public string Note { get; set; }
        public DateTime? PaidDate { get; set; }
        public DateTime? StartPickTime { get; set; }
        public DateTime? EndPickTime { get; set; }
        public DateTime? EndReturnTime { get; set; }
        public DateTime? EndDeliveryTime { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? DMTransferCODToHubTime { get; set; }
        public DateTime? HubTransferCODToAccountantTime { get; set; }
        public DateTime? AccountantReceivedCODTime { get; set; }
        public string CusNote { get; set; }
        public string DeliveryNote { get; set; }
        public string ReturnNote { get; set; }
        public string PickupImagePath { get; set; }
        public string DeliveryImagePath { get; set; }
        public string ReturnImagePath { get; set; }
        public int? CurrentHubId { get; set; }
        public int? CreatedHubId { get; set; }
        public double? LatFrom { get; set; }
        public double? LngFrom { get; set; }
        public double? LatTo { get; set; }
        public double? LngTo { get; set; }
        public int? CurrentEmpId { get; set; }
        public int? RequestShipmentId { get; set; }
        public double COD { get; set; }
        public double Insured { get; set; }
        public bool IsAgreementPrice { get; set; }
        public double DefaultPrice { get; set; }
        public double RemoteAreasPrice { get; set; }
        public double FuelPrice { get; set; }
        public double TotalDVGT { get; set; }
        public double OtherPrice { get; set; }
        public double VATPrice { get; set; }
        public double TotalPrice { get; set; }
        public string CompanyFrom { get; set; }
        public string CompanyTo { get; set; }
        public string AddressNoteFrom { get; set; }
        public string AddressNoteTo { get; set; }
        public int? FromDistrictId { get; set; }
        public int? FromProvinceId { get; set; }
        public int? ToDistrictId { get; set; }
        public int? ToProvinceId { get; set; }
        public int? StructureId { get; set; }
        public int? PackageId { get; set; }
        public int TotalBox { get; set; }
        public int? ReceiveHubId { get; set; }
        public DateTime? AssignPickTime { get; set; }
        public bool IsPaidPrice { get; set; }
        public bool IsPaidCODToCustomer { get; set; }
        public int? KeepingCODHubId { get; set; }           //Hub đang giữ cod
        public int? KeepingTotalPriceHubId { get; set; }    //Hub đang giữ tổng phí
        public int? KeepingCODEmpId { get; set; }           //NV đang giữ cod
        public int? KeepingTotalPriceEmpId { get; set; }    //NV đang giữ tổng phí
        public int? ListReceiptMoneyCODId { get; set; }     //Bảng kê nộp tiền COD
        public int? ListReceiptMoneyTotalPriceId { get; set; }  //Bảng kê nộp tiền cước phí
        public int? ListCustomerPaymentCODId { get; set; }     //Bảng kê thanh toán COD khách hàng 
        public int? ListCustomerPaymentTotalPriceId { get; set; }  //Bảng kê thanh toán cước phí khách hàng 
        public int? ListGoodsId { get; set; }
        public int? ShipmentId { get; set; }
        public bool IsCreditTransfer { get; set; }
        public bool IsRecoveryDeliveryComplete { get; set; }
        public bool IsReturn { get; set; }
        public int? ToStreetId { get; set; }
        //TPL        
        public int? TPLCurrentId { get; set; }
        public int? TPLId { get; set; }
        public string TPLNumber { get; set; }
        public DateTime? TPLCreatedWhen { get; set; }
        public int? TotalItem { get; set; }
        public double TPLPrice { get; set; }
        public double TPLPriceReal { get; set; }
        public bool? IsPushVSE { get; set; }
        public double CompensationValue { get; set; }
        public int? HandlingEmpId { get; set; }
        public int? TypeId { get; set; }
        public string ReasonDelete { get; set; }
        public bool? IsPushImgPickup { get; set; }
        public string SenderCode { get; set; }
        public string SOENTRY { get; set; }
        public Int64? RowNum { get; set; }
        public Int32? TotalCount { get; set; }
        public Proc_GetShipmentCurrentEmp() { }

        public static IEntityProc GetEntityProc(int? userId = null, string statusIds = null, string listShipmentIds = null, string searchText = null, int? pageNumber = 1, int? pageSize = 20)
        {
            SqlParameter UserId = new SqlParameter("@UserId", userId);
            if (!userId.HasValue) UserId.Value = DBNull.Value;
            SqlParameter StatusIds = new SqlParameter("@StatusIds", statusIds);
            if (string.IsNullOrWhiteSpace(statusIds)) StatusIds.Value = DBNull.Value;
            SqlParameter ListShipmentIds = new SqlParameter("@ListShipmentIds", listShipmentIds);
            if (string.IsNullOrWhiteSpace(listShipmentIds)) ListShipmentIds.Value = DBNull.Value;
            SqlParameter SearchText = new SqlParameter("@SearchText", searchText);
            if (string.IsNullOrWhiteSpace(searchText)) SearchText.Value = DBNull.Value;
            SqlParameter PageNumber = new SqlParameter("@PageNUmber", pageNumber);
            SqlParameter PageSize = new SqlParameter("@PageSize", pageSize);

            return new EntityProc(
                $"{ProcName} @UserId,@StatusIds,@ListShipmentIds,@SearchText,@PageNUmber,@PageSize",
                new SqlParameter[] {
                    UserId,
                    StatusIds,
                    ListShipmentIds,
                    SearchText,
                    PageNumber,
                    PageSize
                }
            );
        }
    }
}
