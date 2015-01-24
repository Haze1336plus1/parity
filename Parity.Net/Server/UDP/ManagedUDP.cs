using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Net.Server.UDP
{
    public class ManagedUDP
    {

        private object _lockingObject = new Object();

        public Dictionary<System.Net.IPEndPoint, Session> SessionList { get; private set; }
        public System.Net.Sockets.Socket Socket { get; private set; }
        public System.Net.IPEndPoint LocalEndPoint { get; private set; }

        protected byte[] ReceiveBuffer;
        protected System.Net.EndPoint RemoteEndPoint;

        public static System.Net.IPEndPoint NetworkEP { get; private set; }

        static ManagedUDP()
        {
            ManagedUDP.NetworkEP = new System.Net.IPEndPoint(System.Net.IPAddress.Parse("0.0.0.0"), 0);
        }

        public ManagedUDP(System.Net.IPEndPoint localEndPoint)
        {
            this.SessionList = new Dictionary<System.Net.IPEndPoint, Session>();
            this.Socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Dgram, System.Net.Sockets.ProtocolType.Udp);
            this.LocalEndPoint = localEndPoint;
            this.ReceiveBuffer = new byte[8192];
        }

        public void Start(bool checkSessions = true)
        {
            this.Socket.Bind(this.LocalEndPoint);
            this.Socket.Blocking = false;
            this.StartReceive();
            if(checkSessions)
                this.CheckSessions(); // initialize this
        }
        public void Stop()
        {
            this.Socket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
            this.Socket.Close();
        }

        private void CheckSessions()
        {
            lock (this._lockingObject)
            {
                for (int i = this.SessionList.Count - 1; i >= 0; i--)
                {
                    Session currentSession = this.SessionList.Values.ElementAt(i);
                    if (Base.App.Time.IsOlder(currentSession.LastResponse, Constant.SessionTimeout))
                    {
                        if (this.OnClientDisconnected != null)
                            this.OnClientDisconnected(this, new ClientDisconnectedEventArgs<Session>(currentSession, System.Net.Sockets.SocketError.TimedOut));
                        this.SessionList.Remove(currentSession.RemoteEndPoint);
                    }
                }
            }
            Base.Thread.Timer.GetInstance().AddAction(Constant.SessionTimeout, CheckSessions);
        }

        private void StartReceive()
        {
            this.RemoteEndPoint = ManagedUDP.NetworkEP.Create(new System.Net.SocketAddress(System.Net.Sockets.AddressFamily.InterNetwork));
            this.Socket.BeginReceiveFrom(this.ReceiveBuffer, 0, this.ReceiveBuffer.Length, System.Net.Sockets.SocketFlags.None, ref this.RemoteEndPoint, ManagedUDP.IReceive, this);
        }

        internal static void IReceive(IAsyncResult ar)
        {
            ManagedUDP udpServer = ar.AsyncState as ManagedUDP;
            if (udpServer == null)
                throw new ArgumentException("Please call .BeginReceiveFrom with a valid ManagedUDP Object");

            System.Net.EndPoint rEP = new System.Net.IPEndPoint(ManagedUDP.NetworkEP.Address, ManagedUDP.NetworkEP.Port);

            lock (udpServer._lockingObject)
            {

                int dataLength = udpServer.Socket.EndReceiveFrom(ar, ref rEP);
                byte[] receivedData = new byte[dataLength];
                Array.Copy(udpServer.ReceiveBuffer, 0, receivedData, 0, receivedData.Length);

                System.Net.IPEndPoint rIEP = (System.Net.IPEndPoint)rEP;

                Session currentSession = null;
                if (!udpServer.SessionList.ContainsKey(rIEP))
                {
                    currentSession = new Session(udpServer, rIEP);
                    if(udpServer.OnClientConnected != null)
                        udpServer.OnClientConnected(udpServer, new ClientConnectedEventArgs<Session>(currentSession));
                    udpServer.SessionList.Add(rIEP, currentSession);
                }
                else
                    currentSession = udpServer.SessionList[rIEP];

                currentSession.DataReceived();
                udpServer.OnDataReceivedImpl(currentSession, receivedData);

            }

            udpServer.StartReceive();
        }

        protected void OnDataReceivedImpl(Session udpSession, byte[] data)
        {
            if (this.OnDataReceived != null)
                this.OnDataReceived(this, new ClientDataReceivedEventArgs<Session>(udpSession, data));
            // do awesome stuff here
        }

        public event OnClientConnectedHandler OnClientConnected;
        public delegate void OnClientConnectedHandler(ManagedUDP sender, ClientConnectedEventArgs<Session> e);

        public event OnClientDisconnectedHandler OnClientDisconnected;
        public delegate void OnClientDisconnectedHandler(ManagedUDP sender, ClientDisconnectedEventArgs<Session> e);

        public event OnDataReceivedHandler OnDataReceived;
        public delegate void OnDataReceivedHandler(ManagedUDP sender, ClientDataReceivedEventArgs<Session> e);

    }
}
