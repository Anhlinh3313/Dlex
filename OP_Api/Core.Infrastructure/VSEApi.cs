using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Drawing.Imaging;

namespace Core.Api.Infrastruture
{
    public class VSEApi
    {
        public const string VSEApiUri = "http://api.vietstarexpress.com:7077/";
        public const string VSEApiUriImg = "http://api.vietstarexpress.com:2511/";
        public const string VSEApiUriDelivery = "http://api.vietstarexpress.com:2511/";
        public const string VSEApiUriBillInfo = "http://api.vietstarexpress.com:2511/";
        public const string VSEApiUriSendSMS = "http://api.vietstarexpress.com:2511/";
        public const string apiKey = "36dd3413600b3f90192d3945e07ea10e";

        public VSEApi()
        {
        }

        public async Task<string> CreateEBill(
            string SO_VAN_DON, DateTime NGAY_VD, string CREATEDBY, string TU_TINH, string TU_HUYEN, string KH_GUI, string NG_SDT, string DIA_CHI_NGUOI_GUI,
            string DEN_TINH, string DEN_HUYEN, string DEN_XA, string NGUOI_GUI, string NGUOI_NHAN, string NN_SDT, string DIA_CHI_NGUOI_NHAN,
            double KL_VAN_DON, double KL_VAN_DON_QD, double TIEN_CUOC, double TIEN_CUOC_KS, string DICH_VU, string HINH_THUC_TT,
            string GHI_CHU_NHAN_HANG, string TRA_NGAY, string TEN_HANG_HOA, double TIEN_THU_HO, string LOAI_HH, int SO_KIEN, string NV_NHAN, DateTime TIME_NHAN
            , string REF_NUMBER, List<PHI_ITEM> PhiItem)
        {
            //return "NO... ONLY VIETSTAR";
            //string sNGAY_VD = NGAY_VD.ToString(@"yyyy-MM-ddTHH:mm:ss");
            string sNGAY_VD = NGAY_VD.ToString(@"yyyy-MM-dd");

            var objContent = JsonConvert.SerializeObject(new
            {
                SO_VAN_DON = SO_VAN_DON,
                NGAY_VD = sNGAY_VD,
                NGAY_DT = sNGAY_VD,
                CREATEDBY = CREATEDBY,
                TU_TINH = TU_TINH,
                TU_HUYEN = TU_HUYEN,
                KH_GUI = KH_GUI,
                DIA_CHI_NGUOI_GUI = DIA_CHI_NGUOI_GUI,
                KH_TRA = "",
                DEN_TINH = DEN_TINH,
                DEN_HUYEN = DEN_HUYEN,
                DEN_XA = DEN_XA,
                NGUOI_GUI = NGUOI_GUI,
                NGUOI_NHAN = NGUOI_NHAN,
                DIA_CHI_NGUOI_NHAN = DIA_CHI_NGUOI_NHAN,
                KL_VAN_DON = KL_VAN_DON,
                KL_VAN_DON_QD = KL_VAN_DON_QD,
                TIEN_CUOC = TIEN_CUOC,
                TIEN_CUOC_KS = TIEN_CUOC_KS,
                M_PRODUCT_ID = DICH_VU,
                HINH_THUC_TT = HINH_THUC_TT,
                GHI_CHU_TRA_HANG = "",
                GHI_CHU_NHAN_HANG = GHI_CHU_NHAN_HANG,
                GHI_CHU_VD_HONG = "",
                GHI_CHU_VD_TRANG = "Vận đơn đồng bộ DSC",
                VAN_DON_TONG_ID = "",
                PHI = 0,
                PHI_KS = 0,
                TRA_NGAY = TRA_NGAY,
                TEN_HANG_HOA = TEN_HANG_HOA,
                NV_NHAN = NV_NHAN,
                TIME_NHAN = TIME_NHAN,
                DOANH_THU = 0,
                TIEN_THU_HO = TIEN_THU_HO,
                LOAI_HH = LOAI_HH,
                BAO_PHAT = 0,
                SO_KIEN = SO_KIEN,
                LOAI_AP_GIA = "",
                NGAY_YC_TRAHANG = "",
                GIA_TRI_HH = 0,
                FMIS_BANG_GIA_ID = "",
                FMIS_BANG_GIA_PUBLIC_ID = "",
                GIA_PUBLIC = "",
                CHUC_NANG = "Bill DSC",
                NVKD = "",
                LOAI_VD = "",
                NG_SDT = NG_SDT,
                NN_SDT = NN_SDT,
                REF_NUMBER = REF_NUMBER,
                PhiItem = PhiItem
            });
            //Console.WriteLine($"objContent: {objContent}");
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(VSEApiUri);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));


