using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Utils
{
    public static class FireBaseUtil
    {
        //flashship HCM
        //public const string BrowserAPIKey = "AAAA-1nB5Do:APA91bGkzPEGLTMKfZI0MadiXXH2z6PapzbVAUqwz-7PRs5fw6grpHJCEfE98fHMQSQTDtIKrQ6iYpakCasImuBUB5mzerm1TL8L6kV1wEv8CVOqmNPoT-yV6ss7Eze8_0Gt7spdPjlP";
        //flashship DN
        //public const string BrowserAPIKey = "AAAAL1-Icr0:APA91bFRJYpSZ3XgenjXMpiYOdMf8DQLWxknTVY6vjCkSyk0fbGKoTec-2nmvVCeHhGi2bG6-A_-5Vicil-iPbe9blAHGnT5T0lnimfUQHvDpP7as21L1aZDCdEHgRaXxTXrxoosUzYN";
        //dlex
        //public const string BrowserAPIKey = "AAAAnlAYTZw:APA91bHFNPhlpVrnTj6V3OzhJTdeV6IgB5ML7IQFXPZq4Hv1RCX7FHdVnTZSdEcffELKikz38p4i2c8N6U0uts2Y99uCXSUzwTeHvv3AU_1bJ47jeLA4JNJTng5FEYYKFjJUZ-CkLSF5";
        //dlexs
        //public const string BrowserAPIKey = "AAAAnlAYTZw:APA91bHFNPhlpVrnTj6V3OzhJTdeV6IgB5ML7IQFXPZq4Hv1RCX7FHdVnTZSdEcffELKikz38p4i2c8N6U0uts2Y99uCXSUzwTeHvv3AU_1bJ47jeLA4JNJTng5FEYYKFjJUZ-CkLSF5";
        //tasetco
        //public const string BrowserAPIKey = "AAAAAI8TvZA:APA91bE7af2t6fDh4evAbx3mshIDIIJz1eNlSVGiVWmoKutUJaP1s1-GSGGXlTvPrOcDaGmhkCiHYb521cfijFNiwyE8WtcVrZSfanJz1MavVS8kRZYRYuNSizwkW8bIkQ5gC0WF3v-Q";
        //vietstar
        //public const string BrowserAPIKey = "AAAAOPH75HQ:APA91bHFsCJ_FRLCZF4gOfu1lfaYD0k4YBboN6y7FM7IUgtJ-SrrEiX8gPgd73QpK0fRrT1locj4fMorrhsmOdL3DI7Mt2GD-KLWIm-IXaL7_1iFGhiHTa07QQtn_Cz5QYZtZH9Oqr70";
        //ghbh
        //public const string BrowserAPIKey = "AAAAL5s3TkA:APA91bFZY_1IsECKgvEJ6vNUffmrIHG8f3qsoK08dhaDJC6egxIBwZt3T9khGalq3rMzIOIuNDBaH9nckfCpY-rLpcbJR_jGpMDlmfsIUKj_RRbocuNz2OnWfl7V4MZkOTBh81tMOwGW";
        //gsdp
        public const string BrowserAPIKey = "AAAAclJdHxg:APA91bF3ojRfKAhDJJDw6mj1TL5U9qqEwN7y_i9LLCJAh4vFr38Auk8bhR_mLqFAU8EdPuJhsgE33BjHA97l6h2U1Jv3qR5kmtHs_p9YgaANrr-qz26SdV7Q_36KlTlFHxknvXLiNDK0";
        //delta
        //public const string BrowserAPIKey = "AAAAHNpkwps:APA91bFVbONYZeTtsQc8pF20-FNdKO-ATxTYVl1m_gfb4FOqGXV4NwxxomRxxK2Be8XC4EviumqrQ0EhPqdYoZTYrrrVU13JHmmAHFcehiIjEs_btlMTTC3crRqD6prlxlYoB3IvndNG";
        //shipnhanh
        //public const string BrowserAPIKey = "AAAAm8gw4IE:APA91bHa1fcxIRQsQuUng6GEzpEUL3YLAVHsCd-5KcoXGwIvR_ucSGMP7oWl7VjBRRqUPqlnp2xvGZlCx9jNC6phJzQN3MnGJX92JZrMKMOQ7v9T0jDOKTxavpdOWB_acMDyIyKEeo2K";
        //Airline Express
        //public const string BrowserAPIKey = "AAAAkwOtt6Y:APA91bH1KdoDENb56QrmaSk_aa-J8uNSH5wtsgzah5YVUMnUh61LH38w9omss4YAemAg5b6CsKl_P2q9T5qF_AhI4-l0QO9-EqmZDOCUzDn79TdkkYbI2GVBS6Xoj8_mxKHuls9q72g3";
        //Vintrans
        //public const string BrowserAPIKey = "AAAA-2VAwpg:APA91bE84hyJ7tP6E6So0ErvVGW5252KCxcRd1MYklXBMxWnVKD6AYWCDxdGduVUz6OS2q1NN3wfql3YFdbJr12JcEyoO4JO4NZQqZJrO5Tymyd_ekOaN0OPevax5bo8miRnVJcFtuJu";
        //pcs
        //public const string BrowserAPIKey = "AAAAxbhGYrk:APA91bFJHd2JavDZheIF-iMkXLVtPpdsTVfORq87WCWhls4oSmVfnWDYv8TLsJccw8vUWC_RteyhpG67jbNGKCjoGcqopnn15IdWvEJDEBE1ghifVs2dh7CPen_zbBomicPoTj67TF66";
        //BE GROUP
        //public const string BrowserAPIKey = "AAAA3jw5nvU:APA91bEmFZA0_gSFXiG0Vq8E0x-Wdz5XwB6KKBJUAolZWBym1KExgcHuMQkJz8QlTjIlR3Ij_KZ9yC8YNnVRvO-pHRmHcwFlXXUfROL7Tufmzdo_TLI09fQZ8lxjGQvK5exwy-gtb-YH";
        //Airline Express
        //public const string BrowserAPIKey = "AAAAkwOtt6Y:APA91bH1KdoDENb56QrmaSk_aa-J8uNSH5wtsgzah5YVUMnUh61LH38w9omss4YAemAg5b6CsKl_P2q9T5qF_AhI4-l0QO9-EqmZDOCUzDn79TdkkYbI2GVBS6Xoj8_mxKHuls9q72g3";

        public const string CONTENT_TYPE_JSON = "application/json";
        public const string TOPIC_NAME_REQUEST_LADING = "post";//Topic type : YÊU CẦU LẤY HÀNG

        public static bool RegisterTopic(string registerToken, int? topicId = null)
        {
            var result = false;
            string topicName = TOPIC_NAME_REQUEST_LADING;
            if (topicId.HasValue)
                topicName += "-" + topicId;

            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("https://iid.googleapis.com/iid/v1/" + registerToken + "/rel/topics/" + topicName);
            Request.Method = "POST";
            Request.KeepAlive = false;
            Request.ContentType = CONTENT_TYPE_JSON;
            Request.Headers.Add(string.Format("Authorization: key={0}", BrowserAPIKey));
            Request.ContentLength = 0;


            WebResponse Response = Request.GetResponse();
            HttpStatusCode ResponseCode = ((HttpWebResponse)Response).StatusCode;
            if (ResponseCode.Equals(HttpStatusCode.Unauthorized) || ResponseCode.Equals(HttpStatusCode.Forbidden))
            {
                //var text = "Unauthorized - need new token";
            }
            else if (!ResponseCode.Equals(HttpStatusCode.OK))
            {
                //var text = "Response from web service isn't OK";
            }
            else
            {
                result = true;
            }
            return result;
        }

        public static dynamic DeleteTopic(string registerToken, int? topicId = null)
        {
            var result = false;
            string topicName = TOPIC_NAME_REQUEST_LADING;
            if (topicId.HasValue)
                topicName += "-" + topicId;

            string postData = "{\"to\": \"/topics/" + topicName + "\","
                    + "\"registration_tokens\": [\"" + registerToken + "\"]"
                    + "}";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("https://iid.googleapis.com/iid/v1:batchRemove");
            Request.Method = "POST";
            Request.KeepAlive = false;
            Request.ContentType = CONTENT_TYPE_JSON;
            Request.Headers.Add(string.Format("Authorization: key={0}", BrowserAPIKey));
            Request.ContentLength = byteArray.Length;

            Stream dataStream = Request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse Response = Request.GetResponse();
            HttpStatusCode ResponseCode = ((HttpWebResponse)Response).StatusCode;
            if (ResponseCode.Equals(HttpStatusCode.Unauthorized) || ResponseCode.Equals(HttpStatusCode.Forbidden))
            {
                //var text = "Unauthorized - need new token";
            }
            else if (!ResponseCode.Equals(HttpStatusCode.OK))
            {
                //var text = "Response from web service isn't OK";
            }
            else
            {
                result = true;
            }
            return result;
        }

        public static async Task<bool> SendNotification(string browserAPIKey, string token, string message, int badge)
        {
            if (badge > 0)
            {
                await SendFCMNotification(
                    browserAPIKey
                    , GetPostData(token, message, badge)
                    );
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string GetPostData(string registrationID, string message, int badge)
        {
            return "{\"to\":\"" + registrationID + "\"," +
                "\"priority\": \"high\"," +
                "\"content_available\":true," +
                "\"notification\":{" +
                "\"sound\": \"default\"," +
                "\"badge\": \"" + badge + "\"," +
                "\"title\": \"Thông báo\"," +
                "\"body\":\"" + message + "\"}}"
                ;
        }

        public static async Task<string> SendFCMNotification(string apiKey, string postData)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://fcm.googleapis.com/");

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("key", "=" + apiKey);

            HttpContent content = new StringContent(
                postData
                , Encoding.UTF8
                , "application/json");

            HttpResponseMessage response = client.PostAsync("fcm/send", content).Result;

            if (response.IsSuccessStatusCode)
            {
                string sOutput = await response.Content.ReadAsStringAsync();
                return sOutput;
            }
            else
            {
                string sOutput = await response.Content.ReadAsStringAsync();
                return sOutput;
            }
        }
    }
}
