using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using Core.Entity.Abstract;
using Core.Entity.Entities;

namespace Core.Entity.Procedures
{
    public class Proc_UpdateKPIShipment : IEntityProcView
    {
        public const string ProcName = "Proc_UpdateKPIShipment";
        [Key]
        public int Id { get; set; }

        public Proc_UpdateKPIShipment()
        {
        }

        public static IEntityProc GetEntityProc(List<KPIShipmentDetail> data)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Id", typeof(int));
            dataTable.Columns.Add("IsEnabled", typeof(bool));
            dataTable.Columns.Add("ConcurrencyStamp", typeof(String));
            dataTable.Columns.Add("CreatedWhen", typeof(DateTime));
            dataTable.Columns.Add("CreatedBy", typeof(int));
            dataTable.Columns.Add("ModifiedWhen", typeof(DateTime));
            dataTable.Columns.Add("ModifiedBy", typeof(int));
            dataTable.Columns.Add("Name", typeof(String));
            dataTable.Columns.Add("Code", typeof(String));
            dataTable.Columns.Add("WardId", typeof(Int32));
            dataTable.Columns.Add("DistrictId", typeof(Int32));
            dataTable.Columns.Add("Vehicle", typeof(String));
            dataTable.Columns.Add("TargetDeliveryTime", typeof(Int32));
            dataTable.Columns.Add("KPIShipmentId", typeof(Int32));
            dataTable.Columns.Add("TargetPaymentCOD", typeof(Int32));


            foreach (var item in data.ToArray())
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow["Id"] = item.Id;
                dataRow["IsEnabled"] = true;
                //dataRow["ConcurrencyStamp"] = item.ConcurrencyStamp;
                //dataRow["CreatedWhen"] = item.CreatedWhen.GetValueOrDefault();
                //dataRow["CreatedBy"] = item.CreatedBy.GetValueOrDefault();
                //dataRow["ModifiedWhen"] = item.ModifiedWhen.GetValueOrDefault();
                //dataRow["ModifiedBy"] = item.ModifiedBy.GetValueOrDefault();
                //dataRow["Name"] = item.Name;
                //dataRow["Code"] = item.Code;
                dataRow["WardId"] = item.WardId;
                //dataRow["DistrictId"] = item.DistrictId.GetValueOrDefault();
                dataRow["Vehicle"] = item.Vehicle;
                dataRow["TargetDeliveryTime"] = item.TargetDeliveryTime;
                dataRow["KPIShipmentId"] = item.KPIShipmentId;
                dataRow["TargetPaymentCOD"] = item.TargetPaymentCOD.GetValueOrDefault(0);
                dataTable.Rows.Add(dataRow);
            }
            SqlParameter Data = new SqlParameter("@ListDataKPIShipment", dataTable);
            Data.TypeName = "KPIShipmentDetail";
            if (data.Count == 0)
                Data.Value = DBNull.Value;

            return new EntityProc(
              $"{ProcName} @ListDataKPIShipment",
              new SqlParameter[] {
                Data
              }
           );
        }
    }
}
