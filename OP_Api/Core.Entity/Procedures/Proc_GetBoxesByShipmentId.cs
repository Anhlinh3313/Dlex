using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_GetBoxesByShipmentId : IEntityProcView
    {
        public const string ProcName = "Proc_GetBoxesByShipmentId";

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

        public Proc_GetBoxesByShipmentId()
        {
        }

        public static IEntityProc GetEntityProc(string ids)
        {
            return new EntityProc(
                $"{ProcName} @Ids",
                new SqlParameter[] {
                new SqlParameter("@Ids", ids)
                }
            );
        }
    }
}
