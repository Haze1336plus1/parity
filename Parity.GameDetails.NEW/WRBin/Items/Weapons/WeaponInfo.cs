using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.GameDetails.WRBin.Items.Weapons
{

    public struct WeaponInfo
    {

        public readonly string BaseWeapon;

        public readonly Base.Enum.Channel[] UseableChannel;
        public readonly Base.Enum.BattleClass[] UseableBranch;
        public readonly byte[] UseableSlot;

        public readonly int Power;
        //public readonly int Defence;
        public readonly int ShootRange;
        public readonly int EffectRange;
        public readonly short ClipSize; // Ammonum
        public readonly int Magazines; // Magazinenum
        public readonly int BulletDrop; // Parabola
        public readonly double ReactAmount;
        public readonly double ReactRecovery;
        public readonly double Accuracy;
        public readonly double FirstShootDelayTime;
        public readonly int ShootSpeed;
        public readonly double Weight;
        public readonly double ReloadTime;
        public readonly short SoundIndex;
        public readonly byte ZoomLimit;
        public readonly string ScopeTexName;
        public readonly bool ExtraAmmo;
        public readonly bool[] SpecialSkill;
        public readonly int RPM;
        public readonly int Recoil;

        private static T[] ParseValues<T>(string[] input) where T : struct
        {
            List<T> retVal = new List<T>();
            for (byte i = 0; i < input.Length; i++)
                if (input[i] == "1") retVal.Add((T)(object)i);
            return retVal.ToArray();
        }

        public WeaponInfo(System.Xml.XmlNode abilityInfoNode)
        {
            var invariant = System.Globalization.CultureInfo.InvariantCulture;

            this.BaseWeapon = abilityInfoNode["BaseWeapon"].InnerText;
            this.UseableChannel = ParseValues<Base.Enum.Channel>(abilityInfoNode["UseableChannel"].InnerText.Split(','));
            this.UseableBranch = ParseValues<Base.Enum.BattleClass>(abilityInfoNode["UseableBranch"].InnerText.Split(','));
            this.UseableSlot = ParseValues<byte>(abilityInfoNode["UseableSlot"].InnerText.Split(','));

            this.Power = int.Parse(abilityInfoNode["Power"].InnerText);
            //this.Defence = int.Parse(abilityInfoNode["DEFENCE"].InnerText);
            this.ShootRange = int.Parse(abilityInfoNode["ShootRange"].InnerText);
            this.ClipSize = short.Parse(abilityInfoNode["AmmoNum"].InnerText);
            this.Magazines = int.Parse(abilityInfoNode["MagazineNum"].InnerText);
            this.EffectRange = int.Parse(abilityInfoNode["EffectRange"].InnerText);
            this.BulletDrop = int.Parse(abilityInfoNode["Parabola"].InnerText);
            this.ReactAmount = double.Parse(abilityInfoNode["ReactAmount"].InnerText, invariant);
            this.ReactRecovery = double.Parse(abilityInfoNode["ReactRecovery"].InnerText, invariant);
            this.Accuracy = double.Parse(abilityInfoNode["Accurate"].InnerText, invariant);
            this.ShootSpeed = int.Parse(abilityInfoNode["ShootSpeed"].InnerText);
            this.Weight = int.Parse(abilityInfoNode["Weight"].InnerText) / 10.0;
            //this.CoolTime = double.Parse(abilityInfoNode["COOLTIME"].InnerText, invariant);
            this.ReloadTime = double.Parse(abilityInfoNode["ReloadTime"].InnerText, invariant);
            this.FirstShootDelayTime = double.Parse(abilityInfoNode["FirstShootDelayTime"].InnerText, invariant);
            this.SoundIndex = short.Parse(abilityInfoNode["SoundIdx"].InnerText);
            this.ZoomLimit = byte.Parse(abilityInfoNode["ZoomLimit"].InnerText);
            this.ScopeTexName = abilityInfoNode["ScopeTexName"].InnerText;
            this.ExtraAmmo = (abilityInfoNode["bExtraAmmo"].InnerText == "1");
            this.SpecialSkill = new bool[] { };
            this.RPM = int.Parse(abilityInfoNode["RPM"].InnerText);
            this.Recoil = int.Parse(abilityInfoNode["Recoil"].InnerText);
        }

    }
}
