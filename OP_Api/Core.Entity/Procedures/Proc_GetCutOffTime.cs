using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetCutOffTime : IEntityProcView
    {
        public const string ProcName = "Proc_GetCutOffTime";
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool? Monday { get; set; }
        public bool? Tuesday { get; set; }
        public bool? Wednesday { get; set; }
        public bool? Thursday { get; set; }
        public bool? Friday { get; set; }
        public bool? Saturday { get; set; }
        public bool? Sunday { get; set; }
        public DateTime? CutTime1st { get; set; }
        public DateTime? CutTime2nd { get; set; }
        public DateTime? CutTime3rd { get; set; }

        public Proc_GetCutOffTime() { }
        public static IEntityProc GetEntityProc()
        {
            return new EntityProc(
                $"{ProcName}",
                new SqlParameter[] { }
            );
        }
    }
}
