using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.GameDetails.WRBin.Items
{
    public abstract class Basic
    {

        public readonly int Id;
        public readonly Overall.BasicInfo BasicInfo;

        public Basic(int id, System.Xml.XmlNode basicNode)
        {
            this.Id = id;
            this.BasicInfo = new Overall.BasicInfo(basicNode["BASIC_INFO"]);
        }

    }
}
