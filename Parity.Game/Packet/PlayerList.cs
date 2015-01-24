#pragma warning disable 0618
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ipa = System.Net.IPAddress;

namespace Parity.Game.Packet
{
    public class PlayerList : Net.Packet.OutPacket
    {

        public PlayerList(Game.Room room, Client.Client[] players)
            : base(Net.PacketCodes.GAME_USER_LIST)
        {
            base.Add(players.Length);
            foreach (Client.Client p in players)
            {
                base.Add(p.Session.Account.Id);
                base.Add(p.Session.SessionID);
                base.Add(room.Players.Container.IndexOf(p));
                base.Add(p.GameSession.ReadyState ? 1 : 0);
                base.Add((int)p.GameSession.Team);
                base.Add(p.GameSession.CurrentItemIndex);
                base.Add(0); // wat?
                base.Add((int)p.GameSession.BattleClass);
                base.Add(p.GameSession.Health);
                base.Add(p.Session.Account.Nickname);
                base.Add(-1); // clanID
                base.Add(-1); // clanIcon
                base.Add("NULL"); // clanName
                base.Add(-1); // commonly 0
                base.Add(-1); // clanRank?
                base.Add(1);
                base.Add(0);
                base.Add(410); // login-id
                base.Add((int)p.Session.Account.Details.Premium);
                base.Add(1); // pc bang?
                base.Add(-1); // disguise badge
                base.Add(p.Session.Account.Details.Kills);
                base.Add(p.Session.Account.Details.Deaths);
                base.Add(251435); // wat?
                base.Add(p.Session.Account.Details.Experience);
                base.Add(-1);
                base.Add(-1);
                base.Add(ipa.HostToNetworkOrder(p.Session.RemoteNetwork.Address.Address));
                base.Add(ipa.HostToNetworkOrder(p.Session.RemoteNetwork.Port));
                base.Add(ipa.HostToNetworkOrder(p.Session.LocalNetwork.Address.Address));
                base.Add(ipa.HostToNetworkOrder(p.Session.LocalNetwork.Port));
                base.Add(0);
            }
        }

    }
}
