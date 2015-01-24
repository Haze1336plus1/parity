using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.Linq;

namespace Parity.Net.Server.TCP
{
    public class Server<Client> where Client : VirtualClient
    {

        #region Members

        public bool ServerRunning { get; private set;}
        public List<System.Net.IPAddress> Bans { get; private set; }
        private Client[] _clients;
        public IEnumerable<Client> Clients { get { return this._clients.Where(x => x != null); } }
        public System.Net.Sockets.Socket Socket { get; private set; }
        public System.Net.IPEndPoint LocalEndPoint { get; private set; }
        public int Backlog { get; private set; }
        public int ConnectionLimit { get; private set; }
        internal byte[][] _connectionReceiveBuffer;

        #endregion

        #region Constructor, Functions

        public Server(IPEndPoint localEndPoint, int backlog = 32, int _connectionLimit = 256, int _connectionReceiveBufferSize = 2048)
	    {
            this.LocalEndPoint = localEndPoint;
            this.Backlog = backlog;
            this.ConnectionLimit = _connectionLimit;

            this._clients = new Client[_connectionLimit];
            this.Bans = new List<System.Net.IPAddress>();

            // initialize connection receive buffer for pinned data
            this._connectionReceiveBuffer = new byte[_connectionLimit][];
            for (int i = 0; i < _connectionLimit; i++)
                this._connectionReceiveBuffer[i] = new byte[_connectionReceiveBufferSize];
	    }

        /// <summary>
        /// Bind socket to LocalEndPoint and listen for connections
        /// </summary>
        public void Start()
        {
            if (this.Socket == null)
            {
                this.Socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
                this.Socket.Bind(this.LocalEndPoint);
                this.Socket.Listen(this.Backlog);
                this.ServerRunning = true;
                this.Socket.BeginAccept(OnAccept, this.Socket);
            }
        }

        /// <summary>
        /// Stop server and disconnect all clients
        /// </summary>
        public void Stop()
        {
            if ((this.Socket != null))
            {
                this.ServerRunning = false;
                foreach (Client c in this._clients)
                {
                    c.Disconnect();
                    //Disconnect each client
                }
                this.Socket.Close();
            }
        }

        /// <summary>
        /// Asynchronous callback for Socket.BeginAccept
        /// </summary>
        protected void OnAccept(IAsyncResult ar)
        {
            System.Net.Sockets.Socket myServer = (System.Net.Sockets.Socket)ar.AsyncState;
            if (this.ServerRunning)
            {
                try
                {
                    System.Net.Sockets.Socket client = myServer.EndAccept(ar);
                    if (this.Bans.Contains(((System.Net.IPEndPoint)client.RemoteEndPoint).Address))
                    {
                        client.Disconnect(false);
                        //Client is banned
                        if (OnIpBlocked != null)
                            OnIpBlocked(this, (System.Net.IPEndPoint)client.RemoteEndPoint);
                    }
                    else
                    {
                        int freeClient = this._clients.IndexOf(null);
                        Client myClient = (Client)typeof(Client).GetConstructor(new System.Type[] { typeof(byte[]), typeof(System.Net.Sockets.Socket) }).Invoke(new object[] { this._connectionReceiveBuffer[freeClient], client });
                        this._clients[freeClient] = myClient;
                        myClient.OnDisconnected += (sender, error) =>
                        {
                            this.CHOnDisconnect(sender, error);
                            this._clients[freeClient] = null; // cya
                        };
                        myClient.OnDataReceived += CHOnDataReceived;
                        if (ClientAccepted != null)
                            ClientAccepted(this, new ClientConnectedEventArgs<Client>(myClient));
                        myClient.BeginListen();
                    }
                }
                catch
                {
                }

                while (!this._clients.Contains(null))
                    System.Threading.Thread.Sleep(5);
                myServer.BeginAccept(OnAccept, myServer);
            }
        }

        #endregion

        #region Events

        public event OnIpBlockedEventHandler OnIpBlocked;
        public delegate void OnIpBlockedEventHandler(Server<Client> sender, System.Net.IPEndPoint e);
        public event ClientAcceptedEventHandler ClientAccepted;
        public delegate void ClientAcceptedEventHandler(Server<Client> sender, ClientConnectedEventArgs<Client> e);
        public event ClientDisconnectedEventHandler ClientDisconnected;
        public delegate void ClientDisconnectedEventHandler(Server<Client> sender, ClientDisconnectedEventArgs<Client> e);
        public event DataReceivedEventHandler DataReceived;
        public delegate void DataReceivedEventHandler(Server<Client> sender, ClientDataReceivedEventArgs<Client> e);

        private void CHOnDisconnect(VirtualClient sender, System.Net.Sockets.SocketError ErrorCode)
        {
            if (ClientDisconnected != null)
                ClientDisconnected(this, new ClientDisconnectedEventArgs<Client>((Client)sender, ErrorCode));
        }
        private void CHOnDataReceived(VirtualClient sender, byte[] data)
        {
            if (DataReceived != null)
                DataReceived(this, new ClientDataReceivedEventArgs<Client>((Client)sender, data));
        }

        #endregion

    }
}