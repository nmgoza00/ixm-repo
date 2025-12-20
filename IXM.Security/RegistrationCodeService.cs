using IXM.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace IXM.Security
{
    public class RegistrationCodeService
    {
        private readonly byte[] _encryptionKey; // from configuration

        public RegistrationCodeService(string key)
        {
            // Ensure 32 bytes for AES-256
            _encryptionKey = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
        }

        public string GenerateCode(RegistrationPayload payload)
        {
            var json = JsonSerializer.Serialize(payload);
            using var aes = Aes.Create();
            aes.Key = _encryptionKey;
            aes.GenerateIV();

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            var plainBytes = Encoding.UTF8.GetBytes(json);
            var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            // Combine IV + cipher
            var combined = aes.IV.Concat(cipherBytes).ToArray();
            return Convert.ToBase64String(combined).Replace("+", "-")
            .Replace("/", "_")
            .Replace("=", "");
        }

        public RegistrationPayload DecryptCode(string code)
        {

            string padded = code
                        .Replace("-", "+")
                        .Replace("_", "/");

            switch (padded.Length % 4)
            {
                case 2: padded += "=="; break;
                case 3: padded += "="; break;
            }


            var fullCipher = Convert.FromBase64String(code);

            using var aes = Aes.Create();
            aes.Key = _encryptionKey;

            var iv = fullCipher.Take(16).ToArray();
            var cipher = fullCipher.Skip(16).ToArray();
            aes.IV = iv;

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            var plainBytes = decryptor.TransformFinalBlock(cipher, 0, cipher.Length);
            var json = Encoding.UTF8.GetString(plainBytes);

            return JsonSerializer.Deserialize<RegistrationPayload>(json);
        }
    }
}
