using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Commands
{
    public class PChat : ICommand
    {
        public string Command
        {
            get { return "pchat"; }
        }

        public string Description
        {
            get { return "<nickname> <message ...>"; }
        }

        public Base.Enum.AuthLevel RequiredLevel
        {
            get { return Base.Enum.AuthLevel.Moderator; }
        }

        public bool HandleCommand(Base.Layout.CommandInvoker invoker, string command, string[] arguments)
        {
            if (arguments.Length > 1)
            {
                string playerName = ArrayExtension.Pop(ref arguments, true).ToLower();
                string message = String.Join(" ", arguments);

                Client.Client targetPlayer = Management.Selection.FindOne(x => x.Session.Account.Nickname.ToLower().Equals(playerName)).FirstOrDefault();
                if (targetPlayer == default(Client.Client))
                {
                    string reply = "Player '" + targetPlayer.Session.Account.Nickname + "' not found";
                    if (invoker.Type == Base.Enum.CommandInvoker.Player)
                        ((Client.Client)invoker.Invoker).Chat(reply);
                    else
                        QA.GetLog()["command"].Write(reply, this.Command);
                }
                else
                {
                    targetPlayer.Chat(message);

                    string reply = "Notified player '" + targetPlayer.Session.Account.Nickname + "' (chat)";
                    if (invoker.Type == Base.Enum.CommandInvoker.Player)
                        ((Client.Client)invoker.Invoker).Chat(reply);
                    QA.GetLog()["command"].Write(reply, this.Command);
                }

                return true;
            }
            else
                return false;
        }
    }
}
