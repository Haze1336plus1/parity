using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Game
{
    // Management Quick Access
    public class Modules
    {

        public static readonly Management.WarRock WarRock;
        public static readonly Management.Defaults Defaults;
        public static readonly Management.Room.Storage RoomStorage;
        public static readonly Management.Shop Shop;
        public static readonly Management.OutboxActivation OutboxActivation;

        public static void Touch() { Equals(Defaults, null); }
        static Modules()
        {
            Base.IO.Informational("Initializing Management Quick Access");
            Modules.WarRock = new Management.WarRock();
            Modules.Defaults = new Management.Defaults();
            Modules.RoomStorage = new Management.Room.Storage();
            Modules.Shop = new Management.Shop();
            Modules.OutboxActivation = new Management.OutboxActivation();
        }

    }
}
