using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_FromProvincePriceServiceSelected : IEntityProcView
    {
        public const string ProcName = "Proc_FromProvincePriceServiceSelected";

        public int Id { get; set; }
        public int ProvinceId { get; set; }
        public int AreaId { get; set; }

        public Proc_FromProvincePriceServiceSelected()
        {

        }

        public static IEntityProc GetEntityProc(int? priceServiceId)
        {

            SqlParameter parameter1 = new SqlParameter(
            "@priceServiceId", priceServiceId);
            if (!priceServiceId.HasValue)
                parameter1.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @priceServiceId",
                new SqlParameter[] {
                parameter1
                }
            );
        }
    }
}
