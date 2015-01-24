using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Commands
{
    public class Kick : ICommand
    {
        public string Command
        {
            get { return "kick"; }
        }

        public string Description
        {
            get { return "<nickname> [reason ...]"; }
        }

        public Base.Enum.AuthLevel RequiredLevel
        {
            get { return Base.Enum.AuthLevel.Moderator; }
        }

        public bool HandleCommand(Base.Layout.CommandInvoker invoker, string command, string[] arguments)
        {
            if (arguments.Length > 0)
            {
                string playerName = ArrayExtension.Pop(ref arguments, true).ToLower();
                string reason = String.Join(" ", arguments).Fallback("No reason given");
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
                    if (invoker.Type == Base.Enum.CommandInvoker.Server ||
                        (invoker.Type == Base.Enum.CommandInvoker.Player &&
                        ((Client.Client)invoker.Invoker).Session.Account.AuthLevel > targetPlayer.Session.Account.AuthLevel))
                    {
                        targetPlayer.Send(Server.PacketFactory.Custom.MessageBox("You got kicked: " + reason));
                        targetPlayer.Disconnect("Kicked by " + invoker.Type.ToString());
                    }
                    else
                        ((Client.Client)invoker.Invoker).Chat("Player '" + targetPlayer.Session.Account.Nickname + "' has same or greater privileges");
                    QA.GetLog()["command"].Write("Kicked '" + playerName + "': '" + reason + "'", this.Command);
                }
                return true;
            }
            return false;
        }
    }
}
