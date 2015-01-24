using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.GameDetails.WRBin.Items.Overall
{
    public struct BasicInfo
    {

        public readonly string English;
        public readonly string Code;
        public readonly bool Active;

        public BasicInfo(System.Xml.XmlNode basicInfoNode)
        {
            this.English = basicInfoNode["ENGLISH"].InnerText;
            this.Code = basicInfoNode["CODE"].InnerText;
            this.Active = (basicInfoNode["ACTIVE"].InnerText == "TRUE");
        }

        public BasicInfo(string english, string code, bool active, int label)
        {
            this.English = english;
            this.Code = code;
            this.Active = active;
        }

    }
}
