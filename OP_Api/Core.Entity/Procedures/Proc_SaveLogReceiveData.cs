using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_SaveLogReceiveData : IEntityProcView
    {
        public const string ProcName = "Proc_SaveLogReceiveData";
        [Key]
        public bool IsSuccess { get; set; }

        public Proc_SaveLogReceiveData() { }

        public static IEntityProc GetEntityProc(string data, string shipmentNumber = null)
        {
            SqlParameter Data = new SqlParameter("@Data", data);
            if (string.IsNullOrWhiteSpace(data)) Data.Value = DBNull.Value;
            //
            SqlParameter ShipmentNumber = new SqlParameter("@ShipmentNumber", shipmentNumber);
            if (string.IsNullOrWhiteSpace(shipmentNumber)) ShipmentNumber.Value = DBNull.Value;
            //
            return new EntityProc(
                $"{ProcName} @Data, @ShipmentNumber",
                new SqlParameter[] {
                    Data,
                    ShipmentNumber
                }
            );
        }
    }
}
