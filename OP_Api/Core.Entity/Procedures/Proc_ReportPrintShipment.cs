using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_ReportPrintShipment : IEntityProcView
    {
        public const string ProcName = "Proc_ReportPrintShipment";

        [Key]
        public int ShipmentId { get; set; }
        public string ShipmentNumber { get; set; }
        public DateTime CreatedWhen { get; set; }
        public int TotalPrintDetail { get; set; }
        public int TotalPrintCodeA4 { get; set; }
        public int TotalPrintSticker { get; set; }
        public int TotalPrintBillAndAdviceOfDelivery { get; set; }
        public int TotalPrintAdviceOfDelivery { get; set; }
        public int TotalPrintBox { get; set; }
        public int TotalPrintPickup { get; set; }
        public Proc_ReportPrintShipment()
        {

        }

        public static IEntityProc GetEntityProc(int? hubId, int? empId = null, int? typePrintId = null, DateTime? dateFrom = null, DateTime? dateTo = null, string searchText = null)
        {
            SqlParameter parameter1 = new SqlParameter(
           "@HubId", hubId);
            if (!hubId.HasValue)
                parameter1.Value = DBNull.Value;
            SqlParameter parameter2 = new SqlParameter(
            "@EmpId", empId);
            if (!empId.HasValue)
                parameter2.Value = DBNull.Value;
            SqlParameter parameter3 = new SqlParameter(
            "@TypePrintId", typePrintId);
            if (!typePrintId.HasValue)
                parameter3.Value = DBNull.Value;
            SqlParameter parameter4 = new SqlParameter(
            "@DateFrom", dateFrom);
            if (!dateFrom.HasValue)
                parameter4.Value = DBNull.Value;
            SqlParameter parameter5 = new SqlParameter(
            "@DateTo", dateTo);
            if (!dateTo.HasValue)
                parameter5.Value = DBNull.Value;

            if (string.IsNullOrWhiteSpace(searchText))
            {
                searchText = "";
            }

            return new EntityProc(
                $"{ProcName} @HubId, @EmpId, @TypePrintId, @DateFrom, @DateTo, @SearchText",
                new SqlParameter[] {
                parameter1,
                parameter2,
                parameter3,
                parameter4,
                parameter5,
                new SqlParameter("@SearchText", searchText),
                }
            );
        }
    }
}
