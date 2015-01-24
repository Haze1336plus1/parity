using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Packet
{
    public class LoginEvent : Net.Packet.OutPacket
    {

        public LoginEvent(string slotcode, string inventory, string costumeInventory)
            : base(Net.PacketCodes.LOGIN_EVENT)
        {
            base.Add(1);
            base.Add(0);
            base.Add(slotcode);
            base.Add(inventory);
            base.Add(costumeInventory);
        }

    }
}
