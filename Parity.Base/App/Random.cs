using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Base.App
{
    public class Random
    {

        public static System.Random R { get; private set; }

        static Random()
        {
            Random.R = new System.Random();
        }

    }
}
