using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_UpdateCountPushVSE : IEntityProcView
    {
        public const string ProcName = "Proc_UpdateCountPushVSE";
        [Key]
        public Guid FakeId { get; set; }
        public int Result { get; set; }

        public Proc_UpdateCountPushVSE() { }

        public static IEntityProc GetEntityProc(int id, bool isPush, string data, string result, int? flag = null)
        {
            SqlParameter parameter1 = new SqlParameter(
           "@Id", id);
            //
            SqlParameter parameter2 = new SqlParameter(
            "@IsPush", isPush);
            //
            SqlParameter parameter3 = new SqlParameter(
            "@Data", data);
            if (string.IsNullOrWhiteSpace(data)) parameter3.Value = DBNull.Value;
            //
            SqlParameter parameter4 = new SqlParameter(
           "@Result", result);
            if (string.IsNullOrWhiteSpace(result)) parameter4.Value = DBNull.Value;

            SqlParameter FLag = new SqlParameter(
           "@FLag", flag);
            if (!flag.HasValue) FLag.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @Id, @IsPush, @Data, @Result, @flag",
                new SqlParameter[] {
                    parameter1,
                    parameter2,
                    parameter3,
                    parameter4, FLag
                }
            );
        }
    }
}
