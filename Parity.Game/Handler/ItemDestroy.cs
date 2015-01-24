using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Handler
{
    [Net.Handler.Requirements()]
    public class ItemDestroy : Net.Handler.IHandler<Client.Client>
    {
        public override Net.Handler.Result Handle(Client.Client sender, Net.Packet.InPacket packet, bool customHandle = false)
        {
            base.Handle(sender, packet, customHandle);

            if (sender.GameSession.InRoom)
                return new Net.Handler.Result("Can't access ItemDestroy from a room");

            //<itemcode> <itemindex>
            string itemCode;
            byte itemIndex;
            if (packet.ParamsCount == 2 &&
                Base.Types.TryParse(packet[0], out itemCode) &&
                Base.Types.TryParse(packet[1], out itemIndex))
            {
                if (itemIndex >= 0 && itemIndex < sender.Session.Inventory.Items.Length)
                {
                    DS.Item item = sender.Session.Inventory.Items[itemIndex];
                    if (item.Code == itemCode)
                    {
                        sender.Session.Inventory.Delete(item); // also unequipps it
                        sender.Send(Server.PacketFactory.ItemDestroy(sender, itemCode));
                        return Net.Handler.Result.Success;
                    }
                    else
                        return new Net.Handler.Result("Item reference does not match given code (got {0}, expected {1})".Process(itemCode, item.Code));
                }
                else
                    return new Net.Handler.Result("Item reference out of range");
            }

            return Net.Handler.Result.Default;
        }
    }
}
