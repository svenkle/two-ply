using System;
using System.Linq;
using System.Text;

namespace Svenkle.TwoPly.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveWhitespace(this string str)
        {
            return new string(str.Where(x => !char.IsWhiteSpace(x)).ToArray());
        }
        
        public static string[] Split(this string str, string separator)
        {
            return str.Split(new[] { separator }, StringSplitOptions.None);
        }

        public static string[] Split(this string str, char separator, int count, StringSplitOptions options)
        {
            return str.Split(new[] { separator }, count, options);
        }

        public static string ToHash(this string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            var shaProvider = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            var hashBytes = shaProvider.ComputeHash(bytes);

            var sb = new StringBuilder();
            foreach (var b in hashBytes)
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }
}
