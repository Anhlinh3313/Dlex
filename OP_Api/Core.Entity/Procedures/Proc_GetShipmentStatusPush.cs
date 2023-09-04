using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetShipmentStatusPush : IEntityProcView
    {
        public const string ProcName = "Proc_GetShipmentStatusPush";
        [Key]
        public int Id { get; set; }
        public int ShipmentId { get; set; }
        public string ShipmentNumber { get; set; }
        public string StatusCode { get; set; }
        public string ReasonCode { get; set; }
        public DateTime? ModifiedWhen { get; set; }
        public DateTime? PushedWhen { get; set; }
        public string Location { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public string RealRecipientName { get; set; }
        public string UserFullName { get; set; }
        public string UserNumberPhone { get; set; }
        public string StatusName { get; set; }
        public int StatusId { get; set; }
        public int Timestamp { get; set; }

        public Proc_GetShipmentStatusPush() { }
        public static IEntityProc GetEntityProc(int senderId)
        {
            SqlParameter SenderId = new SqlParameter("@SenderId", senderId);

            return new EntityProc(
                $"{ProcName} @SenderId",
                new SqlParameter[] {
                    SenderId
                }
            );
        }
    }
}
