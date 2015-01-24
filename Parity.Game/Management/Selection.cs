using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Game.Management
{
    public class Selection
    {

        public static IEnumerable<Client.Client> FindOne(Func<Client.Client, bool> pred, bool isActive = false)
        {
            if(isActive)
                return (from Client.Client c in QA.Core.LobbyServer.Clients where c.Session.IsActive && pred(c) select c);
            else
                return (from Client.Client c in QA.Core.LobbyServer.Clients where pred(c) select c);
        }

        public static IEnumerable<Client.Client> ActiveClients(bool inRoom = false)
        {
            if (inRoom)
                return FindOne(x => x.GameSession.InRoom, true);
            else
                return FindOne(x => true, true);
        }

        public static IEnumerable<Client.Client> ClientsFromChannel(Base.Enum.Channel channel, bool inRoom = false)
        {
            return (from Client.Client c in ActiveClients(inRoom) where c.GameSession.Channel == channel orderby c.Session.SessionID ascending select c);
        }

    }
}
