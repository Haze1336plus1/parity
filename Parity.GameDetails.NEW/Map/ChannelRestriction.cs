using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.GameDetails.Map
{
    public class ChannelRestriction
    {

        public Parity.Base.Enum.Channel[] Allowed { get; private set; }

        public ChannelRestriction(bool CQC, bool BG, bool AI)
        {
            var iList = new List<Parity.Base.Enum.Channel>();
            if (CQC) iList.Add(Parity.Base.Enum.Channel.CQC);
            if (BG) iList.Add(Parity.Base.Enum.Channel.BG);
            if (AI) iList.Add(Parity.Base.Enum.Channel.AI);
            this.Allowed = iList.ToArray();
        }

    }
}
