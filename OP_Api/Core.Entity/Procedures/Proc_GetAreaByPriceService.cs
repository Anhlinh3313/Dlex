using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetAreaByPriceService : IEntityProcView
    {
        public const string ProcName = "Proc_GetAreaByPriceService";
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double ExpectedTime { get; set; }
        public string ConcurrencyStamp { get; set; }
        public bool IsEnabled { get; set; }

        public Proc_GetAreaByPriceService()
        {
        }

        public static IEntityProc GetEntityProc(int? PriceServiceId)
        {
            SqlParameter parameter1 = new SqlParameter(
           "@PriceServiceId", PriceServiceId);
            if (!PriceServiceId.HasValue)
                parameter1.Value = DBNull.Value;
            //
            return new EntityProc(
                $"{ProcName} @PriceServiceId",
                new SqlParameter[] {
                    parameter1
                }
            );
        }
    }
}
