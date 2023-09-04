using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;
namespace Core.Entity.Procedures
{
    public class Proc_GetReceiptMoneyListShipment : IEntityProcView
    {
        public const string ProcName = "Proc_GetReceiptMoneyListShipment";
        [Key]
        public int Id { get; set; }
        public string ShipmentNumber { get; set; }
        public string SenderName { get; set; }
        public string CompanyFrom { get; set; }
        public string ReceiverCode2 { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public string ShippingAddress { get; set; }
        public double? COD { get; set; }
        public double? TotalPrice { get; set; }

        public Proc_GetReceiptMoneyListShipment() { }
        public static IEntityProc GetEntityProc(int id)
        {
            SqlParameter sqlParameter1 = new SqlParameter("@Id", id);

            return new EntityProc(
                $"{ProcName} @Id",
                new SqlParameter[] {
                    sqlParameter1
                }
            );
        }
    }
}
