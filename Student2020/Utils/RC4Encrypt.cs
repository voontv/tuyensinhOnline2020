using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student2020.Utils
{
    public static class RC4Encrypt
    {
        private static readonly Encoding unicode = Encoding.Unicode;
        private static readonly string privateKey = "GPD8uUvWvSJ8tRXqEYSs";

        public static string Encrypt(string key, string data)
        {
            key = key.Trim().ToLower();
            data = data.Trim().ToLower();
            return Convert.ToBase64String(Encrypt(unicode.GetBytes(key + privateKey), unicode.GetBytes(data)));
        }

        public static string Decrypt(string key, string data)
        {
            key = key.Trim().ToLower();
            data = data.Trim().ToLower();
            return unicode.GetString(Encrypt(unicode.GetBytes(key + privateKey), Convert.FromBase64String(data)));
        }

        private static byte[] Encrypt(byte[] key, byte[] data)
        {
            return EncryptOutput(key, data).ToArray();
        }

        private static byte[] Decrypt(byte[] key, byte[] data)
        {
            return EncryptOutput(key, data).ToArray();
        }

        private static byte[] EncryptInitalize(byte[] key)
        {
            byte[] s = Enumerable.Range(0, 256)
              .Select(i => (byte)i)
              .ToArray();

            for (int i = 0, j = 0; i < 256; i++)
            {
                j = (j + key[i % key.Length] + s[i]) & 255;

                Swap(s, i, j);
            }

            return s;
        }

        private static IEnumerable<byte> EncryptOutput(byte[] key, IEnumerable<byte> data)
        {
            byte[] s = EncryptInitalize(key);

            int i = 0;
            int j = 0;

            return data.Select((b) =>
            {
                i = (i + 1) & 255;
                j = (j + s[i]) & 255;

                Swap(s, i, j);

                return (byte)(b ^ s[(s[i] + s[j]) & 255]);
            });
        }

        private static void Swap(byte[] s, int i, int j)
        {
            byte c = s[i];

            s[i] = s[j];
            s[j] = c;
        }
    }
}
