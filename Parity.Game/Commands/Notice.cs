using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Commands
{
    public class Notice : ICommand
    {
        public string Command
        {
            get { return "notice"; }
        }

        public string Description
        {
            get { return "<message ...>"; }
        }

        public Base.Enum.AuthLevel RequiredLevel
        {
            get { return Base.Enum.AuthLevel.Administrator; }
        }

        public bool HandleCommand(Base.Layout.CommandInvoker invoker, string command, string[] arguments)
        {
            if (arguments.Length > 0)
            {
                var noticeMessage = Server.PacketFactory.Chat.Notice(String.Join(" ", arguments));
                foreach (Client.Client client in Management.Selection.ActiveClients())
                    client.Send(noticeMessage);
                return true;
            }
            else
                return false;
        }
    }
}
