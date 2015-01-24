using System;
using System.Collections.Generic;
using System.Text;

namespace Parity.Net.Packet
{
    public class InPacket : Base
    {

        private bool _UnpackingSuccess;
        public bool UnpackingSuccess
        {
            get { return this._UnpackingSuccess; }
        }

        private int _Timestamp;
        public int Timestamp
        {
            get { return this._Timestamp; }
        }

        public string Block(int iIndex)
        {
            return base._Params[iIndex];
        }
        public string[] Params()
        {
            return this._Params.ToArray();
        }

        public int ParamsCount
        {
            get { return base._Params.Count; }
        }

        public InPacket(string RawData) : base()
        {
            bool mSuccess = false;
            string[] mArrSplit = RawData.Split(Convert.ToChar(0x20));
            //check for at least 2 blocks, numeric and opcode greater than 0
            /*if (mArrSplit.Length > 1 && WarField.Gear.DataTypes.IsInt64(mArrSplit[0]) && WarField.Gear.DataTypes.IsUInt16(mArrSplit[1]) && uint.Parse(mArrSplit[1]) > 0)*/
            {
                this._Timestamp = 0;
                base._Operationcode = 0;

                if (Parity.Base.Types.TryParse(mArrSplit[0], out this._Timestamp) &&
                    Parity.Base.Types.TryParse(mArrSplit[1], out this._Operationcode))
                {
                    //Gear.Tools.Array<string>.ArrayCut(ref mArrSplit, mArrSplit.Length - 2, true);
                    //Gear.Tools.Array<string>.ArrayCut(ref mArrSplit, mArrSplit.Length - 1);

                    mArrSplit = Parity.Base.App.ArrayHelp.Slice<string>(mArrSplit, -1, 1);
                    mArrSplit = Parity.Base.App.ArrayHelp.Slice<string>(mArrSplit, 0, 2);

                    foreach (string b in mArrSplit)
                    {
                        base._Params.Add(b.Replace('\x1D', '\x20'));
                    }
                    mSuccess = true;
                }
            }
            this._UnpackingSuccess = mSuccess;
        }

        public string this[int Index]
        {
            get { return this._Params[Index]; }
        }

    }
}
