using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Core.Api
{
    class ChangeFormatDataFile
    {
        public static double ChangeFormatINT(string data)
        {
          try
            {

                string dataresult = "";

                if (data.Contains(".") == true)
                {
                    string[] arrListStr = data.Split('.');
                    foreach (var item in arrListStr)
                    {
                        dataresult = dataresult+ FormatNumberType(item) + ".";
                    }
                    return double.Parse(dataresult.Substring(0, dataresult.Length - 1));
                }
                else
                {
                    dataresult = FormatNumberType(data);
                    return double.Parse(dataresult);
                }
            }
            catch
            {
                return double.Parse(data);
            }
            
        }
        public static string FormatNumberType(string number)
        {
           var Icheck =  string.Format(CultureInfo.InvariantCulture, "{0:n0}", int.Parse(number));
            return Icheck;
        }

    }
}
