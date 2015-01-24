using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Base.Layout
{
    public struct AccountSession
    {

        public int AccountId;
        public int SessionKey;

        public AccountSession(int AccountId, int SessionKey)
        {
            this.AccountId = AccountId;
            this.SessionKey = SessionKey;
        }

        public static AccountSession CreateSession(int AccountId)
        {
            return new AccountSession(AccountId, App.Random.R.Next(100000000, 999999999));
        }

    }
}
