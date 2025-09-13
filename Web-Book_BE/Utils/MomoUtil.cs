using System.Security.Cryptography;
using System.Text;

namespace Web_Book_BE.Utils
{
    public class MomoUtil
    {
        public static string CreateSignature(string rawData, string secretKey)
        {
            var encoding = new UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(secretKey);
            byte[] messageBytes = encoding.GetBytes(rawData);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashMessage = hmacsha256.ComputeHash(messageBytes);
                return BitConverter.ToString(hashMessage).Replace("-", "").ToLower();
            }
        }
    }
}
