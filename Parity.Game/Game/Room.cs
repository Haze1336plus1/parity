using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Game.Game
{
    public class Room
    {

        public int Id { get; private set; }
        public Base.Enum.Channel Channel { get; private set; }
        public Base.Enum.Game.Type Type { get; private set; }

        public Details @Details { get; private set; }
        public PlayerContainer Players { get; private set; }
        public Mode Data { get; protected set; }

        public Room(Base.Enum.Game.Type type, Details details, Client.Client master)
        {
            this.Id = -1;
            this.Type = type;
            this.Details = details;
            this.Players = new PlayerContainer(this);
            this.Players.Initialize(master);
        }

        public void UpdateMode()
        {
            this.Data = Mode.GetMode(this, this.Details.Mode);
            this.Details.MapId = Mode.GetDefaultMap(this.Details.Mode);
        }

        internal void Initialize(int id, Base.Enum.Channel channel)
        {
            this.Id = id;
            this.Channel = channel;
            this.UpdateMode();
        }

        public void Append(Net.Packet.OutPacket packet)
        {
            packet.Add(this.Id);
            packet.Add((int)this.Details.Status);
            packet.Add((int)this.Details.Status);
            packet.Add(this.Players.MasterIndex); // masterSeat
            packet.Add(this.Details.Name);
            packet.Add(this.Details.HasPassword ? 1 : 0);
            packet.Add(this.Players.PlayerLimit); // playerLimit
            packet.Add(this.Players.Count); // playerCount
            packet.Add(this.Details.MapId);
            packet.Add(0); // modeSetting1
            packet.Add(0); // modeSetting2
            packet.Add((int)this.Details.TimeLimit);

            packet.Add((int)this.Details.Mode);
            packet.Add(4); // wat
            if (this.Details.Status == Base.Enum.Game.Status.Playing_Denied || this.Players.PlayerLimitLocked || this.Players.Count == this.Players.PlayerLimit)
                packet.Add(0); // unjoinable
            else
                packet.Add(1); // joinable
#if !TEST_
            packet.Add(this.Players.Master.Session.Inventory.HasItem("CC02") ? 1 : 0);
#else
            packet.Add((this.Players.Master != null && this.Players.Master.Session.Inventory.HasItem("CC02")) ? 1 : 0);
#endif
            packet.Add(4); // wtf again
            packet.Add((int)this.Type);
            packet.Add((int)this.Details.LevelLimit);
            packet.Add(this.Details.PremiumOnly ? 1 : 0);
            packet.Add(0); // votekick
            packet.Add(0); // autostart
            packet.Add(100); // average ping
            packet.Add((int)this.Details.PingLimit);
            packet.Add(-1);

            /*packet.Add(this.Id);
            packet.Add((int)this.Details.Status);
            packet.Add((int)this.Details.Status);
            packet.Add(this.Players.MasterIndex); // masterSeat
            packet.Add(this.Details.Name);
            packet.Add(this.Details.HasPassword ? 1 : 0);
            packet.Add(this.Players.PlayerLimit); // playerLimit
            packet.Add(this.Players.Count); // playerCount
            packet.Add(this.Details.MapId);
            packet.Add(this.Data.Setting1); // modeSetting1
            packet.Add(this.Data.Setting2); // modeSetting2
            packet.Add((int)this.Details.TimeLimit);

            packet.Add((int)this.Details.Mode);
            packet.Add(0); // wat
            if (this.Details.Status == Base.Enum.Game.Status.Playing_Denied || this.Players.PlayerLimitLocked || this.Players.Count == this.Players.PlayerLimit)
                packet.Add(0); // unjoinable
            else
                packet.Add(1); // joinable

            // not sure from this point
            //packet.Add(this.Players.Master.Session.Inventory.HasItem("CC02") ? 1 : 0);
            packet.Add((int)Base.Enum.Game.Type.Event); 
            //packet.Add((int)this.Type);
            packet.Add(0); // weapon restriction

            // restrictions below (3 blocks)
            packet.Add(0); // what?
            packet.Add(this.Details.IsVipRoom ? 1 : 0); // is vip room
            packet.Add(0); // clan state, 0 = nope, 1 = x vs y, 2+ = clan waiting

            packet.Add((int)this.Details.LevelLimit);
            packet.Add(this.Details.PremiumOnly ? 1 : 0);
            packet.Add(this.Details.VotekickActive ? 1 : 0); // votekick
            packet.Add(0); // autostart
            packet.Add((int)this.Details.PingLimit);
            packet.Add(-1);*/
        }

    }
}
