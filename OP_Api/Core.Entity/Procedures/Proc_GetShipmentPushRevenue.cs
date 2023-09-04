using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetShipmentPushRevenue : IEntityProcView
    {
        public const string ProcName = "Proc_GetShipmentPushRevenue";
        [Key]
        public int ShipmentId { get; set; }
        public int DeliveryStatus { get; set; }
        public string StatusCOD { get; set; }
        public string FromHubCode { get; set; }
        public string FromHubName { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? EndPickTime { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? EndDeliveryTime { get; set; }
        public string SenderCode { get; set; }
        public string SenderName { get; set; }
        public string DriverCode { get; set; }
        public string DriverName { get; set; }
        public double Insured { get; set; }
        public double COD { get; set; }
        public double TotalPrice { get; set; }
        public double PriceCOD { get; set; }
        public string Reason { get; set; }
        public string FromProvinceCode { get; set; }
        public string FromProvinceName { get; set; }

        public Proc_GetShipmentPushRevenue() { }
        public static IEntityProc GetEntityProc(string shipmnetNumber)
        {
            SqlParameter ShipmentNumber = new SqlParameter("@ShipmentNumber", shipmnetNumber);
            if (string.IsNullOrWhiteSpace(shipmnetNumber)) ShipmentNumber.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @ShipmentNumber",
                new SqlParameter[] {
                    ShipmentNumber
                }
            );
        }
    }
}
