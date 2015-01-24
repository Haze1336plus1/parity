using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Game.Handler
{
    [Net.Handler.Requirements(true, false)]
    public class ChangeChannel : Net.Handler.IHandler<Client.Client>
    {
        public override Net.Handler.Result Handle(Client.Client sender, Net.Packet.InPacket packet, bool customHandle = false)
        {
            base.Handle(sender, packet, customHandle);
            if (!sender.Session.IsActive)
                return new Net.Handler.Result("Client is not active");
            Base.Enum.Channel channel = default(Base.Enum.Channel);
            if (packet.ParamsCount == 1 && Base.Types.ParseEnum(packet[0], out channel))
            {
                if (sender.GameSession.InRoom)
                    return new Net.Handler.Result("ChangeChannel fail, player in Room");
                sender.GameSession.Channel = channel;
                sender.Send(Server.PacketFactory.ChangeChannel(channel));
                sender.AccountController.SendRoomlist();
                return Net.Handler.Result.Success;
            }
            return Net.Handler.Result.Default;
        }
    }
}
