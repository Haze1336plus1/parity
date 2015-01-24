using System;
using System.Collections.Generic;
using System.Text;

namespace Parity.Net.Packet
{
    public abstract class OutPacket : Base
    {

        public void Add(object mBlock)
        {
            mBlock = mBlock ?? "";
            base._Params.Add(mBlock.ToString());
        }
        public void AddFew(params object[] mBlocks)
        {
            for(int iIndex = 0; iIndex < mBlocks.Length; iIndex ++)
            {
                string b = string.Empty;
                if (mBlocks[iIndex] != null)
                    b = mBlocks[iIndex].ToString();
                base._Params.Add(b.ToString());
            }
        }

        public OutPacket(int _Operationcode, params object[] _Blocks) : base()
        {
            base._Operationcode = _Operationcode;
            if (_Blocks != null && _Blocks.Length > 0)
            {
                this.Add(_Blocks);
            }
        }

    }
}
