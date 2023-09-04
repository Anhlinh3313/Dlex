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
    public class ApiADayRoi
    {
        public const string PushStatusShipmentUri = "https://api-logistics-eton.vincommerce.com/";
        public const string userName = "beExpress";
        public const string passWord = "b3#xp4322";
        private string token = "";
        public ApiADayRoi()
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(string.Format("{0}:{1}", userName, passWord));
            token = Convert.ToBase64String(plainTextBytes);
        }
        public async Task<ResultADayRoi> PushStatusToADayRoi(string _shipmentNumber, int _statusId, string _shipmentStatusName, string _receiverName,
            string _eventDateTime)
        {
            //
            if (Util.IsNull(_shipmentStatusName)) _shipmentStatusName = "";
            if (Util.IsNull(_receiverName)) _receiverName = "";
            //
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(PushStatusShipmentUri);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            // Add Authorization
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", token);
            string api = string.Format("webhook/receive-msg/beexpress");

            var objContent = JsonConvert.SerializeObject(new
            {
                shipmentNumber = _shipmentNumber,
                shipmentStatusId = _statusId,
                statusId = _statusId,
                shipmentStatusName = _shipmentStatusName,
                receiverName = _receiverName,
                eventDate = _eventDateTime,
                ladingSchedules = new List<object>()
            });
            HttpContent content = new StringContent(
                objContent.ToString()
                , Encoding.UTF8
                , "application/json");

            HttpResponseMessage response = client.PostAsync(api, content).Result;
            ResultADayRoi resultADayRoi = new ResultADayRoi();
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    string sOutput = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrWhiteSpace(sOutput))
                    {
                        resultADayRoi.status = -103;
                        resultADayRoi.message = "Return result empty, plase try again!";
                        return resultADayRoi;
                    }
                    else
                    {
                        try
                        {
                            var objOutput = JsonConvert.DeserializeObject<ResultADayRoi>(sOutput);
                            return objOutput;
                        }
                        catch (Exception ex)
                        {
                            resultADayRoi.status = -102;
                            resultADayRoi.message = ex.Message;
                            return resultADayRoi;
                        }
                    }
                }
                catch (Exception ex)
                {
                    resultADayRoi.status = -101;
                    resultADayRoi.message = ex.Message;
                    return resultADayRoi;
                }
            }
            else
            {
                resultADayRoi.status = -100;
                resultADayRoi.message = "Connect api to ADayRoi fail!";
                return resultADayRoi;
            }
        }
    }

    public class ResultADayRoi
    {
        public int status { get; set; }
        public string message { get; set; }
    }
}
