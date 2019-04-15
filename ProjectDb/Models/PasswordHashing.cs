using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace ProjectDb.Models
{
    public static class PasswordHashing
    {
        /// <summary>
        /// Extension Method hash password
        /// </summary>
        /// <param name="Password"></param>
        /// <returns></returns>
         public static string HashCode(this string Password)
        {
            
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: Password,
                salt: new byte[128 / 8],
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }

        /// <summary>
        /// Authenticate hased password with non hash
        /// </summary>
        /// <param name="Password"></param>
        /// <param name="dbPassword"></param>
        /// <returns></returns>
        public static bool Authenticate(this string Password, string dbPassword)
        {
            return Password.HashCode() == dbPassword;
        }
    }
}

