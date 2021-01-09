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

        private static int FromHex(char value)
        {
            if (value <= '9') return value - '0';
            return value - 'A' + 10;
        }

        public static string Encode(string msg)
        {
            var buffer = new StringBuilder();
            foreach(var ch in msg)
            {
                buffer.Append(ToHex(ch / 16));
                buffer.Append(ToHex(ch % 16));
            }

            return buffer.ToString();
        }

        public static string Decode(string msg)
        {
            var buffer = new StringBuilder();
            for(var i=0; i< msg.Length; i+= 2)
            {
                var value = (char) (FromHex(msg[i]) * 16 + FromHex(msg[i + 1]));
                buffer.Append(value);
            }

            return buffer.ToString();
        }
    }
}
