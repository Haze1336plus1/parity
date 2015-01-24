using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Commands
{
    public class Chat : ICommand
    {
        public string Command
        {
            get { return "chat"; }
        }

        public string Description
        {
            get { return "<message ...>"; }
        }

        public Base.Enum.AuthLevel RequiredLevel
        {
            get { return Base.Enum.AuthLevel.Moderator; }
        }

        public bool HandleCommand(Base.Layout.CommandInvoker invoker, string command, string[] arguments)
        {
            if (arguments.Length > 0)
            {
                var systemChat = Server.PacketFactory.Chat.System(String.Join(" ", arguments));
                foreach (Client.Client client in Management.Selection.ActiveClients())
                    client.Send(systemChat);
                return true;
            }
            else
                return false;
        }
    }
}
