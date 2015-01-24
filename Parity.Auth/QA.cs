using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Auth
{
    public class QA
    {

        public static Server.Core Core { get; private set; }

        internal static void SetCore(Server.Core core)
        {
            QA.Core = core;
        }

        public static DS.Client GetDBClient()
        {
            return Server.Core.GetInstance().DSManager.GetClient();
        }

        public static DS.Model GetDBModel()
        {
            return Server.Core.GetInstance().DSModel;
        }

        public static DS.Model GetDBManager()
        {
            return Server.Core.GetInstance().DSModel;
        }

    }
}
