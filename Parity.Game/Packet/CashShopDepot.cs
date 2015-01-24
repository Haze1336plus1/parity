using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Packet
{
    public class CashShopDepot : Net.Packet.OutPacket
    {
        public CashShopDepot(Base.Enum.CashShopDepotError error)
            : base((int)Net.PacketCodes.CSHOP_DEPOT)
        {
            base.Add((int)Base.Enum.ItemshopAction.OutboxOpen);
            base.Add((int)error);
        }

        public CashShopDepot(Base.Enum.ItemshopAction action, bool showMessage, int userId, int dinar, int credits, DS.OutboxItem[] items, string inventory, string costumeInventory, string slotCode)
            : base((int)Net.PacketCodes.CSHOP_DEPOT)
        {
            base.Add((int)action);
            base.Add((int)Base.Enum.CashShopDepotError.Success);
            base.Add(dinar);
            base.Add(0);
            base.Add(credits);
            base.Add("LIST");
            base.Add(items.Length);
            foreach (DS.OutboxItem item in items)
            {
                base.Add(item.Id);
                base.Add(userId);
                base.Add(item.Code);
                base.Add(item.Duration);
                base.Add(item.BoughtAt.ToString("yyyy-MM-dd"));
                base.Add(item.From);
                base.Add("You");
                base.Add(0);
            }
            base.Add(showMessage ? 1 : 0); // showMessage?
            base.Add(inventory);
            base.Add(slotCode);
            base.Add(costumeInventory);
            base.Add(0);
            base.Add(1);
        }
    }
}
