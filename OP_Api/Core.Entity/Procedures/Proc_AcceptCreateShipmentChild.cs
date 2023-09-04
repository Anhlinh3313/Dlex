using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_AcceptCreateShipmentChild : IEntityProcView
    {
        public const string ProcName = "Proc_AcceptCreateShipmentChild";
        [Key]
        public int Result { get; set; }

        public Proc_AcceptCreateShipmentChild() { }

        public static IEntityProc GetEntityProc(int shipmentId, bool IsPaymentChild)
        {
            SqlParameter parameter1 = new SqlParameter("@ShipmentId", shipmentId);
            //
            SqlParameter parameter2 = new SqlParameter("@IsPaymentChild", IsPaymentChild);
            return new EntityProc(
                $"{ProcName} @ShipmentId, @IsPaymentChild",
                new SqlParameter[] {
                    parameter1,
                    parameter2,
                }
            );
        }
    }
}
