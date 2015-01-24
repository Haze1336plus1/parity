using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Auth.Client
{
    public class Client : Net.Server.TCP.VirtualClient
    {

        public Controller AccountController { get; private set; }
        public SessionDetails Session { get; private set; }
        public byte[] OverlappedReceiveData;

        public Client(byte[] _receiveBuffer, System.Net.Sockets.Socket socket)
            : base(_receiveBuffer, socket)
        {
            this.AccountController = new Controller(this);
            this.Session = new SessionDetails(this);
            this.OverlappedReceiveData = new byte[] { };
            // lelelel
        }

        public void Send(Net.Packet.OutPacket packet)
        {
            byte[] sendData = Net.Packet.PacketWriter.WritePacket(packet, Net.Keys.LoginKey);
            //sendData = Base.App.AES.Encrypt(sendData, "ja95u4U4IAgxSQ7BnlkRxI3xuEvbN1zX", "parityProject", "SHA1", 2, "OFRna73m*aze01xY", 256);
            base.Socket.Send(sendData, System.Net.Sockets.SocketFlags.None);
        }

    }
}
