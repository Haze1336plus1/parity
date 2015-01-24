using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Auth.Client
{
    public class Controller
    {

        public readonly Client Owner;

        public Controller(Client owner)
        {
            this.Owner = owner;
        }

        public Base.Code.Login TryLogin(string username, string password)
        {
            var serverCore = Server.Core.GetInstance();
            DS.Model dbmodel = QA.GetDBModel();
            DS.Account targetAccount = dbmodel.Account(username, false);
            if (targetAccount != null)
            {
                if (targetAccount.Password == Base.App.MD5.Hash(password))
                {
                    Base.Layout.AccountSession newSession = targetAccount.CreateSession(dbmodel);
                    if (newSession.AccountId > 0)
                    {
                        this.Owner.Session.SetDetails(targetAccount, newSession);
                        return Base.Code.Login.Success;
                    }
                    return Base.Code.Login.Already_Logged_In;
                }
                return Base.Code.Login.Invalid_Password;
            }
            return Base.Code.Login.Unregistered_User;
        }

    }
}
