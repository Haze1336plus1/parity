using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Base.Enum.Game
{
    public enum TimeLimit
    {

        Unlimited = -1,
        None,
        //m%d = %d minutes
        m10,
        m20,
        m30,
        m40,
        m60

    }
}
