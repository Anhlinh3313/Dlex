using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_PriceService : IEntityProcView
    {
        public const string ProcName = "Proc_PriceService";

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
        public bool? IsPublic { get; set; }
        public double Price { get; set; }
        public double WeightFrom { get; set; }
        public double WeightTo { get; set; }
        public int FormulaId { get; set; }
        public double WeightPlus { get; set; }
        public int? Plus { get; set; }
        public double? Weight { get; set; }
        public double ValueFrom { get; set; }
        public double ValueTo { get; set; }
        public bool IsKeepWeight { get; set; }
        public int? PricingTypeId { get; set; }
        public double ExpectedTime { get; set; }
        /// <summary>
        /// /
        /// </summary>

        public Proc_PriceService()
        {
        }

        public static IEntityProc GetEntityProc(int plus, int? senderId, int? fromProvinceId, int? fromDistrictId, int serviceId, int? toDistrictId, double? weight, int? StructureId, int PricingTypeId, double totalItem, double insurrance, int? priceServiceId, double cod, double? km = null)
        {
            //int plus = 0;
            SqlParameter parameter0 = new SqlParameter(
            "@IsPlus", plus);
            //
            SqlParameter parameter1 = new SqlParameter(
           "@SenderId", senderId);
            if (!senderId.HasValue)
                parameter1.Value = DBNull.Value;
            //
            SqlParameter parameterFromProvinceId = new SqlParameter(
            "@FromProvinceId", fromProvinceId);
            if (!fromProvinceId.HasValue) parameterFromProvinceId.Value = DBNull.Value;

            SqlParameter parameter2 = new SqlParameter(
            "@FromDistrictId", fromDistrictId);
            if (!fromDistrictId.HasValue) parameter2.Value = DBNull.Value;
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

            SqlParameter COD = new SqlParameter(
            "@COD", cod);

            SqlParameter KM = new SqlParameter(
            "@KM", km);
            if (!km.HasValue)
                KM.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @IsPlus, @SenderId, @FromProvinceId, @FromDistrictId, @ServiceId, @ToDistrictId, @Weight, @StructureId, @PricingTypeId, @TotalItem, @Insurrance, @PriceServiceId, @COD, @KM",
                new SqlParameter[] {
                    parameter0,
                    parameter1,
                    parameterFromProvinceId,
                    parameter2,
                    parameter3,
                    parameter4,
                    parameter5,
                    parameter6,
                    parameter7,
                    parameter8,
                    parameter9,
                    parameter10,
                    COD,
                    KM
                }
            );
        }
    }
}
