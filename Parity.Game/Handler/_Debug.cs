using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Handler
{
    [Net.Handler.Requirements(false)]
    public class _Debug : Net.Handler.IHandler<Client.Client>
    {
        public override Net.Handler.Result Handle(Client.Client sender, Net.Packet.InPacket packet, bool customHandle = false)
        {
            base.Handle(sender, packet, customHandle);
            Base.IO.Debug(packet.Print());
            return Net.Handler.Result.Success;
        }
    }
}
