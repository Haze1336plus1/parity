using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Game.Management.Room
{
    public class Storage
    {

        public StorageChannel[] Channel { get; private set; }
        public StorageChannel this[int index] { get { return this.Channel[index]; } }

        public Storage()
        {
            this.Channel = new StorageChannel[Enum.GetValues(typeof(Base.Enum.Channel)).Length];
            for (int i = 1; i < this.Channel.Length; i++) // 0th item is channel None, which shouldn't be playable.
                this.Channel[i] = new StorageChannel(this, (Base.Enum.Channel)i, (short)(int)Base.Compile.GameDefaults["Lobby.RoomLimit"]);

            // create Channel.None dimension with 0 capacity
            this.Channel[0] = new StorageChannel(this, Base.Enum.Channel.None, 0);
        }

        public void Share(Game.Room targetRoom, Base.Enum.RoomInfoChangeAction action)
        {
            var changeNotification = Server.PacketFactory.RoomInfoChange(targetRoom.Id, action, targetRoom);
            var selection = Management.Selection.FindOne((x => !x.GameSession.InRoom && x.GameSession.Channel == targetRoom.Channel), true);
            foreach (Client.Client player in selection)
                player.Send(changeNotification);

        }

    }
}
