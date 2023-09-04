using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_PriceServicePlus : IEntityProcView
    {
        public const string ProcName = "Proc_PriceServicePlus";

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int ServiceId { set; get; }
        public int? WeightGroupId { get; set; }
        public int? AreaGroupId { get; set; }
        public int? PriceListId { get; set; }
        public bool IsAuto { get; set; }
        public double VATPercent { get; set; }
        public double FuelPercent { get; set; }
        public double DIM { get; set; }
        public double RemoteAreasPricePercent { get; set; }
        public DateTime? PublicDateFrom { set; get; }
        public DateTime? PublicDateTo { set; get; }
        public double Price { get; set; }
        public double WeightFrom { get; set; }
        public double WeightTo { get; set; }
        public int FormulaId { get; set; }
        public double WeightPlus { get; set; }
        public int? Plus { get; set; }
        public double? Weight { get; set; }
        /// <summary>
        /// /
        /// </summary>

        public Proc_PriceServicePlus()
        {
        }

        public static IEntityProc GetEntityProc(int? senderId, int fromDistrictId, int serviceId, int? toDistrictId, double? weight, int? StructureId, int PricingTypeId, int totalItem, double insurrance, int? priceServiceId)
        {
            int plus = 1;
            SqlParameter parameter0 = new SqlParameter(
            "@Plus", plus);
            //
            SqlParameter parameter1 = new SqlParameter(
           "@SenderId", senderId);
            if (!senderId.HasValue)
                parameter1.Value = DBNull.Value;
            //
            SqlParameter parameter2 = new SqlParameter(
            "@FromDistrictId", fromDistrictId);
            //
            SqlParameter parameter3 = new SqlParameter(
            "@ServiceId", serviceId);
            //
            SqlParameter parameter4 = new SqlParameter(
            "@ToDistrictId", toDistrictId);
            if (!toDistrictId.HasValue)
                parameter4.Value = DBNull.Value;
            //
            SqlParameter parameter5 = new SqlParameter(
            "@Weight", weight);
            if (!weight.HasValue)
                parameter5.Value = DBNull.Value;
            //
            SqlParameter parameter6 = new SqlParameter(
            "@StructureId", StructureId);
            if (!StructureId.HasValue)
                parameter6.Value = DBNull.Value;
            //
            SqlParameter parameter7 = new SqlParameter(
            "@PricingTypeId", PricingTypeId);
            //
            SqlParameter parameter8 = new SqlParameter(
            "@TotalItem", totalItem);
            //
            SqlParameter parameter9 = new SqlParameter(
            "@Insurrance", insurrance);
            //
            SqlParameter parameter10 = new SqlParameter(
            "@PriceServiceId", priceServiceId);
            if (!priceServiceId.HasValue)
                parameter10.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @Plus, @SenderId, @FromDistrictId, @ServiceId, @ToDistrictId, @Weight, @StructureId, @PricingTypeId, @TotalItem, @Insurrance, @PriceServiceId",
                new SqlParameter[] {
                    parameter0,
                    parameter1,
                    parameter2,
                    parameter3,
                    parameter4,
                    parameter5,
                    parameter6,
                    parameter7,
                    parameter8,
                    parameter9,
                    parameter10
                }
            );
        }
    }
}
