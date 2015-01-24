using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Parity.Net.Packet
{
    public abstract class Base
    {

        protected int _Operationcode;
        public int Operationcode
        {
            get { return this._Operationcode; }
        }

        protected List<string> _Params;

        public Base()
        {
            this._Operationcode = default(int);
            this._Params = new List<string>();
        }

        public override string ToString()
        {
            return String.Join(" ", new string[] {
                    Environment.TickCount.ToString(),
                    this._Operationcode.ToString()
                }
                .Concat(
                    this._Params.Select(b => b.Replace('\x20', '\x1D'))
                )
            ) + "\n";
        }
        public string Print()
        {
            string mPacket = String.Concat(this._Operationcode, " ");
            foreach (string iBlock in this._Params)
                mPacket += iBlock.Replace('\x20', '\x1D') + " ";
            mPacket = mPacket.Substring(0, mPacket.Length - 1);
            return mPacket;
        }

        /*
        public virtual string GetPacket()
        {
            StringBuilder packetBuilder = new StringBuilder();
            packetBuilder.AppendFormat("{0} {1} ", Environment.TickCount, this._Operationcode);
            foreach (string iBlock in this._Blocks)
            {
                packetBuilder.AppendFormat(iBlock);
                packetBuilder.Append(' ');
            }
            packetBuilder.Remove(packetBuilder.Length - 1, 1);
            packetBuilder.Append('\n');
            return packetBuilder.ToString();
        }
        */

    }
}
