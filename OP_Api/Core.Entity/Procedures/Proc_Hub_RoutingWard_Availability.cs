using System;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_Hub_RoutingWard_Availability : IEntityProcView
    {
		public const string ProcName = "Proc_Hub_RoutingWard_Availability";

		public int Id { get; set; }
		public int HubId { get; set; }
		public int WardId { get; set; }
		public string WardName { get; set; }
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
		public int ProvinceId { get; set; }
		public string ProvinceName { get; set; }
        public int? HubRoutingId { get; set; }


		public Proc_Hub_RoutingWard_Availability()
		{
		}

		public static IEntityProc GetEntityProc(int hubId, int hubRoutingId, bool isTruckDelivery)
		{
			return new EntityProc(
				$"{ProcName} @HubId, @HubRoutingId, @IsTruckDelivery",
				new SqlParameter[] {
					new SqlParameter("@HubId", hubId),
					new SqlParameter("@HubRoutingId", hubRoutingId),
                    new SqlParameter("@IsTruckDelivery", isTruckDelivery)
                }
			);
		}
    }
}
