using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Packet
{
    public class RoomList : Net.Packet.OutPacket
    {

        public RoomList(Game.Room[] rooms, int rindex)
             : base(Net.PacketCodes.ROOM_LIST)
        {
            Game.Room[] rlist = (from Game.Room r in rooms where r.Id >= rindex && r.Id < (rindex + 15) select r).ToArray(); //where r != null &&

            /*
            base.Add(-1);
            base.Add(rindex / 13); // current page
            base.Add(10); // number of selectable pages
            base.Add(rlist.Length);
            foreach (Game.Room r in rlist)
                r.Append(this);
            */
            base.Add(rlist.Length);
            base.Add(rindex / 15);
            base.Add(0);
            foreach (Game.Room r in rlist)
                r.Append(this);
            base.Add(0);
        }

    }
}
