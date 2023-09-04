using System;
namespace Core.Infrastructure.Utils
{
    public static class Util
    {
        public static bool IsNull(object value)
        {
            if (value != null && !string.IsNullOrWhiteSpace(value.ToString()))
            {
                return false;
            }
            return true;
        }

        public static bool IsInt(object value)
        {
            if (IsNull(value))
                return false;

            int data;
            return int.TryParse(value.ToString(), out data);
        }

        public static bool IsBool(object value)
        {
            if (IsNull(value))
                return false;

            bool data;
            return bool.TryParse(value.ToString(), out data);
        }

        public static bool IsDateTime(object value)
        {
            if (IsNull(value))
                return false;

            DateTime data;
            return DateTime.TryParse(value.ToString(), out data);
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static double DateTimeToUnixTimeStamp(DateTime datetime)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var timestamp = (datetime.Subtract(dtDateTime)).TotalSeconds;
            return timestamp;
        }
        public static string AcronymFullname(string fullname)
        {
            var acronymFullname = "";
            try
            {
                if (fullname.Contains(" "))
                {
                    var items = fullname.Split(' ');
                    for (int i = 0; i < items.Length; i++)
                    {
                        if (i < items.Length - 1)
                        {
                            acronymFullname += items[i][0];
                        }
                        else if (i == (items.Length - 1))
                        {
                            acronymFullname += items[i];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                acronymFullname = "ER";
            }
            return acronymFullname;
        }
    }
}
