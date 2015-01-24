using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Packet
{
    public class LevelUp : Net.Packet.OutPacket
    {

        public LevelUp(byte newLevel, int experience, int addDinar, string slotCode, string inventory, string characters)
            : base(Net.PacketCodes.PROMOTION)
        {
            base.Add(newLevel);
            base.Add(experience);
            base.Add(0);
            base.Add(addDinar);
            base.Add(slotCode);
            base.Add(inventory);
            base.Add(characters);
        }

    }
}
