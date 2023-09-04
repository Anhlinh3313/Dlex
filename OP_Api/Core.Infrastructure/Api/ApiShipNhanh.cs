using Core.Infrastructure.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Api
{
    public class ApiShipNhanh
    {
        public const string PushStatusShipmentUri = "http://postapi.shipnhanh.vn/";
        public const string userName = "none";
        public const string passWord = "none";
        private string token = "";
        public ApiShipNhanh()
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(string.Format("{0}:{1}", userName, passWord));
            token = Convert.ToBase64String(plainTextBytes);
        }
        public async Task<ResultShipNhanh> PushStatusToShipNhanh(string _shipmentNumber, int _statusId, string _shipmentStatusName, string _receiverName,
            string _eventDateTime, string _location)
        {
            //
            if (Util.IsNull(_shipmentStatusName)) _shipmentStatusName = "";
            if (Util.IsNull(_receiverName)) _receiverName = "";
            if (Util.IsNull(_location)) _location = "";
            //
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(PushStatusShipmentUri);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            // Add Authorization
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", token);
            string api = string.Format("api/tpl/GetFlashipLading");

            var objContent = JsonConvert.SerializeObject(new
            {
                tracking_number = _shipmentNumber,
                status = _shipmentStatusName,
                reason_code = "",
                datetime = _eventDateTime,
                location = _location,
                latitude = "",
                longitude = "",
                receiver_name = _receiverName,
                receiver_signature = "",
                tracking_url = "",
                statusId = _statusId
            });
            HttpContent content = new StringContent(
                objContent.ToString()
                , Encoding.UTF8
                , "application/json");

            HttpResponseMessage response = client.PostAsync(api, content).Result;
            ResultShipNhanh resSN = new ResultShipNhanh();
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    string sOutput = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrWhiteSpace(sOutput))
                    {
                        resSN.isSuccess = false;
                        resSN.message = "Return result empty, plase try again!";
                        return resSN;
                    }
                    else
                    {
                        try
                        {
                            var objOutput = JsonConvert.DeserializeObject<ResultShipNhanh>(sOutput);
                            return objOutput;
                        }
                        catch (Exception ex)
                        {
                            resSN.isSuccess = false;
                            resSN.message = ex.Message;
                            return resSN;
                        }
                    }
                }
                catch (Exception ex)
                {
                    resSN.isSuccess = false;
                    resSN.message = ex.Message;
                    return resSN;
                }
            }
            else
            {
                resSN.isSuccess = false;
                resSN.message = "Connect api to ADayRoi fail!";
                return resSN;
            }
        }
    }

    public class ResultShipNhanh
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
    }
}
