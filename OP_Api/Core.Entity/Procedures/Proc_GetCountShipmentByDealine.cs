using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetCountShipmentByDealine : IEntityProcView
    {
        public const string ProcName = "Proc_GetCountShipmentByDealine";

        [Key]
        public int CountInventory { get; set; }
        public int CountNormal { get; set; }
        public int CountDeadline { get; set; }
        public int CountIsLate { get; set; }
        public int CountPrioritize { get; set; }
        public int CountIncidents { get; set; }

        public Proc_GetCountShipmentByDealine() { }

        public static IEntityProc GetEntityProc(int hubId)
        {
            SqlParameter HubId = new SqlParameter("@HubId", hubId);

            return new EntityProc(
                $"{ProcName} @HubId",
                new SqlParameter[] {
                    HubId
                }
            );
        }
    }
}
