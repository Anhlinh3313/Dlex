using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_DetectAddressTo : IEntityProcView
    {
        public const string ProcName = "Proc_DetectAddressTo";

        public int Id { get; set; }
        public string ToProvinceName { get; set; }
        public string ToDistrictName { get; set; }
        public string ToWardName { get; set; }
        public string ToHubName { get; set; }

        public Proc_DetectAddressTo()
        {
        }

        public static IEntityProc GetEntityProc(string shipmentNumber)
        {
            return new EntityProc(
                $"{ProcName} @ShipmentNumber",
                new SqlParameter[] {
                new SqlParameter("@ShipmentNumber", shipmentNumber)
                }
            );
        }
    }
}
