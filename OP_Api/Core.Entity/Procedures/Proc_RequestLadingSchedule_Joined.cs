using System;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_RequestLadingSchedule_Joined: IEntityProcView
    {
        public const string ProcName = "Proc_RequestLadingSchedule_Joined";

        public int Id { get; set; }
        public int ShipmentId { get; set; }
        public string Location { get; set; }
        public string Note { get; set; }
        public string DateCreated { get; set; }
        public string TimeCreated { get; set; }
        public int HubId { get; set; }
        public string HubName { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public int ShipmentStatusId { get; set; }
        public string ShipmentStatusName { get; set; }
        public int? ReasonId { get; set; }
        public string ReasonName { get; set; }


        public Proc_RequestLadingSchedule_Joined()
        {
        }

        public static IEntityProc GetEntityProc(int shipmentId)
        {
            return new EntityProc(
                $"{ProcName} @ShipmentId",
                new SqlParameter[] {
                new SqlParameter("@ShipmentId", shipmentId)
                }
            );
        }
    }
}
