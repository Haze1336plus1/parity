using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Client
{
    public class GameplayClient : Net.Server.TCP.VirtualClient
    {

        public GameplayClient(byte[] _receiveBuffer, System.Net.Sockets.Socket _socket)
            : base(_receiveBuffer, _socket)
        {
        }

    }
}
