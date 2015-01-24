using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Game.Packet
{
    public class ChangeChannel : Net.Packet.OutPacket
    {

        public ChangeChannel(Base.Enum.Channel newChannel)
            : base(Net.PacketCodes.SET_CHANNEL)
        {
            base.Add(1); // errorCode, are there still any more?!
            base.Add((int)newChannel);
        }

    }
}
