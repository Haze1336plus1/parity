using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.GameDetails.WRBin.Items
{
    public class Weapon : Basic
    {

        public readonly Overall.BuyInfo BuyInfo;
        public readonly Overall.UseInfo UseInfo;

        public readonly Weapons.AbilityInfo AbilityInfo;
        public readonly Overall.TargetInfo TargetInfo;

        public Weapon(int id, System.Xml.XmlNode weaponNode)
            : base(id, weaponNode)
        {
            this.BuyInfo = new Overall.BuyInfo(weaponNode["BUY_INFO"]);
            this.UseInfo = new Overall.UseInfo(weaponNode["USE_INFO"]);

            this.AbilityInfo = new Weapons.AbilityInfo(weaponNode["ABILITY_INFO"]);
            this.TargetInfo = new Overall.TargetInfo(weaponNode["TARGET_INFO"]);
        }

    }
}
