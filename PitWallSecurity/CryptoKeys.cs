using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PitWallSecurity
{

    internal static class CryptoKeys
    {
        private const string Passphrase = "PitWall-CSE445-Group63";
        private static readonly byte[] Salt = Encoding.UTF8.GetBytes("PitWallSalt2026");

        internal static readonly byte[] Key; // 32 bytes = AES-256
        internal static readonly byte[] IV;  // 16 bytes = one AES block

        static CryptoKeys()
        {
            // PBKDF2 turns the passphrase into strong key bytes
            using (var kdf = new Rfc2898DeriveBytes(Passphrase, Salt, 10000))
            {
                Key = kdf.GetBytes(32);
                IV = kdf.GetBytes(16);
            }
        }
    }

}
