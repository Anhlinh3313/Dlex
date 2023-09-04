using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_ReportEmployee: IEntityProcView
    {
        public const string ProcName = "Proc_ReportEmployee";

        public Int64 Id { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public int TotalShipmentKeeping { get; set; }
        public int PickupComplete { get; set; }
        public int Transferring { get; set; }
        public int Delivering { get; set; }
        public int DeliveryFail { get; set; }
        public int ReturnFail { get; set; }
        public int Returning { get; set; }
        public int TransferReturning { get; set; }
        public Double TotalCODKeeping { get; set; }
        public Double TotalPriceKeeping { get; set; }
        public Proc_ReportEmployee()
        {

        }

        public static IEntityProc GetEntityProc(int? hubId, int? empId = null)
        {
            SqlParameter parameter1 = new SqlParameter(
           "@HubId", hubId);
            if (!hubId.HasValue)
                parameter1.Value = DBNull.Value;
            SqlParameter parameter2 = new SqlParameter(
            "@EmpId", empId);
            if (!empId.HasValue)
                parameter2.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @HubId, @EmpId",
                new SqlParameter[] {
                parameter1,
                parameter2
                }
            );
        }
    }
}
