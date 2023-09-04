using Core.Data;
using Core.Data.Core;
using Core.Entity.Procedures;
using Core.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using WCFTracking;

namespace Core.Business.Core.Utils
{
    public static class GetDataTrackingUtils
    {
        public static void GetDataTrackingEMS()
        {
            string codeConnectTPL = "EMS";
            int setTime = 1;
            while (true)
            {
                using (var context = new ApplicationContext())
                {
                    var _unitOfWork = new UnitOfWork(context);
                    var listShipmentUpdateTPL = _unitOfWork.Repository<Proc_GetListShipmentUpdateStatusTPL>()
                        .ExecProcedure(Proc_GetListShipmentUpdateStatusTPL.GetEntityProc(codeConnectTPL));
                    foreach (var itemShipment in listShipmentUpdateTPL)
                    {
                        WebServiceTrackingSoapClient client = new WebServiceTrackingSoapClient(WebServiceTrackingSoapClient.EndpointConfiguration.WebServiceTrackingSoap);
                        var resTracking = client.TrackingAsync(itemShipment.ShipmentNumber, codeConnectTPL).Result;
                        if (resTracking.Length > 0)
                        {
                            var lastUpdate = itemShipment.AssignReturnTransferTime;
                            var shipmentTracking = resTracking[0];
                            foreach (var itemSchedule in shipmentTracking.ScheduleDetail)
                            {
                                bool isAllowInsert = false;
                                string strTimeChange = string.Format("{0} {1}", itemSchedule.Date, itemSchedule.Time);
                                DateTime timeChange = DateTime.ParseExact(strTimeChange, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                                if (Util.IsNull(lastUpdate))
                                {
                                    isAllowInsert = true;
                                }
                                else if (timeChange > lastUpdate)
                                {
                                    isAllowInsert = true;
                                }
                                if (isAllowInsert == true)
                                {

                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
