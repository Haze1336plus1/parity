using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Packet
{
    public class ItemProcess : Net.Packet.OutPacket
    {

        public ItemProcess(Base.Enum.PurchaseError error)
            : base(Net.PacketCodes.ITEM_PROCESS)
        {
            base.Add((int)error);
        }

        public ItemProcess(string inventory, int dinar, string slotCode)
            : base(Net.PacketCodes.ITEM_PROCESS)
        {
            base.Add((int)Base.Enum.PurchaseError.Success);
            base.Add((int)Base.Enum.ItemshopAction.Purchase);
            base.Add(-1); // ?
            base.Add(3); // ?
            base.Add(4); // ?
            base.Add(inventory);
            base.Add(dinar);
            base.Add(slotCode);
        }

    }
}
