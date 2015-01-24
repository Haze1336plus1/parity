using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.GameDetails.Map
{
    public class GameModeRestriction
    {

        public Base.Enum.Game.Mode[] Allowed { get; private set; }

        public GameModeRestriction(bool Explosive, bool FreeForAll, bool Deathmatch, bool Conquest, bool ExplosiveBG, bool Hero, bool TotalWar, bool Survive, bool Defence, bool Escape)
        {
            var iList = new List<Base.Enum.Game.Mode>();
            if (Explosive) iList.Add(Base.Enum.Game.Mode.Explosive);
            if (FreeForAll) iList.Add(Base.Enum.Game.Mode.FreeForAll);
            if (Deathmatch) { iList.Add(Base.Enum.Game.Mode.Deathmatch); iList.Add(Base.Enum.Game.Mode.FourVsFour); }
            if (Conquest) iList.Add(Base.Enum.Game.Mode.Conquest);
            if (ExplosiveBG) iList.Add(Base.Enum.Game.Mode.ExplosiveBG);
            if (Hero) iList.Add(Base.Enum.Game.Mode.HeroMode);
            if (TotalWar) iList.Add(Base.Enum.Game.Mode.TotalWar);
            if (Survive) iList.Add(Base.Enum.Game.Mode.Survive);
            if (Defence) iList.Add(Base.Enum.Game.Mode.Defence);
            if (Escape) iList.Add(Base.Enum.Game.Mode.Escape);
            this.Allowed = iList.ToArray();
        }
        public GameModeRestriction(Base.Enum.Game.Mode[] Allowed)
        {
            this.Allowed = Allowed;
        }

    }
}
