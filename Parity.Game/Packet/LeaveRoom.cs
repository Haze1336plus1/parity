using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Packet
{
    public class LeaveRoom : Net.Packet.OutPacket
    {

        public LeaveRoom(short sessionId, int oldSlot, int masterIndex, int playerExperience, int playerDinar)
            : base(Net.PacketCodes.EXIT_ROOM)
        {
            base.Add(1); // success
            base.Add(sessionId);
            base.Add(oldSlot);
            base.Add(0);
            base.Add(masterIndex);
            base.Add(playerExperience);
            base.Add(playerDinar);
        }

    }
}
