using System.Security.Cryptography;
using System.Text;

namespace OnlineStore.Server.Authorization.Utilities
{
    public class KeyTool
    {
        private const string PUBLIC_KEY = "PUBLIC_KEY";
        private const string PRIVATE_KEY = "PRIVATE_KEY";

        public static RSA GetPublicKey()
        {
            return GetKey(PUBLIC_KEY, "Warning! Public key is not found!");
        }

        public static RSA GetPrivateKey()
        {
            return GetKey(PRIVATE_KEY, "Warning! Private key is not found!");
        }

        private static RSA GetKey(string environmentVariable, string warningText)
        {
            var rsa = RSA.Create();
            string key64 = Environment.GetEnvironmentVariable(environmentVariable) ?? throw new Exception(warningText);
            string key = Encoding.UTF8.GetString(Convert.FromBase64String(key64));
            rsa.ImportFromPem(key);
            return rsa;
        }
    }
}
