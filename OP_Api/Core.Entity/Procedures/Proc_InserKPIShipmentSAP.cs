﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_InserKPIShipmentSAP : IEntityProcView
    {
        public const string ProcName = "Proc_InserKPIShipmentSAP";

        [Key]
        public bool Result { get; set; }

        public Proc_InserKPIShipmentSAP()
        {
        }

        public static IEntityProc GetEntityProc(dynamic tableEmployyee)
        {

            return new EntityProc(
               $"{ProcName} @Tb_KPIShipmentSAP",
               new SqlParameter[] {
                new SqlParameter("@Tb_KPIShipmentSAP", SqlDbType.Structured)
                    {
                        TypeName = "Tb_KPIShipmentSAP",
                        Value = tableEmployyee
                    }
               }
           );
        }
    }
}
