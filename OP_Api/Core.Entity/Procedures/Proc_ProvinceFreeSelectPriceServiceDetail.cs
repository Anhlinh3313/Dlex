using Core.Entity.Abstract;
using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_ProvinceFreeSelectPriceServiceDetail : IEntityProcView
    {
        public const string ProcName = "Proc_ProvinceFreeSelectPriceServiceDetail";

        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public Proc_ProvinceFreeSelectPriceServiceDetail()
        {

        }

        public static IEntityProc GetEntityProc(int? areaGroupId, int? priceServiceId)
        {
            SqlParameter parameter1 = new SqlParameter(
           "@areaGroupId", areaGroupId);
            if (!areaGroupId.HasValue)
                parameter1.Value = DBNull.Value;

            SqlParameter parameter2 = new SqlParameter(
            "@priceServiceId", priceServiceId);
            if (!priceServiceId.HasValue)
                parameter2.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @areaGroupId, @priceServiceId",
                new SqlParameter[] {
                parameter1,
                parameter2
                }
            );
        }
    }
}