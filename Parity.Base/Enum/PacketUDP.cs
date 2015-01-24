using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Base.Enum
{
    public enum PacketUDP : byte
    {

        Unknown,
        Authenticate,
        UpdateIP,
        Tunneling

    }
}
