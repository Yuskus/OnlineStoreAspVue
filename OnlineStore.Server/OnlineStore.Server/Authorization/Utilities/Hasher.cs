using System.Security.Cryptography;
using System.Text;

namespace OnlineStore.Server.Authorization.Utilities
{
    public class Hasher
    {
        public static (byte[], byte[]) CreatePasswordHash(string password)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] salt = GenSalt();
            byte[] combined = FillingArray(passwordBytes, salt);
            byte[] hash = GenHash(combined);
            return (hash, salt);
        }

        public static bool IsPasswordValid(string password, byte[] existingPasswordHash, byte[] salt)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] combined = FillingArray(passwordBytes, salt);
            byte[] supposedHash = GenHash(combined);
            return supposedHash.SequenceEqual(existingPasswordHash);
        }

        private static byte[] GenHash(byte[] data)
        {
            return SHA512.HashData(data);
        }

        private static byte[] GenSalt()
        {
            using var a = RandomNumberGenerator.Create();
            byte[] salt = new byte[16];
            a.GetNonZeroBytes(salt);
            return salt;
        }

        private static byte[] FillingArray(byte[] passwordBytes, byte[] salt)
        {
            byte[] combined = new byte[salt.Length + passwordBytes.Length];
            Buffer.BlockCopy(salt, 0, combined, 0, salt.Length);
            Buffer.BlockCopy(passwordBytes, 0, combined, salt.Length, passwordBytes.Length);
            return combined;
        }
    }
}
