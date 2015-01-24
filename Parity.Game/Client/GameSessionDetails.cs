using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Parity.Game.Client
{
    public class GameSessionDetails
    {

        #region Data

        // Current channel of player
        public Base.Enum.Channel Channel { get; set; }

        // What did he set as RoomStartingIndex (RoomList handler)?
        public short RoomStartingIndex { get; set; }

        // What is the Players average ping?
        public ushort Ping { get; set; }

        // Current GameSession for a match (ingame)
        public GameSessionRoom Room { get; private set; }

        // Check if the current GameSession.Room is set
        public bool InRoom { get { return this.Room.Current != null; } }

        // Ready state in a room
        public bool ReadyState { get; set; }

        // Which item does the player have on hand right now (ingame)?
        public short CurrentItemIndex { get; set; }

        // Return the Players Team by checking the Index 
        public Base.Enum.Team Team
        {
            get
            {
                return this.Room.Current.Players.Container.IndexOf(this.Owner) < (this.Room.Current.Players.PlayerLimit / 2) ? Base.Enum.Team.DER : Base.Enum.Team.NIU;
            }
        }

        // Get the BattleClass of soldier selected on last spawn
        public Base.Enum.BattleClass BattleClass { get; set; }

        // Health is like between -1 (dead) and 1000 (full hp)
        public short Health { get; set; }

        // Check if health is greater than 0. If so: you're alive!
        public bool IsAlive { get { return this.Health > 0; } }

        #endregion

        #region Constructor

        public readonly Client Owner;

        /// <summary>
        /// Create a new GameSessionDetails instance for a Client (single instance)
        /// </summary>
        public GameSessionDetails(Client owner)
        {
            this.Owner = owner;
            this.Room = new GameSessionRoom(this);
        }

        #endregion

    }
}
