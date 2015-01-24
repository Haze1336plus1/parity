using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Base.Layout
{
    public struct Server
    {

        public readonly ushort Id;
        public readonly string Name;
        public readonly string Ip;
        public readonly ushort Port;
        public ushort ActivePlayers;
        // server type; derppp

        public Server(ushort id, string name, string ip, ushort port)
        {
            this.Id = id;
            this.Name = name;
            this.Ip = ip;
            this.Port = port;
            this.ActivePlayers = 0;
        }

    }
}
