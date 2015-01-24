using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace PacketSniffer.Header
{
    public class TCP
    {
        //TCP header fields
        public readonly ushort usSourcePort;              //Sixteen bits for the source port number
        public readonly ushort usDestinationPort;         //Sixteen bits for the destination port number
        public readonly uint uiSequenceNumber = 555;          //Thirty two bits for the sequence number
        public readonly uint uiAcknowledgementNumber = 555;   //Thirty two bits for the acknowledgement number
        public readonly ushort usDataOffsetAndFlags = 555;      //Sixteen bits for flags and data offset
        public readonly ushort usWindow = 555;                  //Sixteen bits for the window size
        public readonly short sChecksum = 555;                 //Sixteen bits for the checksum
        //(checksum can be negative so taken as short)
        public readonly ushort usUrgentPointer;           //Sixteen bits for the urgent pointer
        //End TCP header fields

        public readonly byte byHeaderLength;            //Header length
        public readonly ushort usMessageLength;           //Length of the data being carried
        public readonly byte[] byTCPData = new byte[4096];//Data carried by the TCP packet

        public TCP(byte[] byBuffer, int nReceived)
        {
            MemoryStream memoryStream = new MemoryStream(byBuffer, 0, nReceived);
            BinaryReader binaryReader = new BinaryReader(memoryStream);

            //The first sixteen bits contain the source port
            usSourcePort = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            //The next sixteen contain the destiination port
            usDestinationPort = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            //Next thirty two have the sequence number
            uiSequenceNumber = (uint)IPAddress.NetworkToHostOrder(binaryReader.ReadInt32());

            //Next thirty two have the acknowledgement number
            uiAcknowledgementNumber = (uint)IPAddress.NetworkToHostOrder(binaryReader.ReadInt32());

            //The next sixteen bits hold the flags and the data offset
            usDataOffsetAndFlags = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            //The next sixteen contain the window size
            usWindow = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            //In the next sixteen we have the checksum
            sChecksum = (short)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            //The following sixteen contain the urgent pointer
            usUrgentPointer = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            //The data offset indicates where the data begins, so using it we
            //calculate the header length
            byHeaderLength = (byte)(usDataOffsetAndFlags >> 12);
            byHeaderLength *= 4;

            //Message length = Total length of the TCP packet - Header length
            usMessageLength = (ushort)(nReceived - byHeaderLength);

            //Copy the TCP data into the data buffer
            Array.Resize(ref this.byTCPData, nReceived - byHeaderLength);
            Array.Copy(byBuffer, byHeaderLength, byTCPData, 0, this.byTCPData.Length);
        }
    }
}
