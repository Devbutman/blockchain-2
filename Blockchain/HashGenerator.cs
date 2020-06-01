using System.Security.Cryptography;
using System.Text;

namespace Blockchain
{
    public static class HashGenerator
    {
        public static string ComputeHash(string rawData)
        {
            using var sha256 = SHA256.Create();

            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));

            var sb = new StringBuilder();
            foreach (var b in bytes) sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }
}