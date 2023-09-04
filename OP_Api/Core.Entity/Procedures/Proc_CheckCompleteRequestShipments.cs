using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using Core.Entity.Abstract;

namespace Core.Entity.Procedures
{
    public class Proc_CheckCompleteRequestShipments : IEntityProcView
    {
        public const string ProcName = "Proc_CheckCompleteRequestShipments";

        [Key]
        public int Id { get; set; }

        public Proc_CheckCompleteRequestShipments()
        {

        }

        public static IEntityProc GetEntityProc(string requestShipmentIds)
        {

            SqlParameter RequestShipmentIds = new SqlParameter("@RequestShipmentIds", requestShipmentIds);
            if(string.IsNullOrEmpty(requestShipmentIds)) 
                RequestShipmentIds.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @RequestShipmentIds",
                new SqlParameter[] {
                    RequestShipmentIds
                }
            );
        }
    }
}
