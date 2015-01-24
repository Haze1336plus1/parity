using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Script
{
    public class Util
    {

        public static void PremiumUser(Client.Client player, Base.Enum.Premium premium, int duration)
        {
            if (player.Session.Account.Details.Premium < premium)
                player.Session.Account.Details.Premium = premium;
            if (player.Session.Account.Details.PremiumTime == 0)
            {
                player.Session.Account.Details.PremiumBegin = DateTime.Now;
                player.Session.Account.Details.PremiumTime = duration;
            }
            else
                player.Session.Account.Details.PremiumTime += duration;
        }

    }
}
