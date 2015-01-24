using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Packet
{
    public class ItemShop : Net.Packet.OutPacket
    {

        public ItemShop()
            : base(Net.PacketCodes.NCASH_PROCESS)
        {
        }

        public ItemShop Open(int credits)
        {
            base.Add((ushort)Base.Enum.ItemshopAction.ShopOpen);
            base.Add(1);
            base.Add(credits);
            base.Add(null);
            base.Add(0);
            base.Add(null);
            return this;
        }

        public ItemShop CashPurchase(Base.Enum.PurchaseError error)
        {
            base.Add((ushort)Base.Enum.ItemshopAction.Purchase);
            base.Add((int)error);
            return this;
        }

    }
}
