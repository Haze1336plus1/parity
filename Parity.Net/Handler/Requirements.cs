using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Net.Handler
{
    [AttributeUsage(AttributeTargets.Class)]
    public class Requirements : Attribute
    {

        public bool Login { get; private set; }
        public bool InRoom { get; private set; }

        public Requirements(bool login = true, bool inRoom = false)
        {
            this.Login = login;
            this.InRoom = inRoom;
        }

    }
}
