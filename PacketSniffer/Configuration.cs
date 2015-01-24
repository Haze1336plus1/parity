using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketSniffer
{
    public class Configuration
    {

        public static readonly Dictionary<string, object> Default;
        public static readonly Dictionary<string, object> Override;

        public static T Get<T>(string key)
        {
            return (T)(Configuration.Override.ContainsKey(key) ? Configuration.Override[key] : Configuration.Default[key]);
        }

        static Configuration()
        {
            Configuration.Default = new Dictionary<string, object>()
            {
                { "Port.Login", 5330 },
                { "Port.Lobby", 10375 },
                { "Port.Gameplay", 10376 }
            };

            Configuration.Override = new Dictionary<string, object>();
            // override
            if (System.IO.File.Exists("override.xml"))
            {
                System.Xml.XmlDocument configDocument = new System.Xml.XmlDocument();
                configDocument.Load("override.xml");
                foreach (System.Xml.XmlNode oelem in configDocument["Override"])
                {
                    if (oelem.NodeType != System.Xml.XmlNodeType.Element)
                        continue;
                    if (Configuration.Default.ContainsKey(oelem.Name) &&
                        !Configuration.Override.ContainsKey(oelem.Name))
                        Configuration.Override.Add(oelem.Name, int.Parse(oelem.InnerText));
                }
            }
        }

    }
}
