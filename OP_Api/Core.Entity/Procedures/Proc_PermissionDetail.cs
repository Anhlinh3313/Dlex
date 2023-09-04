using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_PermissionDetail : IEntityProcView
    {
        public const string ProcName = "Proc_UploadExcelWithTableValued";

        [Key]
        public int Id { get; set; }
        public int ShipmentId { get; set; }
        public string Content { get; set; }
        public double? Weight { get; set; }
        public double? CalWeight { get; set; }
        public double? Length { get; set; }
        public double? Width { get; set; }
        public double? Height { get; set; }
        public int? PackTypeId { get; set; }
        public double? WoodWeight { get; set; }

        public Proc_PermissionDetail()
        {
        }

        public static IEntityProc GetEntityProc(dynamic shipments)
        {
            return new EntityProc(
                $"{ProcName} @shipments",
                new SqlParameter[] {
                new SqlParameter("@shipments", SqlDbType.Structured)
                    {
                        TypeName = "UploadExcelType",
                        Value = shipments
                    }
                }
            );
        }
    }
}
