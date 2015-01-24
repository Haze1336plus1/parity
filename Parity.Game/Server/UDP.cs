using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Game.Server
{
    public class UDP
    {

        public Net.Server.UDP.ManagedUDP Server1;
        public Net.Server.UDP.ManagedUDP Server2;

        public UDP()
        {
            var serverIP = System.Net.IPAddress.Parse(QA.Core.Config.Host.IP);

            var initServer = new Func<Net.Server.UDP.ManagedUDP, ushort, Net.Server.UDP.ManagedUDP>((Net.Server.UDP.ManagedUDP server, ushort port) =>
            {
                server = new Net.Server.UDP.ManagedUDP(new System.Net.IPEndPoint(serverIP, port));
                server.OnClientConnected += server_OnClientConnected;
                server.OnClientDisconnected += server_OnClientDisconnected;
                server.OnDataReceived += server_OnDataReceived;
                return server;
            });

            this.Server1 = initServer(this.Server1, 5350);
            this.Server2 = initServer(this.Server2, 5351);

        }

        public void Start()
        {
            this.Server1.Start();
            this.Server2.Start(false);
        }
        public void Stop()
        {
            this.Server1.Stop();
            this.Server2.Stop();
        }

        private void server_OnDataReceived(Net.Server.UDP.ManagedUDP sender, Net.Server.UDP.ClientDataReceivedEventArgs<Net.Server.UDP.Session> e)
        {
            Net.Packet.UDP.Base receivedData = Net.Packet.UDP.Base.GetFromPackage(sender, e.Data);
            if (receivedData != null)
            {
                if (receivedData.Identity == Base.Enum.PacketUDP.Authenticate)
                {
                    Net.Packet.UDP.Authenticate udpAuthenticate = receivedData as Net.Packet.UDP.Authenticate;
                    Client.Client tcpClient = Management.Selection.FindOne(x => x.Session.SessionID == udpAuthenticate.SessionId).FirstOrDefault();
                    if (tcpClient != null)
                    {
                        if (!tcpClient.RemoteEndPoint.Address.Equals(e.Client.RemoteEndPoint.Address))
                            tcpClient.Disconnect("Different RemoteEndPoint on UDPAuthenticate");
                        if (tcpClient.Session.Account.Id != udpAuthenticate.UserId)
                            tcpClient.Disconnect("Different UserId on UDPAuthenticate");
                        byte[] response = udpAuthenticate.CreateResponse(sender);
                        if (response != null)
                            e.Client.Send(response);
                    }
                }
                else if (receivedData.Identity == Base.Enum.PacketUDP.UpdateIP)
                {
                    Net.Packet.UDP.UpdateIP udpUpdateIP = receivedData as Net.Packet.UDP.UpdateIP;
                    Client.Client tcpClient = Management.Selection.FindOne(x => x.Session.SessionID == udpUpdateIP.SessionId).FirstOrDefault();
                    if (tcpClient != null)
                    {
                        if (!tcpClient.RemoteEndPoint.Address.Equals(e.Client.RemoteEndPoint.Address))
                            tcpClient.Disconnect("Different RemoteEndPoint on UDPUpdateIP");
                        tcpClient.Session.RemoteNetwork = e.Client.RemoteEndPoint;
                        tcpClient.Session.LocalNetwork = udpUpdateIP.LocalEndPoint;
                        e.Client.Send(udpUpdateIP.CreateResponse(sender, tcpClient.Socket.RemoteEndPoint));
                    }
                }
                else
                {
                    Base.IO.Debug("UDP Invalid PacketUDP.Identity: " + receivedData.Identity.ToString());
                }
            }
        }

        private void server_OnClientDisconnected(Net.Server.UDP.ManagedUDP sender, Net.Server.UDP.ClientDisconnectedEventArgs<Net.Server.UDP.Session> e)
        {
            Client.Client tcpClient = Management.Selection.FindOne(x => x.Session.RemoteNetwork.Address == e.Client.RemoteEndPoint.Address).FirstOrDefault();
            if (tcpClient != null)
                tcpClient.Disconnect("UDP Connection timeout");
        }

        private void server_OnClientConnected(Net.Server.UDP.ManagedUDP sender, Net.Server.UDP.ClientConnectedEventArgs<Net.Server.UDP.Session> e)
        {
        }

    }
}
