using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Auth.Handler
{
    public class Login : Net.Handler.IHandler<Client.Client>
    {

        protected Net.Handler.Result HandleLogin(Client.Client sender, string username, string password, string guid)
        {
            Base.IO.Debug("Login '{0}' with GUID {1}".Process(username, guid));
            Base.Code.Login errorCode = sender.AccountController.TryLogin(username, password);
            if (errorCode == Base.Code.Login.Success)
            {
                sender.Send(new Packet.ServerList(sender.Session));
                return new Net.Handler.Result();
            }
            sender.Send(new Packet.Login(errorCode));
            return new Net.Handler.Result("Login failed: " + errorCode.ToString());
        }

        public override Net.Handler.Result Handle(Client.Client sender, Net.Packet.InPacket packet, bool customHandle = false)
        {
            base.Handle(sender, packet, customHandle);

            string authstring = string.Empty;
            string guid = string.Empty;

            if (base.RequireLength(6, 7) &&
                base.RequireType<uint>(0) &&
                base.RequireExact(1, "0") &&
                base.RequireValue(2, out guid) &&
                base.RequireValue(3, out authstring))
            {
                string[] authsplit = authstring.Split(':');
                if (authsplit.Length == 2 && guid.Length == 32)
                {
                    string username = authsplit[0];
                    string password = authsplit[1];
                    return this.HandleLogin(sender, username, password, guid);
                }
                return new Net.Handler.Result("authstring.Length == {0}, guid.Length == {1}".Process(authsplit.Length, guid.Length));
            }
            return Net.Handler.Result.Default;
        }

    }
}
