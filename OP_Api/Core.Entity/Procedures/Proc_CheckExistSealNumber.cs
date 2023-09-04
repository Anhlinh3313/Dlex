using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_CheckExistSealNumber : IEntityProcView
    {
        public const string ProcName = "Proc_CheckExistSealNumber";
        [Key]
        public int TotalCount { get; set; }

        public Proc_CheckExistSealNumber() { }
        public static IEntityProc GetEntityProc(string sealNumber)
        {
            SqlParameter SealNumber = new SqlParameter("@SealNumber", sealNumber);
            if (string.IsNullOrWhiteSpace(sealNumber)) SealNumber.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @SealNumber",
                new SqlParameter[] {
                    SealNumber
                }
            );
        }
    }
}
