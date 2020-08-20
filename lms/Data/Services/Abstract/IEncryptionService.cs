using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lms.Data.Services
{
    public interface IEncryptionService
    {
        /// <summary>
        /// Create random salt
        /// </summary>
        /// <returns></returns>
        string CreateSalt();

        /// <summary>
        /// Generates hashed password
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        string EncryptPassword(string password, string salt);
        string GenerateRefCode();
        string Encrypt(string input);
        string Decrypt(string cipherText);

    }
}
