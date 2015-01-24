using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Auth.Handler
{
    public class Patch : Net.Handler.IHandler<Client.Client>
    {

        public override Net.Handler.Result Handle(Client.Client sender, Net.Packet.InPacket packet, bool customHandle = false)
        {
            base.Handle(sender, packet, customHandle);
            if(base.RequireLength(0))
            {
                sender.Send(new Packet.Patch(QA.Core.Config.Patch));
                return new Net.Handler.Result();
            }
            return Net.Handler.Result.Default;
        }

    }
}
