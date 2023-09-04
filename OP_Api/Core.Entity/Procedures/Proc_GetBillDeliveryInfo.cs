using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetBillDeliveryInfo : IEntityProcView
    {
        public const string ProcName = "Proc_GetBillDeliveryInfo";
        [Key]
        public int Id { get; set; }
        public string ShipmentNumber { get; set; }
        public string DeliveryUserCode { get; set; }
        public string RealRecipientName { get; set; }
        public string DeliveryNote { get; set; }
        public string ImagePath { get; set; }
        public DateTime? EndDeliveryTime { get; set; }
        public DateTime? CreatedWhen { get; set; }

        public Proc_GetBillDeliveryInfo() { }
        public static IEntityProc GetEntityProc()
        {

            return new EntityProc(
                $"{ProcName} ",
                new SqlParameter[] {
                }
            );
        }
    }
}
