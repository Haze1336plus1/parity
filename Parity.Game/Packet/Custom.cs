using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Packet
{
    public class Custom : Net.Packet.OutPacket
    {

        public Custom()
            : base(Net.PacketCodes.CustomPacket)
        {
        }

        public Custom DefaultEQ(string fullPack)
        {
            base.Add(0); // defaultEQ clientAction type
            base.Add(fullPack);
            return this;
        }

        public Custom MessageBox(string message)
        {
            base.Add(1); // messagebox clientAction type
            base.Add(message);
            return this;
        }
        public Custom Chat(string message, Color color)
        {
            base.Add(2); // chat clientAction type
            base.Add(message);
            base.Add(color.Code);
            return this;
        }

    }
}
