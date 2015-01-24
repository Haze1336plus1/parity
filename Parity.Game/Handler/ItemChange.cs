using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Handler
{
    [Net.Handler.Requirements()]
    public class ItemChange : Net.Handler.IHandler<Client.Client>
    {

        public override Net.Handler.Result Handle(Client.Client sender, Net.Packet.InPacket packet, bool customHandle = false)
        {
            base.Handle(sender, packet, customHandle);

            Base.Enum.ItemChangeAction itemChangeAction;
            Base.Enum.BattleClass battleClass;
            int itemId;
            string itemCode;
            string itemType;
            byte clientSlot;

            //packet[2] == itemType (commonly I because of Item reference)
            if (packet.ParamsCount == 6 &&
                Base.Types.ParseEnum(packet[0], out itemChangeAction) &&
                Base.Types.ParseEnum(packet[1], out battleClass) &&
                Base.Types.TryParse(packet[2], out itemType) &&
                Base.Types.TryParse(packet[3], out itemId) &&
                Base.Types.TryParse(packet[4], out itemCode) &&
                Base.Types.TryParse(packet[5], out clientSlot))
            {
                byte slot = clientSlot;
                if(itemChangeAction == Base.Enum.ItemChangeAction.Slot6th)
                {
                    if (itemCode.Length == 9 && itemCode[4] == '-')
                        itemCode = itemCode.Substring(5);
                    clientSlot = 8;
                    itemChangeAction = Base.Enum.ItemChangeAction.Equip;
                }
                GameDetails.ItemInformation itemInfo = Modules.WarRock.ItemsContainer.GetItemInfo(itemCode);
                if (itemInfo != null)
                {
                    if (itemInfo.Type == Base.Enum.Item.Type.Weapon ||
                        itemInfo.Type == Base.Enum.Item.Type.Equipment)
                    {
                        if (itemType == "I")
                        {
                            // warrock sends item index, even if another item was dropped in this session. solve this!
                            if (true) // itemId >= 0 && itemId < sender.Session.Inventory.Items.Length)
                            {
                                if (itemChangeAction == Base.Enum.ItemChangeAction.Equip)
                                {
                                    if (Modules.WarRock.BranchContainer.Classes[(int)battleClass].SlotInfo[slot].Allowed.Contains(itemInfo.Weapon))
                                    {
                                        DS.Item item = sender.Session.Inventory.Items[itemId];
                                        if (item.Code == itemCode)
                                        {
                                            if (sender.Session.Inventory.SlotAllowed(slot))
                                            {
                                                sender.Session.Inventory.Equip(item, battleClass, clientSlot);
                                                sender.Send(Server.PacketFactory.ItemChange(sender, battleClass));
                                                return Net.Handler.Result.Success;
                                            }
                                            else
                                                Base.IO.Debug("Slot {0} not allowed".Process(slot));
                                        }
                                        else
                                            Base.IO.Debug("Item reference does not match (got {0}, expected {1})".Process(itemCode, item.Code));
                                    }
                                    else
                                        return new Net.Handler.Result("Branch doesn't accept {0} on slot {1}".Process(itemInfo.Weapon.ToString(), slot));
                                }
                                else if (itemChangeAction == Base.Enum.ItemChangeAction.Unequip)
                                {
                                    DS.Item item = sender.Session.Inventory.GetItem(itemCode);
                                    if (item != null)
                                    {
                                        sender.Session.Inventory.Unequip(item.Code, battleClass, true);
                                        sender.Send(Server.PacketFactory.ItemChange(sender, battleClass));
                                        return Net.Handler.Result.Success;
                                    }
                                    else
                                        return new Net.Handler.Result("Can't unequip not owned item ({0} on class {1})".Process(itemCode, battleClass));
                                }
                                else
                                {
                                    Base.IO.Debug("itemChangeAction {0} not implemented".Process(itemChangeAction.ToString()));
                                }
                            }
                            else
                                Base.IO.Debug("Item reference out of range");
                        }
                        else if (itemType == itemCode[0].ToString())
                        {
                            if (itemChangeAction == Base.Enum.ItemChangeAction.Equip)
                            {
                                if (Modules.WarRock.BranchContainer.Classes[(int)battleClass].SlotInfo[slot].Allowed.Contains(itemInfo.Weapon))
                                {
                                    DS.Item defaultItem = Modules.Defaults.GetEquipment(battleClass, slot);
                                    if (defaultItem != null && defaultItem.Code == itemCode)
                                    {
                                        sender.Session.Inventory.Equip(defaultItem, battleClass, clientSlot);
                                        sender.Send(Server.PacketFactory.ItemChange(sender, battleClass));
                                        return Net.Handler.Result.Success;
                                    }
                                    else
                                        return new Net.Handler.Result("No default item for {0} (slot {1}) (wanted {2})".Process(battleClass.ToString(), slot, itemCode));
                                }
                                else
                                    return new Net.Handler.Result("Branch doesn't accept {0} on slot {1}".Process(itemInfo.Weapon.ToString(), slot));
                            }
                            else if (itemChangeAction == Base.Enum.ItemChangeAction.Unequip)
                            {
                                if (Modules.Defaults.IsDefaultEquipment(battleClass, itemCode))
                                {
                                    sender.Session.Inventory.Unequip(itemCode, battleClass);
                                    sender.Send(Server.PacketFactory.ItemChange(sender, battleClass));
                                    return Net.Handler.Result.Success;
                                }
                                else
                                    return new Net.Handler.Result("Can't unequip {0} as item type {1} (class {2}, slot {3}), item is not default".Process(itemCode, itemType, battleClass.ToString(), slot));
                            }
                            else
                                return new Net.Handler.Result("{0} is an invalid operation on non-inventory item type".Process(itemChangeAction.ToString()));
                        }
                        else
                            return new Net.Handler.Result("Invalid itemType {0} on itemCode {1}".Process(itemType, itemCode));
                    }
                    else
                        return new Net.Handler.Result("ItemCode {0} is of type {1}, expected Weapon or Equipment".Process(itemCode, itemInfo.Type.ToString()));
                }
                else
                    return new Net.Handler.Result("ItemCode {0} related ItemInformation not listed".Process(itemCode));
            }

            return Net.Handler.Result.Default;
        }

    }
}
