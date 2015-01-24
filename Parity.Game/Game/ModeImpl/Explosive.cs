using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Game.ModeImpl
{
    public class Explosive : Mode
    {

        public Explosive(Room owner)
            : base(owner)
        {
            
        }

        protected override Base.Enum.Team OnPlayerDied(Client.Client player, Client.Client killer, Base.Enum.Team scoringTeam)
        {
            // dont score
            return Base.Enum.Team.None;
        }
        protected override bool OnAskGameEnd()
        {
            int niuCount = this.Owner.Players.FromTeam(Base.Enum.Team.NIU).Count();
            if (niuCount > 0)
                this.Score1 ++;
            else
                this.Score2 ++;

            return (this.Score1 > this.Setting1 ||
                this.Score2 > this.Setting1);
        }

    }
}
