using System;

namespace Web_Book_BE.Utils
{
    public static class IdGenerator
    {
        private static readonly Random _random = new Random();
        public static string RandomDigits(int length = 6)
        {
            string digits = "";
            for (int i = 0; i < length; i++)
            {
                digits += _random.Next(0, 10);
            }
            return digits;
        }
    }
}
