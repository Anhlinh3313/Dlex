using System;
using System.Text.RegularExpressions;

namespace Core.Infrastructure.Utils
{
    public static class ShipmentNumberFormatUtil
    {
        public static bool IsValidShipmentNumberToWarehouse(string input)
        {
            if (CheckShipmentNumber(input) == false)
            {
                if (input.ToString().Length > 6 && input.ToString().Length < 20)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            if (Util.IsInt(input) && input.ToString().Length > 4 && input.ToString().Length < 15)
            {
                // lấy ra n -1 ký từ đầu tiên
                var numCheck = (Int32.Parse(input) / 10).ToString();
                var sumNumCheck = 0; // check vị trí lẻ  x 3 , vị trí chắn x 1, sau đó tính tổng 
                for (int i = 0; i < numCheck.Length; i++)
                {
                    int num;
                    bool isSuccess;
                    isSuccess = int.TryParse(numCheck[i].ToString(), out num);
                    if ((i + 1) % 2 == 1)
                    {
                        sumNumCheck += num * 3;
                    }
                    else
                    {
                        sumNumCheck += num;
                    }
                }
                int numMod = sumNumCheck % 10;
                var lastChar = "";
                if (numMod == 10 || numMod == 0)
                {
                    lastChar = "0";
                }
                else
                {
                    lastChar = (10 - numMod).ToString();
                }
                //var decimalString = (10 - (double)sumNumCheck / 10).ToString(); // lấy ra số có thập phân theo quy luật
                var stringAfterFormat = $"{numCheck}{lastChar}";
                if (stringAfterFormat == input)
                {
                    return true;
                }
            }
            //else if (input.ToString().Length > 6 && input.ToString().Length < 20)
            //{
            //    return true;
            //}
            return false;
        }

        public static bool CheckShipmentNumber(string shipmentNumber)
        {
            var regex = @"^[0-9a-zA-Z]{1,4}[0-9-]{1,12}$";
            var result = Regex.Match(shipmentNumber, regex, RegexOptions.IgnoreCase);
            return result.Success;
        }
    }
}
