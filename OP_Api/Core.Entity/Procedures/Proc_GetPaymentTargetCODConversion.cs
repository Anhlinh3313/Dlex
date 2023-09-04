using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetPaymentTargetCODConversion : IEntityProcView
    {
        public const string ProcName = "Proc_GetPaymentTargetCODConversion";

        [Key]
        public int PaymentTargetCODConversion { get; set; }


        public Proc_GetPaymentTargetCODConversion() { }

        public static IEntityProc GetEntityProc(int shipmentId)
        {
            SqlParameter ShipmentId = new SqlParameter("@ShipmentId", shipmentId + "");
            //      
            return new EntityProc(
                $"{ProcName} @ShipmentId",
                new SqlParameter[] {
                ShipmentId,
                }
            );
        }
    }
}
