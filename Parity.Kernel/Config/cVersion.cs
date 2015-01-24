using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Kernel.Config
{
    public class cVersion
    {

        // old stuff
        /*public byte Format { get; private set; }
        public byte Launcher { get; private set; }
        public byte Updater { get; private set; }
        public ushort Client { get; private set; }
        public byte Sub { get; private set; }
        public byte Option { get; private set; }*/

        // old stuff, but required
        public string PatchSrv { get; private set; }
        // new stuff
        public int Version { get; private set; }

        public cVersion(System.Xml.XmlNode configNode)
        {
            /*this.Format = byte.Parse(configNode.Attributes["Format"].Value);
            this.Launcher = byte.Parse(configNode.Attributes["Launcher"].Value);
            this.Updater = byte.Parse(configNode.Attributes["Updater"].Value);
            this.Client = ushort.Parse(configNode.Attributes["Client"].Value);
            this.Sub = byte.Parse(configNode.Attributes["Sub"].Value);
            this.Option = byte.Parse(configNode.Attributes["Option"].Value);*/

            this.PatchSrv = configNode.Attributes["PatchSrv"].Value;
            this.Version = int.Parse(configNode.Attributes["Version"].Value);
        }

    }
}
