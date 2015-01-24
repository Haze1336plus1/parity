using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Packet
{
    public class RoomInfoChange : Net.Packet.OutPacket
    {

        public RoomInfoChange(int roomId, Base.Enum.RoomInfoChangeAction action, Game.Room room = null)
            : base(Net.PacketCodes.ROOM_INFO_CHANGE)
        {
            base.Add(roomId);
            base.Add((byte)action);
            if (action != Base.Enum.RoomInfoChangeAction.Deleted)
            {
                //base.Add(9); // magic number
                room.Append(this);
            }
        }

    }
}
