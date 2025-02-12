using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace InostyProject.Data
{
    public class PasswordHandler
    {
        public bool checkHashedPassword(string password, string salt, string input)
        {
            Console.WriteLine("StoredPassword: " + password);
            Console.WriteLine("Salt: " + salt);
            Console.WriteLine("Input: " + input);
            byte[] saltBytes = Convert.FromBase64String(salt);
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: input,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 32)
                );

            Console.WriteLine("PasswordToCheck: " + hashed);

            return hashed == password;
        }

        public string hashPassword(string password, out byte[] salt)
        {
            Console.WriteLine(password);
            salt = RandomNumberGenerator.GetBytes(16);
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 32)
                );

            Console.WriteLine($"Password: {hashed}");
            Console.WriteLine($"Salt: {salt}");

            return hashed;
        }
    }
}
