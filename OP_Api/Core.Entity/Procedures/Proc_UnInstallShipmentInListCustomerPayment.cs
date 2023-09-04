using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;
namespace Core.Entity.Procedures
{
    public class Proc_UnInstallShipmentInListCustomerPayment : IEntityProcView
    {
        public const string ProcName = "Proc_UnInstallShipmentInListCustomerPayment";
        [Key]
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public Proc_UnInstallShipmentInListCustomerPayment() { }

        public static IEntityProc GetEntityProc(int listCustomerPaymentId, int shipmentId)
        {            
            SqlParameter ListCustomerPaymentId = new SqlParameter("@ListCustomerPaymentId", listCustomerPaymentId);
            //
            SqlParameter ShipmentId = new SqlParameter("@ShipmentId", shipmentId);
            return new EntityProc(
                $"{ProcName} @ListCustomerPaymentId,@ShipmentId",
                new SqlParameter[] {
                    ListCustomerPaymentId,
                    ShipmentId,
                }
            );
        }
    }
}
