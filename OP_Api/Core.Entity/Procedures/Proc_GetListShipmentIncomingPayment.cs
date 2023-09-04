using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetListShipmentIncomingPayment : IEntityProcView
    {
        public const string ProcName = "Proc_GetListShipmentIncomingPayment";
        [Key]
        public int Id { get; set; }
        public string CustomerCode { get; set; }
        public double? DocTotal { get; set; }
        public double? Total { get; set; }
        public DateTime? DocDate { get; set; }
        public string PaymentType { get; set; }
        public string BankAccount { get; set; }
        public DateTime? TransferDate { get; set; }
        public string DocumentNo { get; set; }
        public string CreatedByFullName { get; set; }
        public string CreatedByCode { get; set; }
        public string ListReceiveMoneyCode { get; set; }

        public Proc_GetListShipmentIncomingPayment() { }
        public static IEntityProc GetEntityProc(int id)
        {
            SqlParameter sqlParameter1 = new SqlParameter("@ListReceiptMoneyId", id);

            return new EntityProc(
                $"{ProcName} @ListReceiptMoneyId",
                new SqlParameter[] {
                    sqlParameter1
                }
            );
        }
    }
}
