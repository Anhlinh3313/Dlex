using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;
namespace Core.Entity.Procedures
{
    public class Proc_GetListGoodsReceive : IEntityProcView
    {
        public const string ProcName = "Proc_GetListGoodsReceive";
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string FromHubCode { get; set; }
        public int TotalShipmentReceive { get; set; }

        public Proc_GetListGoodsReceive() { }
        public static IEntityProc GetEntityProc(int? currenthubId = null, int? fromHubId = null)
        {
            SqlParameter CurrentHubId = new SqlParameter("@CurrentHubId", currenthubId);
            if (!currenthubId.HasValue) CurrentHubId.Value = DBNull.Value;

            SqlParameter FromHubId = new SqlParameter("@FromHubId", fromHubId);
            if (!fromHubId.HasValue) FromHubId.Value = DBNull.Value;
            return new EntityProc(
                $"{ProcName} @CurrentHubId, @FromHubId",
                new SqlParameter[] {
                    CurrentHubId,
                    FromHubId
                }
            );
        }
    }
}
