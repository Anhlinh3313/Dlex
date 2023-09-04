using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_ReportCancelShipment : IEntityProcView
    {
        public const string ProcName = "Proc_ReportCancelShipment";

        [Key]
        public int ShipmentId { get; set; }
        public string ShipmentNumber { get; set; }
        public int? UserId { get; set; }
        public string UserCode { get; set; }
        public string UserFullName { get; set; }
        public int? UserHubId { get; set; }
        public string UserHubName { get; set; }
        public DateTime DeletedShipmentAt { get; set; }
        public int? RequestShipmentId { get; set; }
        public DateTime CreatedWhen { get; set; }
        public string CusNote { get; set; }
        public DateTime? OrderDate { get; set; }


        public int? PaymentTypeId { get; set; }
        public string PaymentTypeName { get; set; }
        public int? ServiceId { get; set; }
        public string ServiceName { get; set; }

        public double DefaultPrice { get; set; }
        public double FuelPrice { get; set; }
        public double RemoteAreasPrice { get; set; }
        public double TotalDVGT { get; set; }
        public double VATPrice { get; set; }
        public double TotalPrice { get; set; }
        public int? DeliverUserId { get; set; }
        public string DeliveryUserName { get; set; }
        public int? SenderId { get; set; }
        public string SenderName { get; set; }
        public string SenderPhone { get; set; }
        public string CompanyFrom { get; set; }
        public string PickingAddress { get; set; }
        public string AddressNoteFrom { get; set; }

        public int? FromProvinceId { get; set; }
        public string FromProvinceName { get; set; }
        public int? FromHubId { get; set; }
        public string FromHubName { get; set; }
        public int? FromHubRoutingId { get; set; }
        public string FromHubRoutingName { get; set; }
        public int? ReceiverId { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public string CompanyTo { get; set; }
        public string ShippingAddress { get; set; }
        public string AddressNoteTo { get; set; }
        public string RealRecipientName { get; set; }
        public DateTime? EndDeliveryTime { get; set; }
        public int? ToProvinceId { get; set; }
        public string ToProvinceName { get; set; }
        public int? ToHubId { get; set; }
        public string ToHubName { get; set; }
        public int? ToHubRoutingId { get; set; }
        public string ToHubRoutingName { get; set; }
        public string ReasonDelete { get; set; }
        public Proc_ReportCancelShipment()
        {

        }

        public static IEntityProc GetEntityProc(int? hubId = null, int? empId = null, int? senderId = null, DateTime? dateFrom = null, DateTime? dateTo = null, string searchText = null)
        {
            SqlParameter parameter1 = new SqlParameter("@HubId", hubId);
            if (!hubId.HasValue) parameter1.Value = DBNull.Value;
            SqlParameter parameter2 = new SqlParameter("@EmpId", empId);
            if (!empId.HasValue) parameter2.Value = DBNull.Value;
            SqlParameter paramSenderId = new SqlParameter("@SenderId", senderId);
            if (!senderId.HasValue) paramSenderId.Value = DBNull.Value;
            SqlParameter parameter3 = new SqlParameter("@DateFrom", dateFrom);
            if (!dateFrom.HasValue) parameter3.Value = DBNull.Value;
            SqlParameter parameter4 = new SqlParameter("@DateTo", dateTo);
            if (!dateTo.HasValue) parameter4.Value = DBNull.Value;

            if (string.IsNullOrWhiteSpace(searchText))
            {
                searchText = "";
            }

            return new EntityProc(
                $"{ProcName} @HubId, @EmpId, @SenderId, @DateFrom, @DateTo, @SearchText",
                new SqlParameter[] {
                parameter1,
                parameter2,
                paramSenderId,
                parameter3,
                parameter4,
                new SqlParameter("@SearchText", searchText),
                }
            );
        }
    }
}
