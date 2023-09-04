using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_CheckExistData : IEntityProcView
    {
        public const string ProcName = "Proc_CheckExistData";
        [Key]
        public Guid FakeId { get; set; }
        public int DataCount { get; set; }

        public Proc_CheckExistData() { }
        public static IEntityProc GetEntityProc(int companyId, string tableName, string columnName, string value, string where)
        {
            SqlParameter CompanyId = new SqlParameter("@CompanyId", companyId);

            SqlParameter TableName = new SqlParameter("@TableName", tableName);
            if (string.IsNullOrWhiteSpace(tableName)) TableName.Value = DBNull.Value;

            SqlParameter ColumnName = new SqlParameter("@ColumnName", columnName);
            if (string.IsNullOrWhiteSpace(columnName)) ColumnName.Value = DBNull.Value;

            SqlParameter Value = new SqlParameter("@Value", value);
            if (string.IsNullOrWhiteSpace(value)) Value.Value = DBNull.Value;

            SqlParameter Where = new SqlParameter("@Where", where);
            if (string.IsNullOrWhiteSpace(where)) Where.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @CompanyId,@TableName,@ColumnName,@Value,@Where",
                new SqlParameter[] {
                    CompanyId,
                    TableName,
                    ColumnName,
                    Value,
                    Where
                }
            );
        }
    }
}
