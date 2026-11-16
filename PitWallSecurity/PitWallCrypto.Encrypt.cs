using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PitWallSecurity
{
    //This section is Ayomide Oshilaja
    public static partial class PitWallCrypto
    {
        /// Encrypts text with AES-256.
        /// Input: plain text string. Output: Base64 ciphertext string.
        
        //==================== Section: Ayomide oshilaja ================================
        public static string Encrypt(string plainText)
        {
            if (plainText == null) throw new ArgumentNullException(nameof(plainText));

            using (Aes aes = Aes.Create())
            {
                aes.Key = CryptoKeys.Key;
                aes.IV = CryptoKeys.IV;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                {

                    byte[] input = Encoding.UTF8.GetBytes(plainText);
                    byte[] output = encryptor.TransformFinalBlock(input, 0, input.Length);
                    return Convert.ToBase64String(output);

                }
            }
        }

        //==================== Section:Decrypt by Gabriela Martinez ================================
        /// Input: Base64 ciphertext string
        /// Output: original plain text string.
        public static string Decrypt(string cipherText)
        {
            if (cipherText == null) throw new ArgumentNullException(nameof(cipherText));

            byte[] input = Convert.FromBase64String(cipherText.Trim());

            using (Aes aes = Aes.Create())
            {
           
                aes.Key = CryptoKeys.Key;
                aes.IV = CryptoKeys.IV;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                { 
                    byte[] output = decryptor.TransformFinalBlock(input, 0, input.Length);
                    return Encoding.UTF8.GetString(output);
                }
            }
        }

    }
}
