using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_UpdateRealExportSAP : IEntityProcView
    {
        public const string ProcName = "Proc_UpdateRealExportSAP";

        [Key]
        public bool Result { get; set; }


        public Proc_UpdateRealExportSAP() { }

        public static IEntityProc GetEntityProc(int shipmentId,int kpiTypeid, DateTime? dateTime)
        {
            SqlParameter parameter1 = new SqlParameter("@ShipmentId", shipmentId + "");

            SqlParameter parameter2 = new SqlParameter("@KPITypeId", kpiTypeid + "");

            SqlParameter parameter3 = new SqlParameter("@DateTime", dateTime);
            if (!dateTime.HasValue)
                parameter3.Value = DBNull.Value;
            //      
            return new EntityProc(
                $"{ProcName} @ShipmentId,@KPITypeId,@DateTime",
                new SqlParameter[] {
                parameter1,parameter2,parameter3
                }
            );
        }
    }
}
