using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Auth.Client
{
    public class SessionDetails
    {

        public readonly Client Owner;

        public DS.Account Account { get; private set; }
        public Base.Layout.AccountSession Session { get; private set; }

        public SessionDetails(Client owner)
        {
            this.Owner = owner;
            this.Session = new Base.Layout.AccountSession();
        }

        public void SetDetails(DS.Account account, Base.Layout.AccountSession session)
        {
            this.Account = account;
            this.Session = session;
        }

    }
}
