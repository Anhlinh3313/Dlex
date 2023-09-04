using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetListShipmentByRequestShipmentId : IEntityProcView
    {
        public const string ProcName = "Proc_GetListShipmentByRequestShipmentId";


        [Key]
        public int Id { get; set; }
        public string ShipmentNumber { get; set; }
        public string ReShipmentNumber { get; set; }
        public DateTime? OrderDate { get; set; }
        public string AddressNoteFrom { get; set; }
        public string FromHubRoutingName { get; set; }
        public string ReceiverCode { get; set; }
        public string AddressNoteTo { get; set; }
        public string ToHubRoutingName { get; set; }
        public double? COD { get; set; }
        public double? CalWeight { get; set; }
        public double? CusWeight { get; set; }
        public string StructureName { get; set; }
        public double? DefaultPrice { get; set; }
        public double? RemoteAreasPrice { get; set; }
        public double? PriceReturn { get; set; }
        public double? TotalDVGT { get; set; }
        public double? TotalPrice { get; set; }
        public int? TotalItem { get; set; }
        public double? PriceCOD { get; set; }
        public string PaymentCODTypeName { get; set; }
        public string Content { get; set; }
        public string CusNote { get; set; }
        public string Note { get; set; }
        public bool? IsReturn { get; set; }
        public string ShipmentStatusName { get; set; }
        public string RealRecipientName { get; set; }
        public DateTime? EndDeliveryTime { get; set; }
        public string DeliveryNote { get; set; }
        public bool? ISPrioritize { get; set; }
        public bool? IsIncidents { get; set; }
        public int? SenderId { get; set; }
        public string ReasonName { get; set; }
        public int? KmNumberTW { get; set; }
        public int? KmNumberTD { get; set; }
        public string CompanyNameFrom { get; set; }
        public double? VATPrice { get; set; }
        public double? FuelPrice { get; set; }
        public int? ShipmentStatusId { get; set; }
        public int? PaymentTypeId { get; set; }
        public int? PaymentCODTypeId { get; set; }
        public bool? IsCreditTransfer { get; set; }
        public int? ListCustomerPaymentCODId { get; set; }
        public int? ListCustomerPaymentTotalPriceId { get; set; }

        public double? Insured { get; set; }
        public string ServiceName { get; set; }
        public string FromWardName { get; set; }
        public string ToWardName { get; set; }
        public string FromDistrictName { get; set; }
        public string ToDistrictName { get; set;}
        public string FromProvinceName { get; set; }
        public string ToProvinceName { get; set; }
        public string ShippingAddress { get; set; }
        public string PickingAddress { get; set; }
        public string SenderName { get; set; }
        public string SenderPhone { get; set; }
        public string SenderCode { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public string FromHubName { get; set; }
        public string ToHubName { get; set; }
        public int? TotalBox { get; set; }
        public double? Weight { get; set; }
        public int? TotalCount { get; set; }

        public static IEntityProc GetEntityProc(
            int? userEmpId,
            string searchText,
            int? pageNumber,
            int? pageSize
            )
        {
            SqlParameter UserEmpId = new SqlParameter("@UserEmpId", userEmpId);
            if (!userEmpId.HasValue)
                UserEmpId.Value = DBNull.Value;
            SqlParameter SearchText = new SqlParameter("@SearchText", searchText);
            if (string.IsNullOrEmpty(searchText))
                SearchText.Value = DBNull.Value;
            SqlParameter PageNumber = new SqlParameter("@PageNumber", pageNumber);
            if (!pageNumber.HasValue)
                PageNumber.Value = DBNull.Value;

            SqlParameter PageSize = new SqlParameter("@PageSize", pageSize);
            if (!pageSize.HasValue)
                PageSize.Value = DBNull.Value;
            return new EntityProc(
                $"{ProcName} @UserEmpId, @SearchText, @PageNumber, @PageSize ",
                new SqlParameter[]
                {
                    UserEmpId,
                    SearchText,
                    PageNumber,
                    PageSize
                }
            );
        }
    }
}
