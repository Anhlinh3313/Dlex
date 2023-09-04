using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_PriceServiceDVGT : IEntityProcView
    {
        public const string ProcName = "Proc_PriceServiceDVGT";

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int FormulaId { get; set; }
        public double ValueFrom { get; set; }
        public double ValueTo { get; set; }
        public double Price { get; set; }
        public double? ValuePlus { get; set; }
        public double VATPercent { get; set; }
        public string CodePriceListDVGT { get; set; }
        public bool? IsPublic { get; set; }
        public int? NumOrder { get; set; }
        public int? CalculateById { get; set; }

        public Proc_PriceServiceDVGT() { }


        public static IEntityProc GetEntityProc(bool isPlus, double? weight, double? m3, double? insured, double? cod, int? items, int? shipments, double? weightWood, int? serviceDVGTId, int? senderId)
        {
            //int plus = 0;
            SqlParameter parameter0 = new SqlParameter("@isPlus", isPlus);
            //
            SqlParameter parameter1 = new SqlParameter("@weight", weight);
            if (!weight.HasValue) parameter1.Value = 0;
            //
            SqlParameter parameter2 = new SqlParameter("@m3", m3);
            if (!m3.HasValue) parameter2.Value = 0;
            //
            SqlParameter parameter3 = new SqlParameter("@insured", insured);
            if (!insured.HasValue) parameter3.Value = 0;
            //
            SqlParameter parameter4 = new SqlParameter("@cod", cod);
            if (!cod.HasValue) parameter4.Value = 0;
            //
            SqlParameter parameter5 = new SqlParameter("@items", items);
            if (!items.HasValue) parameter5.Value = 0;
            //
            SqlParameter parameter6 = new SqlParameter("@shipments", shipments);
            if (!shipments.HasValue) parameter6.Value = 1;
            //
            SqlParameter parameter7 = new SqlParameter("@weightWood", weightWood);
            if (!weightWood.HasValue) parameter7.Value = 0;
            //
            SqlParameter parameter8 = new SqlParameter("@serviceDVGTId", serviceDVGTId);
            if (!serviceDVGTId.HasValue) parameter8.Value = DBNull.Value;
            //
            SqlParameter parameter9 = new SqlParameter("@senderId", senderId);
            if (!senderId.HasValue) parameter9.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @isPlus, @weight, @m3, @insured, @cod, @items, @shipments, @weightWood, @serviceDVGTId, @senderId",
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
                    parameter9
                }
            );
        }

    }
}
