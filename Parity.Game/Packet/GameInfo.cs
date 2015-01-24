using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Game.Packet
{
    public class GameInfo : Net.Packet.OutPacket
    {

        public GameInfo(Base.Code.GameInfo errorCode)
            : base(Net.PacketCodes.JOIN_SERV)
        {
            base.Add((int)errorCode);
        }

        public GameInfo(short sessionId, int userId, string nickname, int experience, int kills, int deaths, int dinar, string slotCode, string[] equipment, string inventory, int inventoryLimit, string[] costumes, string inventoryCostumes, Base.Enum.Premium premium)
            : base(Net.PacketCodes.JOIN_SERV)
        {
            base.Add((int)Base.Code.GameInfo.Success);
            base.Add(QA.Core.Config.GameConfig.Name);
            base.Add(sessionId);
            base.Add(userId);
            base.Add(sessionId);
            base.Add(nickname);
            base.Add(-1); // clan ID
            base.Add(-1); // clan Picture
            base.Add("NULL"); // clan Name
            base.Add(-1); // have Clan? -1 : 1
            base.Add(-1); // clan Rank
            base.Add(0); // pc bang?
            base.Add(0); // level, senseless
            base.Add(experience);
            base.Add(920712); // korean birth date? - 12.07.1992 ?
            base.Add(0); // whats thawt?!
            base.Add(dinar);
            base.Add(kills);
            base.Add(deaths);
            base.Add(0); // has reward today? 0 : 1
            base.Add(0); // reward counter
            base.Add(0);
            base.Add(0);
            base.Add(0);
            base.Add(0);
            base.Add(0);
            base.Add(slotCode);
            base.Add(equipment[0]);
            base.Add(equipment[1]);
            base.Add(equipment[2]);
            base.Add(equipment[3]);
            base.Add(equipment[4]);
            base.Add(inventory);
            base.Add(inventoryLimit);
            base.Add(costumes[0]);
            base.Add(costumes[1]);
            base.Add(costumes[2]);
            base.Add(costumes[3]);
            base.Add(costumes[4]);
            base.Add(inventoryCostumes);
            base.Add((int)premium);
            // letter event stuff?
            base.Add(0);
            base.Add(-1);
            base.Add(0);
            base.Add(0);
            base.Add(0);
            base.Add(1); // has AI? 0 : 1
            base.Add(0); // might be something magic!

            return; // NEW ONE

            /*
            base.Add((int)Base.Code.GameInfo.Success);
            base.Add(QA.Core.Config.GameConfig.Name);
            base.Add(sessionId);
            base.Add(userId);
            base.Add(sessionId);
            base.Add(nickname);
            // clan stuff
            base.Add(-1);
            base.Add(-1);
            base.Add("NULL");
            base.Add(-1);
            base.Add(123);
            // clan stuff end
            base.Add((int)Base.Enum.Premium.None);
            base.Add(0); // level
            base.Add(experience); //experience);
            base.Add((int)Base.Enum.Premium.None);
            base.Add((int)Base.Enum.Premium.None);
            base.Add((int)Base.Enum.Premium.None);
            base.Add(kills);
            base.Add(deaths);
            base.Add(0); // CLAN WIN
            base.Add(0); // CLAN LOSE
            base.Add(slotCode);
            base.Add(equipment[0]);
            base.Add(equipment[1]);
            base.Add(equipment[2]);
            base.Add(equipment[3]);
            base.Add(equipment[4]);
            base.Add(inventory);
            base.Add(inventoryLimit);
            base.Add(costumes[0]);
            base.Add(costumes[1]);
            base.Add(costumes[2]);
            base.Add(costumes[3]);
            base.Add(costumes[4]);
            base.Add(inventoryCostumes);
            // server related activations (events, flags)
            base.Add(0);
            base.Add(0);
            base.Add(-1); // -1 = smile badge
            base.Add(0);
            base.Add(0);
            base.Add(0);
            base.Add(0);
            base.Add(0);
            */
        }

    }
}
