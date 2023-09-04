
using Core.Entity.Entities;
using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Core.Business.ViewModels
{
    public class UpdateKPIModelByProc : List<UpLoadExcelKPIModel>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            var sqlRow = new SqlDataRecord(

                  new SqlMetaData("HubRoutingCode", SqlDbType.NVarChar, 4000),
                  new SqlMetaData("CutOffTimeId", SqlDbType.Int),
                  new SqlMetaData("CutOffTimeCode", SqlDbType.NVarChar, 4000),
                  new SqlMetaData("KPIFullLading", SqlDbType.Float),
                  new SqlMetaData("KPIExportSAP", SqlDbType.Float),
                  new SqlMetaData("StartTransferTime", SqlDbType.Float),
                  new SqlMetaData("KPITransfer", SqlDbType.Float),
                  new SqlMetaData("KPIStartDeliveryTime", SqlDbType.Float),
                  new SqlMetaData("KPIDelivery", SqlDbType.Float),
                  new SqlMetaData("KPIPaymentMoney", SqlDbType.Float),
                  new SqlMetaData("KPIConfirmPaymentMoney", SqlDbType.Float),
                  new SqlMetaData("IsAllowOverDayKPIStartDeliv", SqlDbType.Bit),
                  new SqlMetaData("isAllowOverDayKPIPaymentMoney", SqlDbType.Bit)

            );
            foreach (UpLoadExcelKPIModel entry in this)
            {
                sqlRow.SetValue(0, entry.HubRoutingCode);
                sqlRow.SetValue(1, entry.CutOffTimeId);
                sqlRow.SetValue(2, entry.CutOffTimeCode);
                sqlRow.SetValue(3, entry.KPIFullLading);
                sqlRow.SetValue(4, entry.KPIExportSAP);
                sqlRow.SetValue(5, entry.StartTransferTime);
                sqlRow.SetValue(6, entry.KPITransfer);
                sqlRow.SetValue(7, entry.KPIStartDeliveryTime);
                sqlRow.SetValue(8, entry.KPIDelivery);
                sqlRow.SetValue(9, entry.KPIPaymentMoney);
                sqlRow.SetValue(10, entry.KPIConfirmPaymentMoney);
                sqlRow.SetValue(11, entry.IsAllowOverDayKPIStartDeliv);
                sqlRow.SetValue(12, entry.IsAllowOverDayKPIPaymentMoney);



                yield return sqlRow;
            }
        }
    }
}