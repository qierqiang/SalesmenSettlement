using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SalesmenSettlement
{
    public static class CodeHelper
    {
        public static bool IsNullOrEmpty(this string source) => string.IsNullOrEmpty(source);

        public static bool IsNullOrBlank(this string source) => source.IsNullOrWhiteSpace();

        public static bool IsNullOrWhiteSpace(this string source) => string.IsNullOrEmpty(source) || source.Trim().Length == 0;

        public static string GetMD5(this string source)
        {
            source = source == null ? string.Empty : source;
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(source));
            return Encoding.Default.GetString(result);
        }
    }
}
