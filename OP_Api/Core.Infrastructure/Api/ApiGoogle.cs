using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Api
{
    public class ApiGoogle
    {
        public const string GoogleApiDistanceUri = "https://maps.googleapis.com/maps/";
        public const string apiKey = "";

        public ApiGoogle()
        {
        }
        public async Task<ResultReturn> GetDistance(string apiKey, string addressFrom, string addressTo, double? latFrom = null, double? lngFrom = null, double? latTo = null, double? lngTo = null)
        {
            var language = "vi-VN";
            string origins = addressFrom;
            if (latFrom.HasValue && latFrom != 0 && lngFrom.HasValue && lngFrom != 0) origins = string.Format("{0},{1}", latFrom, lngFrom);
            string destinations = addressTo;
            if (latTo.HasValue && latTo != 0 && lngTo.HasValue && lngTo != 0) destinations = string.Format("{0},{1}", latTo, lngTo);
            string units = "imperial";
            //Console.WriteLine($"objContent: {objContent}");
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(GoogleApiDistanceUri);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            string api = string.Format("api/distancematrix/json?language={0}&key={1}&units={2}&origins={3}&destinations={4}", language, apiKey, units, origins, destinations);
            //
            var formContent = new MultipartFormDataContent
            {
            //send form text values here
                 //{new StringContent(VD_ID),"VD_ID"}
            };
            HttpResponseMessage response = client.PostAsync(api, null).Result;

            if (response.IsSuccessStatusCode)
            {
                string sOutput = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(sOutput))
                {
                    return null;
                }
                else
                {
                    try
                    {
                        var objOutput = JsonConvert.DeserializeObject<ResultReturn>(sOutput);
                        return objOutput;
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                }
            }
            else
            {
                return null;
            }
        }
    }
    public class ResultReturn
    {
        public string status { get; set; }
        public List<string> destination_addresses { get; set; }
        public List<string> origin_addresses { get; set; }
        public List<Row> rows { get; set; }
    }

    public class Row
    {
        public List<Element> elements { get; set; }
    }

    public class Element
    {
        public Distance distance { get; set; }
        public Duration duration { get; set; }
        public string status { get; set; }
    }

    public class Distance
    {
        public string text { get; set; }
        public double value { get; set; }
    }

    public class Duration
    {
        public string text { get; set; }
        public double value { get; set; }
    }
}
