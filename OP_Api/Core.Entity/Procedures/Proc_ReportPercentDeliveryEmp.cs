using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_ReportPercentDeliveryEmp : IEntityProcView
    {
        public const string ProcName = "Proc_ReportPercentDeliveryEmp";
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string MonthPercentDeliveryFirst { get; set; }
        public string MonthPercentDeliverySuccess { get; set; }
        public string WeekPercentDeliveryFirst { get; set; }
        public string WeekPercentDeliverySuccess { get; set; }
        public string DayPercentDeliveryFirst { get; set; }
        public string DayPercentDeliverySuccess { get; set; }

        public Proc_ReportPercentDeliveryEmp() { }
        public static IEntityProc GetEntityProc(int userId)
        {
            SqlParameter UserId = new SqlParameter("@UserId", userId);

            return new EntityProc(
                $"{ProcName} @UserId",
                new SqlParameter[] {
                    UserId
                }
            );
        }
    }
}
