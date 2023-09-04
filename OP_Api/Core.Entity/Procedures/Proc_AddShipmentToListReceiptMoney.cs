using Core.Entity.Abstract;
using Core.Entity.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Core.Entity.Procedures
{
    public class Proc_AddShipmentToListReceiptMoney : IEntityProcView
    {
        public const string ProcName = "Proc_AddShipmentToListReceiptMoney";

        [Key]
        public Guid FakeId { get; set; }
        public bool IsSuccess { get; set; }

        public Proc_AddShipmentToListReceiptMoney()
        {
        }

        public static IEntityProc GetEntityProc(int listReceiptId, List<ShipmentToReceipt> listShipments, bool isClear)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Id", typeof(int));
            dataTable.Columns.Add("COD", typeof(double));
            dataTable.Columns.Add("TotalPrice", typeof(double));
            //DataTable dtt = new DataTable();
            //dtt = data.ToDataTable();
            foreach (var item in listShipments.ToArray())
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow["Id"] = item.Id;
                dataRow["COD"] = item.COD;
                dataRow["TotalPrice"] = item.TotalPrice;
                dataTable.Rows.Add(dataRow);
            }

            SqlParameter DataListShipments = new SqlParameter("@ListShipments", dataTable);
            DataListShipments.TypeName = "TYPE_ShipmentToReceipt";
            if (listShipments.Count == 0)
                DataListShipments.Value = DBNull.Value;

            return new EntityProc(
                $"{ProcName} @ListReceiptId, @ListShipments, @IsClear",
                new SqlParameter[] {
                    new SqlParameter("@ListReceiptId", listReceiptId),
                    DataListShipments,
                    new SqlParameter("@IsClear", isClear)
                }
            );
        }
    }
}
