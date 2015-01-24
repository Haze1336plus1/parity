using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parity.Game.Handler
{
    [Net.Handler.Requirements()]
    public class CreateRoom : Net.Handler.IHandler<Client.Client>
    {

        protected Base.App.Enumerizer<Base.Enum.PlayerLimit> PlayerLimitEnumerizer;

        public CreateRoom()
        {
            this.PlayerLimitEnumerizer = new Base.App.Enumerizer<Base.Enum.PlayerLimit>(new int[] { 8, 16, 20, 24, 32, 0, 4 }, (Base.Enum.PlayerLimit pLimit) => { return (int)(byte)pLimit; });
        }

        public override Net.Handler.Result Handle(Client.Client sender, Net.Packet.InPacket packet, bool customHandle = false)
        {
            base.Handle(sender, packet, customHandle);

            if (sender.GameSession.InRoom)
                sender.Disconnect("Can't create Room while in a room already");

            string roomName;
            string password;
            bool hasPassword;
            Base.Enum.Game.Mode gameMode;
            Base.Enum.PlayerLimit playerLimit;
            byte mapId;
            Base.Enum.Game.Type gameType;
            Base.Enum.Game.TimeLimit timeLimit;
            Base.Enum.Game.Difficulty difficulty;
            Base.Enum.Game.PingLimit pingLimit;
            Base.Enum.Game.LevelLimit levelLimit;

            if (RequireLength(17) &&
                RequireValue(0, out roomName) &&
                RequireValue(1, out hasPassword) &&
                RequireValue(2, out password) &&
                RequireEnum(3, out playerLimit) &&
                RequireValue(4, out mapId) &&
                RequireEnum(5, out gameMode) &&
                RequireEnum(7, out gameType) &&
                RequireEnum(8, out levelLimit) &&
                RequireEnum(13, out difficulty) &&
                RequireEnum(14, out timeLimit) &&
                RequireEnum(15, out pingLimit))
            {
                if (!Modules.WarRock.MapDetailContainer.IsDefined(mapId))
                    return new Net.Handler.Result("MapID {0} is not defined".Process(mapId));
                int iplayerLimit = this.PlayerLimitEnumerizer[playerLimit];
                Base.IO.Debug("Creating Room '{0}' with password? {1}:{2}, mapId: {3}, gameMode: {4}, playerLimit: {5}".Process(roomName, hasPassword.ToString(), password, mapId, gameMode, iplayerLimit));
                Base.IO.Debug(packet.Print());

                Game.Details roomDetails = new Game.Details(roomName, hasPassword ? password : null, false, false, gameMode, mapId, timeLimit, levelLimit, pingLimit);
                Game.Room room = new Game.Room(gameType, roomDetails, sender);

                Modules.RoomStorage[(int)Base.Enum.Channel.CQC].Register(room);
                sender.GameSession.Room.Join(room);

                return Net.Handler.Result.Success;

            }

            // dis is the new one
            /*string roomName;
            bool hasPassword;
            string password;
            Base.Enum.Game.Mode gameMode;
            Base.Enum.PlayerLimit playerLimit;
            byte mapId;
            bool isClanBattle;
            byte gameSetting1;
            bool isAutostart;
            if (RequireLength(19) &&
                RequireValue(0, out roomName) &&
                RequireValue(1, out hasPassword) &&
                RequireValue(2, out password) &&
                RequireEnum(3, out playerLimit) &&
                RequireValue(4, out mapId) &&
                RequireEnum(5, out gameMode) &&
                RequireValue(7, out isClanBattle) &&
                RequireValue(12, out gameSetting1) &&
                RequireValue(16, out isAutostart))
            {
                if (!Modules.WarRock.MapDetailContainer.IsDefined(mapId))
                    return new Net.Handler.Result("MapID {0} is not defined".Process(mapId));
                int iplayerLimit = this.PlayerLimitEnumerizer[playerLimit];
                Base.IO.Debug("Creating Room '{0}' with password? {1}:{2}, mapId: {3}, gameMode: {4}, playerLimit: {5}, isClanBattle: {6}, gameSeting1: {7}, isAutostart: {8}".Process(roomName, hasPassword.ToString(), password, mapId, gameMode, iplayerLimit, isClanBattle.ToString(), gameSetting1, isAutostart.ToString()));
                Base.IO.Debug(packet.Print());
                return Net.Handler.Result.Success;
            }*/
            return Net.Handler.Result.Default;
        }
    }
}
