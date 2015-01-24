using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Game.Client
{
    public class Client : Net.Server.TCP.VirtualClient
    {

        public Controller AccountController { get; private set; }
        public SessionDetails Session { get; private set; }
        public GameSessionDetails GameSession { get; private set; }
        public byte[] OverlappedReceiveData;

        public Client(byte[] _receiveBuffer, System.Net.Sockets.Socket socket)
            : base(_receiveBuffer, socket)
        {
            this.AccountController = new Controller(this);
            this.Session = new SessionDetails(this);
            this.GameSession = new GameSessionDetails(this);
            this.OverlappedReceiveData = new byte[] { };
            // lelelel
        }

        public void Chat(string message)
        {
            this.Send(Server.PacketFactory.Chat.System(message));
        }

        public void Send(Net.Packet.OutPacket packet)
        {
            byte[] sendData = Net.Packet.PacketWriter.WritePacket(packet, Net.Keys.GameKey);
            if (base.Socket != null && base.Socket.Connected)
            {
                try
                {
                    base.Socket.Send(sendData, System.Net.Sockets.SocketFlags.None);
                }
                catch
                {
                    this.Disconnect(); // gone :/
                }
            }
        }

    }
}
