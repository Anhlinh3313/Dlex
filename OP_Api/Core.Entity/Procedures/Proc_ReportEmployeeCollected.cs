using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_ReportEmployeeCollected : IEntityProcView
    {
        public const string ProcName = "Proc_ReportEmployeeCollected";

        public int Id { get; set; }
        public string ShipmentNumber { get; set; }
        public string SenderCode { get; set; }
        public string SenderName { get; set; }
        public string SenderPhone { get; set; }
        public string PickingAddress { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public string ShippingAddress { get; set; }
        public string SOENTRY { get; set; }
        public double TotalCollectedCOD { get; set; }
        public double TotalCollectedPrice { get; set; }
        public string PaymentTypeName { get; set; }
        public string PaymentCODTypeName { get; set; }
        public string ShipmentStatusName { get; set; }
        public int TotalCount { get; set; }
        public double TotalCollected { get; set; }
        public bool? IsCreditTransfer { get; set; }
        public Proc_ReportEmployeeCollected()
        {

        }

        public static IEntityProc GetEntityProc(int userId, int? pageSize = 10, int? pageNumber = 1)
        {
            SqlParameter _UserId = new SqlParameter(
           "@UserId", userId);

            SqlParameter _PageSize = new SqlParameter(
            "@PageSize", pageSize);
            if (!pageSize.HasValue)
                _PageSize.Value = 10;

            SqlParameter _PageNumber = new SqlParameter(
            "@PageNumber", pageNumber);
            if (!pageNumber.HasValue)
                _PageNumber.Value = 1;

            return new EntityProc(
                $"{ProcName} @UserId, @PageSize, @PageNumber",
                new SqlParameter[] {
                _UserId,
                _PageSize,
                _PageNumber
                }
            );
        }
    }
}
