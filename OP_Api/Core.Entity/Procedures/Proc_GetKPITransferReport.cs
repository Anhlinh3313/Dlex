﻿using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
	public class Proc_GetKPITransferReport : IEntityProcView
	{
		public const string ProcName = "Proc_GetKPITransferReport";
		[Key]
		public int Id { get; set; }
		public string ShipmentNumber { get; set; }
		public string CustomerCode { get; set; }
		public string HubRoutingCode { get; set; }
		public string HubRoutingName { get; set; }
		public string ARHour { get; set; }
		public string ARDate { get; set; }
		public string COTDate { get; set; }
		public string COT { get; set; }
		public int DayOfWeek { get; set; }
		public int DeliveryFrequency { get; set; }
		public string StartTransferDate { get; set; }
		public string StartTransferTime { get; set; }
		public double? TransferLeadTime { get; set; }
		public string TransferDateArrived { get; set; }
		public string TransferTimeArrived { get; set; }
		public string RealTransferDateArrived { get; set; }
		public string RealTransferTimeArrived { get; set; }
		public string TransferUserCode { get; set; }
		public string TransferUserName { get; set; }
		public string KPITransferResult { get; set; }
		public int? TotalCount { get; set; }


		public Proc_GetKPITransferReport() { }
		public static IEntityProc GetEntityProc(int? centerHubId = null, int? poHubId = null, int? stationHubId = null, int? customerId = null, DateTime? fromDate = null, DateTime? toDate = null,
				string searchText = null, int? pageNumer = null, int? pageSize = null, bool? isSortDescending = null)
		{
			SqlParameter parameter1 = new SqlParameter(
		"@CenterHubId", centerHubId);
			if (!centerHubId.HasValue)
				parameter1.Value = DBNull.Value;
			SqlParameter parameter2 = new SqlParameter(
			"@POHubId", poHubId);
			if (!poHubId.HasValue)
				parameter2.Value = DBNull.Value;
			SqlParameter parameter3 = new SqlParameter(
			"@StationHubId", stationHubId);
			if (!stationHubId.HasValue)
				parameter3.Value = DBNull.Value;
			SqlParameter parameter4 = new SqlParameter(
			"@CustomerId", customerId);
			if (!customerId.HasValue)
				parameter4.Value = DBNull.Value;
			SqlParameter parameter5 = new SqlParameter(
			"@FromDate", fromDate);
			if (!fromDate.HasValue)
				parameter5.Value = DBNull.Value;
			SqlParameter parameter6 = new SqlParameter(
			"@ToDate", toDate);
			if (!toDate.HasValue)
				parameter6.Value = DBNull.Value;
			SqlParameter parameter7 = new SqlParameter(
			"@SearchText", searchText);
			if (String.IsNullOrWhiteSpace(searchText))
				parameter7.Value = DBNull.Value;
			SqlParameter parameter8 = new SqlParameter(
			"@PageNumer", pageNumer);
			if (!pageNumer.HasValue)
				parameter8.Value = DBNull.Value;
			SqlParameter parameter9 = new SqlParameter(
			"@PageSize", pageSize);
			if (!pageSize.HasValue)
				parameter9.Value = DBNull.Value;
			SqlParameter parameter10 = new SqlParameter(
			"@IsSortDescending", isSortDescending);
			if (!isSortDescending.HasValue)
				parameter10.Value = DBNull.Value;
			return new EntityProc(
				$"{ProcName} @CenterHubId, @POHubId, @StationHubId, @CustomerId, @FromDate, @ToDate, @SearchText, @PageNumer, @PageSize, @IsSortDescending",
				new SqlParameter[] {
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
