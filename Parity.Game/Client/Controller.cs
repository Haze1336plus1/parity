using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parity.Game.Client
{
    public class Controller
    {

        public readonly Client Owner;

        public Controller(Client owner)
        {
            this.Owner = owner;
        }

        public bool LoginToServer(int userId, uint sessionKey)
        {
            DS.Model dbModel = QA.GetDBModel();
            DS.Account newSession = dbModel.Account(userId);
            if (newSession == null ||
                newSession.Online ||
                newSession.Session != sessionKey)
                return false;
            dbModel.GoOnline(newSession, QA.Core.Config.GameConfig.Id); // logon to this server:D
            this.Owner.Session.SetSession(newSession);
            return true;
        }

        public void Logout()
        {
            // todo: make better. actually using TWO clients at same time - one here, one at DS.Details.Save(DS.Model)
            this.Owner.Session.Inventory.Apply();
            this.Owner.Session.Character.Apply();
            using(DS.Client dsclient = QA.GetDBClient())
            {
                using (var trans = dsclient.Connection.BeginTransaction())
                {
                    dsclient
                        .Command("UPDATE `accounts` AS a SET a.online = 0 WHERE a.account_id = @accountId")
                        .SetParameter("@accountId", this.Owner.Session.Account.Id)
                        .Execute();
                    this.Owner.Session.Account.Details.Save(QA.GetDBModel());
                    trans.Commit();
                }
            }
        }

        public void SendRoomlist()
        {
            this.Owner.Send(Server.PacketFactory.RoomList(this.Owner, Modules.RoomStorage.Channel[(int)this.Owner.GameSession.Channel].All()));
        }

    }
}
