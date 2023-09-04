using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_LadingSchedule_Joined : IEntityProcView
    {
        public const string ProcName = "Proc_LadingSchedule_Joined";

        [Key]
        public int Id { get; set; }
        public int ShipmentId { get; set; }
        public string Location { get; set; }
        public string Note { get; set; }
        public string DateCreated { get; set; }
        public string TimeCreated { get; set; }
        public int? HubId { get; set; }
        public string HubName { get; set; }
        public int? UserId { get; set; }
        public string UserFullName { get; set; }
        public string UserPhone { get; set; }
        public string UserCode { get; set; }
        public int ShipmentStatusId { get; set; }
        public string ShipmentStatusName { get; set; }
        public int? ReasonId { get; set; }
        public string ReasonName { get; set; }
        //
        public bool IsDelay { get; set; }
        public double DelayTime { get; set; }
        public string DelayReason { get; set; }
        public string DelayNote { get; set; }
        public int? IncidentsId { get; set; }
        public string ToHubCode { get; set; }
        public string ToHubName { get; set; }
        public string ToUserCode { get; set; }
        public string ToUserFullname { get; set; }

        public Proc_LadingSchedule_Joined()
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
