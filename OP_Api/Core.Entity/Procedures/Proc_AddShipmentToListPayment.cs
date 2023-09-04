using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_AddShipmentToListPayment : IEntityProcView
    {
        public const string ProcName = "Proc_AddShipmentToListPayment";
        [Key]
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public Proc_AddShipmentToListPayment() { }
        public static IEntityProc GetEntityProc(int listPaymentId, string strShipmentIds)
        {
            SqlParameter ListPaymwentId = new SqlParameter("@ListPaymwentId", listPaymentId);
            SqlParameter StrShipmentIds = new SqlParameter("@StrShipmentIds", strShipmentIds);
            if (string.IsNullOrWhiteSpace(strShipmentIds)) StrShipmentIds.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @ListPaymwentId,@StrShipmentIds",
                new SqlParameter[] {
                    ListPaymwentId,StrShipmentIds
                }
            );
        }
    }
}
