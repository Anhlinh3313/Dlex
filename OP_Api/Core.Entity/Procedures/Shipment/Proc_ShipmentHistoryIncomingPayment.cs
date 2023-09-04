using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;
namespace Core.Entity.Procedures
{
    public class Proc_ShipmentHistoryIncomingPayment : IEntityProcView
    {
        public const string ProcName = "Proc_ShipmentHistoryIncomingPayment";


        [Key]
        public int Id { get; set; }
        public int? ShipmentId { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool IsSuccess { get; set; }
        public string Data { get; set; }
        public string Result { get; set; }
        public int? Flag { get; set; }

        public static IEntityProc GetEntityProc(int id)
        {
            SqlParameter Id = new SqlParameter("@Id", id);
            return new EntityProc(
                $"{ProcName} @Id ",
                new SqlParameter[]
                {
                    Id
                }
            );
        }
    }
}
