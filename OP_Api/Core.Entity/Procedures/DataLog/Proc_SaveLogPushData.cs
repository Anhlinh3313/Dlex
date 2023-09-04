using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;
namespace Core.Entity.Procedures
{
    public class Proc_SaveLogPushData : IEntityProcView
    {
        public const string ProcName = "Proc_SaveLogPushData";
        [Key]
        public Guid FakeId { get; set; }

        public Proc_SaveLogPushData() { }

        public static IEntityProc GetEntityProc(string data, string result = null, bool? isSuccess = null)
        {
            SqlParameter Data = new SqlParameter("@Data", data);
            if (string.IsNullOrWhiteSpace(data)) Data.Value = DBNull.Value;
            //
            SqlParameter Result = new SqlParameter("@Result", result);
            if (string.IsNullOrWhiteSpace(result)) Result.Value = DBNull.Value;
            //
            SqlParameter IsSuccess = new SqlParameter("@IsSuccess", isSuccess);
            if (!isSuccess.HasValue) IsSuccess.Value = DBNull.Value;
            return new EntityProc(
                $"{ProcName} @Data, @Result, @IsSuccess",
                new SqlParameter[] {
                    Data,
                    Result,
                    IsSuccess
                }
            );
        }
    }
}
