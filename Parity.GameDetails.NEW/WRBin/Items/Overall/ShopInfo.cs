using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.GameDetails.WRBin.Items.Overall
{
    public struct ShopInfo
    {

        public readonly bool Buyable;
        public readonly string Label;
        public readonly int Buytype; // todo: get enum
        public readonly int BuyOption;
        public readonly int[] PriceDinar;
        public readonly int[] PriceCredits;
        public readonly byte RequiredLevel;
        public readonly bool UseButton;
        public readonly bool PremiumOnly;
        public readonly bool ShopExchange;
        public readonly int SortPoint;
        public readonly int UseDurability;
        public readonly int RepairDinar;
        public readonly int RewardDinar;
        public readonly int PackageType;
        public readonly int PackageComponent;

        private static int[] ProcessPrice(string input)
        {
            int[] outPrice = new int[6];
            string[] splitInput = input.Split(',');
            Array.Resize(ref splitInput, 6);
            for (int i = 0; i < 6; i++)
                outPrice[i] = int.Parse(splitInput[i].Fallback("-1"));
            return outPrice;
        }

        public ShopInfo(System.Xml.XmlNode buyInfoNode)
        {
            this.Buyable = (buyInfoNode["bBuy"].InnerText == "1");
            this.Label = buyInfoNode["Label"].InnerText;
            this.Buytype = int.Parse(buyInfoNode["BuyType"].InnerText);
            this.BuyOption = int.Parse(buyInfoNode["BUYOPTION"].InnerText);
            this.PriceDinar = ProcessPrice(buyInfoNode["DinarCost"].InnerText);
            this.PriceCredits = ProcessPrice(buyInfoNode["CashCost"].InnerText);
            this.RequiredLevel = byte.Parse(buyInfoNode["ReqLevel"].InnerText);
            this.UseButton = (buyInfoNode["bUseButton"].InnerText == "1");
            this.PremiumOnly = (buyInfoNode["bPremiumOnly"].InnerText == "1");
            this.ShopExchange = (buyInfoNode["bShopExchange"].InnerText == "1");
            this.SortPoint = int.Parse(buyInfoNode["SortPoint"].InnerText);
            this.UseDurability = int.Parse(buyInfoNode["UseDurability"].InnerText);
            this.RepairDinar = int.Parse(buyInfoNode["RepairDinar"].InnerText);
            this.RewardDinar = int.Parse(buyInfoNode["RewardDinar"].InnerText);
            this.PackageType = int.Parse(buyInfoNode["nPackageType"].InnerText);
            this.PackageComponent = int.Parse(buyInfoNode["PackageComponent"].InnerText);
        }

    }
}
