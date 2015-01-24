using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Handler
{
    [Net.Handler.Requirements()]
    public class CashShopDepot : Net.Handler.IHandler<Client.Client>
    {
        public override Net.Handler.Result Handle(Client.Client sender, Net.Packet.InPacket packet, bool customHandle = false)
        {
            base.Handle(sender, packet, customHandle);
            if (sender.GameSession.InRoom)
                return new Net.Handler.Result("Can't access CashShopDepot from a room");

            Base.Enum.ItemshopAction action;
            int outboxId;
            int outboxIndex;
            string itemCode;

            if (packet.ParamsCount == 5 &&
                Base.Types.ParseEnum(packet[0], out action) &&
                Base.Types.TryParse(packet[1], out outboxId) &&
                packet[2] == "1" &&
                Base.Types.TryParse(packet[3], out outboxIndex) &&
                Base.Types.TryParse(packet[4], out itemCode))
            {
                if (outboxIndex >= 0 && outboxIndex < sender.Session.Inventory.Outbox.Length)
                {
                    DS.OutboxItem obItem = sender.Session.Inventory.Outbox[outboxIndex];
                    if (obItem.Code == itemCode)
                    {
                        if (obItem.Id == outboxId)
                        {
                            if (action == Base.Enum.ItemshopAction.OutboxUse)
                            {
                                string[] addItems = null;
                                
                                // Activate item if necessary
                                Script.IOutboxActivation activation = Modules.OutboxActivation.GetActivation(obItem.Code);
                                if (activation != null)
                                    addItems = activation.Items;
                                else
                                    addItems = new string[] { obItem.Code };

                                // Create item(s)
                                if ((sender.Session.Inventory.Items.Length + addItems.Length) <= sender.Session.Inventory.Limit)
                                {
                                    foreach (string item in addItems)
                                    {
                                        if (item[0] == 'B')
                                            sender.Session.Character.Create(item, obItem.Duration);
                                        else
                                            sender.Session.Inventory.Create(item, obItem.Duration);
                                    }
                                    if(activation != null)
                                        activation.Activate(sender, obItem.Duration); // activate ONLY if you have enough items

                                    QA.GetLog()["shop"].Write("Activating OutboxItem {0} ({1} days)".Process(obItem.Code, obItem.Duration));
                                    sender.Session.Inventory.OutboxDelete(obItem);
                                    sender.Send(Server.PacketFactory.Shop.Outbox(Base.Enum.ItemshopAction.OutboxUse, sender, true));
                                }
                                else
                                    sender.Send(Server.PacketFactory.Shop.OutboxError(Base.Enum.CashShopDepotError.InventoryFull));
                            }
                            else if (action == Base.Enum.ItemshopAction.OutboxDelete)
                            {
                                sender.Session.Inventory.OutboxDelete(obItem);
                                sender.Send(Server.PacketFactory.Shop.Outbox(Base.Enum.ItemshopAction.OutboxDelete, sender, true));
                            }
                            else
                                return new Net.Handler.Result("Invalid CashShopDepot action: " + action.ToString());
                            return Net.Handler.Result.Success;
                        }
                        else
                            return new Net.Handler.Result("Item id does not match (got {0}, expected {1})".Process(outboxId, obItem.Id));
                    }
                    else
                        return new Net.Handler.Result("Item code does not match (got {0}, expected {1})".Process(itemCode, obItem.Code));
                }
                else
                    return new Net.Handler.Result("Outbox reference out of range");
            }
            else if (packet.ParamsCount == 4)
                return Net.Handler.Result.Success; // ignore that. client bug
            return Net.Handler.Result.Default;
        }
    }
}
