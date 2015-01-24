using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Auth.Client.Remote
{
    public class SessionDetails
    {

        public readonly Client Owner;

        public SessionDetails(Client owner)
        {
            this.Owner = owner;
        }

    }
}
