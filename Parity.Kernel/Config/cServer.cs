using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Kernel.Config
{
    public class cServer
    {

        public string IP { get; private set; }
        public ushort Port { get; private set; }

        public cServer(System.Xml.XmlNode configNode)
        {
            this.IP = configNode["Address"].InnerText.Fallback("127.0.0.1");
            this.Port = ushort.Parse(configNode["Port"].InnerText.Fallback("5330"));
        }

    }
}
