using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.GameDetails.WRBin.Items
{
    public class Weapon : Basic
    {

        public readonly Overall.ShopInfo ShopInfo;
        public readonly Weapons.WeaponInfo WeaponInfo;
        public readonly Overall.TargetInfo TargetInfo;

        public Weapon(int id, System.Xml.XmlNode weaponNode)
            : base(id, weaponNode)
        {
            this.ShopInfo = new Overall.ShopInfo(weaponNode["SHOP_INFO"]);
            this.WeaponInfo = new Overall.WeaponInfo(weaponNode["WEAPON_INFO"]);
            this.TargetInfo = new Overall.TargetInfo(weaponNode["WEAPON_TARGET_DAMAGE_INFO"]);
        }

    }
}
