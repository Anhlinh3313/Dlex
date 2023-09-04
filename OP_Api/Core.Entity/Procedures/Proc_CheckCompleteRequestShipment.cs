using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_CheckCompleteRequestShipment : IEntityProcView
    {
        public const string ProcName = "Proc_CheckCompleteRequestShipment";

        [Key]
        public int Id { get; set; }

        public Proc_CheckCompleteRequestShipment()
        {

        }

        public static IEntityProc GetEntityProc(int? requestShipmentId)
        {

            SqlParameter RequestShipmentId = new SqlParameter("@RequestShipmentId", requestShipmentId);
            if (!requestShipmentId.HasValue) RequestShipmentId.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @RequestShipmentId",
                new SqlParameter[] {
                    RequestShipmentId
                }
            );
        }
    }
}
