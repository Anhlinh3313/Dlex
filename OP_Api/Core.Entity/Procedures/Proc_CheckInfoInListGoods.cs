using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_CheckInfoInListGoods : IEntityProcView
    {
        public const string ProcName = "Proc_CheckInfoInListGoods";

        [Key]
        public int CountShipment { get; set; }
        public double SumWeight { get; set; }
        public double SumCalWeight { get; set; }
        public int SumTotalBox { get; set; }
        public Proc_CheckInfoInListGoods() { }
        public static IEntityProc GetEntityProc(int listGoodsId)
        {
            SqlParameter sqlParameter1 = new SqlParameter("@ListGoodsId", listGoodsId);
            return new EntityProc(
                $"{ProcName} @ListGoodsId",
                new SqlParameter[] {
                    sqlParameter1
                }
            );
        }
    }
}
