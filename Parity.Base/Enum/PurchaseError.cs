using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Base.Enum
{
    public enum PurchaseError
    {

        Success = 1,
        NoPremium = 98010,
        CantBuy = 97020,
        NotEnoughDinar = 97040,
        Invalid = 97010,
        InventoryFull = 97070,
        MaximumTimeLimit = 97100,
        LowLevel = 97060

    }
}