            HttpContent content = new StringContent(
                objContent.ToString()
                , Encoding.UTF8
                , "application/json");

            HttpResponseMessage response = client.PostAsync($"api/DSCBill?apiKey={apiKey}", content).Result;

            if (response.IsSuccessStatusCode)
            {
                string sOutput = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(sOutput))
                {
                    return "Server return null!";
                }
                else
                {
                    JObject json = JObject.Parse(sOutput);
                    if ("true".Equals(json["success"].ToString().ToLower()))
                    {
                        return "success";
                    }
                }
                return sOutput;
            }
            else
            {
                string sOutput = await response.Content.ReadAsStringAsync();
                var objOutput = JsonConvert.DeserializeObject<ApiReturn>(sOutput);

                return objOutput.message;
            }
        }

        public async Task<string> PushImagePickup(
           string VD_ID, string SVD, string SMS_ID, string FILENAME, string MANV, string LY_DO, string KY_NHAN, string IMAGE)
        {
            //

            byte[] image = Convert.FromBase64String(IMAGE);
            using (var ms = new MemoryStream(image))
            {
                Image img = Image.FromStream(ms);
                if (img.Width > 1200 || img.Height > 1200)
                {
                    int newW = 0;
                    int newH = 0;
                    if (img.Width > img.Height)
                    {
                        newW = 1200;
                        double percent = (1200 * 100 / img.Width);
                        newH = (int)Math.Round((img.Height * percent / 100), 0);
                    }
                    else
                    {
                        newH = 1200;
                        double percent = (1200 * 100 / img.Height);
                        newW = (int)Math.Round((img.Width * percent / 100), 0);
                    }
                    //
                    Size size = new Size(newW, newH);
                    img = (Image)(new Bitmap(img, size));
                    //
                    //ImageToByteArray();
                    //img.Save(ms, img.RawFormat);
                    var imageBytes = ImageToByteArray(img);
                    // Convert byte[] to Base64 String
                    string base64String = Convert.ToBase64String(imageBytes);
                    IMAGE = base64String;
                }
            }
            //
            var formContent = new MultipartFormDataContent
            {
            //send form text values here
                 {new StringContent(VD_ID),"VD_ID"},
                 {new StringContent(SVD), "SVD"},
                 {new StringContent(SMS_ID),"SMS_ID"},
                 {new StringContent(MANV),"MANV"},
                 {new StringContent(KY_NHAN),"KY_NHAN"},
                 {new StringContent(LY_DO),"LY_DO"},
                 {new StringContent(FILENAME),"FILENAME"},
                 {new StringContent(IMAGE),"IMAGE"}
            };
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(VSEApiUriImg);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.PostAsync($"api/MsgGateWay/syncVdTrang", formContent).Result;

            if (response.IsSuccessStatusCode)
            {
                string sOutput = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(sOutput))
                {
                    return "Server return null!";
                }
                else
                {
                    JObject json = JObject.Parse(sOutput);
                    if ("true".Equals(json["isError"].ToString().ToLower()))
                    {
                        return "faile: " + sOutput;
                    }
                    else
                    {
                        return "success";
                    }
                }
            }
            else
            {
                string sOutput = await response.Content.ReadAsStringAsync();
                var objOutput = JsonConvert.DeserializeObject<ApiReturn>(sOutput);

                return objOutput.message;
            }
        }

        private byte[] ImageToByteArray(Image imageIn)
        {
            using (var stream = new MemoryStream())
            {
                imageIn.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }


        public async Task<string> PushImageDelivery(
         string _VD_ID, string _SVD, string _SMS_ID, string _MANV, string _KY_NHAN, string _LY_DO, string
            _FILENAME, string _IMAGE, string _NGAY)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(VSEApiUriDelivery);
            //
            byte[] image = Convert.FromBase64String(_IMAGE);
            using (var ms = new MemoryStream(image))
            {
                Image img = Image.FromStream(ms);
                if (img.Width > 1000 || img.Height > 1000)
                {
                    int newW = 0;
                    int newH = 0;
                    if (img.Width > img.Height)
                    {
                        newW = 1000;
                        double percent = (1000 * 100 / img.Width);
                        newH = (int)Math.Round((img.Height * percent / 100), 0);
                    }
                    else
                    {
                        newH = 1000;
                        double percent = (1000 * 100 / img.Height);
                        newW = (int)Math.Round((img.Width * percent / 100), 0);
                    }
                    //
                    Size size = new Size(newW, newH);
                    img = new Bitmap(img, size);
                    //
                    //ImageToByteArray();
                    //img.Save(ms, img.RawFormat);
                    var imageBytes = ImageToByteArray(img);
                    // Convert byte[] to Base64 String
                    string base64String = Convert.ToBase64String(imageBytes);
                    _IMAGE = base64String;
                }
            }
            //
            var formContent = new MultipartFormDataContent
            {
                { new StringContent(_VD_ID),"VD_ID"},
                 {new StringContent(_SVD), "SVD"},
                 {new StringContent(_SMS_ID),"SMS_ID"},
                 {new StringContent(_FILENAME),"FILENAME"},
                 {new StringContent(_MANV),"MANV"},
                 {new StringContent(_LY_DO),"LY_DO"},
                 {new StringContent(_KY_NHAN),"KY_NHAN"},
                 {new StringContent(_IMAGE),"IMAGE"},
                 {new StringContent(_NGAY),"NGAY"}
            };
            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.PostAsync($"api/MsgGateWay/SyncVDHong_DSC", formContent).Result;
            if (response.IsSuccessStatusCode)
            {
                string sOutput = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(sOutput))
                {
                    return "Server return null!";
                }
                else
                {
                    JObject json = JObject.Parse(sOutput);
                    if ("true".Equals(json["isError"].ToString().ToLower()))
                    {
                        return "faile: " + json["errorMessage"];
                    }
                    else
                    {
                        return "success";
                    }
                }
            }
            else
            {
                string sOutput = await response.Content.ReadAsStringAsync();
                var objOutput = JsonConvert.DeserializeObject<ApiReturn>(sOutput);
                return objOutput.message;
            }
        }


        public async Task<BILL_INFO> GetBillInfo(string shipmentNumber)
        {
            var objContent = JsonConvert.SerializeObject(new
            {
                user = "50611",
                pass = "qqqq",
                apikey = "eSg1JdaPo1k:APA91bHGaq6ilA_1N7cVh2nxUpSunxy5s0Z5IMWY70uTs5BPNrwvvhzpA2pU24cncdgBh5LHV_w6O5_ZPUUXOxwMP-ImM2Rem6ATmWMP5t3S9WotemyLKHVMq572ywK2F3nlNQwbFS89",
                data = new { bill = shipmentNumber }
            });
            //Console.WriteLine($"objContent: {objContent}");
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(VSEApiUriBillInfo);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));


            HttpContent content = new StringContent(
                objContent.ToString()
                , Encoding.UTF8
                , "application/json");

            HttpResponseMessage response = client.PostAsync($"api/billinfo", content).Result;

            if (response.IsSuccessStatusCode)
            {
                string sOutput = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(sOutput))
                {
                    return null;
                }
                else
                {
                    JObject json = JObject.Parse(sOutput);
                    if ("true".Equals(json["success"].ToString().ToLower()))
                    {
                        var objOutput = JsonConvert.DeserializeObject<ResultReturn>(sOutput);
                        return objOutput.data;
                    }
                }
                return null;
            }
            else
            {
                return null;
            }
        }


        public async Task<bool> SendSMSNormal(string phoneNumber, string messager)
        {
            var objContent = JsonConvert.SerializeObject(new
            {
                user = "50611",
                pass = "qqqq",
                apikey = "eSg1JdaPo1k:APA91bHGaq6ilA_1N7cVh2nxUpSunxy5s0Z5IMWY70uTs5BPNrwvvhzpA2pU24cncdgBh5LHV_w6O5_ZPUUXOxwMP-ImM2Rem6ATmWMP5t3S9WotemyLKHVMq572ywK2F3nlNQwbFS89",
                data = new { Phone = phoneNumber, Messenger = messager }
            });
            //Console.WriteLine($"objContent: {objContent}");
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(VSEApiUriSendSMS);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));


            HttpContent content = new StringContent(
                objContent.ToString()
                , Encoding.UTF8
                , "application/json");

            HttpResponseMessage response = client.PostAsync($"api/SMS", content).Result;

            if (response.IsSuccessStatusCode)
            {
                string sOutput = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(sOutput))
                {
                    return false;
                }
                else
                {
                    JObject json = JObject.Parse(sOutput);
                    if ("true".Equals(json["success"].ToString().ToLower()))
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        public class ApiReturn
        {
            public string message { get; set; }
        }

        public class ApiSendSMSNormalReturn
        {
            public string success { get; set; }
        }

        public class ResultReturn
        {
            public string success { get; set; }
            public BILL_INFO data { get; set; }
        }

        public class VSEStatusResult
        {
            public bool success { get; set; }
            public string message { get; set; }
        }

        public class PHI_ITEM
        {
            public PHI_ITEM() { }
            public string SO_VAN_DON { get; set; }
            public string MA_PHI { get; set; }
            public string PHI { get; set; }
            public double? PHI_KS { get; set; }
        }

        public static class FMIS_PHI
        {
            public static string PHI_HOA_CHAT_CHAT_LONG = "06";
            public static string PHI_NANG_HA_HANG = "07";
            public static string PHI_BAO_PHAT = "08";
            public static string PHI_BAO_HIEM = "09";
            public static string PHI_HO_SO_THAU = "10";
            public static string PHI_HOA_DON = "11";
            public static string PHI_HOA_TOC = "12";
            public static string PHI_KIEM_DEM = "05";
            public static string PHI_KHAC = "03";
            public static string PHI_HANG_XA = "01";
            public static string PHI_THU_HO = "02";
            public static string PHI_DONG_KIEN = "04";
        }

        public class BILL_INFO
        {
            public BILL_INFO() { }
            public string SO_VAN_DON { get; set; }
            public DateTime NGAY_VD { get; set; }
            public string HINH_THUC_TT { get; set; }
            public string TRA_NGAY { get; set; }
            public string IS_CHUYEN_HOAN { get; set; }
            public string TU_TINH { get; set; }
            public string TU_HUYEN_MA { get; set; }
            public string KH_GUI_VALUE { get; set; }
            public string DIA_CHI_NGUOI_GUI { get; set; }
            public string KH_TRA_VALUE { get; set; }
            public string DEN_TINH { get; set; }
            public string DEN_HUYEN_MA { get; set; }
            public string DEN_XA_MA { get; set; }
            public string NGUOI_GUI { get; set; }
            public string NGUOI_NHAN { get; set; }
            public string DIA_CHI_NGUOI_NHAN { get; set; }
            public double? KL_VAN_DON_QD_KS { get; set; }
            public double? KL_VAN_DON_KS { get; set; }
            public double? TIEN_CUOC { get; set; }
            public double? TIEN_CUOC_KS { get; set; }
            public string PRODUCT_VALUE { get; set; }
            public string GHI_CHU_TRA_HANG { get; set; }
            public string GHI_CHU_NHAN_HANG { get; set; }
            public double? PHI { get; set; }
            public double? PHI_KS { get; set; }
            public string NV_NHAN_VALUE { get; set; }
            public DateTime? TIME_NHAN { get; set; }
            public string NV_TRA_VALUE { get; set; }
            public DateTime? TIME_TRA { get; set; }
            public string TEN_NGUOI_NHAN { get; set; }
            public string NGUOI_UPDATE_LIEN_TRANG { get; set; }
            public string NGUOI_UPDATE_LIEN_HONG { get; set; }
            public double? DOANH_THU { get; set; }
            public double? TIEN_THU_HO { get; set; }
            public string LOAI_HH { get; set; }
            public int? SO_KIEN { get; set; }
            public double? GIA_TRI_HH { get; set; }
            public double? GIA_PUBLIC { get; set; }
            public string MA_BUU_CUC_TRA { get; set; }
            public string NVKD_MA { get; set; }
            public string SO_VAN_DON_HOAN { get; set; }
            public string NG_SDT { get; set; }
            public string NN_SDT { get; set; }
            public List<PHI_ITEM> DT_PHI { get; set; }
        }
    }
}
