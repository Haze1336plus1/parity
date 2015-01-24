using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Packet
{
    public class ItemDestroy : Net.Packet.OutPacket
    {

        public ItemDestroy(string itemCode, string inventoryString, string slotCode, string[] equipmentString)
            : base(Net.PacketCodes.ITEM_DESTROY)
        {
            base.Add(1);
            base.Add(itemCode);
            base.Add(inventoryString);
            base.Add(slotCode);
            base.Add(equipmentString[0]);
            base.Add(equipmentString[1]);
            base.Add(equipmentString[2]);
            base.Add(equipmentString[3]);
            base.Add(equipmentString[4]);
        }

    }
}
