using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Packet
{
    public class ItemChange : Net.Packet.OutPacket
    {

        public ItemChange(Base.Enum.BattleClass battleClass, string equipmentString)
            : base(Net.PacketCodes.BITEM_CHANGE)
        {
            base.Add(1);
            base.Add((int)battleClass);
            base.Add(equipmentString);
        }

    }
}
