using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace PacketSniffer.Header
{
    public class IP
    { 
      //IP Header fields 
        public readonly byte byVersionAndHeaderLength; // Eight bits for version and header 
                                             // length 
        public readonly byte byDifferentiatedServices; // Eight bits for differentiated 
                                             // services
        public readonly ushort usTotalLength;          // Sixteen bits for total length 
        public readonly ushort usIdentification;       // Sixteen bits for identification
        public readonly ushort usFlagsAndOffset;       // Eight bits for flags and frag. 
                                             // offset 
        public readonly byte byTTL;                    // Eight bits for TTL (Time To Live) 
        public readonly byte byProtocol;               // Eight bits for the underlying 
                                             // protocol 
        public readonly short sChecksum;               // Sixteen bits for checksum of the 
                                             //  header 
        public readonly uint uiSourceIPAddress;        // Thirty two bit source IP Address 
        public readonly uint uiDestinationIPAddress;   // Thirty two bit destination IP Address 

      //End IP Header fields   
        public readonly byte byHeaderLength;             //Header length 
        public readonly byte[] byIPData = new byte[4096]; //Data carried by the datagram
        public IP(byte[] byBuffer)
        {
            //Create MemoryStream out of the received bytes
            MemoryStream memoryStream = new MemoryStream(byBuffer, 0, byBuffer.Length);

            //Next we create a BinaryReader out of the MemoryStream
            BinaryReader binaryReader = new BinaryReader(memoryStream);

            //The first eight bits of the IP header contain the version and
            //header length so we read them
            byVersionAndHeaderLength = binaryReader.ReadByte();

            //The next eight bits contain the Differentiated services
            byDifferentiatedServices = binaryReader.ReadByte();

            //Next eight bits hold the total length of the datagram
            usTotalLength =
                     (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            //Next sixteen have the identification bytes
            usIdentification =
                      (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            //Next sixteen bits contain the flags and fragmentation offset
            usFlagsAndOffset =
                      (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            //Next eight bits have the TTL value
            byTTL = binaryReader.ReadByte();

            //Next eight represent the protocol encapsulated in the datagram
            byProtocol = binaryReader.ReadByte();

            //Next sixteen bits contain the checksum of the header
            sChecksum = IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

            //Next thirty two bits have the source IP address
            uiSourceIPAddress = (uint)(binaryReader.ReadInt32());

            //Next thirty two hold the destination IP address
            uiDestinationIPAddress = (uint)(binaryReader.ReadInt32());

            //Now we calculate the header length
            byHeaderLength = byVersionAndHeaderLength;

            //The last four bits of the version and header length field contain the
            //header length, we perform some simple binary arithmetic operations to
            //extract them
            byHeaderLength <<= 4;
            byHeaderLength >>= 4;

            //Multiply by four to get the exact header length
            byHeaderLength *= 4;

            //Copy the data carried by the datagram into another array so that
            //according to the protocol being carried in the IP datagram
            Array.Copy(byBuffer,
                       byHeaderLength, //start copying from the end of the header
                       byIPData, 0, usTotalLength - byHeaderLength);
        }
    }
}
