using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_UpLoadExcelKPI : IEntityProcView
    {
        public const string ProcName = "Proc_UpLoadExcelKPI";

        [Key]
        public bool Result { get; set; }

        public Proc_UpLoadExcelKPI()
        {
        }

        public static IEntityProc GetEntityProc(dynamic tb_UpLoadKPIShipment)
        {

            return new EntityProc(
               $"{ProcName} @Tb_UpLoadKPIShipmentSAP",
               new SqlParameter[] {
                new SqlParameter("@Tb_UpLoadKPIShipmentSAP", SqlDbType.Structured)
                    {
                        TypeName = "Tb_UpLoadKPIShipmentSAP",
                        Value = tb_UpLoadKPIShipment
                    }
               }
           );
        }
    }
}
