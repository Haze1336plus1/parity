using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.DS
{
    public class Configuration
    {

        public string Host { get; private set; }
        public ushort Port { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Database { get; private set; }
        public string GameDatabase { get; private set; }
        public byte PoolSize { get; private set; }

        public Configuration(string configurationFile)
        {
            System.Xml.XmlDocument cfgDoc = new System.Xml.XmlDocument();
            cfgDoc.Load(configurationFile);
            var cfg = cfgDoc["Database"];
            this.Host = cfg.Attributes["Host"].Value;
            this.Port = ushort.Parse(cfg.Attributes["Port"].Value);
            this.Username = cfg["Auth"].Attributes["Username"].Value;
            this.Password = cfg["Auth"].Attributes["Password"].Value;
            this.Database = cfg["Manager"].Attributes["Database"].Value;
            this.GameDatabase = cfg["Manager"].Attributes["GameDatabase"].Value;
            this.PoolSize = byte.Parse(cfg["Manager"].Attributes["PoolSize"].Value);
        }

    }
}
