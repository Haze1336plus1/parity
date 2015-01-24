using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Client.Management
{
    public class Client : Net.Server.TCP.VirtualClient
    {

        public bool Authenticated { get; set; }

        public Client(byte[] _receiveBuffer, System.Net.Sockets.Socket _socket)
            : base(_receiveBuffer, _socket)
        {
            this.Authenticated = false;
        }

    }
}
