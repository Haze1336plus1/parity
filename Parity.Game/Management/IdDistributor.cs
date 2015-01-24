using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Game.Management
{
    public class IdDistributor
    {

        public static readonly short SessionIdStart = 1000;
        public static readonly short SessionIdLimit = 5000;

        public static short GetSession()
        {

            short[] idCollection = (from Client.Client cclient in QA.Core.LobbyServer.Clients where cclient.Session.IsActive orderby cclient.Session.SessionID select cclient.Session.SessionID).ToArray();

            short sessionId = SessionIdStart;
            for (; sessionId < idCollection.Length; sessionId++)
            {
                if (sessionId != idCollection[sessionId - SessionIdStart])
                    return sessionId;
            }

            return idCollection.Length == 0 ? IdDistributor.SessionIdStart : (short)(sessionId + 1);

        }

    }
}
