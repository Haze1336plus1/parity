using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketSniffer.Net
{
    public class Sniffer
    {

        public SharpPcap.ICaptureDevice Device { get; private set; }
        public Parity.Base.Log Log { get; private set; }
        public Parity.Base.Log GameplayLog { get; private set; }

        protected byte[] keys;
        protected uint[] averagePackets;
        protected byte[] ReceiveData;

        protected byte[] OverlapLClient;
        protected byte[] OverlapLServer;

        protected byte[] OverlapGClient;
        protected byte[] OverlapGServer;

        public Sniffer(SharpPcap.ICaptureDevice device)
        {
            this.Device = device;
            this.Device.OnPacketArrival += Device_OnPacketArrival;

            this.keys = new byte[4];
            this.averagePackets = new uint[6];
            this.ReceiveData = new byte[2048];
            this.OverlapLClient = new byte[0];
            this.OverlapLServer = new byte[0];
            this.OverlapGClient = new byte[0];
            this.OverlapGServer = new byte[0];

            var fnameMatch = System.Text.RegularExpressions.Regex.Match(device.ToString(), @"Addr:\s*(?<ipAddress>(?:\d{1,3}\.){3}\d{1,3})\nNetmask.*HW addr:\s*(?<macAddress>.*?)\n", System.Text.RegularExpressions.RegexOptions.Singleline);
            this.Log = new Parity.Base.Log("sniff_" + fnameMatch.Groups["ipAddress"].Value + "_" + fnameMatch.Groups["macAddress"].Value);
            this.GameplayLog = new Parity.Base.Log(this.Log.BaseName + "_gameplay");
            this.Log.AddChannel("Login:Client");
            this.Log.AddChannel("Client:Login");
            this.Log.AddChannel("Lobby:Client");
            this.Log.AddChannel("Client:Lobby");
            this.GameplayLog.AddChannel("GP:Client");
            this.GameplayLog.AddChannel("Client:GP");

            this.Log.Write("Opening Device '" + this.Device.Name + "' ...");
            this.Device.Open(SharpPcap.DeviceMode.Normal, 5000);
            this.Log.Write("Device opened!");
        }

        private void Device_OnPacketArrival(object sender, SharpPcap.CaptureEventArgs e)
        {

            int lport = Configuration.Get<int>("Port.Login");
            int lobbyport = Configuration.Get<int>("Port.Lobby");
            int gpport = Configuration.Get<int>("Port.Gameplay");

            try
            {
                PacketDotNet.Packet ePacket = PacketDotNet.Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);
                PacketDotNet.IpPacket iPacket = ePacket.Extract(typeof(PacketDotNet.IpPacket)) as PacketDotNet.IpPacket;
                if (iPacket == null)
                    return;

                PacketDotNet.TcpPacket tPacket = ePacket.Extract(typeof(PacketDotNet.TcpPacket)) as PacketDotNet.TcpPacket;
                if (tPacket != null && tPacket.PayloadData.Length > 0)
                {
                    byte pKey = (byte)(tPacket.PayloadData[tPacket.PayloadData.Length - 1] ^ 0x0A);
                    Parity.Net.Packet.InPacket[] packetList = new Parity.Net.Packet.InPacket[0];
                    string logChannel = string.Empty;
                    if (tPacket.SourcePort == lport)
                    {
                        if (this.keys[0] > 0) pKey = this.keys[0];
                        packetList = Parity.Net.Packet.PacketReader.GetPackets(tPacket.PayloadData, pKey, ref this.OverlapLClient);
                        logChannel = "Login:Client";
                        this.averagePackets[0] = this.averagePackets[0] + (uint)packetList.Length;
                        if (this.OverlapLClient.Length == 0)
                            this.keys[0] = pKey;
                    }
                    else if (tPacket.DestinationPort == lport)
                    {
                        if (this.keys[1] > 0) pKey = this.keys[1];
                        packetList = Parity.Net.Packet.PacketReader.GetPackets(tPacket.PayloadData, pKey, ref this.OverlapLServer);
                        logChannel = "Client:Login";
                        this.averagePackets[1] = this.averagePackets[1] + (uint)packetList.Length;
                        if (this.OverlapLServer.Length == 0)
                            this.keys[1] = pKey;
                    }
                    else if (tPacket.SourcePort == lobbyport)
                    {
                        if (this.keys[2] > 0) pKey = this.keys[2];
                        packetList = Parity.Net.Packet.PacketReader.GetPackets(tPacket.PayloadData, pKey, ref this.OverlapGClient);
                        logChannel = "Lobby:Client";
                        this.averagePackets[2] = this.averagePackets[2] + (uint)packetList.Length;
                        if (this.OverlapGClient.Length == 0)
                            this.keys[2] = pKey;

                    }
                    else if (tPacket.DestinationPort == lobbyport)
                    {
                        if (this.keys[3] > 0) pKey = this.keys[3];
                        packetList = Parity.Net.Packet.PacketReader.GetPackets(tPacket.PayloadData, pKey, ref this.OverlapGServer);
                        logChannel = "Client:Lobby";
                        this.averagePackets[3] = this.averagePackets[3] + (uint)packetList.Length;
                        if (this.OverlapGServer.Length == 0)
                            this.keys[3] = pKey;
                    }
                    if (logChannel != string.Empty &&
                        packetList.Length > 0)
                    {
                        foreach (Parity.Net.Packet.InPacket aPacket in packetList)
                        {
                            this.Log[logChannel].Write(aPacket.Print(), "{0}:{1} -> {2}:{3} | Key 0x{4}".Process(iPacket.SourceAddress.ToString(), tPacket.SourcePort.ToString(), iPacket.DestinationAddress.ToString(), tPacket.DestinationPort.ToString(), pKey.ToString("X2")));
                        }
                    }
                    else
                    {
                        if (tPacket.SourcePort == gpport)
                        {
                            this.GameplayLog["GP:Client"].Write(BitConverter.ToString(tPacket.PayloadData), "{0}:{1} -> {2}:{3}".Process(iPacket.SourceAddress.ToString(), tPacket.SourcePort.ToString(), iPacket.DestinationAddress.ToString(), tPacket.DestinationPort.ToString()));
                            this.averagePackets[4] = this.averagePackets[4] + 1;
                        }
                        else if (tPacket.DestinationPort == gpport)
                        {
                            this.GameplayLog["Client:GP"].Write(BitConverter.ToString(tPacket.PayloadData), "{0}:{1} -> {2}:{3}".Process(iPacket.SourceAddress.ToString(), tPacket.SourcePort.ToString(), iPacket.DestinationAddress.ToString(), tPacket.DestinationPort.ToString()));
                            this.averagePackets[5] = this.averagePackets[5] + 1;
                        }
                    }
                }
            }
            catch
            {
                // SAMMA WASN DA LOSCH!
            }
        }

        public void StartReceive()
        {
            this.Device.Filter = "tcp port {0} or {1} or {2}"
                .Process(
                    Configuration.Get<int>("Port.Login"),
                    Configuration.Get<int>("Port.Lobby"),
                    Configuration.Get<int>("Port.Gameplay")
                );
            this.Device.StartCapture();

            var updateAverage = new System.Threading.Thread(() =>
            {
                while (true)
                {
                    System.Threading.Thread.Sleep(1000);
                    Parity.Base.IO.GetInstance().SetHeading("Capturing | LOGIN {0}:{1} > GAME {2}:{3} > GAMEPLAY {4}:{5}".Process(
                        this.averagePackets[0], this.averagePackets[1],
                        this.averagePackets[2], this.averagePackets[3],
                        this.averagePackets[4], this.averagePackets[5]));
                    //this.averagePackets = new uint[6];
                }
            });
            updateAverage.Start();
        }

    }
}
