using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetNotificationMenu : IEntityProcView
    {
        public const string ProcName = "Proc_GetNotificationMenu";
        [Key]
        public int UserId { get; set; }
        public int HubId { get; set; }
        public int CountNewRequest { get; set; }
        public int CountHandleShipment { get; set; }
        public int CountPrioritize { get; set; }
        public int CountComplain { get; set; }
        public int CountIncidents { get; set; }
        public int CountCompensation { get; set; }
        public int CountWaitngHandling { get; set; }
        public int CountShipmentDelay { get; set; }
        public int CountWaitingAcceptReturn { get; set; }
        public int CountWaitingCreateReturn { get; set; }
        public int CountShipmentIncidents { get; set; }
        public int CountListReceiptMoneyAccept { get; set; }
        public int CountListReceiptMoneyReject { get; set; }
        //

        public Proc_GetNotificationMenu() { }
        public static IEntityProc GetEntityProc(int userId)
        {
            SqlParameter sqlParameter1 = new SqlParameter("@UserId", userId);

            return new EntityProc(
                $"{ProcName} @UserId",
                new SqlParameter[] {
                    sqlParameter1
                }
            );
        }
    }
}
