using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Base
{
    public class Pair<T1, T2>
    {

        public T1 First;
        public T2 Second;

        public Pair(T1 first, T2 second)
        {
            this.First = first;
            this.Second = second;
        }

    }
}
