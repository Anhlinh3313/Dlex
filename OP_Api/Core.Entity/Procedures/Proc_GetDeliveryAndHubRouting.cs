using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetDeliveryAndHubRouting : IEntityProcView
    {
        public const string ProcName = "Proc_GetDeliveryAndHubRouting";

        [Key]
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Code { get; set; }
        public string ListHubRoutingIds { get; set; }

        public Proc_GetDeliveryAndHubRouting() { }

        public static IEntityProc GetEntityProc(int listGoodsId)
        {
            return new EntityProc(
               $"{ProcName} @ListGoodsId",
                new SqlParameter[] {
                new SqlParameter("@ListGoodsId", listGoodsId)}
                );
        }
    }
}
