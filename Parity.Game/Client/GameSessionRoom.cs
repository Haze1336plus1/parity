using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Client
{
    public class GameSessionRoom
    {

        public readonly GameSessionDetails Owner;
        public int Kills;
        public int Deaths;
        public Game.Room Current { get; private set; }

        public GameSessionRoom(GameSessionDetails owner)
        {
            this.Owner = owner;
        }

        /// <summary>
        /// Reset the Statistics (Kills, Deaths) after each match (and applies stats, if necessary)
        /// </summary>
        public void ResetStats()
        {
            if (this.Kills > 0 || this.Deaths > 0)
            {
                var details = this.Owner.Owner.Session.Account.Details;
                details.Kills += this.Kills;
                details.Deaths += this.Deaths;
                using (var client = QA.GetDBClient())
                {
                    client.Command("UPDATE `details` AS d SET d.kills = :kills, d.deaths = :deaths WHERE d.id = :accountId;")
                        .SetParameter("kills", details.Kills)
                        .SetParameter("deaths", details.Deaths)
                        .SetParameter("accountId", details.Id)
                        .Execute();
                }
            }

            this.Kills = 0;
            this.Deaths = 0;
        }

        /// <summary>
        /// Join a room by setting the 'Current' property, registering to PlayerContainer and sending the required packets
        /// </summary>
        /// <param name="r">Room to be joined (attempt)</param>
        /// <returns>True if room was joined. False if not</returns>
        public bool Join(Game.Room r)
        {
            if (Current != null)
                return false;

            // todo: send playerlist to each other (before join, self)
            // todo: send playerlist to self (before join, all players)
            // todo: send playerlist to self (after join, self)
            var playerListToAllBeforeJoinSelf = Server.PacketFactory.PlayerList(r, new Client[] { this.Owner.Owner });
            var playerListToSelfBeforeJoinAll = Server.PacketFactory.PlayerList(r, r.Players.Players());
            var playerListToSelfAfterJoinSelf = Server.PacketFactory.PlayerList(r, new Client[] { this.Owner.Owner });

            r.Players.Players().ForEach(x => x.Send(playerListToAllBeforeJoinSelf));
            this.Owner.Owner.Send(playerListToSelfBeforeJoinAll);

            // we're in the PlayerContainer already, when we just created the room. means: master is set by default
            // however. if player is not in the PlayerContainer already, add him
            if (!r.Players.Container.Contains(this.Owner.Owner))
                r.Players.AddPlayer(this.Owner.Owner);

            this.Owner.Owner.Send(playerListToSelfAfterJoinSelf);

            this.Current = r;
            this.ResetStats();

            this.Owner.Owner.Send(Server.PacketFactory.JoinRoom(this.Owner.Owner));

            return true;
        }

        /// <summary>
        /// Leave the current room, if we're in a room right now
        /// </summary>
        public void Leave()
        {
            if (this.Current == null)
                return; // bye
            this.ResetStats();
        }

    }
}
