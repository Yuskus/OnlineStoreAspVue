using OnlineStore.Server.Extensions.BCL.Exceptions;
using System.Security.Cryptography;
using System.Text;

namespace OnlineStore.Server.Authorization.Utilities
{
    public static class KeyTool
    {
        private const string PUBLIC_KEY = "PUBLIC_KEY";
        private const string PRIVATE_KEY = "PRIVATE_KEY";

        public static RSA GetPublicKey(IConfiguration configuration)
        {
            return GetKey(configuration, PUBLIC_KEY, "Warning! Public key is not found!");
        }

        public static RSA GetPrivateKey(IConfiguration configuration)
        {
            return GetKey(configuration, PRIVATE_KEY, "Warning! Private key is not found!");
        }

        private static RSA GetKey(IConfiguration configuration, string environmentVariable, string warningText)
        {
            string key64 = configuration[environmentVariable]
                ?? throw new EnvironmentVariableNotFoundException(warningText);

            string key = Encoding.UTF8.GetString(Convert.FromBase64String(key64));
            var rsa = RSA.Create();
            rsa.ImportFromPem(key);
            return rsa;
        }
    }
}
