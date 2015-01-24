using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Game.Game
{
    public class PlayerContainer
    {

        #region Data

        private object lockingObject = new Object();
        public readonly Room Owner;
        public readonly Client.Client[] Container;

        public int Count { get; private set; }
        public int MasterIndex { get; private set; }
        public int PlayerLimit { get; set; }
        public bool PlayerLimitLocked { get; set; }

        #endregion

        #region Constructor, Initialize

        /// <summary>
        /// Create a new PlayerContainer instance, depends on Room
        /// </summary>
        public PlayerContainer(Room owner)
        {
            this.Owner = owner;
            this.Container = new Client.Client[32];
            this.PlayerLimit = 8; // default
        }

        /// <summary>
        /// Set the master if he's the first one
        /// </summary>
        internal void Initialize(Client.Client master)
        {
            // if we have a master already: fuck that...
            if (this.Count > 0)
                return;
            this.AddPlayer(master);
        }

        #endregion

        #region Properties
        
        /// <summary>
        /// Get the Player by MasterIndex
        /// </summary>
        public Client.Client Master
        {
            get
            {
                return this.Container[this.MasterIndex];
            }
        }

        #endregion

        #region Functions

        /// <summary>
        /// Get an IEnumerable of Client.Client containing all existing players in a room
        /// </summary>
        public IEnumerable<Client.Client> Players()
        {
            lock (this.lockingObject)
            {
                for (int index = 0; index < this.Container.Length; index++)
                    if (this.Container[index] != null)
                        yield return this.Container[index];
            }
        }

        /// <summary>
        /// Get an IEnumerable of Client.Client containing all existing players in a room and a given team
        /// </summary>
        public IEnumerable<Client.Client> FromTeam(Base.Enum.Team team)
        {
            lock (this.lockingObject)
            {
                int startOffset = (team == Base.Enum.Team.DER ? 0 : this.PlayerLimit / 2);
                int endOffset = (startOffset == 0 ? this.PlayerLimit / 2 : this.PlayerLimit);
                for (int index = startOffset; index < endOffset; index++)
                    if (this.Container[index] != null)
                        yield return this.Container[index];
            }
        }

        /// <summary>
        /// Switch the Team of a player
        /// </summary>
        /// <returns>True if team switch was successfull, false if not</returns>
        public bool TeamSwitch(Client.Client player)
        {
            lock (this.lockingObject)
            {
                int oldIndex = this.Container.IndexOf(player);
                if (player.GameSession.Team == Base.Enum.Team.None) return false;

                int startOffset = (player.GameSession.Team == Base.Enum.Team.DER ? 0 : this.PlayerLimit / 2);
                int endOffset = (startOffset == 0 ? this.PlayerLimit / 2 : this.PlayerLimit);

                for (int index = startOffset; index < endOffset; index++)
                {
                    if (this.Container[index] == null)
                    {
                        this.Container[oldIndex] = null;
                        this.Container[index] = player;
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Try to add a player with optionally preferred index
        /// </summary>
        /// <param name="preferredIndex">Index of player in virtual container. -1 for auto balance</param>
        /// <returns>True if player has taken space in the container. False if something went wrong</returns>
        public bool AddPlayer(Client.Client player, sbyte preferredIndex = -1)
        {
            lock(this.lockingObject)
            {

                // no more space!
                if (this.Count == this.PlayerLimit)
                    return false;

                // automatically balance teams
                if (preferredIndex == -1)
                {
                    sbyte teamSize = (sbyte)(this.PlayerLimit / 2);
                    int countDER = this.FromTeam(Base.Enum.Team.DER).Count();
                    int countNIU = this.Count - countDER;
                    if (countDER == teamSize ||  // derberan team is full? go to NIU
                        countDER > countNIU) // more derberan players as niu players? go to NIU, too
                    {
                        // add to second half (NIU)
                        for (sbyte i = teamSize; i < this.PlayerLimit; i++)
                        {
                            if (this.Container[i] == null)
                            {
                                this.Container[i] = player;
                                return true;
                            }
                        }
                    }
                    else
                    {
                        // add to first half (DER)
                        for (sbyte i = 0; i < teamSize; i++)
                        {
                            if (this.Container[i] == null)
                            {
                                this.Container[i] = player;
                                return true;
                            }
                        }
                    }
                }
                else if (preferredIndex >= 0 &&
                    preferredIndex < this.PlayerLimit &&
                    this.Container[preferredIndex] == null)
                {
                    // if player at preferredIndex does not exist; join the room! =D
                    this.Container[preferredIndex] = player;
                    return true;
                }
            }
            return false;
        }

        #endregion

    }
}
