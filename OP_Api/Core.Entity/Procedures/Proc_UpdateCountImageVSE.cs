using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_UpdateCountImageVSE : IEntityProcView
    {
        public const string ProcName = "Proc_UpdateCountImageVSE";
        [Key]
        public int Result { get; set; }

        public Proc_UpdateCountImageVSE() { }

        public static IEntityProc GetEntityProc(int id, bool isPush, string data, string result)
        {
            SqlParameter parameter1 = new SqlParameter(
           "@Id", id);
            //
            SqlParameter parameter2 = new SqlParameter(
            "@IsPush", isPush);
            //
            SqlParameter parameter3 = new SqlParameter(
            "@Data", data);
            //
            SqlParameter parameter4 = new SqlParameter(
            "@Result", result);

            return new EntityProc(
                $"{ProcName} @Id, @IsPush, @Data, @Result",
                new SqlParameter[] {
                    parameter1,
                    parameter2,
                    parameter3,
                    parameter4
                }
            );
        }
    }
}
