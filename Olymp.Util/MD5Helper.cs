using System.Security.Cryptography;
using System.Text;

namespace Olymp.Util
{
    public class MD5Helper
    {
        public static string CalculateMD5Hash(string input)
        {
            var md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hash = md5.ComputeHash(inputBytes);
            var sb = new StringBuilder();
            foreach (var t in hash) sb.Append(t.ToString("X2"));

            return sb.ToString();
        }
    }
}