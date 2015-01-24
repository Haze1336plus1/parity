using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Packet
{
    public class UserList : Net.Packet.OutPacket
    {

        public UserList(Client.Client[] playerList, Base.Enum.UserListFilter filter)
            : base(Net.PacketCodes.USER_LIST)
        {

            base.Add(1); // success
            base.Add((byte)filter);
            base.Add(playerList.Length);
            base.Add(null);
            foreach (Client.Client player in playerList)
            {
                base.Add(player.Session.Account.Id);
                base.Add(player.Session.SessionID);
                base.Add(-1);
                base.Add(-1);
                base.Add("NULL");
                base.Add(0); // hmm.. maybe 1 or 2, not sure
                base.Add(player.Session.Account.Nickname);
                base.Add(48); // what?!
                base.Add(-1);
                base.Add(3); // commonly 0
                base.Add(1);
                base.Add(1);
                base.Add(-1);
            }

        }

    }
}
