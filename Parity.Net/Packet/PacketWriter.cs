using System;
using System.Collections.Generic;
using System.Text;

namespace Parity.Net.Packet
{
    public class PacketWriter
    {

        public static byte[] WritePacket(OutPacket mPacket, byte Key)
        {
            List<byte> mBytes = new List<byte>();
            foreach (byte mB in mPacket.ToString().ToCharArray())
                mBytes.Add((byte)(mB ^ Key));
            return mBytes.ToArray();
        }

        public static byte[] WritePackets(OutPacket[] iPackets, byte Key)
        {
            List<byte> mBytes = new List<byte>();
            foreach (OutPacket mPacket in iPackets)
            {
                foreach (byte mB in mPacket.ToString().ToCharArray())
                    mBytes.Add((byte)(mB ^ Key));
            }
            return mBytes.ToArray();
        }

    }
}
