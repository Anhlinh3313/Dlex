using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_CheckUpLoadExcelShipment : IEntityProcView
    {
        public const string ProcName = "Proc_CheckUpLoadExcelShipment"; 
        [Key]
        public bool? Result { get; set; }
        public string Message { get; set; }

        public Proc_CheckUpLoadExcelShipment() { }

        public static IEntityProc GetEntityProc(string shipmentNumbers,string requestCodes)
        {
            SqlParameter parameter1 = new SqlParameter("@ShipmentNumbers", shipmentNumbers);
            if (string.IsNullOrWhiteSpace(shipmentNumbers))
                parameter1.Value = DBNull.Value;
            SqlParameter parameter2 = new SqlParameter("@RequestCodes", requestCodes);
            if (string.IsNullOrWhiteSpace(requestCodes))
                parameter2.Value = DBNull.Value;
            return new EntityProc(
                $"{ProcName} @ShipmentNumbers,@RequestCodes",
                new SqlParameter[] {
                    parameter1,parameter2
                }
            );
        }
    }
}
