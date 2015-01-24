using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Game.Management.Room
{
    public class StorageChannel
    {

        public readonly Storage Owner;
        public readonly Base.Enum.Channel Channel;
        public Game.Room[] @Room { get; private set; }
        public Game.Room this[int index] { get { return this.@Room[index]; } }
        public int RoomCount { get; private set; }
        public int RoomLimit { get { return this.@Room.Length; } }

        public IEnumerable<Game.Room> All()
        {
            return (from Game.Room r in this.@Room where r != null select r);
        }
        /*public IEnumerable<Game.Room> AllNotEmpty()
        {
            return (from Game.Room r in this.@Room where r != null select r);
        }*/

        public StorageChannel(Storage owner, Base.Enum.Channel channel, short roomCount)
        {
            this.Owner = owner;
            this.Channel = channel;
            this.@Room = new Game.Room[roomCount];
        }

        public short Register(Game.Room room)
        {
            for (short i = 0; i < this.@Room.Length; i++)
            {
                if (this.@Room[i] == null)
                {
                    this.@Room[i] = room;
                    room.Initialize(i, this.Channel);
                    this.RoomCount++;
                    this.Owner.Share(room, Base.Enum.RoomInfoChangeAction.Created);
                    return i;                    
                }
            }
            return -1;
        }
        public void Delete(Game.Room room)
        {
            int index = Array.IndexOf(this.@Room, room);
            if (index >= 0)
            {
                this.@Room[index] = null;
                this.RoomCount--;
                this.Owner.Share(room, Base.Enum.RoomInfoChangeAction.Deleted);
            }
        }

    }
}
