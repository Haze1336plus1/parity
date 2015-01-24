using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Packet
{
    public class CostumeItemChange : Net.Packet.OutPacket
    {

        public CostumeItemChange(Base.Enum.BattleClass battleClass, string newEquipment)
            : base(Net.PacketCodes.CITEM_CHANGE) 
        {
            base.Add(1);
            base.Add((int)battleClass);
            base.Add(newEquipment);
        }

    }
}
