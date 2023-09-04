using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_ReportShipmentVersion: IEntityProcView
    {
        public const string ProcName = "Proc_ReportShipmentVersion";

        [Key]
        public int Id { get; set; }
        public int ShipmentId { get; set; }
        public string ReShipmentNumber { get; set; }
        public string ShopCode { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public string CompanyTo { get; set; }
        public string ShippingAddress { get; set; }
        public string AddressNoteTo { get; set; }
        public DateTime CreatedWhen { get; set; }
        public string CreatedByName { get; set; }
        public string Version { get; set; }
        public string ToProvinceName { get; set; }
        public string ToDistrictName { get; set; }
        public string ToWardName { get; set; }
        public string ToHubName { get; set; }
        public string CurrentHubName { get; set; }
        public double? Weight { get; set; }
        public double? CalWeight { get; set; }
        public int TotalBox { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public string SenderName { get; set; }
        public string SenderPhone { get; set; }
        public int SenderId { get; set; }
        public string CompanyFrom { get; set; }
        public string PickingAddress { get; set; }
        public string AddressNoteFrom { get; set; }
        public string FromProvinceName { get; set; }
        public string FromDistrictName { get; set; }
        public string FromWardName { get; set; }
        public string FromHubName { get; set; }
        public string ServiceName { get; set; }
        public string StructureName { get; set; }
        public string PaymentTypeName { get; set; }
        public string CurrentUserName { get; set; }
        public string ShipmentStatusName { get; set; }
        public double FuelPrice { get; set; }
        public double TotalDVGT { get; set; }
        public double OtherPrice { get; set; }
        public double VATPrice { get; set; }
        public double DefaultPrice { get; set; }
        public double TotalPrice { get; set; }
        public double RemoteAreasPrice { get; set; }
        public double Insured { get; set; }
        public double COD { get; set; }
        public string Note { get; set; }
        public Int64? RowNum { get; set; }
        public int? TotalCount { get; set; }
        public int TotalBoxs { get; set; }

        public Proc_ReportShipmentVersion()
        {
        }

        public static IEntityProc GetEntityProc(
            DateTime? dateFrom = null,
            DateTime? dateTo = null, 
            int? hubId = null,
            int? senderId = null, 
            int? empId = null, 
            int? shipmentId = null, 
            string shipmentNumber = null, 
            int? pageNumber = null, 
            int? pageSize = null, 
            bool? isSortByCOD = false)
        {
            SqlParameter DateFrom = new SqlParameter("@DateFrom", dateFrom);
            if (!dateFrom.HasValue)
                DateFrom.Value = DBNull.Value;

            SqlParameter DateTo = new SqlParameter("@DateTo", dateTo);
            if (!dateTo.HasValue)
                DateTo.Value = DBNull.Value;

            SqlParameter HubId = new SqlParameter("@HubId", hubId);
            if (!hubId.HasValue)
                HubId.Value = DBNull.Value;

            SqlParameter SenderId = new SqlParameter("@SenderId", senderId);
            if (!senderId.HasValue)
                SenderId.Value = DBNull.Value;

            SqlParameter EmpId = new SqlParameter("@EmpId", empId);
            if (!empId.HasValue)
                EmpId.Value = DBNull.Value;
            
            SqlParameter ShipmentId = new SqlParameter("@ShipmentId", shipmentId);
            if (!shipmentId.HasValue)
                ShipmentId.Value = DBNull.Value;

            if (!string.IsNullOrWhiteSpace(shipmentNumber))
            {
                shipmentNumber = shipmentNumber.Replace("/r/n", " ");
                shipmentNumber = shipmentNumber.Trim().Replace(" ", ",");
            }
            SqlParameter ShipmentNumber = new SqlParameter("@ShipmentNumber", shipmentNumber);
            if (string.IsNullOrWhiteSpace(shipmentNumber)) ShipmentNumber.Value = DBNull.Value;

            SqlParameter PageNumber = new SqlParameter("@PageNumber", pageNumber);
            if (!pageNumber.HasValue)
                PageNumber.Value = DBNull.Value;
            SqlParameter PageSize = new SqlParameter("@PageSize", pageSize);
            if (!pageSize.HasValue)
                PageSize.Value = DBNull.Value;

            SqlParameter IsSortByCOD = new SqlParameter("@IsSortByCOD", isSortByCOD);
            if (!isSortByCOD.HasValue)
                IsSortByCOD.Value = false;

            return new EntityProc(
                $"{ProcName} @DateFrom,@DateTo,@HubId,@SenderId,@EmpId,@ShipmentId,@ShipmentNumber,@PageNumber,@PageSize,@IsSortByCOD",
                new SqlParameter[] {
                    DateFrom,
                    DateTo,
                    HubId,
                    SenderId,
                    EmpId,
                    ShipmentId,
                    ShipmentNumber,
                    PageNumber,
                    PageSize,
                    IsSortByCOD
                }
            );
        }
    }
}
