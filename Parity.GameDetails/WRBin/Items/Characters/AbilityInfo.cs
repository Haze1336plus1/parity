using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.GameDetails.WRBin.Items.Characters
{
    public struct AbilityInfo
    {

        public readonly double HeadshotDefense;
        public readonly double BodyDefense;
        public readonly double IncreaseClanEXP;
        public readonly double DecreaseWeightRatio;
        public readonly double DecreaseFallingDamage;
        public readonly double DecreaseSPConsum;
        public readonly double IncreaseDinarRatio;
        public readonly double IncreaseEXPRatio;
        public readonly double ItemWeight;
        public readonly double IncreaseAccuracy;
        public readonly double IncreaseThrowRange;
        public readonly double ExtendOption1;
        public readonly double ExtendOption2;
        public readonly int AddDinar;
        public readonly int WearInfo;
        public readonly byte PlatformInfo; // hexadecimal
        public readonly int UsingTypeCount;
        public readonly Base.Enum.Item.Character[] UsingType;
        public readonly int PackageItemCount;
        public readonly string[] PackageItem;

        public AbilityInfo(System.Xml.XmlNode abilityInfoNode)
        {
            this.HeadshotDefense = int.Parse(abilityInfoNode["HeadShotDefense"].InnerText) / 100.0;
            this.BodyDefense = int.Parse(abilityInfoNode["BodyDefense"].InnerText) / 100.0;
            this.IncreaseClanEXP = int.Parse(abilityInfoNode["IncClanEXP"].InnerText) / 100.0;
            this.DecreaseWeightRatio = int.Parse(abilityInfoNode["DecWeightRatio"].InnerText) / 100.0;
            this.DecreaseFallingDamage = int.Parse(abilityInfoNode["DecFallingDamag"].InnerText) / 100.0;
            this.DecreaseSPConsum = int.Parse(abilityInfoNode["DecSPComsum"].InnerText) / 100.0;
            this.IncreaseDinarRatio = int.Parse(abilityInfoNode["IncDinarRatio"].InnerText) / 100.0;
            this.IncreaseEXPRatio = int.Parse(abilityInfoNode["IncEXPRatio"].InnerText) / 100.0;
            this.ItemWeight = int.Parse(abilityInfoNode["ItemWeight"].InnerText) / 100.0;
            this.IncreaseAccuracy = int.Parse(abilityInfoNode["IncAccuracy"].InnerText) / 100.0;
            this.IncreaseThrowRange = int.Parse(abilityInfoNode["IncThrowRange"].InnerText) / 100.0;
            this.ExtendOption1 = int.Parse(abilityInfoNode["ExtendOption1"].InnerText) / 100.0;
            this.ExtendOption2 = int.Parse(abilityInfoNode["ExtendOption2"].InnerText) / 100.0;
            this.AddDinar = int.Parse(abilityInfoNode["AddDinar"].InnerText);
            this.WearInfo = int.Parse(abilityInfoNode["WearInfo"].InnerText);
            this.PlatformInfo = byte.Parse(abilityInfoNode["platformInfo"].InnerText, System.Globalization.NumberStyles.HexNumber);
            this.UsingTypeCount = int.Parse(abilityInfoNode["UsingTypeCount"].InnerText);

            this.UsingType = abilityInfoNode["UsingType"].InnerText
                .Split(',')
                .Select(new Func<string, Base.Enum.Item.Character>((string val) => {
                    return (Base.Enum.Item.Character)(val[0] - 'A');
                })).ToArray();

            this.PackageItemCount = int.Parse(abilityInfoNode["PackageItemCount"].InnerText);
            if (abilityInfoNode["PackageItem"].InnerText.Fallback("0") == "0")
                this.PackageItem = new string[] { };
            else
                this.PackageItem = abilityInfoNode["PackageItem"].InnerText.Split(',');
        }

    }
}
