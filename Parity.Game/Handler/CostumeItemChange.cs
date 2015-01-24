using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Handler
{
    [Net.Handler.Requirements()]
    public class CostumeItemChange : Net.Handler.IHandler<Client.Client>
    {

        public override Net.Handler.Result Handle(Client.Client sender, Net.Packet.InPacket packet, bool customHandle = false)
        {
            base.Handle(sender, packet, customHandle);

            bool isEquip;
            Base.Enum.BattleClass battleClass;
            Base.Enum.Item.Character characterType;
            string itemCode;
            if (RequireLength(6) &&
                RequireValue(0, out isEquip) &&
                RequireEnum(1, out battleClass) &&
                RequireEnum(5, out characterType) &&
                RequireValue(4, out itemCode))
            {
                GameDetails.ItemInformation itemInfo = Modules.WarRock.ItemsContainer.GetItemInfo(itemCode);
                DS.Item character = Base.Decision.NotNull(sender.Session.Character.GetItem(itemCode), Modules.Defaults.GetCharacter(battleClass, (byte)characterType));
                if (character != null)
                {
                    if (characterType == itemInfo.Character)
                    {
                        if (!isEquip)
                            sender.Session.Character.Equip(character, battleClass, (byte)itemInfo.Character);
                        else
                            sender.Session.Character.Unequip(character.Code, battleClass, true);
                        sender.Send(Server.PacketFactory.CostumeItemChange(sender, battleClass));
                        return Net.Handler.Result.Success;
                    }
                    else
                        return new Net.Handler.Result("Tried to equip costume '{0}' to {1}, expected {2}".Process(itemCode, characterType, itemInfo.Character));
                }
                else
                    return new Net.Handler.Result("Can't change not owned costume '{0}'".Process(itemCode));
                return Net.Handler.Result.Success;
            }

            return Net.Handler.Result.Default;
        }

    }
}
