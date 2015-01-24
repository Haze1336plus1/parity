﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.GameDetails.WRBin.Items
{
    public class Resource : Basic
    {

        public readonly Overall.BuyInfo BuyInfo;
        public readonly Overall.UseInfo UseInfo;

        public Resource(int id, System.Xml.XmlNode resourceNode)
            : base(id, resourceNode)
        {
            this.BuyInfo = new Overall.BuyInfo(resourceNode["BUY_INFO"]);
            this.UseInfo = new Overall.UseInfo(resourceNode["USE_INFO"]);
        }

    }
}