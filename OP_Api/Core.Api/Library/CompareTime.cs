using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Library
{
    public class CompareTime
    {
        public static List<DateTime> GetCompareTime(DateTime startTimeDiscount, DateTime EndTimeDiscount, DateTime startTimeBK, DateTime startEndBK)
        {
            DateTime startTime = new DateTime();
            DateTime endTime = new DateTime();

            if(startTimeDiscount <= startTimeBK)
            {
                startTime = startTimeBK;
            }
            else
            {
                startTime = startTimeDiscount;
            }

            if (EndTimeDiscount >= startEndBK)
            {
                endTime = startEndBK;
            }
            else
            {
                endTime = EndTimeDiscount;
            }
            List<DateTime> list = new List<DateTime>();
            list.Add(startTime);
            list.Add(endTime);

            return list;
        }
    }
}
