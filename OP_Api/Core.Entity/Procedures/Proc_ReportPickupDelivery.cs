using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_ReportPickupDelivery : IEntityProcView
    {
        public const string ProcName = "Proc_ReportPickupDelivery";
        [Key]
        public int TotalDeliveyCompleteOfCurrentDay { get; set; }
        public int TotalDeliveyCompleteOfCurrentMonth { get; set; }
        public int TotalReturnCompleteOfCurrentDay { get; set; }
        public int TotalReturnCompleteOfCurrentMonth { get; set; }
        public int TotalPickupCompleteOfCurrentDay { get; set; }
        public int TotalPickupCompleteOfCurrentMonth { get; set; }

        public Proc_ReportPickupDelivery()
        {
        }

        public static IEntityProc GetEntityProc(int empId, DateTime? datetime = null)
        {

            SqlParameter EmpParam = new SqlParameter("@EmpId", empId);

            SqlParameter dateTimeParam = new SqlParameter("@DateTime", datetime);
            if (!datetime.HasValue)
                dateTimeParam.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @EmpId, @DateTime",
                new SqlParameter[] {
                EmpParam,
                dateTimeParam
                }
            );
        }
    }
}
