using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Handler
{
    [Net.Handler.Requirements()]
    public class CashProcess : Net.Handler.IHandler<Client.Client>
    {

        protected Base.App.Enumerizer<Base.Enum.PaymentPeriod> PeriodEnumerizer;

        public CashProcess()
        {
            this.PeriodEnumerizer = new Base.App.Enumerizer<Base.Enum.PaymentPeriod>(new int[] { 0, 7, 15, 30, 1, 365 }, (Base.Enum.PaymentPeriod p) => { return (int)(byte)p; });
        }

        protected void ShopOpen(Client.Client sender)
        {
            // refresh outbox
            // send itemshop open, but not required.
            sender.Session.Inventory.UpdateOutbox();
            sender.Send(Server.PacketFactory.Shop.Open(sender));
            sender.Send(Server.PacketFactory.Shop.Outbox(Base.Enum.ItemshopAction.OutboxOpen, sender));
        }
        protected Net.Handler.Result Purchase(Client.Client sender, Net.Packet.InPacket packet)
        {
            // get paramters
            Base.Enum.PaymentPeriod paymentPeriod;
            int price;
            int itemId;
            if (packet.ParamsCount == 8 && 
                Base.Types.TryParse(packet[2], out itemId) && 
                Base.Types.ParseEnum(packet[3], out paymentPeriod) && 
                packet[4] == "0" &&  // why ?
                Base.Types.TryParse(packet[5], out price) && 
                packet[7] == "0")
            {
                string itemCode = packet[6];
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
                        if (itemId == item.Id)
                        {
                            int realPrice = item.GetPrice(paymentPeriod, true);
                            if (realPrice == price)
                            {
                                success = true;
                                if (sender.Session.Account.Details.Credits >= price)
                                {
                                    if (!item.PremiumOnly || (item.PremiumOnly && sender.Session.Account.Details.Premium >= Base.Enum.Premium.None))
                                    {
                                        if (Management.Level.GetLevel(sender.Session.Account.Details.Experience) >= item.RequiredLevel)
                                        {
                                            DS.OutboxItem hasItem = sender.Session.Inventory.GetItemOutbox(itemCode);
                                            if (hasItem == null || hasItem.Duration < (int)Base.Compile.GameDefaults["Inventory.MaxItemTime"])
                                            {
                                                logMessage = "successful";
                                                sender.Session.Account.Details.Credits -= price;
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
                                    sender.Send(Server.PacketFactory.Shop.CashPurchase(Base.Enum.PurchaseError.NotEnoughDinar));
                                    logMessage = "not enough dinars";
                                }
                            }
                            else
                                logMessage = "invalid (credits) price ({0} give, {1} correct)".Process(price, realPrice);
                        }
                        else
                            logMessage = "itemID does not match ({0} given, {1} correct)".Process(itemId, item.Id);
                    }
                    else
                        logMessage = "CashProcess does not offer items of type {0}".Process(itemType.ToString());
                }
                else
                    logMessage = "itemcode not found";

                logMessage =
                    "Moving '#{0}:{1}' into Outbox '#{2}:{3}', {4} days for {5} credits".Process(
                        itemId,
                        itemCode,
                        sender.Session.Account.Id,
                        sender.Session.Account.Nickname,
                        duration,
                        price)
                        + (String.IsNullOrEmpty(logMessage) ? "" : ", " + logMessage);
                if (success)
                {
                    QA.GetLog()["shop"].Write(logMessage, "Purchase (credits)");
                    return Net.Handler.Result.Success;
                }
                else
                    return new Net.Handler.Result(logMessage);
            }
                
            // check that
            // verify everything
            // purchase if everything is fine
            return Net.Handler.Result.Default;
        }

        public override Net.Handler.Result Handle(Client.Client sender, Net.Packet.InPacket packet, bool customHandle = false)
        {
            base.Handle(sender, packet, customHandle);

            if(sender.GameSession.InRoom)
                return new Net.Handler.Result("Can't access CashProcess from a room");

            Base.Enum.ItemshopAction action;
            if(packet.ParamsCount > 0 && Base.Types.ParseEnum(packet[0], out action))
            {
                if (action == Base.Enum.ItemshopAction.ShopOpen && packet.ParamsCount == 2)
                {
                    this.ShopOpen(sender);
                    return Net.Handler.Result.Success;
                }
                else if (action == Base.Enum.ItemshopAction.Purchase)
                {
                    return this.Purchase(sender, packet);
                }
                else
                    return new Net.Handler.Result("Invalid CashProcess action: " + action.ToString());
            }
            return Net.Handler.Result.Default;
        }

    }
}
