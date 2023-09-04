using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_ReportEmployeeOne : IEntityProcView
    {
        public const string ProcName = "Proc_ReportEmployeeOne";
        public int Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public int AssignEmployeePickup { get; set; }
        public int Picking { get; set; }
        public int AssignEmployeeTransfer { get; set; }
        public int Transferring { get; set; }
        public int AssignEmployeeTransferReturn { get; set; }
        public int TransferReturning { get; set; }
        public int AssignEmployeeDelivery { get; set; }
        public int Delivering { get; set; }
        public int AssignEmployeeReturn { get; set; }
        public int Returning { get; set; }
        public int PickupCompleted { get; set; }
        public int DeliveryFailed { get; set; }
        public int TotalInWarehouse { get; set; }
        public Proc_ReportEmployeeOne() { }

        public static IEntityProc GetEntityProc(int userId)
        {
            SqlParameter sqlParameter1 = new SqlParameter("@EmpId", userId);

            return new EntityProc(
                $"{ProcName} @EmpId",
                new SqlParameter[] {
                    sqlParameter1
                }
            );
        }
    }
}
