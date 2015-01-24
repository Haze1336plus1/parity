using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.GameDetails.WRBin.Items.Weapons
{

    public struct AbilityInfo
    {

        public readonly int Power;
        public readonly int Defence;
        public readonly int SootRange;
        public readonly short ClipSize; // Ammonum
        public readonly int Magazines; // Magazinenum
        public readonly int EffectRange;
        public readonly int BulletDrop; // Parabola
        public readonly double ReactAmount;
        public readonly double ReactRecovery;
        public readonly double Accuracy;
        public readonly int ShootSpeed;
        public readonly double Weight;
        public readonly double CoolTime;
        public readonly double ReloadTime;
        public readonly double HeatFactor;
        public readonly double CoolFactor;
        public readonly double FsdTime; // wtf
        public readonly double ScdTime; // wtf²
        public readonly byte ArmorType;

        public AbilityInfo(System.Xml.XmlNode abilityInfoNode)
        {
            var invariant = System.Globalization.CultureInfo.InvariantCulture;
            this.Power = int.Parse(abilityInfoNode["POWER"].InnerText);
            this.Defence = int.Parse(abilityInfoNode["DEFENCE"].InnerText);
            this.SootRange = int.Parse(abilityInfoNode["SOOTRANGE"].InnerText);
            this.ClipSize = short.Parse(abilityInfoNode["AMMONNUM"].InnerText);
            this.Magazines = int.Parse(abilityInfoNode["MAGAZINENUM"].InnerText);
            this.EffectRange = int.Parse(abilityInfoNode["EFFECTRANGE"].InnerText);
            this.BulletDrop = int.Parse(abilityInfoNode["PARABOLA"].InnerText);
            this.ReactAmount = double.Parse(abilityInfoNode["REACTAMOUNT"].InnerText, invariant);
            this.ReactRecovery = double.Parse(abilityInfoNode["REACTRECOVERY"].InnerText, invariant);
            this.Accuracy = double.Parse(abilityInfoNode["ACCURATE"].InnerText, invariant);
            this.ShootSpeed = int.Parse(abilityInfoNode["SHOOTSPEED"].InnerText);
            this.Weight = int.Parse(abilityInfoNode["WEIGHT"].InnerText) / 10.0;
            this.CoolTime = double.Parse(abilityInfoNode["COOLTIME"].InnerText, invariant);
            this.ReloadTime = double.Parse(abilityInfoNode["RELOADTIME"].InnerText, invariant);
            this.HeatFactor = double.Parse(abilityInfoNode["HEATFACTOR"].InnerText, invariant);
            this.CoolFactor = double.Parse(abilityInfoNode["COOLFACTOR"].InnerText, invariant);
            this.FsdTime = double.Parse(abilityInfoNode["FSDTIME"].InnerText, invariant);
            this.ScdTime = double.Parse(abilityInfoNode["SCDTIME"].InnerText, invariant);
            this.ArmorType = byte.Parse(abilityInfoNode["ARMORTYPE"].InnerText);
        }

    }
}
