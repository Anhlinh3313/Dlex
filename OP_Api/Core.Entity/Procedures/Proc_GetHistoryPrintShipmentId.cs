using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetHistoryPrintShipmentId : IEntityProcView
    {
        public const string ProcName = "Proc_GetHistoryPrintShipmentId";

        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public DateTime CreatedWhen { get; set; }
        public int PrintTypeId { get; set; }
        public string PrintTypeName { get; set; }
        public int HubId { get; set; }
        public string HubName { get; set; }
        public int StatusPrintId { get; set; }
        public string StatusPrintName { get; set; }
        public Proc_GetHistoryPrintShipmentId()
        {

        }

        public static IEntityProc GetEntityProc(int shipmentId, int? hubId = null, int? empId = null, int? typePrintId = null, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
           // SqlParameter parameter1 = new SqlParameter(
           //"@ShipmentId", shipmentId);
           // if (!shipmentId.HasValue)
           //     parameter1.Value = DBNull.Value;
            SqlParameter parameter2 = new SqlParameter(
           "@HubId", hubId);
            if (!hubId.HasValue)
                parameter2.Value = DBNull.Value;
            SqlParameter parameter3 = new SqlParameter(
            "@EmpId", empId);
            if (!empId.HasValue)
                parameter3.Value = DBNull.Value;
            SqlParameter parameter4 = new SqlParameter(
            "@TypePrintId", typePrintId);
            if (!typePrintId.HasValue)
                parameter4.Value = DBNull.Value;
            SqlParameter parameter5 = new SqlParameter(
            "@DateFrom", dateFrom);
            if (!dateFrom.HasValue)
                parameter5.Value = DBNull.Value;
            SqlParameter parameter6 = new SqlParameter(
            "@DateTo", dateTo);
            if (!dateTo.HasValue)
                parameter6.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @ShipmentId, @HubId, @EmpId, @TypePrintId, @DateFrom, @DateTo",
                new SqlParameter[] {
                    new SqlParameter("@ShipmentId", shipmentId),
                    parameter2,
                    parameter3,
                    parameter4,
                    parameter5,
                    parameter6
                }
            );
        }
    }
}
