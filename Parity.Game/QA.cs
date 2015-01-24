using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Game
{
    // Quick Access
    public class QA
    {

        public static Server.Core Core { get; private set; }

        internal static void SetCore(Server.Core core)
        {
            QA.Core = core;
        }

        public static Base.Log GetLog()
        {
            return QA.Core.Log;
        }

        public static DS.Client GetDBClient()
        {
            return QA.Core.DSManager.GetClient();
        }

        public static DS.Model GetDBModel()
        {
            return QA.Core.DSModel;
        }

        public static DS.Model GetDBManager()
        {
            return QA.Core.DSModel;
        }

    }
}
