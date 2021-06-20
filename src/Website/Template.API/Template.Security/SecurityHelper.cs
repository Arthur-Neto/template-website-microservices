using System;
using System.Security.Cryptography;
using System.Text;

namespace Template.Security
{
    public static class SecurityHelper
    {
        public static bool IsValidHash(string hash, string salt, string plainValue)
        {
            var bytesPlainValue = Encoding.UTF8.GetBytes(plainValue);
            var bytesSalt = Encoding.UTF8.GetBytes(salt);
            var byteResult = new Rfc2898DeriveBytes(bytesPlainValue, bytesSalt, 10000);

            return Convert.ToBase64String(byteResult.GetBytes(24)).Equals(hash);
        }

        public static string GenerateHash(string plainValue, string salt)
        {
            var bytesPlainValue = Encoding.UTF8.GetBytes(plainValue);
            var bytesSalt = Encoding.UTF8.GetBytes(salt);
            var byteResult = new Rfc2898DeriveBytes(bytesPlainValue, bytesSalt, 10000);

            return Convert.ToBase64String(byteResult.GetBytes(24));
        }

        public static string GenerateSalt()
        {
            var bytes = new byte[128 / 8];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(bytes);

            return Convert.ToBase64String(bytes);
        }
    }
}
