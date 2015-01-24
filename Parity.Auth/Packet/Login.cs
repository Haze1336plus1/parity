using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Auth.Packet
{
    public class Login : Net.Packet.OutPacket
    {

        public Login(Base.Code.Login errorCode)
            : base(Net.PacketCodes.LAUTHENTICATION)
        {
            base.Add((int)errorCode);
        }

    }
}
