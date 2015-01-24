using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Handler
{
    [Net.Handler.Requirements()]
    public class ItemProcess : Net.Handler.IHandler<Client.Client>
    {

        protected Base.App.Enumerizer<Base.Enum.PaymentPeriod> PeriodEnumerizer;

        public ItemProcess()
        {
            this.PeriodEnumerizer = new Base.App.Enumerizer<Base.Enum.PaymentPeriod>(new int[] { 0, 7, 15, 30, 1, 365 }, (Base.Enum.PaymentPeriod p) => { return (int)(byte)p; });
        }

        protected Net.Handler.Result Purchase(Client.Client sender, string itemCode, Base.Enum.PaymentPeriod paymentPeriod, int price)
        {
            int duration = this.PeriodEnumerizer[paymentPeriod];
            Base.Layout.ShopItem item = Modules.Shop[itemCode];
            Base.Enum.Item.Type itemType = Modules.WarRock.ItemsContainer.GetItemInfo(itemCode).Type;

            string logMessage = string.Empty;
            bool success = false;

            if (item != null)
            {
                if (itemType == Base.Enum.Item.Type.Weapon ||
                    itemType == Base.Enum.Item.Type.Etc)
                {
                    int realPrice = item.GetPrice(paymentPeriod);
                    if (realPrice == price)
                    {
                        success = true;
                        if (sender.Session.Account.Details.Dinar >= price)
                        {
                            if (!item.PremiumOnly || (item.PremiumOnly && sender.Session.Account.Details.Premium >= Base.Enum.Premium.None))
                            {
                                if (Management.Level.GetLevel(sender.Session.Account.Details.Experience) >= item.RequiredLevel)
                                {
                                    DS.Item hasItem = sender.Session.Inventory.GetItem(itemCode);
                                    if (hasItem == null || hasItem.Duration < (int)Base.Compile.GameDefaults["Inventory.MaxItemTime"])
                                    {
                                        logMessage = "successful";
                                        sender.Session.Account.Details.Dinar -= price;
                                        sender.Session.Inventory.Create(itemCode, duration);
                                        sender.Send(Server.PacketFactory.Shop.Purchase(sender));
                                    }
                                    else
                                    {
                                        // reached maximum time limit
                                        sender.Send(Server.PacketFactory.Shop.Purchase(Base.Enum.PurchaseError.MaximumTimeLimit));
                                        logMessage = "reached maximum time limit";
                                    }
                                }
                                else
                                {
                                    // low level
                                    sender.Send(Server.PacketFactory.Shop.Purchase(Base.Enum.PurchaseError.LowLevel));
                                    logMessage = "low level";
                                }
                            }
                            else
                            {
                                // no premium
                                // WARNING: normally you even CAN'T buy a premium only item client-sided :| so may disconnect him at this point
                                sender.Send(Server.PacketFactory.Shop.Purchase(Base.Enum.PurchaseError.NoPremium));
                                logMessage = "premium required";
                            }
                        }
                        else
                        {
                            // not enough money
                            sender.Send(Server.PacketFactory.Shop.Purchase(Base.Enum.PurchaseError.NotEnoughDinar));
                            logMessage = "not enough dinars";
                        }
                    }
                    else
                        logMessage = "invalid (dinars) price ({0} give, {1} correct)".Process(price, realPrice);
                }
                else
                    logMessage = "ItemProcess does not offer items of type {0}".Process(itemType.ToString());
            }
            else
                logMessage = "itemcode not found";

            logMessage =
                "Purcahse '#{0}' into Inventory '#{1}:{2}', {3} days for {4} dinars".Process(
                    itemCode,
                    sender.Session.Account.Id,
                    sender.Session.Account.Nickname,
                    duration,
                    price)
                    + (String.IsNullOrEmpty(logMessage) ? "" : ", " + logMessage);

            QA.GetLog()["shop"].Write(logMessage, "Purchase (dinars)");
            if (success)
                return Net.Handler.Result.Success;
            else
                return new Net.Handler.Result(logMessage);
        }

        public override Net.Handler.Result Handle(Client.Client sender, Net.Packet.InPacket packet, bool customHandle = false)
        {
            if (sender.GameSession.InRoom)
                return new Net.Handler.Result("Can't access ItemProcess from a room");

            Base.Enum.ItemshopAction action;
            Base.Enum.PaymentPeriod paymentPeriod;
            int price;
            if (packet.ParamsCount == 6 && 
                    Base.Types.ParseEnum(packet[0], out action) &&
                    action == Base.Enum.ItemshopAction.Purchase &&
                    Base.Types.ParseEnum(packet[4], out paymentPeriod) &&
                    Base.Types.TryParse(packet[5], out price))
            {
                string itemCode = packet[1];
                return this.Purchase(sender, itemCode, paymentPeriod, price);
            }
            return Net.Handler.Result.Default;
        }

    }
}
