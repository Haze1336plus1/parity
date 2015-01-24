using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Kernel.Config
{
    public class cGame
    {

        public byte Id { get; private set; }
        public string Name { get; private set; }
        public string BotName { get; private set; }
        public string Database { get; private set; }

        public cGame(System.Xml.XmlNode configNode)
        {
            this.Id = byte.Parse(configNode.Attributes["Id"].Value);
            this.Name = configNode["Name"].InnerText;
            this.BotName = configNode["BotName"].InnerText;
            this.Database = configNode["Database"].InnerText;
        }

    }
}
