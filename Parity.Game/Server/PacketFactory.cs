using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Game.Server
{
    public class PacketFactory
    {

        // Chat
        public class Chat
        {

            // Raw Chat
            //public static Packet.Custom Raw(string message,

            // Chat.Notice
            public static Net.Packet.OutPacket Notice(string message)
            {
                return new Packet.Chat(
                    -1,
                    "NULL",
                    Base.Enum.ChatChannel.Notice,
                    1000,
                    "NULL",
                    message);
            }

            // Chat.Lobby
            public static Net.Packet.OutPacket Lobby(Client.Client origin, string message, Base.Enum.ChatChannel chatChannel)
            {
                short targetSession = -1;
                if(origin.Session.Account.AuthLevel == Base.Enum.AuthLevel.Administrator)
                {
                    targetSession = 999;
                    chatChannel = Base.Enum.ChatChannel.RoomAll;
                }
                else if(origin.Session.Account.AuthLevel == Base.Enum.AuthLevel.Moderator)
                {
                    targetSession = 998;
                    chatChannel = Base.Enum.ChatChannel.RoomAll;
                }

                return new Packet.Chat(
                    origin.Session.SessionID,
                    origin.Session.Account.Nickname,
                    chatChannel,
                    targetSession,
                    "NULL",
                    origin.Session.Account.Nickname + " >> " + message);
            }

            // Chat.System
            public static Net.Packet.OutPacket System(string message)
            {
                string botName = QA.Core.Config.GameConfig.BotName;
                return new Packet.Chat(
                    -1,
                    botName,
                    Base.Enum.ChatChannel.RoomAll,
                    999,
                    "NULL",
                    botName + " >> " + message);
            }

            // Chat.Whisper
            public static Net.Packet.OutPacket Whisper(Client.Client origin, Client.Client target, string message, bool self = false)
            {
                return new Packet.Chat(
                    origin.Session.SessionID,
                    origin.Session.Account.Nickname,
                    (self ? Base.Enum.ChatChannel.WhisperSelf : Base.Enum.ChatChannel.Whisper),
                    target.Session.SessionID,
                    target.Session.Account.Nickname,
                    origin.Session.Account.Nickname + " >> " + message);
            }

            // Chat.Clan
            public static Net.Packet.OutPacket Clan(Client.Client origin, string message)
            {
                return new Packet.Chat(
                    origin.Session.SessionID,
                    origin.Session.Account.Nickname,
                    Base.Enum.ChatChannel.Clan,
                    -1,
                    "NULL",
                    origin.Session.Account.Nickname + " >> " + message);
            }

        }

        public class Shop
        {

            // Shop.Open
            public static Net.Packet.OutPacket Open(Client.Client player)
            {
                return new Packet.ItemShop().Open(player.Session.Account.Details.Credits);
            }

            // Shop.OutboxError
            public static Net.Packet.OutPacket OutboxError(Base.Enum.CashShopDepotError error)
            {
                return new Packet.CashShopDepot(error);
            }

            // Shop.Outbox
            public static Net.Packet.OutPacket Outbox(Base.Enum.ItemshopAction action, Client.Client player, bool showMessage = false)
            {
                return new Packet.CashShopDepot(
                    action,
                    showMessage, 
                    player.Session.Account.Id,
                    player.Session.Account.Details.Dinar,
                    player.Session.Account.Details.Credits,
                    player.Session.Inventory.Outbox,
                    player.Session.Inventory.ToString(),
                    player.Session.Character.ToString(),
                    player.Session.Inventory.SlotCode);
            }

            // Shop.CashPurchase (error)
            public static Net.Packet.OutPacket CashPurchase(Base.Enum.PurchaseError error)
            {
                return new Packet.ItemShop().CashPurchase(error);
            }

            // Shop.Purchase (error)
            public static Net.Packet.OutPacket Purchase(Base.Enum.PurchaseError error)
            {
                return new Packet.ItemProcess(error);
            }

            // Shop.Purchase
            public static Net.Packet.OutPacket Purchase(Client.Client player)
            {
                return new Packet.ItemProcess(
                    player.Session.Inventory.ToString(),
                    player.Session.Account.Details.Dinar,
                    player.Session.Inventory.SlotCode);
            }

        }

        public class Custom
        {

            // Custom.MessageBox
            public static Net.Packet.OutPacket MessageBox(string message)
            {
                return new Packet.Custom().MessageBox(message);
            }

            // Custom.DefaultEQ
            public static Net.Packet.OutPacket DefaultEQ()
            {
                string fullPack = "";
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        DS.Item eq = Modules.Defaults.Equipment[i][j];
                        fullPack += (eq == null ? "FAIL" : eq.Code ) + ",";
                    }
                }
                fullPack = fullPack.Substring(0, fullPack.Length - 1);
                return new Packet.Custom().DefaultEQ(fullPack);
            }

        }

        // ItemChange
        public static Net.Packet.OutPacket ItemChange(Client.Client player, Base.Enum.BattleClass battleClass)
        {
            return new Packet.ItemChange(battleClass, player.Session.Inventory.GetEquipment(battleClass));
        }

        // KeepAlive
        public static Net.Packet.OutPacket KeepAlive(Client.Client sender)
        {
            return new Packet.KeepAlive(
                sender.GameSession.Ping,
                (int)Math.Ceiling(sender.Session.Account.Details.PremiumRemain.TotalSeconds), 
                sender.Session.SessionTime);
        }

        // Greeting
        public static Net.Packet.OutPacket Greeting()
        {
            return new Packet.Greeting();
        }

        // GameInfo
        public static Net.Packet.OutPacket GameInfo(Client.Client player)
        {
            Packet.GameInfo gi = new Packet.GameInfo(
                player.Session.SessionID,
                player.Session.Account.Id,
                player.Session.Account.Nickname,
                player.Session.Account.Details.Experience,
                player.Session.Account.Details.Kills,
                player.Session.Account.Details.Deaths,
                player.Session.Account.Details.Dinar,
                player.Session.Inventory.SlotCode,
                new string[] {
                    player.Session.Inventory.GetEquipment(Base.Enum.BattleClass.Engineer),
                    player.Session.Inventory.GetEquipment(Base.Enum.BattleClass.Medic),
                    player.Session.Inventory.GetEquipment(Base.Enum.BattleClass.Patrol),
                    player.Session.Inventory.GetEquipment(Base.Enum.BattleClass.Assult),
                    player.Session.Inventory.GetEquipment(Base.Enum.BattleClass.Antitank)
                },
                player.Session.Inventory.ToString(),
                player.Session.Inventory.Limit,
                new string[] {
                    player.Session.Character.GetEquipment(Base.Enum.BattleClass.Engineer),
                    player.Session.Character.GetEquipment(Base.Enum.BattleClass.Medic),
                    player.Session.Character.GetEquipment(Base.Enum.BattleClass.Patrol),
                    player.Session.Character.GetEquipment(Base.Enum.BattleClass.Assult),
                    player.Session.Character.GetEquipment(Base.Enum.BattleClass.Antitank)
                },
                player.Session.Character.ToString(),
                player.Session.Account.Details.Premium);
            
            return gi;
        }

        // GameInfo-ErrorCode
        public static Net.Packet.OutPacket GameInfo(Base.Code.GameInfo errorCode)
        {
            return new Packet.GameInfo(errorCode);
        }

        // ChangeChannel
        public static Net.Packet.OutPacket ChangeChannel(Base.Enum.Channel channel)
        {
            return new Packet.ChangeChannel(channel);
        }

        // RoomList
        public static Net.Packet.OutPacket RoomList(Client.Client player, IEnumerable<Game.Room> rooms)
        {
            return new Packet.RoomList(
                rooms.ToArray(),
                player.GameSession.RoomStartingIndex);
        }

        // LeaveRoom
        public static Net.Packet.OutPacket LeaveRoom(Client.Client player, Game.Room room, byte oldSlot)
        {
            return new Packet.LeaveRoom(
                player.Session.SessionID,
                oldSlot,
                room.Players.MasterIndex,
                player.Session.Account.Details.Experience,
                player.Session.Account.Details.Dinar);
        }

        // JoinRoom
        public static Net.Packet.OutPacket JoinRoom(Client.Client player)
        {
            return new Packet.JoinRoom(
                player.GameSession.Room.Current,
                player.GameSession.Room.Current.Players.Container.IndexOf(player));
        }

        // ItemDestroy
        public static Net.Packet.OutPacket ItemDestroy(Client.Client player, string deletedItem)
        {
            return new Packet.ItemDestroy(
                deletedItem,
                player.Session.Inventory.ToString(),
                player.Session.Inventory.SlotCode,
                new string[] {
                    player.Session.Inventory.GetEquipment(Base.Enum.BattleClass.Engineer),
                    player.Session.Inventory.GetEquipment(Base.Enum.BattleClass.Medic),
                    player.Session.Inventory.GetEquipment(Base.Enum.BattleClass.Patrol),
                    player.Session.Inventory.GetEquipment(Base.Enum.BattleClass.Assult),
                    player.Session.Inventory.GetEquipment(Base.Enum.BattleClass.Antitank)
                });
        }

        // ItemExpired
        public static Net.Packet.OutPacket ItemExpired(Client.Client player, DS.Item[] expired)
        {
            return new Packet.ItemExpired(
                expired,
                player.Session.Inventory.SlotCode,
                new string[] {
                    player.Session.Inventory.GetEquipment(Base.Enum.BattleClass.Engineer),
                    player.Session.Inventory.GetEquipment(Base.Enum.BattleClass.Medic),
                    player.Session.Inventory.GetEquipment(Base.Enum.BattleClass.Patrol),
                    player.Session.Inventory.GetEquipment(Base.Enum.BattleClass.Assult),
                    player.Session.Inventory.GetEquipment(Base.Enum.BattleClass.Antitank)
                },
                player.Session.Inventory.ToString());
        }

        // CostumeItemChange
        public static Net.Packet.OutPacket CostumeItemChange(Client.Client player, Base.Enum.BattleClass battleClass)
        {
            return new Packet.CostumeItemChange(
                battleClass,
                player.Session.Character.GetEquipment(battleClass));
        }

        // UserList
        public static Net.Packet.OutPacket UserList(Base.Enum.UserListFilter filter)
        {
            Client.Client[] players = new Client.Client[0];
            if (filter == Base.Enum.UserListFilter.Waiting)
                players = (from Client.Client player in QA.Core.LobbyServer.Clients where !player.GameSession.InRoom select player).ToArray();
            if (players.Length > 50)
                Array.Resize(ref players, 50);
            return new Packet.UserList(players, filter);
        }

        // RoomInfoChange
        public static Net.Packet.OutPacket RoomInfoChange(int roomId, Base.Enum.RoomInfoChangeAction action, Game.Room room = null)
        {
            return new Packet.RoomInfoChange(roomId, action, room);
        }

        // LoginEvent
        public static Net.Packet.OutPacket LoginEvent(Client.Client player)
        {
            return new Packet.LoginEvent(player.Session.Inventory.SlotCode, player.Session.Inventory.ToString(), player.Session.Character.ToString());
        }

        // PlayerList
        public static Net.Packet.OutPacket PlayerList(Game.Room room, IEnumerable<Client.Client> players)
        {
            return new Packet.PlayerList(room, players.ToArray());
        }

    }

}
