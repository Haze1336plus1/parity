using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Kernel.Config
{
    public class Auth
    {

        public cServer Host { get; private set; }
        public cServer RemoteHost { get; private set; }
        public cVersion Patch { get; private set; }
        public cServerListServer[] Servers { get; set; }
        public Dictionary<string, string> RemoteFileMap { get; private set; }

        public Auth(System.Xml.XmlNode configDoc)
        {
            this.Host = new cServer(configDoc["Server"]);
            this.RemoteHost = new cServer(configDoc["RemoteServer"]);
            this.Patch = new cVersion(configDoc["Patch"]);

            var serversList = new List<cServerListServer>();
            foreach (System.Xml.XmlNode serverNode in configDoc["ServerList"])
                if (serverNode.NodeType == System.Xml.XmlNodeType.Element &&
                    serverNode.Name == "Server")
                    serversList.Add(new cServerListServer(serverNode));

            this.RemoteFileMap = new Dictionary<string, string>();
            foreach (System.Xml.XmlNode rfNode in configDoc["RemoteFile"])
            {
                if (rfNode.NodeType == System.Xml.XmlNodeType.Element &&
                    rfNode.Name == "File")
                    this.RemoteFileMap.Add(rfNode.Attributes["Map"].Value, rfNode.Attributes["File"].Value);
            }

            this.Servers = serversList.ToArray();
        }

    }
}
