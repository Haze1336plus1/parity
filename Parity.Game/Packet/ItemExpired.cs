using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Packet
{
    public class ItemExpired : Net.Packet.OutPacket
    {

        public ItemExpired(DS.Item[] expiredItems, string slotCode, string[] equipment, string inventory)
            : base(Net.PacketCodes.SBI_CHANGE)
        {
            base.Add(1);
            base.Add(slotCode);
            base.Add(equipment[0]);
            base.Add(equipment[1]);
            base.Add(equipment[2]);
            base.Add(equipment[3]);
            base.Add(equipment[4]);
            base.Add(inventory);
            base.Add(expiredItems.Length);
            foreach (DS.Item i in expiredItems)
                base.Add(i.Code);
        }

    }
}
