using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Parity.WRDevTool.FCLD
{

    [StructLayout(LayoutKind.Sequential, Size = 16, CharSet = CharSet.Ansi)]
    public struct FCLDHeader
    {

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public char[] Format;
        public uint FFTag; // not required
        public float Version;
        public int EntryCount;

        public FCLDHeader(FCLDHeader prev, int entryCount)
        {
            this.Format = prev.Format;
            this.FFTag = prev.FFTag;
            this.Version = prev.Version;
            this.EntryCount = entryCount;
        }

    }

    public struct FCLDEntry
    {
        public string FileName;
        public DateTime ModifyTime;
        public uint FileSize;
        public uint FileChecksum;

        public FCLDEntry(string fileName, DateTime modifyTime, uint fileSize, uint fileChecksum)
        {
            this.FileName = fileName;
            this.ModifyTime = modifyTime;
            this.FileSize = fileSize;
            this.FileChecksum = fileChecksum;
        }
    }

    public class FileCodeListDocument
    {

        public static unsafe uint ChecksumData(byte[] data)
        {
            uint checksum = 0;
            byte[] newBytes = new byte[data.Length + (4 - data.Length % 4)];
            int stepSize = newBytes.Length / 4;
            Array.Copy(data, 0, newBytes, 0, data.Length);
            fixed (byte* dataPtr = &newBytes[0])
            {
                uint* dataPtrU = (uint*)dataPtr;
                for (int i = 0; i < stepSize; i++)
                    checksum ^= (uint)(dataPtrU[i]);
            }
            return checksum;
        }

        public FileCodeListDocument(string FileName)
        {
            this.Content = System.IO.File.ReadAllBytes(FileName);
            this._Entrys = new List<FCLDEntry>();
        }

        public FileCodeListDocument(byte[] Data)
        {
            this.Content = Data;
            this._Entrys = new List<FCLDEntry>();
        }

        public byte[] Content { get; private set; }
        public FCLDHeader Header { get; private set; }
        protected List<FCLDEntry> _Entrys;
        public FCLDEntry[] Entrys { get { return this._Entrys.ToArray(); } }

        public void RegisterEntry(FCLDEntry iEntry)
        {
            this._Entrys.Add(iEntry);
        }
        public byte[] Serialize()
        {
            byte[] returnValue = null;

            using(System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            using (System.IO.BinaryWriter writer = new System.IO.BinaryWriter(memoryStream))
            {
                this.Header = new FCLDHeader(this.Header, this._Entrys.Count);
                writer.Write(System.Text.Encoding.UTF8.GetBytes(this.Header.Format));
                writer.Write(this.Header.FFTag);
                writer.Write(this.Header.Version);
                writer.Write(this.Header.EntryCount);

                foreach (FCLDEntry iEntry in this._Entrys)
                {
                    writer.Write(iEntry.FileName.Length);
                    writer.Write(System.Text.Encoding.ASCII.GetBytes(iEntry.FileName.ToUpper()));
                    writer.Write((byte)0);
                    writer.Write(iEntry.ModifyTime.ToFileTime());
                    writer.Write(iEntry.FileSize);
                    writer.Write(iEntry.FileChecksum);
                }
                
                // commit data, close stream, set return value
                writer.Flush();
                int memoryLength = (int)memoryStream.Length;
                writer.Close();
                returnValue = memoryStream.GetBuffer();
                Array.Resize(ref returnValue, memoryLength);
            }
            return returnValue;
        }

        public void Process()
        {
            int headerSize = Marshal.SizeOf(typeof(FCLDHeader));
            {
                IntPtr headerPtr = Marshal.AllocHGlobal(headerSize);
                Marshal.Copy(this.Content, 0, headerPtr, headerSize);
                this.Header = (FCLDHeader)Marshal.PtrToStructure(headerPtr, typeof(FCLDHeader));
                Marshal.FreeHGlobal(headerPtr);
            }
            using (System.IO.BinaryReader bReader = new System.IO.BinaryReader(new System.IO.MemoryStream(this.Content)))
            {
                bReader.BaseStream.Seek(headerSize, System.IO.SeekOrigin.Begin);

                for (int iIndex = 0; iIndex < this.Header.EntryCount; iIndex++)
                {
                    string fileName = Encoding.ASCII.GetString(bReader.ReadBytes(bReader.ReadInt32()));
                    bReader.ReadByte(); // null termination?
                    long fileTime = bReader.ReadInt64();
                    this._Entrys.Add(new FCLDEntry(
                        fileName,
                        DateTime.FromFileTime(fileTime),
                        bReader.ReadUInt32(),
                        bReader.ReadUInt32()
                    ));
                }
            }
        }
    }
}
