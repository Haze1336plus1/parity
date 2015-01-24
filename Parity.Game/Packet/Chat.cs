using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Packet
{
    public class Chat : Net.Packet.OutPacket
    {

        public Chat(short sourceSession, string sourceName, Base.Enum.ChatChannel chatChannel, short targetSession, string targetName, string message)
			:base (Net.PacketCodes.CHAT)
        {
            base.Add(1); // error code
            base.Add(sourceSession);
            base.Add(sourceName);
            base.Add((int)chatChannel);
            base.Add(targetSession);
            base.Add(targetName);
            base.Add(message);

        }

    }
}
