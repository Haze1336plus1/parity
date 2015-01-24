using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Net.Server.UDP
{
    public class Constant
    {

        public static long SessionTimeout { get; private set; }

        static Constant()
        {
            Constant.SessionTimeout = 60 * 1000; // 60 seconds
        }

    }
}
