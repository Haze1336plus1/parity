using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Auth.Client.Remote
{
    public class Client : Net.Server.TCP.VirtualClient
    {

        public SessionDetails Session { get; private set; }

        public Client(byte[] _receiveBuffer, System.Net.Sockets.Socket socket)
            : base(_receiveBuffer, socket)
        {
            this.Session = new SessionDetails(this);
        }

        public void Send(string message)
        {
            base.Send(Encoding.UTF8.GetBytes(message));
        }

    }
}
