using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Student2020.Utils
{
    public class HexaEncode
    {
        private static char ToHex(int value)
        {
            if (value <= 9) return (char) ('0' + value);
            return (char) ('A' + (value - 10));
        }

        private static byte FromHex(char value)
        {
            if (value <= '9') return (byte) (value - '0');
            return (byte) (value - 'A' + 10);
        }

        public static string Encode(byte[] msg)
        {
            var buffer = new StringBuilder();
            foreach(var ch in msg)
            {
                buffer.Append(ToHex(ch / 16));
                buffer.Append(ToHex(ch % 16));
            }

            return buffer.ToString();
        }

        public static byte[] Decode(string msg)
        {
            var data = new byte[msg.Length / 2];

            for(var i=0; i <msg.Length; i+= 2)
            {
                data[i/2] = (byte)((FromHex(msg[i]) << 4) | FromHex(msg[i + 1]));
            }

            return data;
        }
    }
}
