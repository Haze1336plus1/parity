using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.GameDetails.Map.Object
{
    public abstract class Base
    {

        public int X { get; private set; }
        public int Y { get; private set; }
        public int Z { get; private set; }

        public Base(int X, int Y, int Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

    }
}
