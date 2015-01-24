using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Base.Exception
{
    public enum CustomFlags : byte
    {
        None = byte.MinValue,

        Critical = (1 << 0),
        NetHandleBreak = (1 << 1),

        ALL = Byte.MaxValue
    }
}
