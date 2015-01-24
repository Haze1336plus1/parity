using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Auth.Packet
{
    public class ServerList : Net.Packet.OutPacket
    {

        public ServerList(Client.SessionDetails sessionDetails)
            : base(Net.PacketCodes.LAUTHENTICATION)
        {
            base.Add((int)Base.Code.Login.Success);
            base.Add(sessionDetails.Account.Id);
            base.Add(0);
            base.Add(sessionDetails.Account.Username);
            base.Add(sessionDetails.Account.Id);
            base.Add(sessionDetails.Account.Nickname);
            base.Add(1);
            base.Add(21);
            base.Add(5538);
            base.Add(0); // 123 for gm rights
            base.Add(sessionDetails.Session.SessionKey);
            base.Add(-1); // ClanID
            base.Add("INVALID"); // ClanName
            base.Add(-1); // ClanRank
            base.Add(-1); // ClanIconID
            base.Add(-1);
            base.Add(-1);
            base.Add(0);
            base.Add(-1);

            Kernel.Config.cServerListServer[] serverList = QA.Core.Config.Servers;

            base.Add(serverList.Length); // server count
            base.Add(null); // space or whut

            for (int i = 0; i < serverList.Length; i++)
            {
                Kernel.Config.cServerListServer currentServer = serverList[i];
                base.Add(currentServer.Id); // server id
                base.Add(currentServer.Name); // server name
                base.Add(currentServer.Host.IP); // server ip
                base.Add(currentServer.Host.Port); // server port
                base.Add(1337); // active players
                base.Add(0); // server type
            }

            base.Add(0);
        }

    }
}
