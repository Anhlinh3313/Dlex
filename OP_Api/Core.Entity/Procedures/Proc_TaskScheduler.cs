using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_TaskScheduler : IEntityProcView
    {
        public const string ProcName = "Proc_TaskScheduler";
        [Key]
        public int Result { get; set; }

        public Proc_TaskScheduler() { }

        public static IEntityProc GetEntityProc(string code, double? nextTime, string note)
        {
            SqlParameter parameter1 = new SqlParameter(
           "@Code", code);
            //
            SqlParameter parameter3 = new SqlParameter(
            "@NextTime", nextTime);
            if (!nextTime.HasValue)
                parameter3.Value = DBNull.Value;
            //
            SqlParameter parameter4 = new SqlParameter(
            "@Note", note);

            return new EntityProc(
                $"{ProcName} @Code, @NextTime, @Note",
                new SqlParameter[] {
                    parameter1,
                    parameter3,
                    parameter4
                }
            );
        }
    }
}
