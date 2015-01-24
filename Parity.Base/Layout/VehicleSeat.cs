using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Base.Layout
{
    public struct VehicleSeat
    {

        public string MainCode;
        public string SubCode;

        public int MainCT;
        public int MainCTx;

        public int SubCT;
        public int SubCTx;

        public VehicleSeat(string mainCode, string subCode, int mainCT, int subCT, int mainCTx, int subCTx)
        {
            this.MainCode = mainCode;
            this.SubCode = subCode;
            this.MainCT = mainCT;
            this.SubCT = subCT;
            this.MainCTx = mainCTx;
            this.SubCTx = subCTx;
        }

        public override string ToString()
        {
            return String.Format("([{0}:{1},{2}], [{3}:{4},{5}])",
                this.MainCode, this.MainCT, this.MainCTx,
                this.SubCode, this.SubCT, this.SubCTx);
        }

    }
}
