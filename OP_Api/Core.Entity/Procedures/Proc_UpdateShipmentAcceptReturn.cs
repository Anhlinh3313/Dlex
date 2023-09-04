using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_UpdateShipmentAcceptReturn : IEntityProcView
    {
        public const string ProcName = "Proc_UpdateShipmentAcceptReturn";

        [Key]
        public int Success { get; set; }

        public Proc_UpdateShipmentAcceptReturn() { }

        public static IEntityProc GetEntityProc(int id, int? totalBox = null, double? weight = null, double? calWeight = null, double? cusWeight = null, double? defaultPrice = null, double? totalDVGT = null, double? remoteAreasPrice = null, double? fuelPrice = null, double? otherPrice = null, double? vatPrice = null, double? totalPrice = null, double? priceCOD = null, double? priceReturn = null, double? totalPriceSYS = null)
        {
            SqlParameter Id = new SqlParameter("@Id", id);

            SqlParameter TotalBox = new SqlParameter("@TotalBox", totalBox);
            if (!totalBox.HasValue)
                TotalBox.Value = DBNull.Value;

            SqlParameter Weight = new SqlParameter("@Weight", weight);
            if (!weight.HasValue)
                Weight.Value = DBNull.Value;

            SqlParameter CalWeight = new SqlParameter("@CalWeight", calWeight);
            if (!calWeight.HasValue)
                CalWeight.Value = DBNull.Value;

            SqlParameter CusWeight = new SqlParameter("@CusWeight", cusWeight);
            if (!cusWeight.HasValue)
                CusWeight.Value = DBNull.Value;

            SqlParameter DefaultPrice = new SqlParameter("@DefaultPrice", defaultPrice);
            if (!defaultPrice.HasValue)
                DefaultPrice.Value = DBNull.Value;

            SqlParameter TotalDVGT = new SqlParameter("@TotalDVGT", totalDVGT);
            if (!totalDVGT.HasValue)
                TotalDVGT.Value = DBNull.Value;

            SqlParameter RemoteAreasPrice = new SqlParameter("@RemoteAreasPrice", remoteAreasPrice);
            if (!remoteAreasPrice.HasValue)
                RemoteAreasPrice.Value = DBNull.Value;

            SqlParameter FuelPrice = new SqlParameter("@FuelPrice", fuelPrice);
            if (!fuelPrice.HasValue)
                FuelPrice.Value = DBNull.Value;

            SqlParameter OtherPrice = new SqlParameter("@OtherPrice", otherPrice);
            if (!otherPrice.HasValue)
                OtherPrice.Value = DBNull.Value;

            SqlParameter VATPrice = new SqlParameter("@VATPrice", vatPrice);
            if (!vatPrice.HasValue)
                VATPrice.Value = DBNull.Value;

            SqlParameter TotalPrice = new SqlParameter("@TotalPrice", totalPrice);
            if (!totalPrice.HasValue)
                TotalPrice.Value = DBNull.Value;

            SqlParameter PriceCOD = new SqlParameter("@PriceCOD", priceCOD);
            if (!priceCOD.HasValue)
                PriceCOD.Value = DBNull.Value;

            SqlParameter PriceReturn = new SqlParameter("@PriceReturn", priceReturn);
            if (!priceReturn.HasValue)
                PriceReturn.Value = DBNull.Value;

            SqlParameter TotalPriceSYS = new SqlParameter("@TotalPriceSYS", totalPriceSYS);
            if (!totalPriceSYS.HasValue)
                TotalPriceSYS.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @Id, @TotalBox,@Weight, @CalWeight, @CusWeight, @DefaultPrice, @TotalDVGT, @RemoteAreasPrice, @FuelPrice, @OtherPrice, @VATPrice, @TotalPrice, @PriceCOD, @PriceReturn, @TotalPriceSYS",
                new SqlParameter[] {
                    Id,
                    TotalBox,
                    Weight,
                    CalWeight,
                    CusWeight,
                    DefaultPrice,
                    TotalDVGT,
                    RemoteAreasPrice,
                    FuelPrice,
                    OtherPrice,
                    VATPrice,
                    TotalPrice,
                    PriceCOD,
                    PriceReturn,
                    TotalPriceSYS
                }
            );
        }
    }
}
