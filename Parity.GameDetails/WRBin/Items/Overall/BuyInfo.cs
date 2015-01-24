using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.GameDetails.WRBin.Items.Overall
{
    public struct BuyInfo
    {

        public readonly bool Buyable;
        public readonly int Buytype; // todo: get enum
        public readonly int BuyOption;
        public readonly int[] PriceDinar;
        public readonly int[] PriceCredits;
        public readonly int RequiredBp;
        public readonly byte RequiredLevel;
        public readonly int RandomNum;

        private static int[] ProcessPrice(string input)
        {
            int[] outPrice = new int[6];
            string[] splitInput = input.Split(',');
            Array.Resize(ref splitInput, 6);
            for (int i = 0; i < 6; i++)
                outPrice[i] = int.Parse(splitInput[i].Fallback("-1"));
            return outPrice;
        }

        public BuyInfo(System.Xml.XmlNode buyInfoNode)
        {
            this.Buyable = (buyInfoNode["BUYABLE"].InnerText == "TRUE");
            this.Buytype = int.Parse(buyInfoNode["BUYTYPE"].InnerText);
            this.BuyOption = int.Parse(buyInfoNode["BUYOPTION"].InnerText);
            this.PriceDinar = ProcessPrice(buyInfoNode["COST"].InnerText);
            this.PriceCredits = ProcessPrice(buyInfoNode["ADD_DINAR"].InnerText);
            this.RequiredBp = int.Parse(buyInfoNode["REQ_BP"].InnerText);
            this.RequiredLevel = byte.Parse(buyInfoNode["REQ_LVL"].InnerText);
            this.RandomNum = int.Parse(buyInfoNode["RANDOM_NUM"].InnerText);
        }

        public BuyInfo(bool buyable, int buytype, int buyOption, int[] priceDinar, int[] priceCredits, int requiredBp, byte requiredLevel, int randomNum)
        {
            this.Buyable = buyable;
            this.Buytype = buytype;
            this.BuyOption = buyOption;
            this.PriceDinar = priceDinar;
            this.PriceCredits = priceCredits;
            this.RequiredBp = requiredBp;
            this.RequiredLevel = requiredLevel;
            this.RandomNum = randomNum;
        }

    }
}
