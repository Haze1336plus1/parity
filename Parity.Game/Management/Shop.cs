using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Management
{
    public class Shop
    {

        protected Base.Layout.ShopItem[] Items;

        public Base.Layout.ShopItem this[string code]
        {
            get
            {
                return (from iItem in this.Items where iItem.Code == code select iItem).FirstOrDefault();
            }
        }

        public Shop()
        {
            List<Base.Layout.ShopItem> itemList = new List<Base.Layout.ShopItem>();

            // Get Weapons
            foreach (GameDetails.WRBin.Items.Weapon weapon in Modules.WarRock.ItemsContainer.Weapons)
            {
                itemList.Add(new Base.Layout.ShopItem(
                    weapon.Id,
                    weapon.BasicInfo.Code,
                    weapon.BuyInfo.RequiredLevel,
                    weapon.BuyInfo.Buyable,
                    weapon.UseInfo.ApplyTarget >= 0,
                    weapon.BuyInfo.PriceDinar,
                    weapon.BuyInfo.PriceCredits));
            }

            // Get Characters
            foreach (GameDetails.WRBin.Items.Character character in Modules.WarRock.ItemsContainer.Characters)
            {
                itemList.Add(new Base.Layout.ShopItem(
                    character.Id,
                    character.BasicInfo.Code,
                    character.BuyInfo.RequiredLevel,
                    character.BuyInfo.Buyable,
                    character.UseInfo.ApplyTarget >= 0,
                    character.BuyInfo.PriceDinar,
                    character.BuyInfo.PriceCredits));
            }

            // Get Etc
            foreach (GameDetails.WRBin.Items.Etc etc in Modules.WarRock.ItemsContainer.Etc)
            {
                itemList.Add(new Base.Layout.ShopItem(
                    etc.Id,
                    etc.BasicInfo.Code,
                    etc.BuyInfo.RequiredLevel,
                    etc.BuyInfo.Buyable,
                    etc.UseInfo.ApplyTarget >= 0,
                    etc.BuyInfo.PriceDinar,
                    etc.BuyInfo.PriceCredits));
            }

            this.Items = itemList.ToArray();

        }

    }
}
