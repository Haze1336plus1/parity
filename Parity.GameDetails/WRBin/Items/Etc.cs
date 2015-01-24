﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.GameDetails.WRBin.Items
{
    public class Etc : Basic
    {

        public readonly Overall.BuyInfo BuyInfo;
        public readonly Overall.UseInfo UseInfo;

        public Etc(int id, System.Xml.XmlNode etcNode)
            : base(id, etcNode)
        {
            this.BuyInfo = new Overall.BuyInfo(etcNode["BUY_INFO"]);
            this.UseInfo = new Overall.UseInfo(etcNode["USE_INFO"]);
        }

    }
}
