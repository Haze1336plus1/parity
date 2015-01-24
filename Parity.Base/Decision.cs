using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Base
{
    public class Decision
    {

        public static T NotNull<T>(T a, T b)
        {
            return (a != null ? a : (b != null ? b : default(T)));
        }

    }
}
