using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetPackageToPrint : IEntityProcView
    {
        public const string ProcName = "Proc_GetPackageToPrint";


        [Key]
        public int Id { get; set; }
        public string PackageCode { get; set; }
        public string SealNumber { get; set; }
        public double? CalWeight { get; set; }
        public double? Weight { get; set; }
        public double? Width { get; set; }
        public double? Length { get; set; }
        public double? Height { get; set; }
        public string Content { get; set; }
        public string FromHubCode { get; set; }
        public string FromHubName { get; set; }
        public string ToHubCode { get; set; }
        public string ToHubName { get; set; }
        public string StatusName { get; set; }
        public string CreatedUserCode { get; set; }
        public string CreatedUserFullName { get; set; }
        public DateTime? CreatedWhen { get; set; }
        public string OrderDatef { get; set; }
        public string ShipmentNumber { get; set; }
        public int? TotalShipment { get; set; }
        public int? TotalBox { get; set; }

        public Proc_GetPackageToPrint()
        {
        }

        public static IEntityProc GetEntityProc(int? packageId = null)
        {
            SqlParameter PackageId = new SqlParameter("@PackageId", packageId);
            if (!packageId.HasValue) PackageId.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @PackageId",
                new SqlParameter[] {
                    PackageId
                }
            );
        }
    }
}
