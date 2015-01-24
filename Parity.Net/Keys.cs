using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Net
{
    public class Keys
    {

        public static byte LoginKey;
        public static byte GameKey;

        public static byte LoginKeyC;
        public static byte GameKeyC;

        static Keys()
        {
            Keys.LoginKey = 0x96;
            Keys.LoginKeyC = 0xC3;
            Keys.GameKey = 0x45;
            Keys.GameKeyC = 0x6B;

            /*Keys.GameKey = 0x71;
            Keys.GameKeyC = 0x11;*/
        }

        public static ushort Key16(byte k)
        {
            return (ushort)(k | k << 8);
        }
        public static uint Key32(byte k)
        {
            return (uint)(k | k << 8 | k << 16 | k << 24);
        }

    }
}
