using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Kernel.Config
{
    public class Game
    {

        public cServer Host { get; private set; }
        public cRate Rates { get; private set; }
        public cGame GameConfig { get; private set; }

        public Game(System.Xml.XmlNode configDoc)
        {
            this.Host = new cServer(configDoc["Host"]);
            this.Rates = new cRate(configDoc["Rate"]);
            this.GameConfig = new cGame(configDoc["Game"]);
        }

    }
}
