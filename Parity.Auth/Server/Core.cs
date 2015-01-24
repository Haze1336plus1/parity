using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Parity.Auth.Server
{
    public class Core : Kernel.ServerBase
    {

        private static object _lockingObject = new Object();
        private static Core _instance;
        public static Core GetInstance()
        {
            lock (Core._lockingObject)
            {
                lock (Core._instance)
                {
                    return Core._instance; // automatically created by kernel
                }
            }
        }

        public Dictionary<int, Net.Handler.IHandler<Client.Client>> HandlerList { get; private set; }
        public Kernel.Config.Auth Config { get; private set; }
        public Net.Server.TCP.Server<Client.Client> Server { get; private set; }
        public Net.Server.TCP.Server<Client.Remote.Client> RemoteServer { get; private set; }
        public Base.Log Log { get; private set; }

        private void Configure()
        {
            System.Xml.XmlDocument configDocument = new System.Xml.XmlDocument();
            configDocument.Load(Base.Compile.FileNames["Auth.Config"]);
            this.Config = new Kernel.Config.Auth(configDocument["Auth"]);
            Console.Title = "Parity AUTH Server";
        }

        public Core()
            : base(new DS.Configuration((string)Base.Compile.FileNames["Database.Config"])) //(Base.Compile.FileNames["Auth.Config"]) // string configurationFile, 
        {

            //  Bootstrap
            
            Base.IO.Informational("Parity Project [AUTH] starting ...");

            this.Configure();
            QA.SetCore(this);

            this.Log = new Base.Log("authServer");
            this.Log.AddChannel("connection");
            this.Log.AddChannel("remoteServer");

            //  Handler

            this.HandlerList = new Dictionary<int, Net.Handler.IHandler<Client.Client>>();

            // removing LPATCH because we no longer need this, doing everything via XML
            //this.HandlerList.Add(Net.PacketCodes.LPATCH, new Handler.Patch());
            this.HandlerList.Add(Net.PacketCodes.LAUTHENTICATION, new Handler.Login());

            //  Server

            this.Server = new Net.Server.TCP.Server<Client.Client>(
                new IPEndPoint(IPAddress.Parse(this.Config.Host.IP), this.Config.Host.Port));

            this.Server.ClientAccepted += Server_ClientAccepted;
            this.Server.ClientDisconnected += Server_ClientDisconnected;
            this.Server.DataReceived += Server_DataReceived;

            // Remote Server

            this.RemoteServer = new Net.Server.TCP.Server<Client.Remote.Client>(
                new IPEndPoint(IPAddress.Parse(this.Config.RemoteHost.IP), this.Config.RemoteHost.Port));

            this.RemoteServer.ClientAccepted += RemoteServer_ClientAccepted;
            this.RemoteServer.ClientDisconnected += RemoteServer_ClientDisconnected;
            this.RemoteServer.DataReceived += RemoteServer_DataReceived;

            // ...

            Base.IO.GetInstance().SetHeading("Parity AUTH up and running!");
            Console.Title = "Parity AUTH";

            Core._instance = this;

        }

        private void RemoteServer_DataReceived(Net.Server.TCP.Server<Client.Remote.Client> sender, Net.Server.TCP.ClientDataReceivedEventArgs<Client.Remote.Client> e)
        {
            string[] message = Encoding.UTF8.GetString(e.Data).Split(' ');
            string command = ArrayExtension.Pop(ref message, true);
            if (command == "remote-file" && message.Length > 0)
            {
                string requestedFile = String.Join(" ", message).Trim();
                if (this.Config.RemoteFileMap.ContainsKey(requestedFile))
                {
                    string rfFileName = System.IO.Path.Combine(Base.Compile.DataRemoteDirectory, this.Config.RemoteFileMap[requestedFile]);
                    if (System.IO.File.Exists(rfFileName))
                        e.Client.Send(System.IO.File.ReadAllBytes(rfFileName));
                    else
                        e.Client.Send("Warning! Remote File not found!");
                }
            }
            else
            {
                this.Log["remoteServer"].Write("Unknown Command '{0}' from {1}".Process(command, e.Client.RemoteEndPoint.ToString()));
                e.Client.Disconnect();
            }
        }

        private void RemoteServer_ClientDisconnected(Net.Server.TCP.Server<Client.Remote.Client> sender, Net.Server.TCP.ClientDisconnectedEventArgs<Client.Remote.Client> e)
        {
            
        }

        private void RemoteServer_ClientAccepted(Net.Server.TCP.Server<Client.Remote.Client> sender, Net.Server.TCP.ClientConnectedEventArgs<Client.Remote.Client> e)
        {
            
        }

        //this.HandlerList[packet.Operationcode].Handle(

        private void Server_DataReceived(Net.Server.TCP.Server<Client.Client> sender, Net.Server.TCP.ClientDataReceivedEventArgs<Client.Client> e)
        {
            var packetList = Net.Packet.PacketReader.GetPackets(e.Data, Net.Keys.LoginKeyC, e.Client.OverlappedReceiveData).ToArray();
            if (e.Client.OverlappedReceiveData.Length < 2048)
            {
                foreach (Net.Packet.InPacket packet in packetList)
                {
                    if (!this.HandlerList.ContainsKey(packet.Operationcode))
                    {
                        e.Client.Disconnect(String.Format("Handler for OPC {0} not found: {1}", packet.Operationcode, packet.Print()));
                        return;
                    }
                    Net.Handler.Result result = this.HandlerList[packet.Operationcode].Handle(e.Client, packet);
                    if (!result.IsSuccess)
                    {
                        if (String.IsNullOrEmpty(result.Message))
                            e.Client.Disconnect(String.Format("Handler for OPC {0} disconnected", packet.Operationcode));
                        else
                            e.Client.Disconnect(String.Format("Handler for OPC {0} disconnected: {1}", packet.Operationcode, result.Message));
                        return;
                    }
                }
            }
            else
                e.Client.Disconnect();
        }

        private void Server_ClientDisconnected(Net.Server.TCP.Server<Client.Client> sender, Net.Server.TCP.ClientDisconnectedEventArgs<Client.Client> e)
        {
            string disconnectReason = Base.Decision.NotNull(e.Client.DisconnectReason, "remote");
            this.Log["connection"].Write(e.Client.RemoteEndPoint.ToString() + ", Reason: " + disconnectReason, "Disconnected");
            e.Client.Disconnect(); // ensure that
        }

        private void Server_ClientAccepted(Net.Server.TCP.Server<Client.Client> sender, Net.Server.TCP.ClientConnectedEventArgs<Client.Client> e)
        {
            e.Client.Send(new Packet.Hello());
            this.Log["connection"].Write(e.Client.RemoteEndPoint.ToString(), "Connected");
        }

        public override void Start()
        {
            base.Start();
            this.Server.Start();
            this.RemoteServer.Start();
            Base.IO.Informational("Parity AUTH Started");
        }

    }
}
