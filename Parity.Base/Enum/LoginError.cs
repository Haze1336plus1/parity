using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Base.Enum
{
    public enum LoginError : byte
    {

        Success,

        NonExistent,
        InvalidPassword,
        InvalidSession,
        AlreadyLoggedIn

    }
}
