using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_BlockListGoods : IEntityProcView
    {
        public const string ProcName = "Proc_BlockListGoods";
        [Key]
        public int Result { get; set; }

        public Proc_BlockListGoods() { }
        public static IEntityProc GetEntityProc(int listGoodsId, int? empId = null)
        {
            SqlParameter sqlParameter1 = new SqlParameter("@ListGoodsId", listGoodsId);
            SqlParameter sqlEmpId = new SqlParameter("@EmpId", empId);
            if (!empId.HasValue) sqlEmpId.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @ListGoodsId, @EmpId",
                new SqlParameter[] {
                    sqlParameter1,sqlEmpId
                }
            );
        }
    }
}
