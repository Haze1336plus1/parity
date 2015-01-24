using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Game.Handler
{
    [Net.Handler.Requirements(false, false)]
    public class GameInfo : Net.Handler.IHandler<Client.Client>
    {

        protected System.Text.RegularExpressions.Regex rSessionKey;

        public GameInfo()
        {
            this.rSessionKey = new System.Text.RegularExpressions.Regex("^[1-9][0-9]{8}$"); // 10000000-999999999
        }

        protected bool ProcessAuthentication(Client.Client player, int userId, string username, string nickname, uint sessionKey)
        {
            if (sessionKey > 0 &&
                player.AccountController.LoginToServer(userId, sessionKey) &&
                player.Session.Account.Username == username &&
                player.Session.Account.Nickname == nickname)
                return true;
            return false;
        }

        public override Net.Handler.Result Handle(Client.Client sender, Net.Packet.InPacket packet, bool customHandle = false)
        {
            base.Handle(sender, packet, customHandle);

            if (sender.Session.IsActive)
                return Net.Handler.Result.Default;
            //1 1 -1 blackhat TROLOLOL 21 0 1 -1 -1 -1 INVALID -1 -1 0 848087120 dnjfhr^
            if (packet.ParamsCount == 17 &&
                packet[0] == "1" &&
                packet[8] == "-1" &&
                packet[9] == "-1" &&
                packet[10] == "-1" &&
                packet[11] == "INVALID" &&
                packet[12] == "-1" &&
                packet[13] == "-1" &&
                packet[14] == "0" &&
                this.rSessionKey.IsMatch(packet[15]) && 
                packet[16] == "dnjfhr^")
            {
                int userId;
                string username;
                string nickname;
                uint sessionKey;
                if (Base.Types.TryParse(packet[1], out userId) &&
                    Base.Types.TryParse(packet[3], out username) &&
                    Base.Types.TryParse(packet[4], out nickname) &&
                    Base.Types.TryParse(packet[15], out sessionKey) &&
                    this.ProcessAuthentication(sender, userId, username, nickname, sessionKey)) // magic part
                {
                    sender.Send(Server.PacketFactory.GameInfo(sender));
                    sender.Send(Server.PacketFactory.Custom.DefaultEQ());

                    // Expired items, kinda hardcoded i guess
                    {
                        var expiredItems = sender.Session.Inventory.Expired
                            .Concat(sender.Session.Character.Expired)
                            .ToArray();
                        if (expiredItems.Length > 0)
                            sender.Send(Server.PacketFactory.ItemExpired(sender, expiredItems));
                    }

                    //sender.Send(Server.PacketFactory.KeepAlive(sender));
                    return Net.Handler.Result.Success;
                }
                else
                    sender.Send(Server.PacketFactory.GameInfo(Base.Code.GameInfo.ErrorLoadingAccount));
            }
            else
                sender.Send(Server.PacketFactory.GameInfo(Base.Code.GameInfo.NormalProcedure));
            return Net.Handler.Result.Default;
        }

    }
}
