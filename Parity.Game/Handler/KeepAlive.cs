using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Handler
{
    [Net.Handler.Requirements()]
    public class KeepAlive : Net.Handler.IHandler<Client.Client>
    {

        public override Net.Handler.Result Handle(Client.Client sender, Net.Packet.InPacket packet, bool customHandle = false)
        {
            base.Handle(sender, packet, customHandle);

            short sessionId;
            long timeStamp;
            if (Base.Types.TryParse(packet[0], out sessionId) &&
                Base.Types.TryParse(packet[1], out timeStamp) &&
                Base.Types.IsValid<ushort>(packet[2]))
            {
                if (sender.Session.SessionID == sessionId)
                {
                    long pingTime = (Base.App.Time.Get() - sender.Session.PingTime);
                    if (pingTime < ushort.MaxValue)
                    {
                        sender.GameSession.Ping = (ushort)pingTime;
                        sender.Session.SessionTime++;
                        return Net.Handler.Result.Success;
                    }
                    else
                        return new Net.Handler.Result("Time difference too big ({0})".Process(pingTime));
                }
                else
                    return new Net.Handler.Result("Tried to give an invalid sessionId (got {0}, expected {1})".Process(sessionId, sender.Session.SessionID));
            }
            return Net.Handler.Result.Default;
        }

    }
}
