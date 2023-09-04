using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_CopyPriceList : IEntityProcView
    {
        public const string ProcName = "Proc_CopyPriceList";

        [Key]
        public bool Result { get; set; }

        public Proc_CopyPriceList()
        {
        }

        public static IEntityProc GetEntityProc(int priceiListCopyId, int priceListNewId)
        {
            return new EntityProc(
                $"{ProcName} @PriceListCopyId, @PriceListNewId",
                new SqlParameter[] {
                    new SqlParameter("@PriceListCopyId", priceiListCopyId),
                    new SqlParameter("@PriceListNewId", priceListNewId)
                }
            );
        }
    }
}
