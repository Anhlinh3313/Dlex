using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_CheckKPIUpLoad : IEntityProcView
    {
        public const string ProcName = "Proc_CheckKPIUpLoad";
        [Key]
        public bool? Result { get; set; }
        public string Message { get; set; }

        public Proc_CheckKPIUpLoad() { }

        public static IEntityProc GetEntityProc(string hubRoutingCodes, string cutOffTimeCodes)
        {
            SqlParameter parameter1 = new SqlParameter("@HubRoutingCodes", hubRoutingCodes);
            //
            SqlParameter parameter2 = new SqlParameter("@CutOffTimeCodes", cutOffTimeCodes);
            return new EntityProc(
                $"{ProcName} @HubRoutingCodes, @CutOffTimeCodes",
                new SqlParameter[] {
                    parameter1,
                    parameter2,
                }
            );
        }
    }
}
