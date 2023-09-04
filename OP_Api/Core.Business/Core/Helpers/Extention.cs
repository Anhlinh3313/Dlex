using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Core.Business.Core.Helpers
{
    public static class Extention
    {
        public static object GetPropValue(this object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
        public static string ToAscii(this string value)
        {
            return RemoveDiacritics(value);
        }
        private static string RemoveDiacritics(this string value)
        {
            string valueFormD = value.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (System.Char item in valueFormD)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(item);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(item);
                }
            }

            return (stringBuilder.ToString().Normalize(NormalizationForm.FormC));
        }
    }
}
