using System.IO.Abstractions;
using System.Security.Cryptography;
using System.Text;

namespace Svenkle.TwoPly.Extensions
{
    public static class FileInfoBaseExtensions
    {
        public static string ToHash(this FileInfoBase fileInfoBase)
        {
            using (var stream = fileInfoBase.OpenRead())
            {
                using (var sha = new SHA1Managed())
                {
                    var hashBytes = sha.ComputeHash(stream);

                    var sb = new StringBuilder();
                    foreach (var b in hashBytes)
                        sb.Append(b.ToString("X2"));

                    return sb.ToString();
                }
            }
        }
    }
}
