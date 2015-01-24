using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Parity.Net.Packet
{
    public class PacketReader
    {

        private static int indexOf(byte[] data, byte needle, int begin = 0)
        {
            for (int i = begin; i < data.Length; i++)
                if (data[i] == needle)
                    return i;
            return -1;
        }

        public static IEnumerable<InPacket> GetPackets(byte[] recv, byte encryptionKey, byte[] overlapped)
        {
            // decrypt recv
            for (int i = 0; i < recv.Length; i++)
                recv[i] ^= encryptionKey;

            byte delimeter = 0x0a;
            byte[][] segments = new byte[0][];
            int foundSegs = 0;
            int index = 0;
            do
            {
                int newIndex = indexOf(recv, delimeter, index);
                if (newIndex == -1 && // no more delimeter matches
                    index < (recv.Length - 1)) // and the last one was not the end
                {
                    overlapped = new byte[recv.Length - index];
                    Array.Copy(recv, index, overlapped, 0, overlapped.Length);
                    break;
                }
                else
                {
                    int start = index + foundSegs;
                    int len = newIndex - index;
                    Array.Resize(ref segments, segments.Length + 1);
                    segments[segments.Length - 1] = new byte[len];
                    Array.Copy(recv, start, segments[segments.Length - 1], 0, len);
                }
                index = newIndex + 1;
            } while (index < recv.Length);

            return segments.Where(x => x.Length > 1).Select(x => new InPacket(Encoding.UTF8.GetString(x)));
        }

    }
}
