using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace lms.Data.Services
{
    public class EncryptionService : IEncryptionService
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;

        public EncryptionService(IDataProtectionProvider dataProtectionProvider)
        {
            _dataProtectionProvider = dataProtectionProvider;
        }

        public string CreateSalt()
        {
            var data = new byte[0x10];

            var crytoServiceProvider = System.Security.Cryptography.RandomNumberGenerator.Create();
            crytoServiceProvider.GetBytes(data);
            return Convert.ToBase64String(data);
        }

        public string EncryptPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = string.Format("{0}{1}", salt, password);
                byte[] saltedPasswordAsBytes = Encoding.UTF8.GetBytes(saltedPassword);
                return Convert.ToBase64String(sha256.ComputeHash(saltedPasswordAsBytes));
            }
        }

        public string GenerateRefCode()
        {
            int min = 100000;
            int max = 999999;
            var random = RandomNumberGenerator.Create();
            var bytes = new byte[sizeof(int)]; // 4 bytes
            random.GetNonZeroBytes(bytes);
            var val = BitConverter.ToInt32(bytes);
            var result = "LMS" + Convert.ToString(((val - min) % (max - min + 1) + (max - min + 1)) % (max - min + 1) + max);
            return result;
        }

        public string Encrypt(string input)
        {
            return input;
            //IDataProtector protector = _dataProtectionProvider.CreateProtector(Key);
            //return protector.Protect(input);
        }

        public string Decrypt(string cipherText)
        {
            return cipherText;
            //var protector = _dataProtectionProvider.CreateProtector(Key);
            //return protector.Unprotect(cipherText);
        }
    }
}
