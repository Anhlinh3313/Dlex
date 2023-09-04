
using Core.Entity.Entities;
using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Core.Business.ViewModels
{
    public class InsertKPIByProc : List<GetKPIModel>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            var sqlRow = new SqlDataRecord(

                  new SqlMetaData("ShipmentId", SqlDbType.Int),
                  new SqlMetaData("ARDate", SqlDbType.DateTime),
                  new SqlMetaData("Type", SqlDbType.Int),
                  new SqlMetaData("COT", SqlDbType.DateTime),
                  new SqlMetaData("KPIFullLading", SqlDbType.DateTime),
                  new SqlMetaData("KPIFullLadingDay", SqlDbType.Float),
                  new SqlMetaData("KPIExportSAP", SqlDbType.DateTime),
                  new SqlMetaData("StartTransferTime", SqlDbType.DateTime),
                  new SqlMetaData("KPITransfer", SqlDbType.DateTime),
                  new SqlMetaData("StartDeliveryTime", SqlDbType.DateTime),
                  new SqlMetaData("KPIDelivery", SqlDbType.DateTime),
                  new SqlMetaData("KPIPaymentMoney", SqlDbType.DateTime),
                  new SqlMetaData("KPIConfirmPaymentMoney", SqlDbType.DateTime)
            );
            foreach (GetKPIModel entry in this)
            {
                sqlRow.SetInt32(0, entry.ShipmentId);
                sqlRow.SetValue(1, entry.ARDate);
                sqlRow.SetValue(2, entry.Type);
                sqlRow.SetValue(3, entry.COT);
                sqlRow.SetValue(4, entry.KPIFullLading);
                sqlRow.SetValue(5, entry.KPIFullLadingDay);
                sqlRow.SetValue(6, entry.KPIExportSAP);
                sqlRow.SetValue(7, entry.StartTransferTime);
                sqlRow.SetValue(8, entry.KPITransfer);
                sqlRow.SetValue(9, entry.StartDeliveryTime);
                sqlRow.SetValue(10, entry.KPIDelivery);
                sqlRow.SetValue(11, entry.KPIPaymentMoney);
                sqlRow.SetValue(12, entry.KPIConfirmPaymentMoney);


                yield return sqlRow;
            }
        }
    }
}