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
    public class ApiLazada
    {
        public const string PushStatusShipmentUri = "https://tps.supertms.com/";
        public const string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwOi8vOiIsImlhdCI6MTU2MzI2NDM0MywiZXhwIjoyMTkzOTg0MzQzLCJuYmYiOjE1NjMyNjQzNDMsImp0aSI6Ink1Y3hucGdYZTlsSkljZk4iLCJzdWIiOiIxZmQ2NGY0Zi05ZDZkLTQ3NDUtYjcxMC0xZjU5ZjlmNDg1MDgifQ.yR3uvSiZbrcZ2jxUJRU8WF-Bqf6ehY5I-cQMFtXTiNU";
        public ApiLazada() { }
        public async Task<ResultLazada> PushStatusToLazada(string tracking_number, string status, string reason_code, string datetime,
            string location, string latitude, string longitude, string receiver_name, string receiver_signature, 
            string driver_name, string driver_contact, string tracking_url)
        {
            //
            if (Util.IsNull(reason_code)) reason_code = "";
            if (Util.IsNull(location)) location = "";
            if (Util.IsNull(receiver_name)) receiver_name = "";
            if (Util.IsNull(receiver_signature)) receiver_signature = "";
            if (Util.IsNull(driver_name)) driver_name = "";
            if (Util.IsNull(driver_contact)) driver_contact = "";
            if (Util.IsNull(tracking_url)) tracking_url = "";
            //
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(PushStatusShipmentUri);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            // Add Authorization
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            string api = string.Format("api/carriers/be-express-vn/packages/statuses");
            //
            var formContent = new MultipartFormDataContent
            {
                //send form text values here
                {new StringContent(tracking_number),"tracking_number"},
                {new StringContent(status),"status"},
                {new StringContent(reason_code),"reason_code"},
                {new StringContent(datetime),"datetime"},
                {new StringContent(location),"location"},
                {new StringContent(latitude),"latitude"},
                {new StringContent(longitude),"longitude"},
                {new StringContent(driver_name),"driver_name"},
                {new StringContent(driver_contact),"driver_contact"},
                {new StringContent(receiver_name),"receiver_name"},
                {new StringContent(receiver_signature),"receiver_signature"},
                {new StringContent(tracking_url),"tracking_url"}
            };
            HttpResponseMessage response = client.PostAsync(api, formContent).Result;
            ResultLazada resultLazada = new ResultLazada();
            //if (response.IsSuccessStatusCode)
            //{
            try
            {
                string sOutput = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(sOutput))
                {
                    resultLazada.status_code = 202;
                    resultLazada.message = "Success";
                    return resultLazada;
                }
                else
                {
                    try
                    {
                        var objOutput = JsonConvert.DeserializeObject<ResultLazada>(sOutput);
                        return objOutput;
                    }
                    catch (Exception ex)
                    {
                        resultLazada.status_code = -102;
                        resultLazada.message = ex.Message;
                        return resultLazada;
                    }
                }
            }
            catch (Exception ex)
            {
                resultLazada.status_code = -101;
                resultLazada.message = ex.Message;
                return resultLazada;
            }
            //}
            //else
            //{
            //    resultLazada.status_code = -100;
            //    resultLazada.message = response.IsSuccessStatusCode.ToString();
            //    return resultLazada;
            //}
        }
    }

    public class ResultLazada
    {
        public string message { get; set; }
        public int status_code { get; set; }
        public debug debug { get; set; }
    }

    public class debug
    {
        public int line { get; set; }
        public string file { get; set; }
        public string Class { get; set; }
        public string[] trace { get; set; }
    }
}
