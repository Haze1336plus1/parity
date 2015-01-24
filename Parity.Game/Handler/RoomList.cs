using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Handler
{
    [Net.Handler.Requirements(true, false)]
    public class RoomList : Net.Handler.IHandler<Client.Client>
    {
        public override Net.Handler.Result Handle(Client.Client sender, Net.Packet.InPacket packet, bool customHandle = false)
        {
            base.Handle(sender, packet, customHandle);
            // SHOW_ALL, INDEX 0: 0 1 0
            // SHOW_WAITING, INDEX 0: 0 0 0
            // intRoomPage boolShowAll boolGoBack

            sbyte roomPage;
            if (RequireLength(3) &&
                RequireValue(0, out roomPage))
            {
                if (sender.GameSession.InRoom)
                    return new Net.Handler.Result("Player is in a Room, can't request RoomList!");
                if(roomPage >= 0)
                    sender.GameSession.RoomStartingIndex = (short)(roomPage * 15);
                
                sender.AccountController.SendRoomlist();
                return Net.Handler.Result.Success;
            }

            /*
            short roomStartingIndex;
            if (packet.ParamsCount == 2 && Base.Types.TryParse(packet[0], out roomStartingIndex))
            {
                if (packet[1] == "3")
                    roomStartingIndex -= 12;
                if (sender.GameSession.InRoom)
                    return new Net.Handler.Result("Player is in a Room, can't request RoomList!");
                if (roomStartingIndex < 0) roomStartingIndex = 0;
                sender.GameSession.RoomStartingIndex = roomStartingIndex;
                sender.AccountController.SendRoomlist();

                return Net.Handler.Result.Success;
            }
            */
            return Net.Handler.Result.Default;
        }
    }
}
