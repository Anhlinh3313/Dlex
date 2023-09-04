using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace Core.Business.Core.Helpers
{
    public static class StringHelper
    {
        public static string[] _REPLACES_LOCATION_NAME = { " ", "NUOC", "TINH", "THANHPHO", "QUAN", "PHUONG", "XA", "HUYEN", "TP", "TT", "COUNTRY", "CITY", "STATE", "PROVINCE", "DISTRICT", "WARD" };
        // <summary>
        /// Compute the distance between two strings.
        /// </summary>
        public static int Compute(string s, string t)
        {
            return LevenshteinDistance.Compute(s, t);
        }
        public static object GetBestMatches<T>(IEnumerable<T> sources, string columnNameResult, string columnNameCompare, string search, uint? percentlimitCost = null, string[] replaces = null) where T : class
        {
            if (sources == null)
            {
                throw new System.ArgumentNullException(nameof(sources));
            }

            search = search.ToAscii().ToUpper();
            if (replaces != null && replaces.Count() > 0)
            {
                search = replaces.Aggregate(search, (c1, c2) => c1.Replace(c2, ""));
            }
            uint limitCost = (percentlimitCost ?? 0) != 0 ? (uint)(search.Length) * percentlimitCost.Value / 100 : 0;
            int index = 0;
            dynamic minDistance = new ExpandoObject();
            minDistance.Cost = 0;
            //minDistance.Id = 0;
            foreach (var source in sources)
            {
                if (source.GetPropValue(columnNameCompare) != null)
                {
                    string value = source.GetPropValue(columnNameCompare) + "";
                    value = value.ToAscii().ToUpper();
                    if (replaces != null && replaces.Count() > 0)
                    {
                        value = replaces.Aggregate(value, (c1, c2) => c1.Replace(c2, ""));
                    }
                    int cost = LevenshteinDistance.Compute(value, search);

                    if (index == 0 || minDistance.Cost > cost)
                    {
                        minDistance.Cost = cost;
                        if (source.GetPropValue(columnNameResult) != null)
                        {
                            minDistance.Id = source.GetPropValue(columnNameResult);
                        }
                        else
                        {
                            minDistance.Id = 0;
                        }
                        //name = location.Name;
                        index++;
                    }
                }
            }
            if (minDistance.Cost > limitCost && limitCost > 0)
            {
                return 0;
            }
            return minDistance.Id;
        }
    }
    public static class LevenshteinDistance
    {
        // <summary>
        /// Compute the distance between two strings.
        /// </summary>
        public static int Compute(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }
            if (m == 0)
            {
                return n;
            }
            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[n, m];
        }
    }

}
