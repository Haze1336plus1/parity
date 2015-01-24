using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Net.Packet.UDP
{
    public class Base
    {

        public Parity.Base.Enum.PacketUDP Identity { get; protected set; }
        public byte[] Data { get; private set; }

        protected Base(byte[] Data)
        {
            this.Identity = Parity.Base.Enum.PacketUDP.Unknown;
            this.Data = Data;
        }

        public virtual byte[] CreateResponse(Server.UDP.ManagedUDP server, params object[] parameters) { return null; }

        public static Base GetFromPackage(Server.UDP.ManagedUDP server, byte[] data)
        {

            Parity.Base.Enum.PacketUDP packetType = Parity.Base.Enum.PacketUDP.Unknown;

            if (data.Length == 14 && data[0] == 0x10 && data[1] == 0x01 && data[2] == 0x01)
                packetType  = Parity.Base.Enum.PacketUDP.Authenticate;
            else if (data.Length == 46 && data[0] == 0x10 && data[1] == 0x10 && data[2] == 0x00 && data[14] == 0x21)
                packetType = Parity.Base.Enum.PacketUDP.UpdateIP;
            else if (data.Length > 20 && data[0] == 0x10 && data[1] == 0x10 && data[2] == 0x00 && (data[14] == 0x2E || data[14] == 0x31 || data[14] == 0x34 || data[14] == 0x30)) //todo: make this better
                packetType = Parity.Base.Enum.PacketUDP.Tunneling;

            if (packetType == Parity.Base.Enum.PacketUDP.Authenticate)
                return new Authenticate(server, data);
            else if (packetType == Parity.Base.Enum.PacketUDP.UpdateIP)
                return new UpdateIP(server, data);

            return null;

        }

    }
}
