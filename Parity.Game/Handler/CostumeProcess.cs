using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Handler
{
    [Net.Handler.Requirements()]
    public class CostumeProcess : Net.Handler.IHandler<Client.Client>
    {

        protected Base.App.Enumerizer<Base.Enum.PaymentPeriod> PeriodEnumerizer;

        public CostumeProcess()
        {
            this.PeriodEnumerizer = new Base.App.Enumerizer<Base.Enum.PaymentPeriod>(new int[] { 0, 7, 15, 30, 1, 365 }, (Base.Enum.PaymentPeriod p) => { return (int)(byte)p; });
        }

        protected Net.Handler.Result Purchase(Client.Client sender, string itemCode, Base.Enum.PaymentPeriod paymentPeriod)
        {
            int duration = this.PeriodEnumerizer[paymentPeriod];
            Base.Layout.ShopItem item = Modules.Shop[itemCode];
            Base.Enum.Item.Type itemType = Modules.WarRock.ItemsContainer.GetItemInfo(itemCode).Type;

            string logMessage = string.Empty;
            bool success = false;

            int realPrice = 0;

            if (item != null)
            {
                if (itemType == Base.Enum.Item.Type.Character)
                {
                    realPrice = item.GetPrice(paymentPeriod);
                    success = true;
                    if (sender.Session.Account.Details.Dinar >= realPrice)
                    {
                        if (!item.PremiumOnly || (item.PremiumOnly && sender.Session.Account.Details.Premium >= Base.Enum.Premium.None))
                        {
                            if (Management.Level.GetLevel(sender.Session.Account.Details.Experience) >= item.RequiredLevel)
                            {
                                DS.OutboxItem hasItem = sender.Session.Inventory.GetItemOutbox(itemCode);
                                if (hasItem == null || hasItem.Duration < (int)Base.Compile.GameDefaults["Inventory.MaxItemTime"])
                                {
                                    logMessage = "successful";
                                    sender.Session.Account.Details.Credits -= realPrice;
                                    sender.Session.Inventory.CreateOutbox(itemCode, duration, sender.Session.Account.Nickname);
                                    sender.Send(Server.PacketFactory.Shop.Outbox(Base.Enum.ItemshopAction.OutboxOpen, sender, true));
                                }
                                else
                                {
                                    // reached maximum time limit
                                    sender.Send(Server.PacketFactory.Shop.CashPurchase(Base.Enum.PurchaseError.MaximumTimeLimit));
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
                    logMessage = "CostumeProcess does not offer items of type {0}".Process(itemType.ToString());
            }
            else
                logMessage = "itemcode not found";

            logMessage =
                "Purcahse (Costume) '#{0}' into Outbox '#{1}:{2}', {3} days for {4} credits".Process(
                    itemCode,
                    sender.Session.Account.Id,
                    sender.Session.Account.Nickname,
                    duration,
                    realPrice)
                    + (String.IsNullOrEmpty(logMessage) ? "" : ", " + logMessage);

            QA.GetLog()["shop"].Write(logMessage, "Purchase (dinars)");
            if (success)
                return Net.Handler.Result.Success;
            else
                return new Net.Handler.Result(logMessage);
        }

        public override Net.Handler.Result Handle(Client.Client sender, Net.Packet.InPacket packet, bool customHandle = false)
        {
            base.Handle(sender, packet, customHandle);

            if (sender.GameSession.InRoom)
                return new Net.Handler.Result("Can't access CostumeProcess from a room");

            Base.Enum.ItemshopAction action;
            string itemCode;
            ushort itemReference;
            Base.Enum.PaymentPeriod paymentPeriod;

            if (packet.ParamsCount == 5 &&
                Base.Types.ParseEnum(packet[0], out action) &&
                action == Base.Enum.ItemshopAction.Purchase &&
                Base.Types.TryParse(packet[1], out itemCode) &&
                Base.Types.TryParse(packet[2], out itemReference) &&
                packet[3] == "-1" &&
                Base.Types.ParseEnum(packet[4], out paymentPeriod))
            {
                return this.Purchase(sender, itemCode, paymentPeriod);
            }

            return Net.Handler.Result.Default;
        }
    }
}
