using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Packet
{
    public class Test : Net.Packet.OutPacket
    {

        public Test(int opc, params string[] args)
            : base(opc, args)
        {
        }

    }
}
