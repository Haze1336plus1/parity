using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.WRDevTool.Controller
{
    public class Config
    {

        public class cFCLD
        {

            public FCLD.FCLDException[] Exceptions { get; private set; }
            public byte[] Template { get; private set; }

            public cFCLD(FCLD.FCLDException[] Exceptions, byte[] Template)
            {
                this.Exceptions = Exceptions;
                this.Template = Template;
            }

        }

        public class cFileDB
        {

            public FCLD.FCLDException[] Exceptions { get; private set; }

            public cFileDB(FCLD.FCLDException[] Exceptions)
            {
                this.Exceptions = Exceptions;
            }

        }

        protected System.Xml.XmlDocument _ConfigDocument { get; private set; }
        public cFCLD FCLD { get; private set; }
        public cFileDB FileDB { get; private set; }

        public Config(string FileName)
        {
            this._ConfigDocument = new System.Xml.XmlDocument();
            this._ConfigDocument.Load(FileName);
        }

        public void Load()
        {
            this._LoadFCLD();
            this._LoadFileDB();
        }

        protected void _LoadFCLD()
        {
            var iList = new List<WRDevTool.FCLD.FCLDException>();
            foreach (System.Xml.XmlNode iElement in this._ConfigDocument["Config"]["FCLD"])
            {
                if (iElement.NodeType != System.Xml.XmlNodeType.Element)
                    continue;
                if (!iElement.Name.ToLower().Equals("exception"))
                    continue;
                var exType = (WRDevTool.FCLD.FCLDException.eType)Enum.Parse(typeof(WRDevTool.FCLD.FCLDException.eType), iElement.Attributes["Type"].Value);
                string exValue = iElement.InnerText;
                iList.Add(new FCLD.FCLDException(exType, exValue));
            }
            string hexTemplate = this._ConfigDocument["Config"]["FCLD"]["template"].InnerText;
            byte[] template = Enumerable.Range(0, hexTemplate.Length)
                     .Where(x => x % 2 == 0)
                     .Select(x => Convert.ToByte(hexTemplate.Substring(x, 2), 16))
                     .ToArray();
            this.FCLD = new cFCLD(iList.ToArray(), template);
        }

        protected void _LoadFileDB()
        {
            var iList = new List<WRDevTool.FCLD.FCLDException>();
            foreach (System.Xml.XmlNode iElement in this._ConfigDocument["Config"]["FileDB"])
            {
                if (iElement.NodeType != System.Xml.XmlNodeType.Element)
                    continue;
                if (!iElement.Name.ToLower().Equals("exception"))
                    continue;
                var exType = (WRDevTool.FCLD.FCLDException.eType)Enum.Parse(typeof(WRDevTool.FCLD.FCLDException.eType), iElement.Attributes["Type"].Value);
                string exValue = iElement.InnerText;
                iList.Add(new FCLD.FCLDException(exType, exValue));
            }
            this.FileDB = new cFileDB(iList.ToArray());
        }

    }
}
