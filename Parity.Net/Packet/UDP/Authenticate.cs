using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Parity.Base.App;

namespace Parity.Net.Packet.UDP
{
    public class Authenticate : Base
    {

        public short SessionId { get; private set; }
        public int UserId { get; private set; }

        // create a CreateResponse stub ._.

        public override byte[] CreateResponse(Server.UDP.ManagedUDP server, params object[] parameters)
        {
            byte[] response = new byte[this.Data.Length];
            Array.Copy(this.Data, 0, response, 0, response.Length);

            Array.Copy(
                BitConverter.GetBytes((ushort)server.LocalEndPoint.Port).Reverse().ToArray(), 0,
                response, 4, 2);

            return response;
        }

        public Authenticate(Server.UDP.ManagedUDP server, byte[] data)
            : base(data)
        {
            this.Identity = Parity.Base.Enum.PacketUDP.Authenticate;

            this.SessionId = BitConverter.ToInt16(
                Parity.Base.App.ArrayHelp.CutOut<byte>(Data, 4, 2).Reverse().ToArray(),
                0);

            this.UserId = BitConverter.ToInt32(
                Parity.Base.App.ArrayHelp.CutOut<byte>(Data, 10, 4).Reverse().ToArray(),
                0);
        }

    }
}
