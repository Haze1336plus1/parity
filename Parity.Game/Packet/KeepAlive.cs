using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Packet
{
    public class KeepAlive : Net.Packet.OutPacket
    {

        public KeepAlive(int ping, int premiumRemain, int sessionTime)
            : base(Net.PacketCodes.KEEPALIVE)
        {
            int pingFrequency = (int)Base.Compile.GameDefaults["Lobby.UpdateFrequency"];
            double pingFrequencySec = pingFrequency / 1000;
            
            base.Add(pingFrequency);
            base.Add(ping);
            base.Add(0); // isEvent ? 0 : 175
            base.Add(-1); // event Duration
            base.Add(0); // event Type
            base.Add("0.0000"); // exp rate
            base.Add("0.0000"); // dinar rate
            base.Add(premiumRemain);
            //base.Add((int)Math.Ceiling(sessionTime * pingFrequencySec));
        }

    }
}
