using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_ReportUpdateReceiveInformation : IEntityProcView
    {
        public const string ProcName = "Proc_ReportUpdateReceiveInformation";

        [Key]
        public int CurrentEmpId { get; set; }
        public string CurrentEmpName { get; set; }
        public string CurrentEmpCode { get; set; }
        public int TotalBillIn1Hour { get; set; }
        public int TotalBillIn12Hour { get; set; }
        public int TotalBillIn24Hour { get; set; }
        public int TotalBillIn36Hour { get; set; }
        public int TotalBillIn48Hour { get; set; }
        public int TotalBillIn48ThanHour { get; set; }
        public int TotalBillAppTC { get; set; }
        public int TotalBillAppTreo { get; set; }
        public int TotalBill { get; set; }

        public double? TotalPercentIn1Hour { get; set; }
        public double? TotalPercentIn12Hour { get; set; }
        public double? TotalPercentIn24Hour { get; set; }
        public double? TotalPercentIn36Hour { get; set; }
        public double? TotalPercentIn48Hour { get; set; }
        public double? TotalPercentIn48ThanHour { get; set; }
        public double? TotalPercentAppTC { get; set; }
        public double? TotalPercentAppTreo { get; set; }
        public double? TotalPercent { get; set; }

        public Proc_ReportUpdateReceiveInformation()
        {
        }

        public static IEntityProc GetEntityProc(DateTime? dateFrom = null, DateTime? dateTo = null, int? hubId = null, int? empId = null)
        {
            SqlParameter DateFrom = new SqlParameter("@DateFrom", dateFrom);
            if (!dateFrom.HasValue)
                DateFrom.Value = DBNull.Value;

            SqlParameter DateTo = new SqlParameter("@DateTo", dateTo);
            if (!dateTo.HasValue)
                DateTo.Value = DBNull.Value;

            SqlParameter HubId = new SqlParameter("@HubId", hubId);
            if (!hubId.HasValue)
                HubId.Value = DBNull.Value;

            SqlParameter EmpId = new SqlParameter("@EmpId", empId);
            if (!empId.HasValue)
                EmpId.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @DateFrom,@DateTo,@HubId,@EmpId",
                new SqlParameter[] {
                DateFrom,
                DateTo,
                HubId,
                EmpId,
                }
            );
        }
    }
}
