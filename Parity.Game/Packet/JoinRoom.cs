using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Packet
{
    public class JoinRoom : Net.Packet.OutPacket
    {

        public JoinRoom(Game.Room room, int index)
            : base(Net.PacketCodes.JOIN_ROOM)
        {
            base.Add((int)Base.Code.RoomJoin.Success);
            base.Add(index);
            room.Append(this);
        }

    }
}
