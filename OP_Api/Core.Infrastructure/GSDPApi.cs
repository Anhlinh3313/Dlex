using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Core.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.Api.Infrastruture
{
    public class GSDPApi
    {
        public const string GSDPApiUri = "http://192.168.10.66:8088/";
        public const string GSDPStagingApiUri = "http://192.168.10.66:8081/";
        public const string apiKey = "";

        public GSDPApi()
        {
        }
        public async Task<List<BILL_INFO>> GetBillInfo(string date)
        {
            //Console.WriteLine($"objContent: {objContent}");
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(GSDPApiUri);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync(string.Format("api/values/GetShipmentWaybillData?date_={0}", date)).Result;

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
                        var objOutput = JsonConvert.DeserializeObject<List<BILL_INFO>>(sOutput);
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
        public async Task<List<BILL_INFO>> GetBillInfoStaging(string date)
        {
            //Console.WriteLine($"objContent: {objContent}");
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(GSDPStagingApiUri);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync(string.Format("api/values/GetShipmentWaybillData?date_={0}", date)).Result;

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
                        var objOutput = JsonConvert.DeserializeObject<List<BILL_INFO>>(sOutput);
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
        public async Task<List<BILL_RETURN>> GetBillReturn(string date)
        {
            //Console.WriteLine($"objContent: {objContent}");
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(GSDPApiUri);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync(string.Format("api/values/GetARCreditMemo?date_={0}", date)).Result;

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
                        var objOutput = JsonConvert.DeserializeObject<List<BILL_RETURN>>(sOutput);
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
        public async Task<List<BILL_RETURN>> GetBillReturnStaging(string date)
        {
            //Console.WriteLine($"objContent: {objContent}");
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(GSDPStagingApiUri);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync(string.Format("api/values/GetARCreditMemo?date_{0}", date)).Result;

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
                        var objOutput = JsonConvert.DeserializeObject<List<BILL_RETURN>>(sOutput);
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

        public async Task<ResRokenGSDP> GetTokenGSDPStaging()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(GSDPStagingApiUri);
            var request = new HttpRequestMessage(HttpMethod.Post, "token");
            // Add an Accept header for JSON format.
            var requestContent = string.Format("grant_type={0}&client_id={1}&client_secret={2}", "client_credentials", "admin", 1234);
            // Add Authorization
            request.Content = new StringContent(requestContent, Encoding.UTF8, "application/x-www-form-urlencoded");
            var response = await client.SendAsync(request);
            ResRokenGSDP res = new ResRokenGSDP();
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    string sOutput = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrWhiteSpace(sOutput))
                    {
                        res.error = "-103";
                        res.error_description = "Return result empty, plase try again!";
                        return res;
                    }
                    else
                    {
                        try
                        {
                            var objOutput = JsonConvert.DeserializeObject<ResRokenGSDP>(sOutput);
                            return objOutput;
                        }
                        catch (Exception ex)
                        {
                            res.error = "-102";
                            res.error_description = ex.Message;
                            return res;
                        }
                    }
                }
                catch (Exception ex)
                {
                    res.error = "-101";
                    res.error_description = ex.Message;
                    return res;
                }
            }
            else
            {
                res.error = "-100";
                res.error_description = "Connect api to HD fail!";
                return res;
            }
        }

        public async Task<ResIncomingPayment> PushIncomingPaymentGSDP(List<IncomingPayment> models, string _company)
        {
            var resToken = await GetTokenGSDPStaging();
            string token = resToken.access_token;
            ResIncomingPayment res = new ResIncomingPayment();
            //
            if (string.IsNullOrWhiteSpace(token))
            {
                res.ErrorCode = 105;
                res.ErrorMessage = "Đăng nhập không thành công";
            }
            using (var client = new HttpClient())
            {
                string apiUri = GSDPStagingApiUri;
                if (_company.ToUpper() == "GSDP") apiUri = GSDPApiUri;
                var postData = JsonConvert.SerializeObject(models);
                client.BaseAddress = new Uri(apiUri);
                var stringContent = new StringContent(postData, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                var result = client.PostAsync("api/Values/IncomingPayment", stringContent);
                string resultContent = await result.Result.Content.ReadAsStringAsync();
                res.body = postData;
                //Console.WriteLine(resultContent);
                try
                {
                    if (string.IsNullOrWhiteSpace(resultContent))
                    {
                        res.ErrorCode = -103;
                        res.ErrorMessage = "Return result empty, plase try again!";
                        return res;
                    }
                    else
                    {
                        try
                        {
                            res = JsonConvert.DeserializeObject<ResIncomingPayment>(resultContent);
                            res.body = postData;
                            return res;
                        }
                        catch (Exception ex)
                        {
                            res.ErrorCode = -102;
                            res.ErrorMessage = ex.Message;
                            return res;
                        }
                    }
                }
                catch (Exception ex)
                {
                    res.ErrorCode = -101;
                    res.ErrorMessage = ex.Message;
                    return res;
                }
            }
        }
        public class ApiReturn
        {
            public string message { get; set; }
        }

        public class ResultReturn
        {
            public string message { get; set; }
            public List<BILL_INFO> datas { get; set; }
        }

        public class BILL_INFO
        {
            public BILL_INFO() { }

            public string GroupCode { get; set; }
            public string GroupName { get; set; }
            public string DocType { get; set; }
            public string DocumentNo { get; set; }
            //public DateTime? ShipDate { get; set; }
            public string ShipDate { get; set; }
            public string CustomerCode { get; set; }
            public string CustomerName { get; set; }
            public string CustomerAddress { get; set; }
            public string Province { get; set; }
            public string Tel { get; set; }
            public string Email { get; set; }
            public string ReceiverCode { get; set; }
            public string ReceiverName { get; set; }
            public string RecAddress { get; set; }
            public string RecProvince { get; set; }
            public string RecWard { get; set; }
            public string RecDistrict { get; set; }
            public string Route { get; set; }
            public string RecTel { get; set; }
            public double DocTotal { get; set; }
            public double CODAmount { get; set; }
            public string DeliveryStaff { get; set; }
            public string DeliveryStatus { get; set; }
            public string ReturnReason { get; set; }
            public string CashTransfer { get; set; }
            public string Contents { get; set; }
            public double Weight { get; set; }
            public string WeightUnit { get; set; }
            public double Volume { get; set; }
            public string VolUnit { get; set; }
            public DateTime? CreateDate { get; set; }
            public string CreateTime { get; set; }
            public string MaKho { get; set; }
            public string DieuKienBaoQuan { get; set; }
            public string Remark { get; set; }
            public string SOENTRY { get; set; }
            public DateTime? SODOCDATE { get; set; }
            public string SODOCTIME { get; set; }
            public string FirmCode { get; set; }
            public string FirmName { get; set; }
            public int SoKien { get; set; }
        }

        public class BILL_RETURN
        {
            public BILL_RETURN() { }

            public string ARNo { get; set; }
            public string ARCreditMemoNo { get; set; }
            public DateTime? ARCreditMemoDocDate { get; set; }
        }
        //
        public class IncomingPayment
        {
            public IncomingPayment() { }

            public string CustomerCode { get; set; }
            public double? DocTotal { get; set; }
            public double? Total { get; set; }
            public DateTime? DocDate { get; set; }
            public string PaymentType { get; set; }
            public string BankAccount { get; set; }
            public DateTime? TransferDate { get; set; }
            public string DocumentNo { get; set; }
            public string Remarks { get; set; }
            public string CreateUser { get; set; }
            public string CashFlow { get; set; }
            public int Flag { get; set; }
            public string TMSNumber { get; set; }
            public int IsTest { get; set; }
        }

        public class ResIncomingPayment
        {
            public ResIncomingPayment() { }
            public int ErrorCode { get; set; } = 500;
            public string ErrorMessage { get; set; }
            public List<ErrorData> ErrorDatas { get; set; }
            public string Message { get; set; }
            public string RefCode { get; set; }
            public string body { get; set; }
        }

        public class ErrorData
        {
            public ErrorData() { }
            public string TMSNumber { get; set; }
            public string ErrorMessage { get; set; }
        }

        public class ResRokenGSDP
        {
            public ResRokenGSDP() { }
            public string access_token { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; }
            public string error { get; set; }
            public string error_description { get; set; }
        }
    }
}
