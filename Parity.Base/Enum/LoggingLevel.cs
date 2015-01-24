using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Base.Enum
{
    public enum LoggingLevel : byte
    {
        Emergency,
        Alert,
        Critical,
        Error,
        Warning,
        Notice,
        Informational,
        Debug
    }
}