using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_CopyPriceService : IEntityProcView
    {
        public const string ProcName = "Proc_CopyPriceService";

        [Key]
        public bool Result { get; set; }

        public Proc_CopyPriceService()
        {
        }

        public static IEntityProc GetEntityProc(int priceServiceId, string newPriceServiceCode)
        {
            SqlParameter parameter1 = new SqlParameter(
            "@PriceServiceId", priceServiceId);
            //
            SqlParameter parameter2 = new SqlParameter(
            "@NewCode", newPriceServiceCode);

            return new EntityProc(
                $"{ProcName} @PriceServiceId, @NewCode",
                new SqlParameter[] {
                    parameter1,
                    parameter2
                }
            );
        }
    }
}
