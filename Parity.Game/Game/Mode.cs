using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Game
{
    public abstract class Mode
    {

        public readonly Room Owner;
        //Bubblefuck with Setting1 / Setting2
        public int Setting1 { get; set; }
        public int Setting2 { get; set; }
        public int Score1 { get; set; }
        public int Score2 { get; set; }
        public int TimeLimit { get; set; }
        public int CurrentTime { get; set; }
        public Base.Enum.VirtualGameState GameState { get; private set; }

        public Mode(Room owner)
        {
            this.Owner = owner;
        }
        public static Mode GetMode(Room owner, Base.Enum.Game.Mode m)
        {
            if (m == Base.Enum.Game.Mode.Explosive)
                return new ModeImpl.Explosive(owner);
            return null;
        }
        public static int GetDefaultMap(Base.Enum.Game.Mode m)
        {
            int mapId = 0;
            if(m == Base.Enum.Game.Mode.Explosive)
                mapId = Modules.WarRock.MapDetailContainer.GetID("Marien");
            return mapId;
        }

        #region Help

        /// <summary>
        /// Check is the game should end or not
        /// </summary>
        /// <returns>True if Game should end</returns>
        public bool CheckGame()
        {
            if (this.CurrentTime >= this.TimeLimit ||
                this.Owner.Players.Count <= 1 ||
                this.Owner.Players.FromTeam(Base.Enum.Team.DER).Count() == 0 ||
                this.Owner.Players.FromTeam(Base.Enum.Team.NIU).Count() == 0)
            {
                return true;
            }

            return false;
        }

        #endregion

        #region Virtual Functions

        public virtual void OnPlayerLeave(Client.Client player)
        {
            if (this.CheckGame())
                this.GameEnd();
        }
        public virtual void Tick(bool second)
        {
            if (this.GameState != Base.Enum.VirtualGameState.Active)
                return;

            if (second)
                this.CurrentTime++;
            this.CheckGame();
        }
        public void GameEnd()
        {
            if (this.GameState == Base.Enum.VirtualGameState.Active)
            {
                if (!this.OnAskGameEnd())
                    return;
                this.GameState = Base.Enum.VirtualGameState.Ending;
                // send game end
                this.OnProcessGameEnd();
                this.CurrentTime = 0;
                Base.Thread.Timer.GetInstance().AddAction((long)Base.Compile.GameDefaults["Game.EndingTime"], this.GameEnd);
            }
            else if (this.GameState == Base.Enum.VirtualGameState.Ending)
            {
                // reset and stop accepting in-game roomactions
                this.OnProcessGameEnd();
                this.CurrentTime = 0;
                this.GameState = Base.Enum.VirtualGameState.Ended;
            }
        }
        public void PlayerDied(Client.Client player, Client.Client killer)
        {
            Base.Enum.Team scoringTeam = 
                (player.GameSession.Team == Base.Enum.Team.NIU ? Base.Enum.Team.DER : Base.Enum.Team.NIU);

            player.GameSession.Room.Deaths++;
            if (killer != null)
                killer.GameSession.Room.Kills++;

            scoringTeam = this.OnPlayerDied(player, killer, scoringTeam);
            if (scoringTeam == Base.Enum.Team.DER)
                this.Score1++;
            else if (scoringTeam == Base.Enum.Team.NIU)
                this.Score2++;
        }

        #endregion

        #region Stubs

        protected virtual void OnProcessGameEnd() { }
        protected virtual bool OnAskGameEnd() { return true; } // return false if game should not end
        protected virtual Base.Enum.Team OnPlayerDied(Client.Client player, Client.Client killer, Base.Enum.Team scoringTeam) { return scoringTeam; }

        #endregion

    }
}
