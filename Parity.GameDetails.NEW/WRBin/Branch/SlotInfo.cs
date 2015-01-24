using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.GameDetails.WRBin.Branch
{
    public struct SlotInfo
    {

        public readonly string Code;
        public readonly Base.Enum.Item.Weapon[] Allowed;

        public SlotInfo(string code, Base.Enum.Item.Weapon[] allowed)
        {
            this.Code = (code.Fallback("0") == "0" ? null : code);
            this.Allowed = allowed;
        }

    }
}
