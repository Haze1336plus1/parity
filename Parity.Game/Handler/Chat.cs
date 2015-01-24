using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Handler
{
    [Net.Handler.Requirements(true, false)]
    public class Chat : Net.Handler.IHandler<Client.Client>
    {

        protected System.Text.RegularExpressions.Regex rFindMessage;
        protected Dictionary<string, Commands.ICommand> CommandList;
        public static readonly char CommandKey = '/';

        public Chat()
        {
            this.rFindMessage = new System.Text.RegularExpressions.Regex(
                ".*?\\s{1,3}>>\\s{1,3}(?<message>.*)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // register chat commands
            this.CommandList = new Dictionary<string, Commands.ICommand>();
            var regCommand = new Action<Commands.ICommand>((Commands.ICommand cmd) => { this.CommandList.Add(cmd.Command, cmd); });

            regCommand(new Commands.Kick());
            regCommand(new Commands.Chat());
            regCommand(new Commands.PChat());
            regCommand(new Commands.Message());
            regCommand(new Commands.PMessage());
            regCommand(new Commands.Notice());
            regCommand(new Commands.Test());
        }

        protected bool HandleChat(Client.Client source, Base.Enum.ChatChannel chatChannel, Client.Client target, string message)
        {
            var chatMessage = Server.PacketFactory.Chat.Lobby(source, message, chatChannel);
            if (chatChannel == Base.Enum.ChatChannel.LobbyAll &&
                !source.GameSession.InRoom)
            {
                Management.Selection.ActiveClients()
                    .ForEach(x => x.Send(chatMessage));
            }
            else if (chatChannel == Base.Enum.ChatChannel.LobbyChannel &&
                !source.GameSession.InRoom)
            {
                Management.Selection.ClientsFromChannel(source.GameSession.Channel)
                    .ForEach(x => x.Send(chatMessage));
            }
            else if (chatChannel == Base.Enum.ChatChannel.RoomAll &&
                source.GameSession.InRoom)
            {
                source.GameSession.Room.Current.Players.Players()
                    .ForEach(x => x.Send(chatMessage));
            }
            else if (chatChannel == Base.Enum.ChatChannel.RoomTeam &&
                source.GameSession.InRoom)
            {
                source.GameSession.Room.Current.Players.Players()
                    .ForEach(x => x.Send(chatMessage));
            }
            else if (chatChannel == Base.Enum.ChatChannel.Whisper)
            {
                if (target != null)
                {
                    target.Send(Server.PacketFactory.Chat.Whisper(source, target, message));
                    source.Send(Server.PacketFactory.Chat.Whisper(source, target, message, true)); // self
                }
            }
            //source.Send(Server.PacketFactory.Custom.MessageBox("You wrote: " + message));
            return true;
        }
        protected bool HandleCommand(Client.Client source, string message)
        {
            string[] messageSplit = message.Split(' ');
            string command = ArrayExtension.Pop(ref messageSplit, true).ToLower();
            if (command == "help")
            {
                foreach (Commands.ICommand allowedCommand in this.CommandList
                    .Where(x => source.Session.Account.AuthLevel >= x.Value.RequiredLevel)
                    .Select(x => x.Value))
                {
                    source.Chat("/{0} {1}".Process(allowedCommand.Command, allowedCommand.Description));
                }
            }
            else if (this.CommandList.ContainsKey(command))
            {
                Commands.ICommand cmd = this.CommandList[command];
                if (source.Session.Account.AuthLevel >= cmd.RequiredLevel)
                {
                    if (!cmd.HandleCommand(
                        new Base.Layout.CommandInvoker(Base.Enum.CommandInvoker.Player, source),
                        command,
                        messageSplit))
                    {
                        source.Chat("Try these arguments: {0}".Process(cmd.Description));
                    }
                }
            }
            else
                source.Chat("Command '{0}' not found :(".Process(command));
            return true;
        }

        public override Net.Handler.Result Handle(Client.Client sender, Net.Packet.InPacket packet, bool customHandle = false)
        {
            base.Handle(sender, packet, customHandle);
            Base.Enum.ChatChannel chatChannel = default(Base.Enum.ChatChannel);
            short targetSession;
            string targetNickname;
            string message;

            if (packet.ParamsCount == 4 &&
                Base.Types.ParseEnum(packet[0], out chatChannel) &&
                Base.Types.TryParse(packet[1], out targetSession) &&
                Base.Types.TryParse(packet[2], out targetNickname) &&
                Base.Types.TryParse(packet[3], out message))
            {
                System.Text.RegularExpressions.Match textMatch = this.rFindMessage.Match(message);
                if (textMatch.Success)
                {
                    string cmessage = textMatch.Groups["message"].Value;
                    bool returnValue = false;
                    Client.Client target = null;

                    if (chatChannel == Base.Enum.ChatChannel.Whisper)
                    {
                        string searchName = targetNickname.ToLower();
                        target = Management.Selection.FindOne(x => x.Session.Account.Nickname.ToLower() == searchName).FirstOrDefault();
                    }

                    // handle stuff here
                    if (cmessage.Length > 1 && cmessage[0] == Chat.CommandKey)
                        returnValue = this.HandleCommand(sender, cmessage.Substring(1));
                    else
                        returnValue = this.HandleChat(sender, chatChannel, target, cmessage);

                    if (chatChannel == Base.Enum.ChatChannel.Whisper)
                    {

                        if (target != sender &&
                            target != null)
                            QA.Core.ChatLog.Write(String.Format("[{0}] {4}:'{1}'->{5}:'{3}' >> '{2}'", returnValue.ToString().ToLower(), sender.Session.Account.Nickname, cmessage, targetNickname, sender.Session.Account.Id, target.Session.Account.Id));
                        else if (target == null)
                        {
                            sender.Send(Server.PacketFactory.Chat.System("User '" + targetNickname + "' is not online"));
                            QA.Core.ChatLog.Write(String.Format("[{0}] {4}:'{1}'->'{3}' (offline) >> '{2}'", returnValue.ToString().ToLower(), sender.Session.Account.Nickname, cmessage, targetNickname, sender.Session.Account.Id));
                            return Net.Handler.Result.Success;
                        }
                        else if (target == sender)
                        {
                            QA.Core.ChatLog.Write(String.Format("[{0}] {3}:'{1}'->(loobpack) >> '{2}'", returnValue.ToString().ToLower(), sender.Session.Account.Nickname, cmessage, sender.Session.Account.Id));
                            return Net.Handler.Result.Success;
                        }
                    }
                    else
                        QA.Core.ChatLog.Write(String.Format("[{0}] {3}:'{1}' >> '{2}'", returnValue.ToString().ToLower(), sender.Session.Account.Nickname, cmessage, sender.Session.Account.Id));

                    if (returnValue)
                        return Net.Handler.Result.Success;
                }
            }
            return Net.Handler.Result.Default;
        }
    }
}
