using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Auth.Packet
{
    public class Hello : Net.Packet.OutPacket
    {

        public Hello()
            : base(Net.PacketCodes.LPASSPORT)
        {
            base.Add(0xDEADBEEFu);
        }

    }
}
