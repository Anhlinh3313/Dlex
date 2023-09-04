using Core.Entity.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace Core.Entity.Procedures
{
    public class Proc_GetInfoHubRouting : IEntityProcView
    {
        public const string ProcName = "Proc_GetInfoHubRouting";
        [Key]
        public int Id { get; set; }
        public int? HubId { get; set; }
        public int? HubRoutingId { get; set; }
        public string WardName { get; set; }
        public string DistrictName { get; set; }
        public string ProvinceName { get; set; }
        public string HubName { get; set; }
        public string HubRoutingName { get; set; }
        public string HubRoutingUser { get; set; }

        public Proc_GetInfoHubRouting() { }
        public static IEntityProc GetEntityProc(bool? isTruckdelivery = false, int? districtId = null, int? wardId = null, double? weight = 0)
        {
            SqlParameter IsTruckdelivery = new SqlParameter("@IsTruckdelivery", isTruckdelivery);
            if (!isTruckdelivery.HasValue) IsTruckdelivery.Value = DBNull.Value;
            SqlParameter DistrictId = new SqlParameter("@DistrictId", districtId);
            if (!districtId.HasValue) DistrictId.Value = DBNull.Value;
            SqlParameter WardId = new SqlParameter("@WardId", wardId);
            if (!wardId.HasValue) WardId.Value = DBNull.Value;
            SqlParameter Weight = new SqlParameter("@Weight", weight);
            if (!weight.HasValue) Weight.Value = DBNull.Value;
            return new EntityProc(
                $"{ProcName} @IsTruckdelivery, @DistrictId, @WardId, @Weight",
                new SqlParameter[] {
                    IsTruckdelivery,
                    DistrictId,
                    WardId,
                    Weight
                }
            );
        }
    }
}
