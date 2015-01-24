using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Parity.Game.Server
{
    public class Core : Kernel.ServerBase
    {

        #region Members

        // Configuration
        public Kernel.Config.Game Config { get; private set; }
        public Kernel.Config.Monitor ConfigMonitor { get; private set; }

        // Server
        public Net.Server.TCP.Server<Client.Client> LobbyServer { get; private set; }
        public Net.Server.TCP.Server<Client.GameplayClient> GameplayServer { get; private set; }
        public Dictionary<int, Handler._Instance> HandlerList { get; private set; }
        public UDP UDPServer { get; private set; }
        //public Net.IOPipe IOPipe { get; private set; }
        
        // Other
        public Base.Log Log { get; private set; }
        public Base.Log ChatLog { get; private set; }
        public Script.RuntimeCode ParityScript { get; private set; }

        private System.Threading.Thread _keepAliveThread;

        #endregion

        #region Constructor, Functions

        /// <summary>
        /// Should be created only once
        /// Constructor of Server.Core
        /// </summary>
        public Core()
            : base(new DS.Configuration((string)Base.Compile.FileNames["Database.Config"])) //(Base.Compile.FileNames["Auth.Config"]) // string configurationFile, 
        {

            //  Bootstrap

            Base.IO.Informational("Parity Project [GAME] starting ...");

            this.Configure();
            QA.SetCore(this);

            this.Log = new Base.Log("gameServer");
            this.Log.AddChannel("connection");
            this.Log.AddChannel("game");
            this.Log.AddChannel("command");
            this.Log.AddChannel("shop");
            //this.Log.AddChannel("chat");

            this.ChatLog = new Base.Log("gameChat");
            this.ChatLog.Format = "{0} | {3}\r\n";

            //  Handler

            this.HandlerList = new Dictionary<int, Handler._Instance>();
            var registerHandler = new Action<int, Type>((int opc, Type ht) =>
            {
                this.HandlerList.Add(opc, new Handler._Instance(ht));
            });

            registerHandler(Net.PacketCodes.C_SERIAL_GSERV, typeof(Handler.Greeting));
            registerHandler(Net.PacketCodes.JOIN_SERV, typeof(Handler.GameInfo));
            registerHandler(Net.PacketCodes.SET_CHANNEL, typeof(Handler.ChangeChannel));
            registerHandler(Net.PacketCodes.CHAT, typeof(Handler.Chat));
            registerHandler(Net.PacketCodes.ROOM_LIST, typeof(Handler.RoomList));
            registerHandler(Net.PacketCodes.KEEPALIVE, typeof(Handler.KeepAlive));
            registerHandler(Net.PacketCodes.CREATE_ROOM, typeof(Handler.CreateRoom));
            registerHandler(Net.PacketCodes.NCASH_PROCESS, typeof(Handler.CashProcess));
            registerHandler(Net.PacketCodes.ITEM_PROCESS, typeof(Handler.ItemProcess));
            registerHandler(Net.PacketCodes.BITEM_CHANGE, typeof(Handler.ItemChange));
            registerHandler(Net.PacketCodes.CSHOP_DEPOT, typeof(Handler.CashShopDepot));
            registerHandler(Net.PacketCodes.ITEM_DESTROY, typeof(Handler.ItemDestroy));
            registerHandler(Net.PacketCodes.COSTUME_PROCESS, typeof(Handler.CostumeProcess));
            registerHandler(Net.PacketCodes.CITEM_CHANGE, typeof(Handler.CostumeItemChange));
            registerHandler(Net.PacketCodes.DEPOT_PROCESS, typeof(Handler.DepotProcess));
            
            registerHandler(Net.PacketCodes.USER_LIST, typeof(Handler._Debug));
            
            //  Server
            
            this.LobbyServer = new Net.Server.TCP.Server<Client.Client>(
                new IPEndPoint(IPAddress.Parse(this.Config.Host.IP), this.Config.Host.Port));
            
            this.LobbyServer.ClientAccepted += Server_ClientAccepted;
            this.LobbyServer.ClientDisconnected += Server_ClientDisconnected;
            this.LobbyServer.DataReceived += Server_DataReceived;
            
            this.GameplayServer = new Net.Server.TCP.Server<Client.GameplayClient>(
                new IPEndPoint(IPAddress.Parse(this.Config.Host.IP), this.Config.Host.Port + 1));
            /*this.GameplayServer.ClientAccepted += GameplayServer_ClientAccepted;
            this.GameplayServer.ClientDisconnected += GameplayServer_ClientDisconnected;
            this.GameplayServer.DataReceived += GameplayServer_DataReceived;*/

            this.UDPServer = new UDP();

#if MMCONSOLE
            this.IOPipe = new Net.IOPipe((string)Base.Compile.GameDefaults["Server.IOPipeName"]);
            this.IOPipe.OnPipeDataReceived += IOPipe_OnPipeDataReceived;
#endif

            //  ParityScript

            Base.IO.GetInstance().SetHeading("Compiling ParityScript");
            Base.IO.Informational("Compiling ParityScript ...");
            this.ParityScript = new Script.RuntimeCode();
            this.ParityScript.Generate();
            
            //  KeepAlive

            this._keepAliveThread = new System.Threading.Thread(this.KeepAliveTick);

            //  Modules

            Modules.Touch();

            Base.IO.GetInstance().SetHeading("Parity GAME up and running!");
            Console.Title = "Parity GAME";

        }

        #region Functions

        /// <summary>
        /// Starts the server, overrid function form Kernel.ServerBase
        /// </summary>
        public override void Start()
        {
            base.Start();
            using (DS.Client dbclient = this.DSManager.GetClient())
            {
                dbclient
                    .Command("UPDATE `accounts` AS a SET a.online = 0 WHERE a.online = @server_id;")
                    .SetParameter("server_id", Config.GameConfig.Id)
                    .Execute();
            }
            this.LobbyServer.Start();
            //this.GameplayServer.Start();
            this.UDPServer.Start();
#if MMCONSOLE
            this.IOPipe.Start();
#endif
            this._keepAliveThread.Start();
            Base.IO.Informational("Parity GAME Started");
        }
        
        /// <summary>
        /// Stop all servers
        /// </summary>
        public void Stop()
        {
            this.LobbyServer.Stop();
            this.GameplayServer.Stop();
            this.UDPServer.Stop();
        }


        /// <summary>
        /// Setup console and read configuration file
        /// </summary>
        /// <param name="reload"></param>
        private void Configure(bool reload = false)
        {
            string configFilename = Base.Compile.FileNames["Game.Config"];

            System.Xml.XmlDocument configDocument = new System.Xml.XmlDocument();
            configDocument.Load(configFilename);
            this.Config = new Kernel.Config.Game(configDocument["Game"]);

            if (!reload)
            {
                this.ConfigMonitor = new Kernel.Config.Monitor(configFilename);
                this.ConfigMonitor.OnFileChanged += ConfigMonitor_OnConfigMonitorFileChanged;

                Console.Title = "Parity GAME Server";
            }
            else
                Base.IO.Informational("Configuration reloaded");

            this.DSModel.SetGameDatabase(this.Config.GameConfig.Database);
        }

        /// <summary>
        /// Disconnects inactive / not responding players in an endless threaded function
        /// </summary>
        private void KeepAliveTick()
        {
            long frequency = (long)(int)Base.Compile.GameDefaults["Lobby.UpdateFrequency"];
            while (true)
            {
                long time = Base.App.Time.Get();
                lock (this.LobbyServer.Clients)
                {
                    foreach (Client.Client c in Management.Selection.ActiveClients())
                    {
                        if (Base.App.Time.IsOlder(c.Session.PingTime, frequency))
                        {
                            if ((c.Session.SessionTimeKA - 5) > c.Session.SessionTime)
                                c.Disconnect("Did not respond to 5 keep alive requests");
                            c.Session.SessionTimeKA++;
                            c.Session.PingTime = time;
                            c.Send(PacketFactory.KeepAlive(c));
                        }
                    }
                }
                System.Threading.Thread.Sleep((int)frequency);
            }
        }

        #endregion

        #region Handler

        /// <summary>
        /// Event handler for Server.DataReceived
        /// </summary>
        private void Server_DataReceived(Net.Server.TCP.Server<Client.Client> sender, Net.Server.TCP.ClientDataReceivedEventArgs<Client.Client> e)
        {
            var packetList = Net.Packet.PacketReader.GetPackets(e.Data, Net.Keys.GameKeyC, e.Client.OverlappedReceiveData).ToArray();
            if (e.Client.OverlappedReceiveData.Length < 2048)
            {
                foreach (Net.Packet.InPacket packet in packetList)
                {
                    string message = "Handler " + packet.Operationcode.ToString();
                    if (e.Client.Session.IsActive)
                        message += " for '{0}' (#{1}) bad".Process(e.Client.Session.Account.Nickname, e.Client.Session.Account.Id);
                    else
                        message += " bad";

                    if (!this.HandlerList.ContainsKey(packet.Operationcode))
                    {
                        e.Client.Disconnect(String.Format("{0} (not found), packet: '{1}'", message, packet.Print()));
                        return;
                    }
                    bool doHandle = true;

                    Handler._Instance hinst = this.HandlerList[packet.Operationcode];

                    if ((hinst.Requirements.Login && !e.Client.Session.IsActive) ||
                        (hinst.Requirements.InRoom && !e.Client.Session.IsActive && !e.Client.GameSession.InRoom))
                        doHandle = false;
                    if (!doHandle)
                    {
                        e.Client.Disconnect(message + ", requirements not matched");
                        return;
                    }

                    Net.Handler.Result result = null;
                    try
                    {
                        result = hinst.Handler.Handle(e.Client, packet);
                    }
                    catch (Base.Exception.CustomException cex)
                    {
                        cex.Handle();
                        if (cex.Flags.HasFlag(Base.Exception.CustomFlags.NetHandleBreak))
                        {
                            message += ", reason: CustomException with NetHandleBreak flag (require failed)";
                            e.Client.Disconnect(message);
                            return;
                        }
                    }

                    if (!result.IsSuccess)
                    {
                        if(!string.IsNullOrEmpty(result.Message))
                            message += ", reason: '{0}'".Process(result.Message);
                        e.Client.Disconnect(message);
                        return;
                    }
                }
            }
            else
                e.Client.Disconnect(); // trying outofmemory! D:
        }

        /// <summary>
        /// Event handler for Server.ClientDisconnected
        /// </summary>
        private void Server_ClientDisconnected(Net.Server.TCP.Server<Client.Client> sender, Net.Server.TCP.ClientDisconnectedEventArgs<Client.Client> e)
        {
            string disconnectReason = Base.Decision.NotNull(e.Client.DisconnectReason, "remote");
            this.Log["connection"].Write(e.Client.RemoteEndPoint.ToString() + ", Reason: " + disconnectReason, "Disconnected");
            e.Client.Disconnect(); // ensure that
            if (e.Client.Session.IsActive)
            {
                e.Client.AccountController.Logout();
            }
        }
        
        /// <summary>
        /// Event handler for Server.ClientAccepted
        /// </summary>
        private void Server_ClientAccepted(Net.Server.TCP.Server<Client.Client> sender, Net.Server.TCP.ClientConnectedEventArgs<Client.Client> e)
        {
            if (this.LobbyServer.Clients.Count() >= Management.IdDistributor.SessionIdLimit)
            {
                e.Client.Disconnect();
                this.Log["connection"].Write(e.Client.RemoteEndPoint.ToString(), "Connected and bye: too many Clients");
            }
            else
                this.Log["connection"].Write(e.Client.RemoteEndPoint.ToString(), "Connected");
        }

        /// <summary>
        /// Event handler for ConfigMonitor.OnConfigMonitorFileChanged
        /// Reconfigures the Core
        /// </summary>
        private void ConfigMonitor_OnConfigMonitorFileChanged(object sender, Base.Event.ConfigMonitorFileChangedEventArgs e)
        {
            this.Configure(true);
        }

        #endregion

        #endregion

    }
}
