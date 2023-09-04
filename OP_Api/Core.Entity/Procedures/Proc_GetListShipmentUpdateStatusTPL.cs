using Core.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_GetListShipmentUpdateStatusTPL : IEntityProcView
    {
        public const string ProcName = "Proc_GetListShipmentUpdateStatusTPL";
        [Key]
        public int Id { get; set; }
        public string ShipmentNumber { get; set; }
        public int ShipmentStatusId { get; set; }
        public int? CurrentEmpId { get; set; }
        public DateTime? AssignReturnTransferTime { get; set; }
        public int PaymentTypeId { get; set; }
        public int? PaymentCODTypeId { get; set; }
        public double? COD { get; set; }
        public bool IsReturn { get; set; }
        public bool IsCreditTransfer { get; set; }

        public Proc_GetListShipmentUpdateStatusTPL() { }
        public static IEntityProc GetEntityProc(string codeConnect)
        {
            SqlParameter CodeConnect = new SqlParameter("@CodeConnect", codeConnect);
            if (string.IsNullOrWhiteSpace(codeConnect)) CodeConnect.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @CodeConnect",
                new SqlParameter[] {
                    CodeConnect
                }
            );
        }
    }
}
