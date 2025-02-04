using System.Security.Cryptography;
using System.Text;

namespace SecureMessenger.Infrastructure.Services
{
    public class AesEncryptionService
    {
        public string Encrypt(string plainText, string password)
        {
            try
            {
                byte[] keyBytes = GenerateAesKey(password);

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = keyBytes;
                    aesAlg.GenerateIV();

                    using (var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
                    using (var msEncrypt = new MemoryStream())
                    {
                        msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);

                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }

                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Encryption ERROR: {ex.Message}");
                throw;
            }
        }

        public string Decrypt(string cipherText, string password)
        {
            try
            {
                byte[] keyBytes = GenerateAesKey(password);
                byte[] fullCipher = Convert.FromBase64String(cipherText);

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = keyBytes;

                    byte[] iv = new byte[aesAlg.BlockSize / 8];
                    Array.Copy(fullCipher, iv, iv.Length);
                    aesAlg.IV = iv;

                    byte[] cipherBytes = new byte[fullCipher.Length - iv.Length];
                    Array.Copy(fullCipher, iv.Length, cipherBytes, 0, cipherBytes.Length);

                    using (var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                    using (var msDecrypt = new MemoryStream(cipherBytes))
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    using (var srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Decryption ERROR: {ex.Message}");
                return "Decryption failed";
            }
        }

        private static byte[] GenerateAesKey(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
