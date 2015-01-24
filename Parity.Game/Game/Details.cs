using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Game.Game
{
    public class Details
    {

        public string Name { get; set; }
        public string Password { get; set; }
        public bool HasPassword { get { return this.Password != null; } }
        public bool PremiumOnly { get; set; }
        public bool VotekickActive { get; set; }
        //public bool IsVipRoom { get; set; }
        public Base.Enum.Game.Status Status { get; private set; }

        private Base.Enum.Game.Mode _Mode;
        public Base.Enum.Game.Mode Mode
        {
            get
            {
                return this._Mode;
            }
            set
            {
                this._Mode = value;
                
            }
        }
        private int _MapId;
        public int MapId
        {
            get { return this._MapId; }
            set
            {
                this._MapId = value;
                this.MapInfo = Modules.WarRock.MapDetailContainer.MapDetails[this._MapId];
            }
        }
        public GameDetails.Map.Details MapInfo { get; private set; }

        public Base.Enum.Game.TimeLimit TimeLimit { get; set; }
        public Base.Enum.Game.LevelLimit LevelLimit { get; set; }
        public Base.Enum.Game.PingLimit PingLimit { get; set; }

        public Details(string name,
            string password = null,
            bool premiumOnly = false,
            bool votekickActive = true,
            //bool isVipRoom = false,
            Base.Enum.Game.Mode mode = Base.Enum.Game.Mode.Explosive,
            int mapId = 0,
            Base.Enum.Game.TimeLimit timeLimit = Base.Enum.Game.TimeLimit.None,
            Base.Enum.Game.LevelLimit levelLimit = Base.Enum.Game.LevelLimit.None,
            Base.Enum.Game.PingLimit pingLimit = Base.Enum.Game.PingLimit.White)
        {
            this.Name = name;
            this.Password = password;
            this.PremiumOnly = premiumOnly;
            this.VotekickActive = votekickActive;
            //this.IsVipRoom = isVipRoom;
            this.Mode = mode;
            this.MapId = mapId;
            this.TimeLimit = timeLimit;
            this.LevelLimit = levelLimit;
            this.PingLimit = pingLimit;
            this.Status = Base.Enum.Game.Status.Waiting;
        }

    }
}
