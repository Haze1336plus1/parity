using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Commands
{
    public class Test : ICommand
    {
        public string Command
        {
            get { return "test"; }
        }

        public string Description
        {
            get { return "[roomName...]"; }
        }

        public Base.Enum.AuthLevel RequiredLevel
        {
            get { return Base.Enum.AuthLevel.User; }
        }

        public bool HandleCommand(Base.Layout.CommandInvoker invoker, string command, string[] arguments)
        {
            if (invoker.Type == Base.Enum.CommandInvoker.Player)
            {
                Client.Client pInvoker = (invoker.Invoker as Client.Client);
                string roomName = String.Join(" ", arguments).Fallback("Default room Name");
                // null as Client.Client, not good
                Game.Room createdRoom = new Game.Room(Base.Enum.Game.Type.Normal, new Game.Details(roomName, password: "magic"), null);
                Modules.RoomStorage[(int)Base.Enum.Channel.CQC].Register(createdRoom);
                pInvoker.Send(new Packet.Custom().Chat("TEST!", new Color(0, 255, 0)));
                pInvoker.Chat("Created Room #{0} ({1})".Process(createdRoom.Id, createdRoom.Details.Name));
                
            }
            return true;
        }
    }
}
