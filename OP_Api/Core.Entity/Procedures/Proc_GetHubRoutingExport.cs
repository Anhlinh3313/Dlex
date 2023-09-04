using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetHubRoutingExport : IEntityProcView
    {
        public const string ProcName = "Proc_GetHubRoutingExport";
        [Key]
        public int Id { get; set; }
        public string HubRoutingCode { get; set; }
        public int? CutOffTimeId { get; set; }
        public string COTCode { get; set; }
        public double? KPIFullLading { get; set; }
        public double? KPIExportSAP { get; set; }
        public double? KPITransfer { get; set; }
        public bool? IsAllowOverDayKPIStartDeliv { get; set; }
        public double? KPIDelivery { get; set; }
        public double? KPIPaymentMoney { get; set; }
        public double? KPIConfirmPaymentMoney { get; set; }
        public string CodeConnect { get; set; }
        public Proc_GetHubRoutingExport() { }
        public bool? IsAllowOverDayKPIPaymentMoney { get; set; }


        public static IEntityProc GetEntityProc(int? codeConnect = null, int? code = null, int? userId = null, int? centerHubId = null, int? poHubId = null, int? stationHubId = null)
        {
            SqlParameter CodeConnect = new SqlParameter("@CodeConnect", codeConnect);
            if (!codeConnect.HasValue) CodeConnect.Value = DBNull.Value;

            SqlParameter Code = new SqlParameter("@Code", code);
            if (!code.HasValue) Code.Value = DBNull.Value;

            SqlParameter UserId = new SqlParameter("@UserId", userId);
            if (!userId.HasValue) UserId.Value = DBNull.Value;

			SqlParameter CenterHubId = new SqlParameter("@CenterHubId", centerHubId);
			if (!centerHubId.HasValue) CenterHubId.Value = DBNull.Value;

			SqlParameter POHubId = new SqlParameter("@POHubId", poHubId);
			if (!poHubId.HasValue) POHubId.Value = DBNull.Value;

			SqlParameter StationHubId = new SqlParameter("@StationHubId", stationHubId);
			if (!stationHubId.HasValue) StationHubId.Value = DBNull.Value;

			return new EntityProc(
                $"{ProcName} @CodeConnect, @Code, @UserId, @CenterHubId, @POHubId, @StationHubId",
                new SqlParameter[] {
                    CodeConnect, Code, UserId, CenterHubId, POHubId, StationHubId
				}
            );
        }
    }
}
