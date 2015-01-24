using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.GameDetails.WRBin.Branch
{
    public class BattleClass
    {

        public readonly BasicInfo @BasicInfo;
        public readonly SlotInfo[] @SlotInfo;

        public BattleClass(System.Xml.XmlNode branchNode)
        {
            var invariant = System.Globalization.CultureInfo.InvariantCulture;

            this.@BasicInfo = new BasicInfo(
                branchNode["BASIC_INFO"]["BRANCH"].InnerText,
                int.Parse(branchNode["BASIC_INFO"]["STR"].InnerText),
                double.Parse(branchNode["BASIC_INFO"]["STR_RATE"].InnerText, invariant),
                int.Parse(branchNode["BASIC_INFO"]["CON"].InnerText),
                double.Parse(branchNode["BASIC_INFO"]["CON_RATE"].InnerText, invariant),
                int.Parse(branchNode["BASIC_INFO"]["DEX"].InnerText),
                double.Parse(branchNode["BASIC_INFO"]["DEX_RATE"].InnerText, invariant),
                int.Parse(branchNode["BASIC_INFO"]["STM"].InnerText),
                double.Parse(branchNode["BASIC_INFO"]["STM_RATE"].InnerText, invariant),
                int.Parse(branchNode["BASIC_INFO"]["WIZ"].InnerText),
                double.Parse(branchNode["BASIC_INFO"]["WIZ_RATE"].InnerText, invariant));

            this.@SlotInfo = new SlotInfo[8];
            for (int slotIndex = 0; slotIndex < 8; slotIndex++)
            {
                List<Base.Enum.Item.Weapon> allowedList = new List<Base.Enum.Item.Weapon>();
                foreach(string allowedFlag in branchNode["SLOT_INFO"][string.Format("_{0}SLOT_CODE", slotIndex)].InnerText.Split(','))
                    allowedList.Add(new ItemInformation("D" + allowedFlag).Weapon);
                this.@SlotInfo[slotIndex] = new SlotInfo(branchNode["SLOT_INFO"][String.Format("_{0}SLOT_ITEM", slotIndex)].InnerText, allowedList.ToArray());
            }
        }

    }
}
