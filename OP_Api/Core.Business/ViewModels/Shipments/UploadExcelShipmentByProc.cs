using Core.Business.ViewModels.Shipments;
using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using System.Data;

namespace Core.Business.ViewModels
{
    public class UploadExcelShipmentByProc: List<CreateUpdateShipmentViewModel>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            var sqlRow = new SqlDataRecord(
                  new SqlMetaData("PickUserId", SqlDbType.Int),
                  new SqlMetaData("DeliverUserId", SqlDbType.Int));

            foreach (CreateUpdateShipmentViewModel entry in this)
            {
                sqlRow.SetInt32(0, entry.Id);
                sqlRow.SetInt32(1, entry.SenderId.Value);
                yield return sqlRow;
            }
        }
    }
}
