using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Base.Layout
{
    public class ShopItem
    {

        public int Id { get; private set; }
        protected int[] priceDinar;
        protected int[] priceCash;
        public string Code { get; private set; }
        public byte RequiredLevel { get; private set; }
        public bool Buyable { get; private set; }
        public bool PremiumOnly { get; private set; }

        public ShopItem(int id, string code, byte requiredLevel, bool buyable, bool premiumOnly, int[] priceDinar, int[] priceCash)
        {
            this.Id = id;
            this.Code = code;
            this.RequiredLevel = requiredLevel;
            this.Buyable = buyable;
            this.PremiumOnly = premiumOnly;
            this.priceDinar = priceDinar;
            this.priceCash = priceCash;
        }

        public int GetPrice(Base.Enum.PaymentPeriod period, bool cash = false)
        {
            if (!cash)
                return this.priceDinar[(int)period];
            else
                return this.priceCash[(int)period];
        }
    }
}
