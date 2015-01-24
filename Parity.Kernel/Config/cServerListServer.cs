using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Kernel.Config
{
    public class cServerListServer
    {

        public byte Id { get; private set; }
        public string Name { get; private set; }
        public cServer Host { get; private set; }

        public cServerListServer(System.Xml.XmlNode configNode)
        {
            this.Id = byte.Parse(configNode.Attributes["ID"].Value.Fallback("0"));
            this.Name = configNode.Attributes["Name"].Value;
            this.Host = new cServer(configNode["Host"]);
        }

    }
}
