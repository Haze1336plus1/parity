using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Game.Handler
{
    [Net.Handler.Requirements(false, false)]
    public class Greeting : Net.Handler.IHandler<Client.Client>
    {

        protected System.Text.RegularExpressions.Regex rmacAddress;

        public Greeting()
        {
            this.rmacAddress = new System.Text.RegularExpressions.Regex("^[a-zA-Z0-9]{12}$");
        }

        protected bool Check(string receiveChecksum, uint gameVersion, string macAddress)
        {
            if (receiveChecksum == "dla#qud$wlr%aks^tp&" && 
                gameVersion == Base.Compile.GameVersion && 
                this.rmacAddress.IsMatch(macAddress))
            {
                return !QA.GetDBModel().IsMacAddressBanned(macAddress);
            }
            return false;
        }

        public override Net.Handler.Result Handle(Client.Client sender, Net.Packet.InPacket packet, bool customHandle = false)
        {
            base.Handle(sender, packet, customHandle);

            if (sender.Session.IsActive)
                return Net.Handler.Result.Default;

            string receiveChecksum;
            uint gameVersion;
            string macAddress;

            if (packet.ParamsCount == 3 &&
                Base.Types.TryParse(packet[0], out receiveChecksum) &&
                Base.Types.TryParse(packet[1], out gameVersion) &&
                Base.Types.TryParse(packet[2], out macAddress))
            {

                if (this.Check(receiveChecksum, gameVersion, macAddress))
                {
                    sender.Session.SetMacAddress(macAddress);
                    sender.Send(Server.PacketFactory.Greeting());
                    return Net.Handler.Result.Success;
                }
            }

            return Net.Handler.Result.Default;
        }

    }
}
