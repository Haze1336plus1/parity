using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.GameDetails.Map.Object
{
    public class Flag : Base
    {

        public Parity.Base.Enum.Team Team { get; set; }
        public bool Flagable { get; private set; }

        public Flag(int X, int Y, int Z, Parity.Base.Enum.Team Team)
            : base(X, Y, Z)
        {
            this.Team = Team;
            if (this.Team == Parity.Base.Enum.Team.None)
                this.Flagable = true;
        }

    }
}
