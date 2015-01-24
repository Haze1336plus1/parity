using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.GameDetails.WRBin.Items
{
    public class Character : Basic
    {

        public readonly Overall.ShopInfo BuyInfo;
        public readonly Overall.UseInfo UseInfo;
        public readonly Characters.AbilityInfo AbilityInfo;

        public Character(int id, System.Xml.XmlNode characterNode)
            : base(id, characterNode)
        {
            this.BuyInfo = new Overall.ShopInfo(characterNode["BUY_INFO"]);
            this.UseInfo = new Overall.UseInfo(characterNode["USE_INFO"]);
            this.AbilityInfo = new Characters.AbilityInfo(characterNode["ABILITY_INFO"]);
        }

    }
}
