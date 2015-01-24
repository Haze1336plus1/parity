using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Net.Server.TCP
{
    public class ClientDisconnectedEventArgs<myClient> : EventArgs
    {

        private myClient _Client;
        public myClient Client
        {
            get { return this._Client; }
        }

        private System.Net.Sockets.SocketError _ErrorCode;
        public System.Net.Sockets.SocketError ErrorCode
        {
            get { return this._ErrorCode; }
        }

        public ClientDisconnectedEventArgs(myClient _Client, System.Net.Sockets.SocketError _ErrorCode)
        {
            this._Client = _Client;
            this._ErrorCode = _ErrorCode;
        }
    }
    public class ClientDataReceivedEventArgs<myClient> : EventArgs
    {

        private myClient _Client;
        public myClient Client
        {
            get { return this._Client; }
        }

        private byte[] _Data;
        public byte[] Data
        {
            get { return this._Data; }
        }

        public ClientDataReceivedEventArgs(myClient _Client, byte[] _Data)
        {
            this._Client = _Client;
            this._Data = _Data;
        }
    }
    public class ClientConnectedEventArgs<myClient> : EventArgs
    {

        private myClient _Client;
        public myClient Client
        {
            get { return this._Client; }
        }

        public ClientConnectedEventArgs(myClient _Client)
        {
            this._Client = _Client;
        }
    }

}
