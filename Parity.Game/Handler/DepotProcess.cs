using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Handler
{
    [Net.Handler.Requirements()]
    public class DepotProcess : Net.Handler.IHandler<Client.Client>
    {

        public override Net.Handler.Result Handle(Client.Client sender, Net.Packet.InPacket packet, bool customHandle = false)
        {
            base.Handle(sender, packet, customHandle);
            if (RequireLength(1) &&
                RequireExact(0, "1117"))
            {
                if (sender.GameSession.InRoom)
                    return new Net.Handler.Result("Can't call DepotProcess from a Room");
                return Net.Handler.Result.Success;
            }
            return Net.Handler.Result.Default;
        }

    }
}
