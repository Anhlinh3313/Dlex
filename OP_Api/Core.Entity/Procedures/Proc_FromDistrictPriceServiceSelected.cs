using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_FromDistrictPriceServiceSelected : IEntityProcView
    {
        public const string ProcName = "Proc_FromDistrictPriceServiceSelected";

        public int Id { get; set; }
        public int DistrictId { get; set; }
        public int AreaId { get; set; }

        public Proc_FromDistrictPriceServiceSelected()
        {

        }

        public static IEntityProc GetEntityProc(int? priceServiceId)
        {

            SqlParameter PriceServiceId = new SqlParameter("@priceServiceId", priceServiceId);
            if (!priceServiceId.HasValue) PriceServiceId.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @priceServiceId",
                new SqlParameter[] {
                PriceServiceId
                }
            );
        }
    }
}
