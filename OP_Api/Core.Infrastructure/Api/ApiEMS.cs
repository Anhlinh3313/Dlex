using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WCFTracking;

namespace Core.Infrastructure.Api
{
    public class ApiEMS
    {
        public ApiEMS() { }
        //http://ws.phanmemchuyenphatnhanh.vn/WebServiceTracking.asmx/Tracking?ListTrackingNumber=ee076677485vn,ef073036010vn&EPX=EMS
        public const string partnerCode = "EMS";
        public async Task<Shipment[]> GetDataTrackingEMS(string listShipmentNumbers)
        {
            WebServiceTrackingSoapClient clientA = new WebServiceTrackingSoapClient(WebServiceTrackingSoapClient.EndpointConfiguration.WebServiceTrackingSoap12);
            var res = await clientA.TrackingAsync(listShipmentNumbers, partnerCode);
            return res;
        }
    }
}
