using Core.Entity.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace Core.Entity.Procedures
{
	public class Proc_GetReturnLicenseReport : IEntityProcView
	{
		public const string ProcName = "Proc_GetReturnLicenseReport";

		[Key]
		public int Id { get; set; }
		public DateTime? OrderDate { get; set; }
		public string LastEmp { get; set; }
		public string ProvinceName { get; set; }
		public string ShipmentNumber { get; set; }
		public string ReceiverName { get; set; }
		public string SenderName { get; set; }
		public int? TotalCount { get; set; }


		public Proc_GetReturnLicenseReport()
		{
		}

		public static IEntityProc GetEntityProc(int? centerHubId, int? poHubId, int? stationHubId, int? customerId, int? deliveryUserId,
								DateTime? fromDate, DateTime? toDate, string searchText, int? pageNumber, int? pageSize, bool? isSortDescending)
		{
			
			SqlParameter parameter0 = new SqlParameter(
			"@CenterHubId", centerHubId);
			if (!centerHubId.HasValue)
				parameter0.Value = DBNull.Value;
			//
			SqlParameter parameter1 = new SqlParameter(
		   "@POHubId", poHubId);
			if (!poHubId.HasValue)
				parameter1.Value = DBNull.Value;
			//
			SqlParameter parameter2 = new SqlParameter(
			"@StationHubId", stationHubId);
			if (!stationHubId.HasValue)
				parameter2.Value = DBNull.Value;
			//
			SqlParameter parameter3 = new SqlParameter(
			"@CustomerId", customerId);
			if (!customerId.HasValue)
				parameter3.Value = DBNull.Value;
			//
			SqlParameter parameter4 = new SqlParameter(
			"@DeliveryId", deliveryUserId);
			if (!deliveryUserId.HasValue)
				parameter4.Value = DBNull.Value;
			//
			SqlParameter parameter5 = new SqlParameter(
			"@FromDate", fromDate);
			if (!fromDate.HasValue)
				parameter5.Value = DBNull.Value;
			//
			SqlParameter parameter6 = new SqlParameter(
			"@ToDate", toDate);
			if (!toDate.HasValue)
				parameter6.Value = DBNull.Value;
			//
			SqlParameter parameter7 = new SqlParameter(
			"@SearchText", searchText);
			if (string.IsNullOrEmpty(searchText))
				parameter7.Value = DBNull.Value;
			//
			SqlParameter parameter8 = new SqlParameter(
			"@PageNumber", pageNumber);
			if (!pageNumber.HasValue)
				parameter8.Value = DBNull.Value;
			//
			SqlParameter parameter9 = new SqlParameter(
			"@PageSize", pageSize);
			if (!pageSize.HasValue)
				parameter9.Value = DBNull.Value;
			//
			SqlParameter parameter10 = new SqlParameter(
			"@IsSortDescending", isSortDescending);
			if (!isSortDescending.HasValue)
				parameter10.Value = DBNull.Value;

		
			
			return new EntityProc(
				$"{ProcName} @CenterHubId,  @POHubId,  @StationHubId,  @CustomerId,  @DeliveryId,  @FromDate,  @ToDate, @SearchText,  @PageNumber, @PageSize, @IsSortDescending",
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

