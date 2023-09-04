using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_UpdateKPIShipmentSAP : IEntityProcView
    {
        public const string ProcName = "Proc_UpdateKPIShipmentSAP";

        [Key]
        public bool Result { get; set; }

        public Proc_UpdateKPIShipmentSAP()
        {
        }

        public static IEntityProc GetEntityProc(dynamic tb_UpLoadKPIShipment)
        {

            return new EntityProc(
               $"{ProcName} @Tb_UpdateKPIShipmentSAP",
               new SqlParameter[] {
                new SqlParameter("@Tb_UpdateKPIShipmentSAP", SqlDbType.Structured)
                    {
                        TypeName = "Tb_UpdateKPIShipmentSAP",
                        Value = tb_UpLoadKPIShipment
                    }
               }
           );
        }
    }
}
