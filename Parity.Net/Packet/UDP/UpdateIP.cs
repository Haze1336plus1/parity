using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Parity.Base.App;

namespace Parity.Net.Packet.UDP
{
    public class UpdateIP : Base
    {

        public short SessionId { get; private set; }
        public System.Net.IPEndPoint LocalEndPoint { get; private set; }

        // create a CreateResponse stub ._.

        public override byte[] CreateResponse(Server.UDP.ManagedUDP server, params object[] parameters)
        {

            var prepareEndPoint = new Func<System.Net.IPEndPoint, byte[]>((System.Net.IPEndPoint ipe) =>
            {
                byte[] retVal = new byte[6];
                Array.Copy(BitConverter.GetBytes(ipe.Port), 0, retVal, 0, 2);
                Array.Copy(ipe.Address.GetAddressBytes(), 0, retVal, 2, 4);
                return retVal;
            });

            byte[] rep = prepareEndPoint((System.Net.IPEndPoint)parameters[0]);
            byte[] lep = prepareEndPoint(this.LocalEndPoint);

            byte[] sessionId = BitConverter.GetBytes(this.SessionId).Reverse().ToArray();

            byte[] response = new byte[]
            {
                0x10, 0x10, 0x00, 0x00,
                sessionId[0], sessionId[1],
                0xFF, 0xFF, 0xFF, 0xFF,
                0x00, 0x00, 0x00, 0x00,
                0x20, 0x00, 0x00, 0x40, 
                0x00, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 
                0x00, 0x00
                /* payload below */
            };
            int headLen = response.Length;
            Array.Resize(ref response, 65);

            byte[] payload = new byte[]
            {
                0x10, 0x00, 0x02, 0x00,
                0x00, 0x00, 0x00, 0x00, /* remoteIp */
                0x00, 0x00, /* remotePort */
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0x10, 0x00, 0x02, 0x00,
                0x00, 0x00, 0x00, 0x00, /* localIP */
                0x00, 0x00, /* localPort */
                0x08, 0x08, 0x08, 0x08, 
                0x08, 0x08, 0x08, 0x08, 
                0x00
            };
            Array.Copy(rep, 0, payload, 4, 6);
            Array.Copy(lep, 0, payload, 22, 6);

            // encrypt payload
            byte encKey = Keys.GameKey;
            for (int i = 0; i < payload.Length; i++)
                payload[i] ^= encKey;

            int rlen = response.Length;
            Array.Resize(ref response, response.Length + payload.Length);
            Array.Copy(payload, 0, response, rlen, payload.Length);

            return response;
        }

        public UpdateIP(Server.UDP.ManagedUDP server, byte[] data)
            : base(data)
        {
            this.Identity = Parity.Base.Enum.PacketUDP.UpdateIP;

            this.SessionId = BitConverter.ToInt16(
                Parity.Base.App.ArrayHelp.CutOut<byte>(Data, 4, 2).Reverse().ToArray(),
                0);

            ushort key16 = Keys.Key16(Keys.GameKeyC);
            uint key32 = Keys.Key32(Keys.GameKeyC);
            this.LocalEndPoint = new System.Net.IPEndPoint(
                BitConverter.ToUInt32(data, 34)^ key32,
                BitConverter.ToUInt16(new byte[] { data[33], data[32] }, 0) ^ key16);

        }

    }
}
