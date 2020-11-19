using System;
using System.Security.Cryptography;
using System.Text;

namespace Encryption
{
    public class SaltedHash
    {
        public string Hash { get; private set; }
        private string Salt { get; set; }
        public string SecurityStamp { get; private set; }

        public SaltedHash(string password)
        {
            Guid rg = Guid.NewGuid();
            var saltBytes = new byte[32];
            SecurityStamp = rg.ToString().Replace("-", "");
            saltBytes = Encoding.ASCII.GetBytes(SecurityStamp);
            Salt = Convert.ToBase64String(saltBytes);
            Hash = ComputeHash(Salt, password);
        }

        private static string ComputeHash(string salt, string password)
        {
            var saltBytes = Convert.FromBase64String(salt);
            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 1000))
                return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));
        }

        public static bool Verify(string securityStamp, string passwordHash, string password)
        {
            var saltBytes = new byte[32];
            saltBytes = Encoding.ASCII.GetBytes(securityStamp);

            string Salt = Convert.ToBase64String(saltBytes);
            return passwordHash == ComputeHash(Salt, password);
        }
    }
}