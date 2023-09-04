using System;
using System.Text;

namespace Core.Infrastructure.Utils
{
    public static class RandomUtil
    {
        public static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        public static string GetCode(int id, ushort length)
        {
            var divide = id / (int)(Math.Pow(10, (length -1) ));
            int surplus = id % (int)Math.Pow(10, (length - 1) );
            //Ký tự A đầu tiên A: 65 , Z:90
            var number = divide >= 10 ? (int)Math.Log10(divide) : 0;
            int numberZ = number / 25;//d= Z - A
            var z = ("").PadLeft(numberZ, 'Z');
            var alpha = z + (char)((number % 25) + 65);
            return alpha + surplus.ToString("D" + (length - 1) );
        }

        public static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public static string IntToLetters(int value)
        {
            string result = string.Empty;
            while (--value >= 0)
            {
                result = (char)('A' + value % 26) + result;
                value /= 26;
            }
            return result;
        }
    }
}
