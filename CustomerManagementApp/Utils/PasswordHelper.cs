using System;
using System.Security.Cryptography;
using System.Text;

namespace CustomerManagementApp.Utils
{
    public static class PasswordHelper
    {
        public static string GenerateSalt()
        {
            // Generate a random salt (e.g., 16 bytes)
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] saltBytes = new byte[16];
                rng.GetBytes(saltBytes);
                return Convert.ToBase64String(saltBytes); // Store the salt in a string format
            }
        }

        public static string GeneratePasswordHash(string salt)
        {
            // Concatenate the password with the salt
            string combined = $"password{salt}";

            // Generate the SHA1 hash
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(combined));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
